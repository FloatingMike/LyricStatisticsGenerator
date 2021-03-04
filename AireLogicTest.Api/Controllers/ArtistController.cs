using System;
using System.Threading.Tasks;
using AireLogicTest.LyricStatistics;
using Microsoft.AspNetCore.Mvc;

namespace AireLogicTest.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistMetadataService _artistMetadataService;

        public ArtistController(IArtistMetadataService artistMetadataService)
        {
            _artistMetadataService = artistMetadataService;
        }

        [HttpGet("search/{artistName}")]
        public async Task<IActionResult> Search(string artistName)
        {
            try
            {
                return Ok(await _artistMetadataService.FindArtistKey(artistName));
            }
            catch (Exception)
            {
                return NotFound("Unable to find any artists");
            }
        }

        [HttpGet("{artistKey}/track-names")]
        public async Task<IActionResult> GetTrackNames(string artistKey)
        {
            try
            {
                return Ok(await _artistMetadataService.GetTrackNamesForArtist(artistKey));
            }
            catch (Exception)
            {
                return NotFound("Unable to find any tracks for that artistKey");
            }
        }
    }
}