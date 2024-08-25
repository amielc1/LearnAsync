using LearnAsync.HashAlgorithm;

namespace LearnAsync
{
    class Program
    {
        static async Task Main()
        {
            string inputFilePath = "input.txt";
            string outputFilePath = "output.txt";
            int numBuckets = 100;

            IHashAlgorithm murmurHashAlgorithm = new MurmurHashAlgorithm();
            FileReader fileReader = new FileReader(murmurHashAlgorithm);
            ChunkProcessor chunkProcessor = new ChunkProcessor();
            PerformanceMonitor performanceMonitor = new PerformanceMonitor();

            string tempDirectory = Path.Combine(Directory.GetCurrentDirectory(), nameof(tempDirectory));

            FileProcessor fileProcessor = new FileProcessor(fileReader, chunkProcessor, performanceMonitor, tempDirectory);

            try
            {
                await fileProcessor.ProcessFile(inputFilePath, outputFilePath, numBuckets);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"An error occurred: {ex.Message}");
            }
            finally
            {
                await fileProcessor.CleanUp(tempDirectory);
            }

            Console.Read();
        }
    }
}
