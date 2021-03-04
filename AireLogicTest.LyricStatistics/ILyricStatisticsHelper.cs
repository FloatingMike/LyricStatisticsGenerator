using System.Collections.Generic;
using AireLogicTest.LyricStatistics.Dtos;

namespace AireLogicTest.LyricStatistics
{
    public interface ILyricStatisticsHelper
    {
        LyricStatisticsDto CalculateStatistics(List<LyricDto> lyrics);
    }
}