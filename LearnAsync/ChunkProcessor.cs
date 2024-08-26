using LearnAsync.Filters;

namespace LearnAsync;

public class ChunkProcessor
{
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    private readonly IFilter<string> _filter;

    public ChunkProcessor(IFilter<string> filter)
    {
        _filter = filter;
    }

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
        using (StreamWriter writer = new StreamWriter(outputFilePath, append: true))
        {
            foreach (var line in lines)
            {
                if (!_filter.Contains(line))
                {
                    _filter.Add(line);
                    await writer.WriteLineAsync(line);
                }
            }
        }
    }
}
