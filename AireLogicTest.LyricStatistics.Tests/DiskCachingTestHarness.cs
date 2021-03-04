namespace AireLogicTest.LyricStatistics.Tests
{
    public class DiskCachingTestHarness : FileCachingServiceBase
    {
        public void WriteFile<T>(T value, string fileName)
        {
            SerialiseToFile<T>(value, fileName);
        }

        public T ReadFile<T>(string fileName) where T : new()
        {
            return LoadFromFile<T>(fileName);
        }
    }
}