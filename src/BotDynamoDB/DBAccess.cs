using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotDynamoDB
{
    public class DBAccess
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _context;

        private const String TableName = "WisdomOfAvasarala";

        public DBAccess()
        {
            _client = new AmazonDynamoDBClient();
            _context = new DynamoDBContext(_client);
        }

        public async Task<Quote> Single(String uuid)
        {
            return await _context.LoadAsync<Quote>(uuid);
        }

        public async Task<List<Quote>> GetAllStatements()
        {
            var statements = new List<Quote>();

            var request = new ScanRequest
            {
                TableName = "WisdomOfAvasarala",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":s", new AttributeValue { S = "1" } },
                    { ":t", new AttributeValue { S = "0" } }
                },
                FilterExpression = "statement = :s AND tweeted = :t"
            };

            var resp = await _client.ScanAsync(request);

            foreach (Dictionary<string, AttributeValue> item in resp.Items)
            {
                var quote = new Quote();
                quote.medium = item["medium"].S;
                if (quote.medium == "TV")
                {
                    quote.uuid = item["uuid"].S;
                    quote.season = item["season"].S;
                    quote.episode = item["episode"].S;
                    quote.runningTime = item["runningTime"].S;
                }
                Boolean found;
                if (quote.medium == "Book")
                {
                    found = Int32.TryParse(item["page"].S, out Int32 page);
                    quote.uuid = item["uuid"].S;
                    quote.book = item["book"].S;
                    quote.chapter = item["chapter"].S;
                    quote.page = (found ? page : 0);
                }

                found = Int32.TryParse(item["quality"].S, out Int32 quality);
                quote.quality = (found ? quality : 0);
                quote.polite = item["polite"].S == "1";
                quote.statement = item["statement"].S == "1";
                quote.reply = item["reply"].S == "1";
                quote.tweeted = item["tweeted"].S == "1";
                quote.quoteText = item["quote"].S;

                statements.Add(quote);
            }

            LambdaLogger.Log($"Statements Count: {statements.Count}\n");

            return statements;
        }

        public async Task<List<Quote>> GetAllResponses()
        {
            var responses = new List<Quote>();

            var request = new ScanRequest
            {
                TableName = "WisdomOfAvasarala",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":s", new AttributeValue { S = "1" }}
                },
                FilterExpression = "reply = :s"
            };

            var resp = await _client.ScanAsync(request);

            foreach (Dictionary<string, AttributeValue> item in resp.Items)
            {
                var quote = new Quote();

                quote.medium = item["medium"].S;
                if (quote.medium == "TV")
                {
                    quote.uuid = item["uuid"].S;
                    quote.season = item["season"].S;
                    quote.episode = item["episode"].S;
                    quote.runningTime = item["runningTime"].S;
                }
                Boolean found;
                if (quote.medium == "Book")
                {
                    found = Int32.TryParse(item["page"].S, out Int32 page);
                    quote.uuid = item["uuid"].S;
                    quote.book = item["book"].S;
                    quote.chapter = item["chapter"].S;
                    quote.page = (found ? page : 0);
                }

                found = Int32.TryParse(item["quality"].S, out Int32 quality);
                quote.quality = (found ? quality : 0);
                quote.polite = item["polite"].S == "1";
                quote.statement = item["statement"].S == "1";
                quote.reply = item["reply"].S == "1";
                quote.quoteText = item["quote"].S;

                responses.Add(quote);
            }

            LambdaLogger.Log($"Responses Count: {responses.Count}\n");

            return responses;
        }

        public async Task<DateTime> GetLastReplyTime()
        {
            DateTime lastReplyTime = new DateTime();

            var request = new QueryRequest
            {
                TableName = "RepliesOfAvasarala",
                IndexName = "dummy-replyTime-index",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":vDummy", new AttributeValue { N = "1" }}
                },
                KeyConditionExpression = "dummy = :vDummy",
                ScanIndexForward = false,
                Limit = 1
            };

            var response = await _client.QueryAsync(request);
            LambdaLogger.Log($"Items Count: {response.Items.Count}\n");

            Boolean found = false;
            Int64 replyTimeNumber = 0;
            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                found = Int64.TryParse(item["replyTime"].N, out replyTimeNumber);
            }

            if (found)
            {
                //20210131235959
                Int32 second = (Int32)(replyTimeNumber % 100);
                replyTimeNumber /= 100;
                Int32 minute = (Int32)(replyTimeNumber % 100);
                replyTimeNumber /= 100;
                Int32 hour = (Int32)(replyTimeNumber % 100);
                replyTimeNumber /= 100;
                Int32 day = (Int32)(replyTimeNumber % 100);
                replyTimeNumber /= 100;
                Int32 month = (Int32)(replyTimeNumber % 100);
                replyTimeNumber /= 100;
                Int32 year = (Int32)(replyTimeNumber);
                lastReplyTime = new DateTime(year, month, day, hour, minute, second);
                LambdaLogger.Log($"Year {year}, month {month}, day {day}, hour {hour}, minute {minute}, second {second}\n");
            }

            return lastReplyTime;
        }

        public async Task<Boolean> SetLastReplyTime(DateTime lastReplyTime, Int32 repliesMade)
        {
            LambdaLogger.Log($"lastReplyTime {lastReplyTime}\n");
            Table repliesTable = Table.LoadTable(_client, "RepliesOfAvasarala");

            var replyRecord = new Document();
            replyRecord["uuid"] = Guid.NewGuid();
            replyRecord["repliesMade"] = repliesMade;
            replyRecord["replyTime"] = Int64.Parse(lastReplyTime.ToString("yyyyMMddHHmmss"));
            replyRecord["dummy"] = 1;

            await repliesTable.PutItemAsync(replyRecord);
            LambdaLogger.Log($"Last reply time {lastReplyTime}, replies made {repliesMade} written to DB\n");

            return true;
        }

        public async Task<Int32> CountStatements()
        {
            var request = new ScanRequest
            {
                TableName = "WisdomOfAvasarala",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":qt", new AttributeValue { S = "1" }}
                },
                FilterExpression = "statement = :qt"
                //FilterExpression = "quoteType <> :qt"
            };

            var response = await _client.ScanAsync(request);
            LambdaLogger.Log($"Consumed Capacity: {response.ConsumedCapacity}\n");
            LambdaLogger.Log($"Content Length: {response.ContentLength}\n");
            LambdaLogger.Log($"Count: {response.Count}\n");
            LambdaLogger.Log($"Scanned Count: {response.ScannedCount}\n");
            LambdaLogger.Log($"Items Count: {response.Items.Count}\n");

            return response.Items.Count;
        }

        public async Task<QuoteCollection> GetQuotes(string paginationToken = "")
        {
            var table = _context.GetTargetTable<Quote>();

            var scanOps = new ScanOperationConfig();
            if (!string.IsNullOrEmpty(paginationToken))
            {
                scanOps.PaginationToken = paginationToken;
            }

            var results = table.Scan(scanOps);
            List<Document> data = await results.GetNextSetAsync();
            IEnumerable<Quote> quotes = _context.FromDocuments<Quote>(data);

            return new QuoteCollection
            {
                PaginationToken = results.PaginationToken,
                Quotes = quotes
            };
        }

        public void SetTweeted(Quote statement)
        {
            LambdaLogger.Log($"Setting tweeted for statement for uuid {statement.uuid}\n");
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();

            var request = new UpdateItemRequest
            {
                TableName = "WisdomOfAvasarala",
                Key = new Dictionary<string, AttributeValue>()
                {
                    { "uuid", new AttributeValue { S = statement.uuid } }
                },
                ExpressionAttributeNames = new Dictionary<string, string>()
                {
                    {"#T", "tweeted"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                {
                    {":t",new AttributeValue {S = "1"}}
                },
                UpdateExpression = "SET #T = :t"
            };

            var response = client.UpdateItemAsync(request).Result;
        }

        public void ResetTweetedValues()
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();

            Int32 item = 0;
            while (true)
            {
                ++item;
                var request = new UpdateItemRequest
                {
                    TableName = "WisdomOfAvasarala",
                    Key = new Dictionary<string, AttributeValue>()
                    {
                        { "uuid", new AttributeValue { S = item.ToString() } }
                    },
                    ExpressionAttributeNames = new Dictionary<string, string>()
                    {
                        {"#T", "tweeted"}
                    },
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                    {
                        {":t",new AttributeValue {S = "0"}}
                    },
                    UpdateExpression = "SET #T = :t"
                };

                try
                {
                    var response = client.UpdateItemAsync(request).Result;
                }
                catch (Exception ex)
                {
                    LambdaLogger.Log($"ERROR - {ex}\n");
                    LambdaLogger.Log($"ERROR - {ex.Message}\n");
                    break;
                }
                LambdaLogger.Log($"Reset all statements\n");
            }
        }
    }
}
