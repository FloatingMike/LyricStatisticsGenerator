using System.Text.Json.Serialization;

namespace AireLogicTest.LyricStatistics.ApiTypes
{
    public class Release
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}