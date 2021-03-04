using Xunit;

namespace AireLogicTest.LyricStatistics.Tests
{
    public class StringHelperTests
    {
        [Theory]
        [InlineData("basic test", 2, 2)]
        [InlineData(" something  more   complicated  ", 3, 3)]
        [InlineData("\n  \nsomething really    \n   crazy  \n", 3, 3)]
        [InlineData("\n  \nsomething really really REALLY   \n   crazy  \n", 5, 3)]
        [InlineData("",0,0)]
        [InlineData(";",1,1)]
        [InlineData("\n",0,0)]
        public void WordCountTest(string input, int expectedWordCount, int uniqueWords)
        {
            var classUnderTest = new StringHelper();

            var result = classUnderTest.WordsInString(input);
            Assert.Equal(expectedWordCount, result.wordCount);
            Assert.Equal(uniqueWords, result.uniqueWords.Count);
        }
    }
}