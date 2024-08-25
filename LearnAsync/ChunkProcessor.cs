using BloomFilter;

namespace LearnAsync
{
    public class ChunkProcessor
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly IBloomFilter _bloomFilter;

        public ChunkProcessor(int expectedItemCount = 10000000, double falsePositiveRate= 0.01)
        { 
            _bloomFilter = FilterBuilder.Build(10000000, 0.01);
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
                    if (!_bloomFilter.Contains(line))
                    {
                        _bloomFilter.Add(line);
                        await writer.WriteLineAsync(line);
                    }
                }
            }
        }
    }
}
