using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AireLogicTest.LyricStatistics.ApiTypes
{
    public class ArtistResultDto
    {
        [JsonPropertyName("releases")]
        public List<Release> Releases { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}