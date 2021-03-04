namespace AireLogicTest.LyricStatistics
{
    public class StringHelper : IStringHelper
    {
        public int WordsInString(string source)
        {
            var wordCount = 0;
            var charIndex = 0;

            // ignore leading whitespace
            while (charIndex < source.Length && char.IsWhiteSpace(source[charIndex]))
                charIndex++;

            while (charIndex < source.Length)
            {
                // skip chars that are not whitespace
                while (charIndex < source.Length && !char.IsWhiteSpace(source[charIndex]))
                {
                    charIndex++;
                }

                wordCount++;

                // ignore trailing whitespace
                while (charIndex < source.Length && char.IsWhiteSpace(source[charIndex]))
                {
                    charIndex++;
                }
            }

            return wordCount;
        }
    }
}