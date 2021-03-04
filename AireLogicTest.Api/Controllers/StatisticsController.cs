using System;
using System.Threading.Tasks;
using AireLogicTest.LyricStatistics;
using Microsoft.AspNetCore.Mvc;

namespace AireLogicTest.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly ISongLyricService _lyricService;
        private readonly IArtistMetadataService _artistMetadataService;

        public StatisticsController(IArtistMetadataService artistMetadataService, ISongLyricService lyricService)
        {
            _lyricService = lyricService;
            _artistMetadataService = artistMetadataService;
        }
        
        [HttpGet("{artistKey}/statistics")]
        public async Task<IActionResult> GetStatistics(string artistKey)
        {
            
            
            try
            {
                var tracks = await _artistMetadataService.GetTrackNamesForArtist(artistKey);
            }
            catch (Exception)
            {
                return NotFound("Unable to find any artists");
            }
        }
    }
}