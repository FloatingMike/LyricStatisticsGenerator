using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using Moq;
using Xunit;

namespace AireLogicTest.LyricStatistics.Tests
{
    public class InMemoryCachingTests
    {
        [Fact]
        public async Task TestArtistSearchCaching()
        {
            var mockLyricService = new Mock<IArtistMetadataService>();
            mockLyricService.Setup(l => l.FindArtistKey(It.IsAny<string>())).ReturnsAsync(new Dictionary<string, string>
            {
                {"1", "Example Artist 1"},
                {"2", "Example Artist 2"},
                {"3", "Example Artist 3"}
            });
            
            var classUnderTest = new CachingArtistMetadataService(mockLyricService.Object, new CachingConfiguration{EnableFileCaching = false});
            var firstCallResult = await classUnderTest.FindArtistKey("Example Artist 1");
            var secondCallResult = await classUnderTest.FindArtistKey("Example Artist 1");
            
            // We're going to just call it again now with an alternative artist name to make it call the mock lyric service again
            await classUnderTest.FindArtistKey("Example Artist 2");
            await classUnderTest.FindArtistKey("Example Artist 2");
            
            Assert.NotEmpty(firstCallResult);
            Assert.Equal(firstCallResult, secondCallResult);
            
            mockLyricService.Verify(x => x.FindArtistKey(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task TestTrackNamesCaching()
        {
            var mockLyricService = new Mock<IArtistMetadataService>();
            mockLyricService.Setup(l => l.GetTrackNamesForArtist(It.IsAny<string>())).ReturnsAsync(new List<string>
            {
                "Test Track 1",
                "Test Track 2"
            });
            
            var classUnderTest = new CachingArtistMetadataService(mockLyricService.Object, new CachingConfiguration{EnableFileCaching = false});
            var firstCallResult = await classUnderTest.GetTrackNamesForArtist("Example Artist 1");
            var secondCallResult = await classUnderTest.GetTrackNamesForArtist("Example Artist 1");
            
            await classUnderTest.GetTrackNamesForArtist("Example Artist 2");
            await classUnderTest.GetTrackNamesForArtist("Example Artist 2");
            
            Assert.NotEmpty(firstCallResult);
            Assert.Equal(firstCallResult, secondCallResult);
            
            mockLyricService.Verify(x => x.GetTrackNamesForArtist(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}