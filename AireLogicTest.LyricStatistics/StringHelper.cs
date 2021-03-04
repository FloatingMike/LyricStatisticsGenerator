using System.Collections.Generic;

namespace AireLogicTest.LyricStatistics
{
    public class StringHelper : IStringHelper
    {
        public (int wordCount, HashSet<string> uniqueWords) WordsInString(string source)
        {
            var wordList = new HashSet<string>();
            
            var wordCount = 0;
            var charIndex = 0;

            // ignore leading whitespace
            while (charIndex < source.Length && char.IsWhiteSpace(source[charIndex]))
                charIndex++;

            while (charIndex < source.Length)
            {
                var skipped = 0;
                // skip chars that are not whitespace
                while (charIndex < source.Length && !char.IsWhiteSpace(source[charIndex]))
                {
                    charIndex++;
                    skipped++;
                }

                wordList.Add(source.Substring(charIndex - skipped, skipped).ToLowerInvariant());
                
                wordCount++;

                // ignore trailing whitespace
                while (charIndex < source.Length && char.IsWhiteSpace(source[charIndex]))
                {
                    charIndex++;
                }
            }

            return (wordCount, wordList);
        }
    }
}