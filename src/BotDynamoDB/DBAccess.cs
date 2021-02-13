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
                    {":s", new AttributeValue { S = "1" }},
                    {":q", new AttributeValue { S = "4" }}
                },
                FilterExpression = "statement = :s AND quality <> :q"
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


        public void BatchWrite()
        {
            var request = DBAddItems.MakeRequest(TableName);
            LambdaLogger.Log($"Request: {request}\n");

            CallBatchWriteTillCompletion(request);
        }

        private void CallBatchWriteTillCompletion(BatchWriteItemRequest request)
        {
            BatchWriteItemResponse response;

            int callCount = 0;
            do
            {
                LambdaLogger.Log("Making request\n");
                response = _client.BatchWriteItemAsync(request).Result;
                callCount++;

                // Check the response.
                var tableConsumedCapacities = response.ConsumedCapacity;
                var unprocessed = response.UnprocessedItems;

                LambdaLogger.Log("Per-table consumed capacity\n");
                foreach (var tableConsumedCapacity in tableConsumedCapacities)
                {
                    LambdaLogger.Log($"{tableConsumedCapacity.TableName} - {tableConsumedCapacity.CapacityUnits}\n");
                }

                LambdaLogger.Log("Unprocessed\n");
                foreach (var unp in unprocessed)
                {
                    LambdaLogger.Log($"{unp.Key} - {unp.Value.Count}\n");
                }

                // For the next iteration, the request will have unprocessed items.
                request.RequestItems = unprocessed;
            } while (response.UnprocessedItems.Count > 0);

            LambdaLogger.Log($"Total # of batch write API calls made: {callCount}\n");
        }
    }
}
