using System;
using Xunit;

namespace AireLogicTest.LyricStatistics.Tests
{
    public class StringHelperTests
    {
        [Theory]
        [InlineData("basic test", 2)]
        [InlineData(" something  more   complicated  ", 3)]
        [InlineData("\n  \nsomething really    \n   crazy  \n", 3)]
        [InlineData("",0)]
        [InlineData(";",1)]
        [InlineData("\n",0)]
        public void WordCountTest(string input, int expectedWordCount)
        {
            var classUnderTest = new StringHelper();
            
            Assert.Equal(expectedWordCount, classUnderTest.WordsInString(input));
        }
    }
    
    
}