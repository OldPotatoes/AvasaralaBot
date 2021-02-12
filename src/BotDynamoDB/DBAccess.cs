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

            var response = await _client.ScanAsync(request);

            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                var quote = new Quote();
                Boolean found = false;

                quote.medium = item["medium"].S;
                if (quote.medium == "TV")
                {
                    quote.uuid = item["uuid"].S;
                    quote.season = item["season"].S;
                    quote.episode = item["episode"].S;
                    quote.runningTime = item["runningTime"].S;
                }
                if (quote.medium == "Book")
                {
                    Int32 page = 0;
                    found = Int32.TryParse(item["page"].S, out page);

                    quote.uuid = item["uuid"].S;
                    quote.book = item["book"].S;
                    quote.chapter = item["chapter"].S;
                    quote.page = (found ? page : 0);
                }

                Int32 quality = 0;
                found = Int32.TryParse(item["quality"].S, out quality);

                quote.quality = (found ? quality : 0);
                quote.polite = item["polite"].S == "1";
                quote.statement = item["statement"].S == "1";
                quote.response = item["response"].S == "1";
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
                    {":s", new AttributeValue { S = "1" }},
                    {":q", new AttributeValue { S = "4" }}
                },
                FilterExpression = "responses = :s AND quality <> :q"
            };

            var response = await _client.ScanAsync(request);

            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                var quote = new Quote();
                Boolean found = false;

                quote.medium = item["medium"].S;
                if (quote.medium == "TV")
                {
                    quote.uuid = item["uuid"].S;
                    quote.season = item["season"].S;
                    quote.episode = item["episode"].S;
                    quote.runningTime = item["runningTime"].S;
                }
                if (quote.medium == "Book")
                {
                    Int32 page = 0;
                    found = Int32.TryParse(item["page"].S, out page);

                    quote.uuid = item["uuid"].S;
                    quote.book = item["book"].S;
                    quote.chapter = item["chapter"].S;
                    quote.page = (found ? page : 0);
                }

                Int32 quality = 0;
                found = Int32.TryParse(item["quality"].S, out quality);

                quote.quality = (found ? quality : 0);
                quote.polite = item["polite"].S == "1";
                quote.statement = item["statement"].S == "1";
                quote.response = item["response"].S == "1";
                quote.quoteText = item["quote"].S;

                responses.Add(quote);
            }

            LambdaLogger.Log($"Statements Count: {responses.Count}\n");

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



        //public void UpdateItems()
        //{

        //}

        //private void UpdateMultipleAttributes()
        //{
        //    var request = new UpdateItemRequest
        //    {
        //        Key = new Dictionary<string, AttributeValue>()
        //    {
        //        { "Id", new AttributeValue {
        //              N = "1000"
        //          } }
        //    },
        //        // Perform the following updates:
        //        // 1) Add two new authors to the list
        //        // 1) Set a new attribute
        //        // 2) Remove the ISBN attribute
        //        ExpressionAttributeNames = new Dictionary<string, string>()
        //    {
        //        {"#A","Authors"},
        //        {"#NA","NewAttribute"},
        //        {"#I","ISBN"}
        //    },
        //        ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
        //    {
        //        {":auth",new AttributeValue {
        //             SS = {"Author YY", "Author ZZ"}
        //         }},
        //        {":new",new AttributeValue {
        //             S = "New Value"
        //         }}
        //    },

        //        UpdateExpression = "ADD #A :auth SET #NA = :new REMOVE #I",

        //        TableName = TableName,
        //        ReturnValues = "ALL_NEW" // Give me all attributes of the updated item.
        //    };

        //    var response = _client.UpdateItemAsync(request).Result;

        //    // Check the response.
        //    var attributeList = response.Attributes; // attribute list in the response.
        //                                             // print attributeList.
        //    Console.WriteLine("\nPrinting item after multiple attribute update ............");
        //    PrintItem(attributeList);
        //}

        //private static void PrintItem(Dictionary<string, AttributeValue> attributeList)
        //{
        //    foreach (KeyValuePair<string, AttributeValue> kvp in attributeList)
        //    {
        //        string attributeName = kvp.Key;
        //        AttributeValue value = kvp.Value;

        //        Console.WriteLine(
        //            attributeName + " " +
        //            (value.S == null ? "" : "S=[" + value.S + "]") +
        //            (value.N == null ? "" : "N=[" + value.N + "]") +
        //            (value.SS == null ? "" : "SS=[" + string.Join(",", value.SS.ToArray()) + "]") +
        //            (value.NS == null ? "" : "NS=[" + string.Join(",", value.NS.ToArray()) + "]")
        //            );
        //    }
        //    Console.WriteLine("************************************************");
        //}

        //public List<String> GetTables()
        //{
        //    var client = new AmazonDynamoDBClient();

        //    LambdaLogger.Log("Getting list of tables\n");
        //    List<String> currentTables = client.ListTablesAsync().Result.TableNames;
        //    LambdaLogger.Log($"Number of tables: {currentTables.Count}\n");

        //    return currentTables;
        //}

        //public void CreateTable()
        //{
        //    var request = new CreateTableRequest
        //    {
        //        TableName = "AnimalsInventory",
        //        AttributeDefinitions = new List<AttributeDefinition>
        //        {
        //            new AttributeDefinition
        //            {
        //                AttributeName = "Id",
        //                AttributeType = "N"
        //            },
        //            new AttributeDefinition
        //            {
        //                AttributeName = "Type",
        //                AttributeType = "S"
        //            }
        //        },
        //        KeySchema = new List<KeySchemaElement>
        //        {
        //            new KeySchemaElement
        //            {
        //                AttributeName = "Id",
        //                KeyType = "HASH"
        //            },
        //            new KeySchemaElement
        //            {
        //                AttributeName = "Type",
        //                KeyType = "RANGE"
        //            },
        //        },
        //        ProvisionedThroughput = new ProvisionedThroughput
        //        {
        //            ReadCapacityUnits = 10,
        //            WriteCapacityUnits = 5
        //        },
        //    };

        //    var client = new AmazonDynamoDBClient();
        //    var response = client.CreateTableAsync(request).Result;

        //    LambdaLogger.Log($"Table created with request ID: {response.ResponseMetadata.RequestId}\n");
        //}
    }
}
