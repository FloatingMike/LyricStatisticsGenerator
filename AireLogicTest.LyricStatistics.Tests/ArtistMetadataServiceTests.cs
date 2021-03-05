using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AireLogicTest.LyricStatistics.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Xunit;

namespace AireLogicTest.LyricStatistics.Tests
{
    public class ArtistMetadataServiceTests
    {
        [Fact]
        public async Task TestArtistSearchParsing()
        {
            var handler = new Mock<HttpMessageHandler>();
            
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",  ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(File.ReadAllText("TestData/ArtistSearchResponse.json"))
                });
            
            var client = new HttpClient(handler.Object);
            
            var classUnderTest = new ArtistMetadataService(client, new ArtistMetaDataServiceConfiguration {MusicBrainzRootUrl = "https://BAD.DOESNTEXIST/", MusicBrainzTimeoutMilliseconds = 10}, new DateTimeProvider(), new Mock<ILogger<ArtistMetadataService>>().Object);

            var results = await classUnderTest.FindArtistKey("doesnt matter");

            Assert.True(results.Any());
        }
        
        [Fact]
        public async Task TestArtistTrackNameCollection()
        {
            var handler = new Mock<HttpMessageHandler>();
            
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",  ItExpr.Is<HttpRequestMessage>(m => m.RequestUri.AbsoluteUri.Contains("release?artist")), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(File.ReadAllText("TestData/ZN-Releases.json"))
                });
            
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",  ItExpr.Is<HttpRequestMessage>(m => m.RequestUri.AbsoluteUri.Contains("inc=recordings")), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(File.ReadAllText("TestData/ZN-Release-Recordings.json"))
                });
            
            var client = new HttpClient(handler.Object);
            
            var classUnderTest = new ArtistMetadataService(client, new ArtistMetaDataServiceConfiguration {MusicBrainzRootUrl = "https://BAD.DOESNTEXIST/", MusicBrainzTimeoutMilliseconds = 10}, new DateTimeProvider(), new Mock<ILogger<ArtistMetadataService>>().Object);

            var results = await classUnderTest.GetTrackNamesForArtist("Test Artist");
            
            Assert.NotEmpty(results);
        }
    }
}