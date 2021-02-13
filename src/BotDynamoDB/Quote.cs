using Amazon.DynamoDBv2.DataModel;
using System;

namespace BotDynamoDB
{
    [DynamoDBTable("WisdomOfAvasarala")]
    public class Quote
    {
        [DynamoDBHashKey]
        public string uuid { get; set; }
        public string medium { get; set; }
        public string book { get; set; }
        public string chapter { get; set; }
        public int? page { get; set; }
        public string season { get; set; }
        public string episode { get; set; }
        public string runningTime { get; set; }
        public int quality { get; set; }
        public bool polite { get; set; }
        public bool statement { get; set; }
        public bool reply { get; set; }

        [DynamoDBProperty("quote")]
        public string quoteText { get; set; }

        public override string ToString()
        {
            return $"Quote: {quoteText}";
        }
    }
}