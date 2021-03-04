using System;
using System.IO;
using System.IO.Compression;
using System.Text.Json;

namespace AireLogicTest.LyricStatistics
{
    /// <summary>
    /// A simple base class to assist in serialising some objects to disk
    /// </summary>
    public abstract class FileCachingServiceBase
    {
        protected void SerialiseToFile<T>(T item, string fileName)
        {
            if (File.Exists(fileName))
            {
                if (File.Exists($"{fileName}.bak"))
                {
                    File.Delete($"{fileName}.bak");
                }

                File.Move(fileName, $"{fileName}.bak");
            }

            using (var fileStream = File.OpenWrite(fileName))
            using (var compressionStream = new GZipStream(fileStream, CompressionMode.Compress))
            using (var jsonWriter = new Utf8JsonWriter(compressionStream))
            {
                JsonSerializer.Serialize(jsonWriter, item);
            }
        }

        protected T LoadFromFile<T>(string fileName) where T : new()
        {
            while (File.Exists(fileName))
            {
                try
                {
                    using (var fileStream = File.OpenRead(fileName))
                    using (var compressionStream = new GZipStream(fileStream, CompressionMode.Decompress))
                    using (var reader = new StreamReader(compressionStream))
                    {
                        return JsonSerializer.Deserialize<T>(reader.ReadToEnd());
                    }
                }
                catch (Exception)
                {
                    // we should log something about this here
                    File.Delete(fileName);
                    if (File.Exists($"{fileName}.bak"))
                    {
                        File.Move($"{fileName}.bak", fileName);
                    }
                }
            }

            return new T();
        }
    }
}