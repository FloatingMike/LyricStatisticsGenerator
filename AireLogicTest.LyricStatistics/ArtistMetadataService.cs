using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AireLogicTest.LyricStatistics.ApiTypes;
using AireLogicTest.LyricStatistics.Configuration;
using Microsoft.Extensions.Logging;

namespace AireLogicTest.LyricStatistics
{
    public class ArtistMetadataService : WebRequestServiceBase, IArtistMetadataService
    {
        private readonly ArtistMetaDataServiceConfiguration _config;
        private readonly ILogger<ArtistMetadataService> _logger;
        
        public ArtistMetadataService(HttpClient client, ArtistMetaDataServiceConfiguration config, IDateTimeProvider dateTimeProvider, ILogger<ArtistMetadataService> logger) : base(client, logger, dateTimeProvider)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<Dictionary<string, string>> FindArtistKey(string searchText)
        {
            var resultDto = await MakeRequestWithDelay<SearchResultDto>($"{_config.MusicBrainzRootUrl}artist?query={searchText}&limit=21", _config.MusicBrainzTimeoutMilliseconds);
            return resultDto?.Artists.ToDictionary(a => a.Id, a => a.Name);
        }

        public async Task<List<string>> GetTrackNamesForArtist(string artistKey)
        {
            // To lookup the lyrics we need to get a list of track names for this artist.
            // We can request the recordings and for each recording request its released track name(s) 
            
            var artistResult = await MakeRequestWithDelay<ArtistResultDto>($"{_config.MusicBrainzRootUrl}artist/{artistKey}?inc=releases", _config.MusicBrainzTimeoutMilliseconds);

            var trackNames = new List<string>();
            
            _logger.LogInformation($"Found {artistResult.Releases.Count} releases for artist '{artistResult.Name}'");
            
            foreach (var release in artistResult.Releases)
            {
                _logger.LogInformation($"Getting Tracks for Release with id {release.Id}");
                var recordingResult = await MakeRequestWithDelay<ReleaseResultDto>($"{_config.MusicBrainzRootUrl}release/{release.Id}?inc=recordings", _config.MusicBrainzTimeoutMilliseconds);
                trackNames.AddRange(recordingResult.Media.SelectMany(m => m.Tracks.Select(t => Regex.Replace(t.Title, @"\(.*?\)", ""))));
            }

            return trackNames.Distinct().ToList();
        }
    }
}