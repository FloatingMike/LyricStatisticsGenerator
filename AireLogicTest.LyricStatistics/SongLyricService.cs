using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AireLogicTest.LyricStatistics.ApiTypes;
using AireLogicTest.LyricStatistics.Configuration;
using AireLogicTest.LyricStatistics.Dtos;
using Microsoft.Extensions.Logging;

namespace AireLogicTest.LyricStatistics
{
    public class SongLyricService : WebRequestServiceBase, ISongLyricService
    {
        private readonly ILogger<SongLyricService> _logger;
        private readonly SongLyricServiceConfiguration _config;
        private SemaphoreSlim _lyricRequestSemaphore = new SemaphoreSlim(1);
        
        public SongLyricService(HttpClient client, SongLyricServiceConfiguration config, IDateTimeProvider dateTimeProvider, ILogger<SongLyricService> logger) : base(client, logger, dateTimeProvider)
        {
            _logger = logger;
            _config = config;
        }
        
        public async Task<LyricDto> GetLyricForTrack(string artistName, string trackName)
        {
            await _lyricRequestSemaphore.WaitAsync();

            while (true)
            {
                _logger.LogInformation($"Retrieving lyrics for track '{trackName}'");
                try
                {
                    var result = await MakeRequestWithDelay<LyricResult>($"{_config.LyricsOvhUrl}{artistName}/{trackName}", _config.LyricsTimeoutMilliseconds, _config.LyricRetries);

                        if (result != null && string.IsNullOrWhiteSpace(result.Error) &&
                            !string.IsNullOrWhiteSpace(result.Lyrics))
                        {
                            _lyricRequestSemaphore.Release();
                            return new LyricDto
                            {
                                Lyrics = result.Lyrics,
                            };
                        }

                        if (result != null && !string.IsNullOrWhiteSpace(result.Error))
                        {
                            // we had an error from the api and not a request failure so we're going to return a null here
                            return null;
                        }
                        else
                        {
                            _logger.LogWarning($"Error requesting track {trackName} for artist {artistName} from lyrics api");
                        }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex,
                        $"Error requesting track {trackName} for artist {artistName} from lyrics api");
                    _logger.LogInformation("Pausing for 10 seconds to give some breating room");
                    await Task.Delay(10000);
                }
            }
        }
    }
}