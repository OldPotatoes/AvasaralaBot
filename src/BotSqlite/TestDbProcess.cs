using System;
using Dapper;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace BotSqlite
{
    public class TestDbProcess
    {
        private SQLiteConnection connection;
        private string dbConnectionString; // = @"Data Source = D:\Code\WisdomOfAvasarala.db";

        public TestDbProcess(String DbConn)
        {
            dbConnectionString = DbConn;
        }

        public void DoSomething()
        {
            DatabaseAccess();
            long count = CountNumQuote();
            Console.WriteLine("Number of rows = " + count);
            var quotes = GetQuote(count);
            foreach(var quote in quotes)
            {
                Console.WriteLine(quote);
            }
            Console.WriteLine("Which quote do you want?");
            string indexQuery = Console.ReadLine();
            int indexQueryInt;
            Int32.TryParse(indexQuery, out indexQueryInt);
            var indexQuotes = GetSpecificQuote(indexQueryInt);
            if (indexQueryInt > 1)
            {
                foreach(var indexQuote in indexQuotes)
                {
                    Console.WriteLine(indexQuote);
                }
            }
            else
            {
                Console.WriteLine("Please input a number greater than zero");
            }
            
            string randQuote = GetRandomQuote();
            Console.WriteLine(randQuote);
        }
        
        public void DatabaseAccess()
        {
            connection = new SQLiteConnection(dbConnectionString);
        }

        public long CountNumQuote()
        {
            string sql = "SELECT COUNT(*) FROM WisdomOfAvasarala";
            long result = connection.Query<long>(sql).First();
            return result;
        }
        public List<Quote> GetQuote(long count = 100)
        {
            string sql = "SELECT Book, Chapter, Page, Quote FROM WisdomOfAvasarala LIMIT @Count";
            List<Quote> result = connection.Query<Quote>(sql, new { Count = count }).AsList();
            return result;
        }
        public List<Quote> GetSpecificQuote(int indexQueryInt)
        {
            string sql = "SELECT Book, Chapter, Page, Quote FROM WisdomOfAvasarala WHERE Id = @Index";
            List<Quote> result = connection.Query<Quote>(sql, new { Index = indexQueryInt }).AsList();
            return result;
        }
        public string GetRandomQuote()
        {
            string maxSql = "SELECT MAX(Id) FROM WisdomOfAvasarala";
            int max = connection.Query<int>(maxSql).First();
            var rand = new Random();
            string sql = "SELECT Book, Chapter, Page, Quote FROM WisdomOfAvasarala WHERE Id = @Rand";
            int randNum = rand.Next(max);
            var result = connection.Query<Quote>(sql, new { Rand = randNum });
            Quote resultQuote = result.AsList()[0];
            return resultQuote.quote;
        }
    }
}