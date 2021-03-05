using System;
using System.Dynamic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AireLogicTest.LyricStatistics;
using AireLogicTest.LyricStatistics.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AireLogicTest
{
    class Program
    {
        static Task Main(string[] args)
        {
            // extract arguments
            var caching = args.All(a => a != "--nocache");
            args = args.Where(a => a != "--nocache").ToArray();

            // initialise services
            var provider = CreateProvider(caching);
            
            // resolve lyric service
            var service = provider.GetService<ArtistLyricStatisticsConsoleService>();
            
            // pass arguments to service and execute
            return service?.Execute(args);
        }

        private static IServiceProvider CreateProvider(bool caching = true)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });
            serviceCollection.AddLyricStatisticServices(caching);
            serviceCollection.AddTransient<ArtistLyricStatisticsConsoleService>();
            serviceCollection.AddSingleton<ArtistMetaDataServiceConfiguration>();
            serviceCollection.AddSingleton<SongLyricServiceConfiguration>();
            serviceCollection.AddSingleton<CachingConfiguration>();
            serviceCollection.AddSingleton<IResultPresentationService, ConsoleResultPresentationService>();
            serviceCollection.AddSingleton<IInputService, ConsoleInputService>();
            return serviceCollection.BuildServiceProvider();
        }
    }
}