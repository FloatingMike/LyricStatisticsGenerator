using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AireLogicTest.LyricStatistics.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLyricStatisticServices(this IServiceCollection serviceCollection, bool caching = true)
        {
            if (caching)
            {
                serviceCollection.AddScoped<ArtistMetadataService>();
                serviceCollection.AddScoped<SongLyricService>();
                serviceCollection.AddSingleton<IArtistMetadataService, CachingArtistMetadataService>(sp => new CachingArtistMetadataService(sp.GetService<ArtistMetadataService>(), sp.GetService<CachingConfiguration>()));
                serviceCollection.AddSingleton<ISongLyricService, CachingSongLyricService>(sp => new CachingSongLyricService(sp.GetService<SongLyricService>(), sp.GetService<CachingConfiguration>(), sp.GetService<ILogger<CachingSongLyricService>>()));
            }
            else
            {
                serviceCollection.AddScoped<IArtistMetadataService, ArtistMetadataService>();
                serviceCollection.AddScoped<ISongLyricService, SongLyricService>();
            }
            
            serviceCollection.AddScoped<ILyricStatisticsHelper, LyricStatisticsHelper>();

            serviceCollection.AddSingleton<IStringHelper, StringHelper>();
            serviceCollection.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            
            serviceCollection.AddScoped<HttpClient>(sp => {
                var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("LyricAnalysisApplication/0.0.1 (Mike.Hardman.Work+lyrics@gmail.com)");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                return client;
            });
            
            return serviceCollection;
        }
    }
}