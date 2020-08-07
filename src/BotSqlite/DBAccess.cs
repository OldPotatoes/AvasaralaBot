using Dapper;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace APCSP_Final_Project
{
    class DatabaseAccess
    {
        private SQLiteConnection connection;
        private string dbConnectionString; // = @"Data Source = D:\Code\WisdomOfAvasarala.db";

        public TestDbProcess(String DbConn)
        {
            dbConnectionString = DbConn;
        }
    }
}