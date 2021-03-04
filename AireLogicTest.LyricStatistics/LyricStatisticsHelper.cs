using System;
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
            var wordStatistics = lyrics.Select(l => _stringHelper.WordsInString(l.Lyrics)).ToList();
            
            var stats = new LyricStatisticsDto
            {
                SongCount = lyrics.Count,
                AvgWordCount = wordStatistics.Average(l => l.wordCount),
                UniqueWordsAcrossAllTracks = wordStatistics.SelectMany(l => l.uniqueWords).Distinct().Count(),
                MinWordLength = wordStatistics.Min(l => l.uniqueWords.Min(w => w.Length)),
                MaxWordLength = wordStatistics.Max(l => l.uniqueWords.Max(w => w.Length)),
            };
            stats.Variance = stats.SongCount > 1 ? wordStatistics.Sum(l => (l.wordCount - stats.AvgWordCount) * (l.wordCount - stats.AvgWordCount)) / stats.SongCount : 0;
            stats.StandardDeviation = stats.SongCount > 1 ? Math.Sqrt(stats.Variance) : 0;

            return stats;
        }
        

    }
}