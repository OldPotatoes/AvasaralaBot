using Amazon.Lambda.Core;
using BotDynamoDB;
using BotTweeter;
using LinqToTwitter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AvasaralaBot_AWSLambda
{
    public class Function
    {
        const Boolean ActuallyTweet = true;
        const String SearchText = "@AvasaralaBot";
        const String AvasaralaBotUser = "AvasaralaBot";
        const Int32 MaxTweetsToReturn = 50;
        const Int64 AvasaralaBotUserId = 1284552017403420675;
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(String input, ILambdaContext context)
        {
            LambdaLogger.Log($"FunctionHandler()\n");
            LambdaLogger.Log($"    Input: {input}\n");
            LambdaLogger.Log($"    Context:{JsonConvert.SerializeObject(context)}\n");

            String ApiKey = Environment.GetEnvironmentVariable("ApiKey");
            String ApiSecretKey = Environment.GetEnvironmentVariable("ApiSecretKey");
            String AccessToken = Environment.GetEnvironmentVariable("AccessToken");
            String AccessTokenSecret = Environment.GetEnvironmentVariable("AccessTokenSecret");

            var db = new DBAccess();
            var tweeter = new Tweeter(ApiKey, ApiSecretKey, AccessToken, AccessTokenSecret);

            if (input == "Tweet")
                TweetWisdom(db, tweeter);
            else if (input == "Reply")
                ReplyToMentions(db, tweeter);
            else
                LambdaLogger.Log($"    Nothing to do\n");

            return "Done!";
        }

        private void TweetWisdom(DBAccess db, Tweeter tweeter)
        {
            LambdaLogger.Log($"TweetWisdom()\n");
            List<Quote> statementsList = db.GetAllStatements().Result;
            Int32 count = statementsList.Count;

            // If this is the last statement, reset all the 'tweeted' values
            if (count <= 1)
            {
                LambdaLogger.Log($"    Published all statements, time to reset\n");
                db.ResetTweetedValues(statementsList, ActuallyTweet).Wait();
            }

            Int32 quoteIndex = new Random().Next(count);
            Quote statement = statementsList[quoteIndex];
            LambdaLogger.Log($"    Quote {quoteIndex}: {statement.quoteText}\n");

            String tweet = DecideTweetText(statement);
            UInt64 id = tweeter.MaybeTweet(tweet, ActuallyTweet);
            db.SetTweeted(statement, ActuallyTweet);
        }

        private async void ReplyToMentions(DBAccess db, Tweeter tweeter)
        {
            LambdaLogger.Log($"ReplyToMentions()\n");
            List<Quote> responsesList = db.GetAllResponses().Result;
            Int32 count = responsesList.Count;

            DateTime lastReplyTime = db.GetLastReplyTime().Result;

            LambdaLogger.Log($"    Last reply time: {lastReplyTime}\n");

            var TweetInfo = new List<TweetDerivationPair>();

            List<Status> tweets = tweeter.GetMentions(SearchText, lastReplyTime, MaxTweetsToReturn).Result;
            //TweetQuery tweetQuery = await tweeter.GetMentions_New(AvasaralaBotUserId, lastReplyTime, MaxTweetsToReturn);
            //Int32 replyCount = tweetQuery.Tweets.Count;
            //foreach (Tweet tweeted in tweetQuery.Tweets)
            foreach (Status tweeted in tweets)
            {
                tweeter.PrintTweet(tweeted);

                TweetDerivationPair tdp = new TweetDerivationPair();
                tdp.Tweet = tweeted;

                if (tweeted.UserID == AvasaralaBotUserId)
                {
                    // Tweet is by AvasaralaBot
                    tdp.Derivation = TweetDerivation.TweetFromAB;
                }
                //else if (tweeted.InReplyToUserID == "0")
                else if (tweeted.InReplyToUserID == 0)
                {
                    // Tweet is the start of a conversation

                    //if (tweeted.Entities.Mentions.Any(m => m.Username == AvasaralaBotUser)) // or SearchText
                    if (tweeted.Entities.UserMentionEntities.Any(m => m.ScreenName == AvasaralaBotUser))
                    {
                        // Tweet mentions AvasaralaBot
                        // Should always be true as these are mentions
                        tdp.Derivation = TweetDerivation.TweetAtAB;

                        // Could also be a retweet?
                    }
                }
                else if (tweeted.InReplyToUserID == AvasaralaBotUserId)
                {
                    // Tweet is replying to AvararalaBot
                    tdp.Derivation = TweetDerivation.ReplyToAB;
                }
                else
                {
                    // Tweet is replying to someone else
                    tdp.Derivation = TweetDerivation.ReplyToOther;
                }

                TweetInfo.Add(tdp);
            }

            Int32 replyCount = 0;
            foreach (TweetDerivationPair tdp in TweetInfo)
            {
                LambdaLogger.Log($"{tdp.Tweet.User.ScreenNameResponse} says: {tdp.Tweet.Text} - Derivation: {tdp.Derivation.ToString()}\n");

                if (tdp.Derivation == TweetDerivation.ReplyToAB ||
                    tdp.Derivation == TweetDerivation.TweetAtAB ||
                    tdp.Derivation == TweetDerivation.RetweetOfAB)
                {
                    Int32 responsesIndex = new Random().Next(count) + 1;
                    Quote response = responsesList[responsesIndex];
                    LambdaLogger.Log($"    Response {responsesIndex}: {response.quoteText}\n");

                    // Add username as prefix to tweet
                    response.quoteText = $"@{tdp.Tweet.User.ScreenNameResponse} {response.quoteText}";
                    String tweet = DecideTweetText(response);
                    UInt64 id = tweeter.MaybeReply(tweet, tdp.Tweet.StatusID, ActuallyTweet);
                    replyCount++;
                }
            }

            if (replyCount > 0)
            {
                var timeUtc = DateTime.UtcNow;
                LambdaLogger.Log($"    UTC now: {timeUtc}\n");
                //LambdaLogger.Log($"    New York: {TimeZoneInfo.ConvertTimeFromUtc(timeUtc, TimeZoneInfo.FindSystemTimeZoneById("America/New_York"))}\n");

                if (ActuallyTweet)
                {
                    Boolean isWritten = db.SetLastReplyTime(timeUtc, replyCount).Result;
                    if (!isWritten)
                    {
                        LambdaLogger.Log($"ERROR: Failed to set last reply time to {timeUtc}\n");
                    }
                }
            }
        }

        private String DecideTweetText(Quote quote)
        {
            LambdaLogger.Log($"DecideTweetText()\n");
            LambdaLogger.Log($"    Quote: {JsonConvert.SerializeObject(quote)}\n");
            String tweet = "";

            if (quote.medium == "Book")
            {
                tweet = $"{quote.quoteText}\n\n{quote.book}, {quote.chapter}\n#Avasarala #TheExpanse";
                if (tweet.Length > 280)
                    tweet = $"{quote.quoteText}\n\n{quote.book}, {quote.chapter}\n#TheExpanse";
                if (tweet.Length > 280)
                    tweet = $"{quote.quoteText}\n\n{quote.book}, {quote.chapter}";
                if (tweet.Length > 280)
                    tweet = $"{quote.quoteText}\n\n{quote.book}";
                if (tweet.Length > 280)
                    tweet = quote.quoteText;
                if (tweet.Length > 280)
                    tweet = $"{quote.quoteText.Substring(0, 279)}…";
            }
            else if (quote.medium == "TV")
            {
                tweet = $"{quote.quoteText}\n\n{quote.season}, {quote.episode}\n#Avasarala #TheExpanse";
                if (tweet.Length > 280)
                    tweet = $"{quote.quoteText}\n\n{quote.season}, {quote.episode}\n#TheExpanse";
                if (tweet.Length > 280)
                    tweet = $"{quote.quoteText}\n\n{quote.season}, {quote.episode}";
                if (tweet.Length > 280)
                    tweet = $"{quote.quoteText}\n\n{quote.episode}";
                if (tweet.Length > 280)
                    tweet = quote.quoteText;
                if (tweet.Length > 280)
                    tweet = $"{quote.quoteText.Substring(0, 279)}…";
            }

            return tweet;
        }
    }
}




