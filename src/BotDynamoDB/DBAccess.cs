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

        public DBAccess()
        {
            _client = new AmazonDynamoDBClient();
            _context = new DynamoDBContext(_client);
        }

        public async Task<Quote> Single(Int32 id)
        {
            return await _context.LoadAsync<Quote>(id);
        }

        public async Task<Int32> CountQuotes()
        {
            var request = new ScanRequest
            {
                TableName = "WisdomOfAvasarala",
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

        public List<String> GetTables()
        {
            var client = new AmazonDynamoDBClient();

            LambdaLogger.Log("Getting list of tables\n");
            List<String> currentTables = client.ListTablesAsync().Result.TableNames;
            LambdaLogger.Log($"Number of tables: {currentTables.Count}\n");

            return currentTables;
        }

        public void CreateTable()
        {
            var request = new CreateTableRequest
            {
                TableName = "AnimalsInventory",
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Id",
                        AttributeType = "N"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "Type",
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Id",
                        KeyType = "HASH"
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "Type",
                        KeyType = "RANGE"
                    },
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 10,
                    WriteCapacityUnits = 5
                },
            };

            var client = new AmazonDynamoDBClient();
            var response = client.CreateTableAsync(request).Result;

            LambdaLogger.Log($"Table created with request ID: {response.ResponseMetadata.RequestId}\n");
        }
    }
}
