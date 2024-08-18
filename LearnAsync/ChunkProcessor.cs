namespace LearnAsync;

public class ChunkProcessor
{
    SemaphoreSlim semaphore = new SemaphoreSlim(1, 1); // Semaphore to control access to the output file

    public async Task ProcessChunkAsync(List<string> chunk, string outputFilePath)
    {
        var uniqueLines = new HashSet<string>(chunk);

        await semaphore.WaitAsync(); // Await the semaphore before accessing the output file
        try
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath, append: true))
            {
                foreach (var line in uniqueLines)
                {
                    await writer.WriteLineAsync(line);
                }
            }
        }
        finally
        {
            semaphore.Release(); 
        }
    }
}
