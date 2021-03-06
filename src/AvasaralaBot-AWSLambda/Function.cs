using Amazon.Lambda.Core;
using BotDynamoDB;
using BotTweeter;
using System;
using System.Collections.Generic;
using LinqToTwitter;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AvasaralaBot_AWSLambda
{
    public class Function
    {
        const Boolean ActuallyTweet = true;
        const String SearchText = "@AvasaralaBot"; //"@SAghdashloo";
        const Int32 MaxTweetsToReturn = 50;
        const Int64 AvasaralaBotUserId = 1284552017403420675;

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(InputEvent input, ILambdaContext context)
        {
            LambdaLogger.Log($"input: {input.Key1}\n");

            String ApiKey = Environment.GetEnvironmentVariable("ApiKey");
            String ApiSecretKey = Environment.GetEnvironmentVariable("ApiSecretKey");
            String AccessToken = Environment.GetEnvironmentVariable("AccessToken");
            String AccessTokenSecret = Environment.GetEnvironmentVariable("AccessTokenSecret");

            var db = new DBAccess();
            var tweeter = new Tweeter(ApiKey, ApiSecretKey, AccessToken, AccessTokenSecret);

            TweetWisdom(db, tweeter);
            ReplyToMentions(db, tweeter);

            return "Done!";
        }

        private void TweetWisdom(DBAccess db, Tweeter tweeter)
        {
            List<Quote> statementsList = db.GetAllStatements().Result;
            Int32 count = statementsList.Count;
            Int32 quoteIndex = new Random().Next(count) + 1;
            Quote statement = statementsList[quoteIndex];
            LambdaLogger.Log($"Quote {quoteIndex}: {statement.quoteText}\n");

            String tweet = DecideTweetText(statement);
            UInt64 id = tweeter.MaybeTweet(tweet, ActuallyTweet);
        }

        private void ReplyToMentions(DBAccess db, Tweeter tweeter)
        {
            List<Quote> responsesList = db.GetAllResponses().Result;
            Int32 count = responsesList.Count;

            DateTime lastReplyTime = db.GetLastReplyTime().Result;
            LambdaLogger.Log($"Last reply time: {lastReplyTime}\n");

            //List<Status> tweets = tweeter.GetTweets(SearchText, lastReplyTime, MaxTweetsToReturn);
            List<Status> tweets = tweeter.GetMentions(SearchText, lastReplyTime, MaxTweetsToReturn);
            //List<Status> tweets = tweeter.GetTweetsFrom(SearchText, MaxTweetsToReturn);
            foreach (Status tweeted in tweets)
            {
                LambdaLogger.Log($"TweetID: {tweeted.StatusID}\n");
                LambdaLogger.Log($"    Created at: {tweeted.CreatedAt}\n");
                LambdaLogger.Log($"    User: {tweeted.User.ScreenNameResponse}\n");
                LambdaLogger.Log($"    Text: {tweeted.FullText}\n");
                LambdaLogger.Log($"    In Reply to tweet: {tweeted.InReplyToStatusID}\n");
                LambdaLogger.Log($"    In Reply to user: {tweeted.InReplyToUserID}\n");
                LambdaLogger.Log($"    User Mentions count: {tweeted.Entities.UserMentionEntities.Count}\n");
                foreach (UserMentionEntity ume in tweeted.Entities.UserMentionEntities)
                {
                    LambdaLogger.Log($"    User Mention: {ume.Id}, {ume.Name}, {ume.ScreenName}\n");
                }
                LambdaLogger.Log($"    Contributors count: {tweeted.Contributors.Count}\n");
                foreach (Contributor contrib in tweeted.Contributors)
                {
                    LambdaLogger.Log($"    Contributor: {contrib.ID}, {contrib.ScreenName}\n");
                }
                LambdaLogger.Log($"    User count: {tweeted.Users.Count}\n");
                foreach (ulong user in tweeted.Users)
                {
                    LambdaLogger.Log($"    User: {user}\n");
                }

                // Screen out retweets (GetMentions does this automatically)

                if (tweeted.InReplyToStatusID != 0)   
                {
                    // This should screen out replies to replies - only reply to initiating tweets
                    LambdaLogger.Log($"Filtering out non-initiating tweet to reply to: {tweeted.StatusID}\n");
                    continue;
                }

                if (tweeted.InReplyToUserID != AvasaralaBotUserId)
                {
                    // This should filter out non-@s, or we will spam every person who replies to our reply
                    LambdaLogger.Log($"Filtering out tweetID as a reply: {tweeted.StatusID}\n");
                    continue;
                }

                Int32 responsesIndex = new Random().Next(count) + 1;
                Quote response = responsesList[responsesIndex];
                LambdaLogger.Log($"Response {responsesIndex}: {response.quoteText}\n");

                // Add username as prefix to tweet
                response.quoteText = $"@{tweeted.User.ScreenNameResponse} {response.quoteText}";
                String tweet = DecideTweetText(response);
                UInt64 id = tweeter.MaybeReply(tweet, tweeted.StatusID, ActuallyTweet);
            }

            if (tweets.Count > 0)
            {
                var timeUtc = DateTime.UtcNow;
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
                DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
                LambdaLogger.Log($"New York: {easternTime}\n");
                Boolean isWritten = db.SetLastReplyTime(easternTime, tweets.Count).Result;
            }
        }

        private String DecideTweetText(Quote quote)
        {
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
