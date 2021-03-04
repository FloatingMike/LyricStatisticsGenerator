using System.Collections.Generic;
using System.Threading.Tasks;
using AireLogicTest.LyricStatistics.Dtos;
using Microsoft.Extensions.Logging;

namespace AireLogicTest.LyricStatistics
{
    /// <summary>
    /// A simple file based cache for the song lyrics
    /// </summary>
    public class CachingSongLyricService : FileCachingServiceBase, ISongLyricService
    {
        private readonly ISongLyricService _lyricService;
        private readonly CachingConfiguration _config;
        private readonly ILogger<CachingSongLyricService> _logger;
        private Dictionary<string, Dictionary<string, LyricDto>> _lyricCache;
        
        public CachingSongLyricService(ISongLyricService lyricService, CachingConfiguration config, ILogger<CachingSongLyricService> logger)
        {
            _lyricService = lyricService;
            _config = config;
            _logger = logger;
            InitialiseCache();
        }
        
        public async Task<LyricDto> GetLyricForTrack(string artistName, string trackName)
        {
            if (!_lyricCache.ContainsKey(artistName))
            {
                _lyricCache.Add(artistName, new Dictionary<string, LyricDto>());
            }

            if (!_lyricCache[artistName].ContainsKey(trackName))
            {
                _lyricCache[artistName].Add(trackName, await _lyricService.GetLyricForTrack(artistName, trackName));
                WriteCache();
            }

            return _lyricCache[artistName][trackName];
        }
        
        
        private void WriteCache()
        {
            if (_config.EnableFileCaching)
            {
                _logger.LogInformation("Saving Lyric Cache to Disk");
                SerialiseToFile(_lyricCache, _config.LyricCacheFileName);
                _logger.LogInformation("Finished Saving Lyric Cache to Disk");
            }
        }

        private void InitialiseCache()
        {
            if (_config.EnableFileCaching)
            {
                _lyricCache =
                    LoadFromFile<Dictionary<string, Dictionary<string, LyricDto>>>(_config.LyricCacheFileName);
            }
            else
            {
                _lyricCache = new Dictionary<string, Dictionary<string, LyricDto>>();
            }
        }
        
    }
}