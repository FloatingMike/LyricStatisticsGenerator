namespace AireLogicTest.LyricStatistics
{
    public class CachingConfiguration
    {
        public bool EnableFileCaching { get; set; } = true;
        public string LyricCacheFileName { get; set; } = "lyricCache.dat";
        public string ArtistSearchCacheFileName { get; set; } = "metadataArtistCache.dat";
        public string ArtistTrackNameCacheFileName { get; set; } = "metadataTrackCache.dat";
    }
}