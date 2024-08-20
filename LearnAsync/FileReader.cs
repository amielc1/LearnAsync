namespace LearnAsync;

public class FileReader
{
     
    public async Task GroupLinesByHashAsync(string inputFilePath, string tempDirectory, int numBuckets)
    {
        if (!Directory.Exists(tempDirectory))
        {
            Directory.CreateDirectory(tempDirectory);
        }

        Console.WriteLine($"Read from file {inputFilePath}, into temporaty files in {tempDirectory}");

        StreamWriter[] tempWriters = new StreamWriter[numBuckets];

        try
        {
            for (int i = 0; i < numBuckets; i++)
            {
                tempWriters[i] = new StreamWriter(Path.Combine(tempDirectory, $"bucket_{i}.txt"));
            }
            using (StreamReader reader = new StreamReader(inputFilePath))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    int bucket = Math.Abs(line.GetHashCode()) % numBuckets;
                    await tempWriters[bucket].WriteLineAsync(line);
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            foreach (var writer in tempWriters)
            {
                await writer.FlushAsync();
                writer.Dispose();
            }
        }
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
     
}
