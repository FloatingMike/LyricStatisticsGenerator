using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Threading.Tasks;
using AireLogicTest.LyricStatistics.Dtos;
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

    public class DiskCacheTests
    {
        [Fact]
        public void TestCacheSerialisation()
        {
            var classUnderTest = new DiskCachingTestHarness();
            var dictionaryToSave = new Dictionary<string, Dictionary<string, LyricDto>>
            {
                { "testArtist1", new Dictionary<string, LyricDto> { {"testSong1" , new LyricDto{ Lyrics = "Test Lyric Value"}}}},
                { "testArtist2", new Dictionary<string, LyricDto> { {"testSong2" , new LyricDto{ Lyrics = "Test Lyric Value"}}, {"testSong3" , new LyricDto{ Lyrics = "Test Lyric Value 2"}}}},
            };

            var testFileName = Path.GetTempFileName();
            
            classUnderTest.WriteFile(dictionaryToSave, testFileName);

            var dictionaryFromDisk =
                classUnderTest.ReadFile<Dictionary<string, Dictionary<string, LyricDto>>>(testFileName);

            Assert.Equal(2, dictionaryFromDisk.Keys.Count);

            Assert.True(dictionaryFromDisk["testArtist2"]["testSong3"].Lyrics == "Test Lyric Value 2");
            
            if (File.Exists(testFileName))
            {
                File.Delete(testFileName);
            }
            
        }
    }

    public class DiskCachingTestHarness : FileCachingServiceBase
    {
        public void WriteFile<T>(T value, string fileName)
        {
            SerialiseToFile<T>(value, fileName);
        }

        public T ReadFile<T>(string fileName) where T : new()
        {
            return LoadFromFile<T>(fileName);
        }
    }
}