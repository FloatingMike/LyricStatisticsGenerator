using System.Threading.Tasks;
using AireLogicTest.LyricStatistics.Dtos;

namespace AireLogicTest.LyricStatistics
{
    public interface ISongLyricService
    {
        Task<LyricDto> GetLyricForTrack(string artistName, string trackName);
    }
}