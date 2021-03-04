using AireLogicTest.LyricStatistics.Dtos;

namespace AireLogicTest.LyricStatistics
{
    using System.Collections.Generic;
    using System.Linq;

    public class LyricStatisticsHelper : ILyricStatisticsHelper
    {
        private readonly IStringHelper _stringHelper;

        public LyricStatisticsHelper(IStringHelper stringHelper)
        {
            _stringHelper = stringHelper;
        }

        public LyricStatisticsDto CalculateStatistics(List<LyricDto> lyrics)
        {
            return new LyricStatisticsDto
            {
                AvgWordCount = lyrics.Average(l => _stringHelper.WordsInString(l.Lyrics)),
            };
        }
    }
}