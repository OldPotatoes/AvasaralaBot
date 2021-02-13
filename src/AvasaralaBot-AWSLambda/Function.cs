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
        const String SearchText = "@SAghdashloo";
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
            List<Quote> statementsList = db.GetAllStatements().Result;
            Int32 count = statementsList.Count;
            Int32 quoteIndex = new Random().Next(count) + 1;

            Quote statement = statementsList[quoteIndex];
            LambdaLogger.Log($"Quote {quoteIndex}: {statement.quoteText}\n");

            String tweet = DecideTweetText(statement);
            var tweeter = new Tweeter(ApiKey, ApiSecretKey, AccessToken, AccessTokenSecret);
            UInt64 id = tweeter.MaybeTweet(tweet, ActuallyTweet);

            //// Reply to mentions
            //// To Do: Limit search by time
            //// To Do: Only respond to original mentions - not to subsequent tweets in the chain
            //// To Do: Change the value of the SearchText
            //List<Quote> responsesList = db.GetAllResponses().Result;
            //count = responsesList.Count;

            //List<Status> tweets = tweeter.GetTweets(SearchText, 20);
            //foreach (Status tweeted in tweets)
            //{
            //    LambdaLogger.Log($"TweetID: {tweeted.StatusID}\n");
            //    LambdaLogger.Log($"    CreatedAt: {tweeted.CreatedAt}\n");
            //    LambdaLogger.Log($"    User: {tweeted.User.ScreenNameResponse}\n");
            //    LambdaLogger.Log($"    Text: {tweeted.FullText}\n");

            //    Int32 responsesIndex = new Random().Next(count) + 1;
            //    Quote response = responsesList[responsesIndex];
            //    LambdaLogger.Log($"Response {responsesIndex}: {response.quoteText}\n");

            //    tweet = DecideTweetText(response);
            //    id = tweeter.MaybeReply(tweet, tweeted.StatusID, ActuallyTweet);
            //}

            return tweet;
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
