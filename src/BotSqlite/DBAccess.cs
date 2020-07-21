using Dapper;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace APCSP_Final_Project
{
    class DatabaseAccess
    {
        private SQLiteConnection connection;

        public DatabaseAccess()
        {
            connection = new SQLiteConnection("Data Source=" + @"..\..\..\Data\AllSets.sqlite");
        }

        public long CountCards()
        {
            string sql = "SELECT COUNT(*) FROM cards";
            long result = connection.Query<long>(sql).First();
            return result;
        }
        public List<Tweet> GetCards(long count)
        {
            string sql = "SELECT Id, Name, Artist, Flavortext FROM cards LIMIT @Count";
            List<Tweet> result = connection.Query<Tweet>(sql, new { Count = count }).AsList();
            return result;
        }
    }
}