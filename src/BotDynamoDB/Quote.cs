using Amazon.DynamoDBv2.DataModel;
using System;

namespace BotDynamoDB
{
    [DynamoDBTable("WisdomOfAvasarala")]
    public class Quote
    {
        [DynamoDBHashKey]
        public int ID { get; set; }
        public string Book { get; set; }
        public int Chapter { get; set; }
        public int PageNum { get; set; }
        [DynamoDBProperty("Quote")]
        public string QuoteText { get; set; }

        public override string ToString()
        {
            return $"Quote: {QuoteText}";
        }
    }
}