using LearnAsync.HashAlgorithm;

public class FileReader
{
    private readonly IHashAlgorithm _hashAlgorithm;

    public FileReader(IHashAlgorithm hashAlgorithm)
    {
        _hashAlgorithm = hashAlgorithm;
    }

    public async Task GroupLinesByHashAsync(string inputFilePath, string tempDirectory, int numBuckets)
    {
        await Console.Out.WriteLineAsync("Grouping lines by hash...");
        EnsureDirectoryExists(tempDirectory);

        var tempWriters = CreateTempWriters(tempDirectory, numBuckets);
        await WriteLinesToBuckets(inputFilePath, tempWriters, numBuckets);

        CloseWriters(tempWriters);
    }

    public async Task<List<string>> ReadFileAsync(string filePath)
    {
        var lines = new List<string>();

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                lines.Add(line);
            }
        }

        return lines;
    }

    private void EnsureDirectoryExists(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    private StreamWriter[] CreateTempWriters(string tempDirectory, int numBuckets)
    {
        Console.WriteLine($"Create {numBuckets} buckets");
        var tempWriters = new StreamWriter[numBuckets];
        for (int i = 0; i < numBuckets; i++)
        {
            tempWriters[i] = new StreamWriter(Path.Combine(tempDirectory, $"bucket_{i}.txt"));
        } 
        return tempWriters;
    }

    private async Task WriteLinesToBuckets(string inputFilePath, StreamWriter[] tempWriters, int numBuckets)
    {
        await Console.Out.WriteLineAsync($"WriteLinesToBuckets - Read lines from {inputFilePath}");
        using (StreamReader reader = new StreamReader(inputFilePath))
        {
            int cycleCounter = 0;
            string line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                int bucket = Math.Abs(_hashAlgorithm.ComputeHash(line)) % numBuckets;
                await tempWriters[bucket].WriteLineAsync(line);

                if (++cycleCounter % 10000 == 0)
                {
                    await Console.Out.WriteLineAsync($"{DateTime.Now:HH:mm:ss} - Processed {cycleCounter} lines.");
                }
            }
        }
    }

    private void CloseWriters(StreamWriter[] writers)
    {
        Console.WriteLine("Close all writers");
        foreach (var writer in writers)
        {
            writer.Flush();
            writer.Dispose();
        }
    }
}