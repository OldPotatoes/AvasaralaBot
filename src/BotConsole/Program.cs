using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace BotConsole
{
    class Program
    {
        public static IConfigurationRoot Configuration  { get; private set; }

        static void Main(string[] args)
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            String DbConn = Configuration.GetConnectionString("DataConnection");

            // IConfigurationSection listOfEmails = Configuration.GetSection("EmailAddresses");
            // foreach (var email in listOfEmails.AsEnumerable())
            //     Console.WriteLine($"{email.Key}:{email.Value}");

            var menu = new Menu(DbConn);
            menu.ShowMenu();
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
