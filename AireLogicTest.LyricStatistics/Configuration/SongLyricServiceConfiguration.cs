namespace AireLogicTest.LyricStatistics.Configuration
{
    public class SongLyricServiceConfiguration
    {
        public object LyricsOvhUrl { get; set; } = "https://api.lyrics.ovh/v1/";
        public int LyricsTimeoutMilliseconds { get; set; } = 15000;
        public int LyricRetries { get; set; } = 3;
    }
}