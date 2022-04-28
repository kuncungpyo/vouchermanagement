using Microsoft.Extensions.Configuration;

using System.Reflection;
using DbUp;
using System;
using System.IO;

namespace DataAccess
{
    class Program
    {
        static int Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());

            builder.AddJsonFile("dbup.appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var connectionInfo = connectionString.Split(';');

            foreach (var info in connectionInfo)
            {
                switch ((info.Split('='))[0])
                {
                    case "server":
                        Console.WriteLine("Data Source : " + (info.Split('='))[1]);
                        break;
                    case "database":
                        Console.WriteLine("DB Name     : " + (info.Split('='))[1]);
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine("========================================================================");

            var upgrader = DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .WithTransaction()
                .WithExecutionTimeout(TimeSpan.FromSeconds(1800))
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                Console.ReadLine();
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            Console.ReadLine();
            return 0;
        }
    }
}
