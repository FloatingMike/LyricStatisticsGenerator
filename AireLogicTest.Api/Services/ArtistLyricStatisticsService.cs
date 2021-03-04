using AireLogicTest.Api.Controllers;
using AireLogicTest.LyricStatistics;
using System.Collections.Generic;

namespace AireLogicTest.Api.Services
{
    public class ArtistLyricStatisticsService
    {
        private readonly ILyricStatisticsHelper _lyricsHelper;
        private readonly IArtistMetadataService _artistMetadataService;
        private readonly ISongLyricService _lyricService;
        
        public ArtistLyricStatisticsService(ILyricStatisticsHelper lyricsHelper,
            IArtistMetadataService artistMetadataService, ISongLyricService lyricService)
        {
            _lyricsHelper = lyricsHelper;
            _artistMetadataService = artistMetadataService;
            _lyricService = lyricService;
        }
    }
}