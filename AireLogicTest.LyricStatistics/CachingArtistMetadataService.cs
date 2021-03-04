using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using System.Threading.Tasks;

namespace AireLogicTest.LyricStatistics
{
    /// <summary>
    /// The caching version of the artist metadata service, is mostly just a wrapper around another implementation of the artist metadata service
    /// </summary>
    public class CachingArtistMetadataService : FileCachingServiceBase, IArtistMetadataService
    {
        private readonly IArtistMetadataService _metadataService;
        private readonly CachingConfiguration _config;
        private Dictionary<string, Dictionary<string, string>> _artistSearchCache;
        private Dictionary<string, List<string>> _trackCache;

        public CachingArtistMetadataService(IArtistMetadataService metadataService, CachingConfiguration config)
        {
            _metadataService = metadataService;
            _config = config;
            InitialiseCache();
        }

        public async Task<Dictionary<string, string>> FindArtistKey(string searchText)
        {
            if (!_artistSearchCache.ContainsKey(searchText))
            {
                _artistSearchCache.Add(searchText, await _metadataService.FindArtistKey(searchText));
                WriteCache();
            }

            return _artistSearchCache[searchText];
        }

        public async Task<List<string>> GetTrackNamesForArtist(string artistKey)
        {
            if (!_trackCache.ContainsKey(artistKey))
            {
                _trackCache.Add(artistKey, await _metadataService.GetTrackNamesForArtist(artistKey));
                WriteCache();
            }

            return _trackCache[artistKey];
        }

        private void WriteCache()
        {
            if (_config.EnableFileCaching)
            {
                SerialiseToFile(_artistSearchCache, _config.ArtistSearchCacheFileName);
                SerialiseToFile(_trackCache, _config.ArtistTrackNameCacheFileName);
            }
        }

        private void InitialiseCache()
        {
            if (_config.EnableFileCaching)
            {
                _artistSearchCache =
                    LoadFromFile<Dictionary<string, Dictionary<string, string>>>(_config.ArtistSearchCacheFileName);
                _trackCache = LoadFromFile<Dictionary<string, List<string>>>(_config.ArtistTrackNameCacheFileName);
            }
            else
            {
                _artistSearchCache = new Dictionary<string, Dictionary<string, string>>();
                _trackCache = new Dictionary<string, List<string>>();
            }
        }
    }
}