namespace AireLogicTest.LyricStatistics
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    public interface IArtistMetadataService
    {
        Task<Dictionary<string, string>> FindArtistKey(string searchText);
        Task<List<string>> GetTrackNamesForArtist(string artistKey);
    }
}