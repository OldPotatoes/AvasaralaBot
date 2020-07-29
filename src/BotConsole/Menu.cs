using System;
using BotSqlite;
using BotTweeter;

namespace BotConsole
{
    public class Menu
    {
        public String DatabaseConnectionString { get; set; }

        public Menu(String DbConn)
        {
            DatabaseConnectionString = DbConn;
        }

        public void ShowMenu()
        {
            Int32 numberOfTweets = 20;
            Boolean keyValueSuccess = false;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkBlue;

            var tweeter = new Tweeter(); 

            while (true)
            {
                Console.Clear();

                Console.WriteLine("Wisdom of Avasarala");
                Console.WriteLine();
                Console.WriteLine("Options:");
                Console.WriteLine($"1: Check {tweeter.AVASARALA_ID} mentions");
                Console.WriteLine($"2: Search for {tweeter.AVASARALA_NAME}");
                Console.WriteLine($"3: Search for {tweeter.AVASARALA_TAG}");
                Console.WriteLine($"4: Get last {numberOfTweets} tweets from {tweeter.AVASARALA_ID}");
                Console.WriteLine("5: Test tweet some wisdom (not implemented)");
                Console.WriteLine("6: Tweet some wisdom (not implemented)");
                Console.WriteLine();
                Console.WriteLine("<ESC>: Quit");

                ConsoleKeyInfo keyInfo = Console.ReadKey(false);
                if (keyInfo.Key == ConsoleKey.Escape)
                    return;

                var keyChar = keyInfo.KeyChar;
                Int32 keyValue;
                keyValueSuccess = Int32.TryParse(keyChar.ToString(), out keyValue);

                if (keyValue == 1)
                {
                    tweeter.GetTweets(tweeter.AVASARALA_ID);
                    PressAKey();
                }
                else if (keyValue == 2)
                {
                    tweeter.GetTweets(tweeter.AVASARALA_NAME);
                    PressAKey();
                }
                else if (keyValue == 3)
                {
                    tweeter.GetTweets(tweeter.AVASARALA_TAG);
                    PressAKey();
                }
                else if (keyValue == 4)
                {
                    tweeter.GetTweetsFrom(tweeter.AVASARALA_ID, numberOfTweets);
                    PressAKey();
                }
                else if (keyValue == 5)
                {
                    DatabaseAccess dbAccess = new DatabaseAccess(DatabaseConnectionString);
                    string tweetText = dbAccess.GetRandomQuote();
                    tweeter.MaybeTweet(tweetText, false);
                    PressAKey();
                }
                else if (keyValue == 6)
                {
                    DatabaseAccess dbAccess = new DatabaseAccess(DatabaseConnectionString);
                    string tweetText = dbAccess.GetRandomQuote();
                    tweeter.MaybeTweet(tweetText, true);
                    PressAKey();
                }
            }
        }

        public void PressAKey()
        {
            Console.WriteLine();
            Console.WriteLine("Press a key to continue.");

            Console.ReadKey(false);
        }
    }
}