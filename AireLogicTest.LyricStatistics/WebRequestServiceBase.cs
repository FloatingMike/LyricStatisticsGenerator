using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AireLogicTest.LyricStatistics
{
    /// <summary>
    /// A simple base class for making rate limited calls to a json api
    /// </summary>
    public abstract class WebRequestServiceBase
    {
        private readonly HttpClient _client;
        private IDateTimeProvider _dateTimeProvider;
        private readonly ILogger _logger;
        private DateTime? _nextRequestAllowed;
        private SemaphoreSlim _requestSemaphore = new SemaphoreSlim(1);

        protected WebRequestServiceBase(HttpClient client, ILogger logger, IDateTimeProvider dateTimeProvider)
        {
            _client = client;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        protected async Task<TResult> MakeRequestWithDelay<TResult>(string url, int timeoutMilliseconds, int retries = 3)
        {
            await _requestSemaphore.WaitAsync(); // make sure only one call can flow through here at a time.
            
            if (_nextRequestAllowed.HasValue && _dateTimeProvider.Now < _nextRequestAllowed.Value)
            {
                var delayTime = (_nextRequestAllowed.Value - _dateTimeProvider.Now).Add(TimeSpan.FromMilliseconds(100));
                _logger.LogInformation("Waiting for {0}ms", delayTime.TotalMilliseconds);
                await Task.Delay(delayTime); // let it cool down make sure there has been enough time since the last request
            }

            var attempts = 0;

            while (attempts <= retries)
            {
                try
                {
                    var result = await _client.GetAsync(url);
                    _nextRequestAllowed = _dateTimeProvider.Now + TimeSpan.FromMilliseconds(timeoutMilliseconds);
                    _requestSemaphore.Release();

                    if (result.IsSuccessStatusCode)
                    {
                        return JsonSerializer.Deserialize<TResult>(await result.Content.ReadAsStringAsync());
                    }

                    switch (result.StatusCode)
                    {
                        case HttpStatusCode.ServiceUnavailable:
                            _logger.LogWarning($"API Returned {result.StatusCode}, will wait to cool down and try again");
                            await Task.Delay(timeoutMilliseconds);
                            break;
                        default:
                            _logger.LogError($"API Returned {result.StatusCode}, will not retry");
                            attempts = retries;
                            break;
                    }
                    
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"Unable to retrieve result from service, pausing and will retry {retries - attempts} more times");
                    await Task.Delay(5000);
                }

                attempts++;
            }

            return default;
        } 
    }
}