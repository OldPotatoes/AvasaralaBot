using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using BotSqlite;
using BotTweeter;

namespace BotService
{
    const int SECOND = 1000;
    const int MINUTE = SECOND * 60;
    const int HOUR = MINUTE * 60;
    const int DAY = HOUR * 24;

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public static IConfigurationRoot Configuration  { get; private set; }

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            String DbConn = Configuration.GetConnectionString("DataConnection");

            Boolean actuallyTweeting = false;

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTime.Now);

                _logger.LogInformation("(Not actually) sending a tweet at: {time}", DateTime.Now);
                DatabaseAccess dbAccess = new DatabaseAccess(DbConn);
                string tweetText = dbAccess.GetRandomQuote();
                _logger.LogInformation($"Quote to (not actually) tweet: {tweetText}");

                var tweeter = new Tweeter(); 
                tweeter.MaybeTweet(tweetText, actuallyTweeting);

                await Task.Delay(12*HOUR, stoppingToken);
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton<IConfigurationRoot>(Configuration);
        }
    }
}
