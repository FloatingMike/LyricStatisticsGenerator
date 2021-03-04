using System.Collections.Generic;
using System.IO;
using AireLogicTest.LyricStatistics.Dtos;
using Xunit;

namespace AireLogicTest.LyricStatistics.Tests
{
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
}