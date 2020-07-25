using System;
using Dapper;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace BotSqlite
{
    public class TestDbProcess
    {
        public void DoSomething()
        {
            Console.WriteLine("Big Database Work Happening Right Now...");
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
            foreach(var indexQuote in indexQuotes)
            {
                Console.WriteLine(indexQuote);
            }

        }
        
        private SQLiteConnection connection;
        private string dbConnectionString = @"Data Source = D:\Code\WisdomOfAvasarala.db";

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

    }
}