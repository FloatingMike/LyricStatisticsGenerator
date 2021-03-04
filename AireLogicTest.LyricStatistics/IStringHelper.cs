using System.Collections.Generic;

namespace AireLogicTest.LyricStatistics
{
    public interface IStringHelper
    {
        (int wordCount, HashSet<string> uniqueWords) WordsInString(string source);
    }
}