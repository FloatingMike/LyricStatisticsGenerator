namespace AireLogicTest.LyricStatistics.Dtos
{
    public class LyricStatisticsDto
    {
        public double AvgWordCount { get; set; }
        public int SongCount { get; set; }
        public int MinWordLength { get; set; }
        public int MaxWordLength { get; set; }
        public double StandardDeviation { get; set; }
        public double Variance { get; set; }
        public int UniqueWordsAcrossAllTracks { get; set; }

        public override string ToString()
        {
            return $"{AvgWordCount} average words across {SongCount} songs analysed with a Word Length Min/Max of {MinWordLength}/{MaxWordLength} and a Variance of {Variance} with {StandardDeviation} Standard Deviation with {UniqueWordsAcrossAllTracks} unique words used across all tracks";
        }
    }
}