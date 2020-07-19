using System;
using System.Data.SQLite;

public class CreateTable
{
    static void Main()
    {
        string cs = @"URI=file:C:\Users\Jano\Documents\test.db";

        using var con = new SQLiteConnection(cs);
        con.Open();

        using var cmd = new SQLiteCommand(con);

        cmd.CommandText = "DROP TABLE IF EXISTS quotes";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"CREATE TABLE quotes(id INTEGER PRIMARY KEY,
                    book TEXT, chapter INT, pageNum INT, words TEXT)";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "INSERT INTO quotes(book, chapter, pageNum, words) VALUES('Calibans War', 5, 48, 'Fuck him')";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "INSERT INTO quotes(book, chapter, pageNum, words) VALUES('Calibans War', 5, 49, 'They’re all fucking men')";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "INSERT INTO quotes(book, chapter, pageNum, words) VALUES('Calibans War', 5, 53, 'So get the fuck out.')";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "INSERT INTO quotes(book, chapter, pageNum, words) VALUES('Calibans War', 12, 125, 'These cunts are digging into my grandma time.')";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "INSERT INTO quotes(book, chapter, pageNum, words) VALUES('Calibans War', 12, 129, 'If he can’t play with the big kids, he shouldn’t be a fucking admiral.')";
        cmd.ExecuteNonQuery();

        Console.WriteLine("Table quotes created");
    }
}
