using System.Text.Json.Serialization;

namespace AireLogicTest.LyricStatistics.ApiTypes
{
    public class LyricResult
    {
        [JsonPropertyName("lyrics")]
        public string Lyrics { get; set; }
        
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}