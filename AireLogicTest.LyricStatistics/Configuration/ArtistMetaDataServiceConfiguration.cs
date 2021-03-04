namespace AireLogicTest.LyricStatistics.Configuration
{
    public class ArtistMetaDataServiceConfiguration
    {
        public string MusicBrainzRootUrl { get; set; } = "https://musicbrainz.org/ws/2/";
        public int MusicBrainzTimeoutMilliseconds { get; set; } = 1500;
    }
}