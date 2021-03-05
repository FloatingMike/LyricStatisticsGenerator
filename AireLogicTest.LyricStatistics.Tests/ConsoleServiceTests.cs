using System.Collections.Generic;
using System.Threading.Tasks;
using AireLogicTest.LyricStatistics.Dtos;
using Moq;
using Xunit;

namespace AireLogicTest.LyricStatistics.Tests
{
    public class ConsoleServiceTests
    {
        [Fact]
        public async Task TestEndToEndConsoleFlow()
        {
            var mockLyricsHelper = new Mock<ILyricStatisticsHelper>();

            mockLyricsHelper.Setup(m => m.CalculateStatistics(It.IsAny<List<LyricDto>>())).Returns(
                new LyricStatisticsDto
                {
                    AvgWordCount = 0,
                });
            
            var mockArtistMetadataService = new Mock<IArtistMetadataService>();

            mockArtistMetadataService.Setup(m => m.FindArtistKey(It.IsAny<string>()))
                .ReturnsAsync(new Dictionary<string, string> {{"key1", "value1"}, {"key2", "value2"}});
            mockArtistMetadataService.Setup(m => m.GetTrackNamesForArtist(It.IsAny<string>()))
                .ReturnsAsync(new List<string>
                    {"Track1", "Track2", "Track3", "Track4", "Track5", "Track6", "Track7", "Track8", "Track9", "Track10"});

            var mockSongLyricService = new Mock<ISongLyricService>();
            mockSongLyricService.Setup(l => l.GetLyricForTrack(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new LyricDto{ Lyrics = "Test Lyrics"});
            
            var mockResultService = new Mock<IResultPresentationService>();
            mockResultService.Setup(r => r.OutputStatus(It.IsAny<string>()));
            
            var mockInputService = new Mock<IInputService>();
            mockInputService.Setup(r => r.RequestInput(It.IsAny<string>())).Returns("0"); // select the first resturned artist anyway

            var classUnderTest = new ArtistLyricStatisticsConsoleService(mockLyricsHelper.Object,
                mockArtistMetadataService.Object, mockSongLyricService.Object, mockResultService.Object,
                mockInputService.Object);

            await classUnderTest.Execute( "Test Artist");
            
            mockResultService.Verify(r => r.PresentResults(It.IsAny<LyricStatisticsDto>()), () => Times.Exactly(1));
        }
    }
}