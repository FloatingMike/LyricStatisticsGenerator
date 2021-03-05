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

            ArtistResultDto artistResult = await MakeRequestWithDelay<ArtistResultDto>($"{_config.MusicBrainzRootUrl}release?artist={artistKey}&limit=100&offset=0", _config.MusicBrainzTimeoutMilliseconds);
            var offset = 100;

            var trackNames = new HashSet<string>();
            var releases = new HashSet<string>();
            
            foreach (var release in artistResult.Releases)
            {
                releases.Add(release.Id);
            }
            
            while (artistResult.Releases.Count == 100)
            {
                artistResult = await MakeRequestWithDelay<ArtistResultDto>($"{_config.MusicBrainzRootUrl}release?artist={artistKey}&limit=100&offset={offset}", _config.MusicBrainzTimeoutMilliseconds);
                foreach (var release in artistResult.Releases)
                {
                    releases.Add(release.Id);
                }
                offset += 100;
                _logger.LogInformation($"Found {artistResult.Releases.Count} releases for artist '{artistKey}' making a total of {releases.Count}");
            }
            
            foreach (var release in releases)
            {
                _logger.LogInformation($"Getting Tracks for Release with id {release}");
                var recordingResult = await MakeRequestWithDelay<ReleaseResultDto>($"{_config.MusicBrainzRootUrl}release/{release}?inc=recordings", _config.MusicBrainzTimeoutMilliseconds);
                // We remove anything in brackets from the song name, it causes duplicates with live recordings and other notable releases
                foreach (var s in recordingResult.Media.SelectMany(m =>
                    m.Tracks.Select(t => Regex.Replace(t.Title, @"\(.*?\)", "").Trim())))
                {
                    trackNames.Add(s);
                }
            }

            return trackNames.ToList();
        }
    }
}