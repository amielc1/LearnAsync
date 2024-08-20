namespace LearnAsync;

public class ChunkProcessor
{
    SemaphoreSlim semaphore = new SemaphoreSlim(1, 1); // Semaphore to control access to the output file

 
    public async Task ProcessChunkAsync(List<string> lines , string outputFilePath)
    {
        await semaphore.WaitAsync(); // Await the semaphore before accessing the output file
        HashSet<string> uniqueLines = new HashSet<string>(lines);
        try
        { 
            using (StreamWriter writer = new StreamWriter(outputFilePath, append: true))
            {
                foreach (var line in uniqueLines)
                {
                    await writer.WriteLineAsync(line);
                }
                uniqueLines.Clear();
            }
        }
        finally
        {
            semaphore.Release(); 
        }
    }
}
