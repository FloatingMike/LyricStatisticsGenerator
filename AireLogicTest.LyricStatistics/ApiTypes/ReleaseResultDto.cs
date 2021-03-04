using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AireLogicTest.LyricStatistics.ApiTypes
{
    public class ReleaseResultDto
    {
        [JsonPropertyName("media")]
        public List<Media> Media { get; set; }
    }
}