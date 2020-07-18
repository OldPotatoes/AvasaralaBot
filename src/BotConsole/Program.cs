using System;
using BotTweeter;

namespace BotConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            var tweeter = new BotTweeter.Tweeter();
            tweeter.Menu();
        }
    }
}
