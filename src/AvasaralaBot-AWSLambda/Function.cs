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

            List<Quote> statements = db.GetAllStatements().Result;
            Int32 statementsCount = statements.Count;

            Int32 statementsQuoteIndex = new Random().Next(statementsCount) + 1;

            Quote statementsQuote = statements[statementsQuoteIndex];
            LambdaLogger.Log($"Quote {statementsQuoteIndex}: {statementsQuote.quoteText}\n");
            LambdaLogger.Log($"UUID: {statementsQuote.uuid}\n");
            LambdaLogger.Log($"Medium: {statementsQuote.medium}\n");
            LambdaLogger.Log($"Quality: {statementsQuote.quality}\n");
            LambdaLogger.Log($"Polite: {statementsQuote.polite}\n");

            List<Quote> responses = db.GetAllResponses().Result;
            Int32 responsesCount = responses.Count;

            Int32 responsesQuoteIndex = new Random().Next(responsesCount) + 1;

            Quote responsesQuote = responses[responsesQuoteIndex];
            LambdaLogger.Log($"Quote {responsesQuoteIndex}: {responsesQuote.quoteText}\n");
            LambdaLogger.Log($"UUID: {responsesQuote.uuid}\n");
            LambdaLogger.Log($"Medium: {responsesQuote.medium}\n");
            LambdaLogger.Log($"Quality: {responsesQuote.quality}\n");
            LambdaLogger.Log($"Polite: {responsesQuote.polite}\n");


            var tweeter = new Tweeter(ApiKey, ApiSecretKey, AccessToken, AccessTokenSecret);
            String tweet = String.Empty;

            tweet = DecideTweetText(responsesQuote);
            UInt64 id = tweeter.MaybeTweet(tweet, false);

            // Reply to a tweet...

            List<Status> tweets = tweeter.GetTweets("@SAghdashloo", 20);
            foreach (Status tweeted in tweets)
            {
                LambdaLogger.Log($"TweetID: {tweeted.StatusID}\n");
                LambdaLogger.Log($"    CreatedAt: {tweeted.CreatedAt}\n");
                LambdaLogger.Log($"    User: {tweeted.User.ScreenNameResponse}\n");
                LambdaLogger.Log($"    Text: {tweeted.FullText}\n");

                ulong tweetId = tweeted.StatusID;

                // 4)     Get random reply
                // 5)     ReplyAsync(ulong tweetID, string status)
            }

            return responsesQuote.quoteText;
        }

        public String DecideTweetText(Quote quote)
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
