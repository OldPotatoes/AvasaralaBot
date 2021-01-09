using Amazon.Lambda.Core;
using BotDynamoDB;
using BotTweeter;
using System;

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

            var db = new DBAccess(ApiKey, ApiSecretKey);

            Int32 count = db.CountQuotes().Result;
            Int32 quoteIndex = new Random().Next(count) + 1;

            Quote quote = db.Single(quoteIndex).Result;
            LambdaLogger.Log($"Quote {quoteIndex}: {quote.QuoteText}\n");

            var tweeter = new Tweeter(ApiKey, ApiSecretKey, AccessToken, AccessTokenSecret);
            tweeter.MaybeTweet(quote.QuoteText, true);

            return quote.QuoteText;
        }

    }
}
