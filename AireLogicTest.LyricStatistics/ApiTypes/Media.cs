using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AireLogicTest.LyricStatistics.ApiTypes
{
    public class Media
    {
        [JsonPropertyName("tracks")]
        public List<Track> Tracks { get; set; }
    }
}