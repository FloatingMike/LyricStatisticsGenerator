using System.Text.Json.Serialization;

namespace AireLogicTest.LyricStatistics.ApiTypes
{
    public class Track
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}