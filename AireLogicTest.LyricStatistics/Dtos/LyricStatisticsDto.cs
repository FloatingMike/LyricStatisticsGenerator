namespace AireLogicTest.LyricStatistics.Dtos
{
    public class LyricStatisticsDto
    {
        public string DataSource { get; set; }
        public string ArtistIdentifier { get; set; }
        public double AvgWordCount { get; set; }
        public int SongCount { get; set; }

        public override string ToString()
        {
            return $"{ArtistIdentifier} from {DataSource} has {AvgWordCount} average words across {SongCount} songs analysed";
        }
    }
}