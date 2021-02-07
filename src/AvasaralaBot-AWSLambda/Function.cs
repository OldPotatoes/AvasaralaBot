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
            Int32 count = statements.Count;

            Int32 quoteIndex = new Random().Next(count) + 1;

            Quote quote = statements[quoteIndex];
            LambdaLogger.Log($"Quote {quoteIndex}: {quote.quoteText}\n");
            LambdaLogger.Log($"UUID: {quote.uuid}\n");
            LambdaLogger.Log($"Medium: {quote.medium}\n");
            LambdaLogger.Log($"Quality: {quote.quality}\n");
            LambdaLogger.Log($"Polite: {quote.polite}\n");

            var tweeter = new Tweeter(ApiKey, ApiSecretKey, AccessToken, AccessTokenSecret);
            String tweet = String.Empty;

            tweet = DecideTweetText(quote);
            UInt64 id = tweeter.MaybeTweet(tweet, true);

            return quote.quoteText;
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
