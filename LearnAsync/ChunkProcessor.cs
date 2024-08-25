
namespace LearnAsync;
public class ChunkProcessor
{
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public async Task ProcessChunkAsync(List<string> lines, string outputFilePath)
    {
        await _semaphore.WaitAsync();

        try
        {
            await WriteUniqueLinesToFile(lines, outputFilePath);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task WriteUniqueLinesToFile(List<string> lines, string outputFilePath)
    {
        var uniqueLines = new HashSet<string>(lines);

        using (StreamWriter writer = new StreamWriter(outputFilePath, append: true))
        {
            foreach (var line in uniqueLines)
            {
                await writer.WriteLineAsync(line);
            }
        }
    }
}