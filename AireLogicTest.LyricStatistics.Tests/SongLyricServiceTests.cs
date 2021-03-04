using System.IO;
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
    public class SongLyricServiceTests
    {
        [Fact]
        public async Task TestSongLyricRequest()
        {
            var handler = new Mock<HttpMessageHandler>();
            
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",  ItExpr.Is<HttpRequestMessage>(m => m.RequestUri.AbsoluteUri.StartsWith("https://lyrics.bad/")), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(File.ReadAllText("TestData/ZN-Lyrics.json"))
                });
            
            var client = new HttpClient(handler.Object);

            var classUnderTest = new SongLyricService(client, new SongLyricServiceConfiguration() {LyricsOvhUrl = "https://lyrics.bad/"}, new DateTimeProvider(), new Mock<ILogger<SongLyricService>>().Object);

            var results = await classUnderTest.GetLyricForTrack("Test Artist", "Test Song");

            
            Assert.True(!string.IsNullOrWhiteSpace(results.Lyrics));
        }
    }
}