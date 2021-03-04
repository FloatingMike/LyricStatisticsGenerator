using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AireLogicTest.LyricStatistics.ApiTypes
{
    public class SearchResultDto
    {
        [JsonPropertyName("artists")]
        public List<Artist> Artists { get; set; }
    }
}