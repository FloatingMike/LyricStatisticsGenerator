using AireLogicTest.LyricStatistics.Dtos;

namespace AireLogicTest
{
    public interface IResultPresentationService
    {
        void OutputStatus(string status);
        void PresentResults(LyricStatisticsDto statistics);
    }
}