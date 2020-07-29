using System;
using Dapper;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace BotSqlite

{
    public class DatabaseAccess
    {
        private SQLiteConnection connection;
        private string dbConnectionString; // = @"Data Source = D:\Code\WisdomOfAvasarala.db";

        public DatabaseAccess(string DbConn)
        {
            dbConnectionString = DbConn;
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