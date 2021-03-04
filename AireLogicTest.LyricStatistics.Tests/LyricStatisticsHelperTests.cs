using System;
using System.Collections.Generic;
using AireLogicTest.LyricStatistics.Dtos;
using Xunit;

namespace AireLogicTest.LyricStatistics.Tests
{
    public class LyricStatisticsHelperTests
    {
        [Fact]
        public void TestMeanStatisticGeneration()
        {
            var lyricList = new List<LyricDto>
            {
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5"},
            };
            
            var classUnderTest = new LyricStatisticsHelper(new StringHelper()); // there is some coupling here I don't like, however it could be swapped out for other implementations should the test require it

            var result = classUnderTest.CalculateStatistics(lyricList);
            
            Assert.True(Math.Abs(result.AvgWordCount - 9) < 0.0001, $"{result.AvgWordCount} is expected to be 9");
        }
        
        [Fact]
        public void TestMinMaxStatisticGeneration()
        {
            var lyricList = new List<LyricDto>
            {
                new LyricDto {Lyrics = "veryLongTestWord1"},
                new LyricDto {Lyrics = "w"},
            };
            
            var classUnderTest = new LyricStatisticsHelper(new StringHelper()); // there is some coupling here I don't like, however it could be swapped out for other implementations should the test require it

            var result = classUnderTest.CalculateStatistics(lyricList);
            
            Assert.Equal(1, result.MinWordLength);
            Assert.Equal(17, result.MaxWordLength);
        }
        
        [Fact]
        public void TestVarianceGeneration()
        {
            var lyricList = new List<LyricDto>
            {
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5"},
            };
            
            var classUnderTest = new LyricStatisticsHelper(new StringHelper()); // there is some coupling here I don't like, however it could be swapped out for other implementations should the test require it

            var result = classUnderTest.CalculateStatistics(lyricList);
            
            Assert.True(Math.Abs(result.Variance - 4) < 0.0001, $"{result.Variance} is expected to be 4");
        }
        
        [Fact]
        public void TestStandardDeviationGeneration()
        {
            var lyricList = new List<LyricDto>
            {
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5"},
            };
            
            var classUnderTest = new LyricStatisticsHelper(new StringHelper()); // there is some coupling here I don't like, however it could be swapped out for other implementations should the test require it

            var result = classUnderTest.CalculateStatistics(lyricList);
            
            Assert.True(Math.Abs(result.StandardDeviation - 2) < 0.0001, $"{result.StandardDeviation} is expected to be 2");
        }
        
        [Fact]
        public void TestUniqueWordCount()
        {
            var lyricList = new List<LyricDto>
            {
                new LyricDto {Lyrics = "word1 word2 Word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 wOrd3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 WORD3      word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5 word6 word7 word8 word9 word10"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5"},
                new LyricDto {Lyrics = "word1 word2 word3 word4 word5"},
            };
            
            var classUnderTest = new LyricStatisticsHelper(new StringHelper()); // there is some coupling here I don't like, however it could be swapped out for other implementations should the test require it

            var result = classUnderTest.CalculateStatistics(lyricList);
            
            Assert.Equal(10, result.UniqueWordsAcrossAllTracks);
        }
    }
}