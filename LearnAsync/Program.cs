using LearnAsync.Filters;
using LearnAsync.HashAlgorithm;
using Microsoft.Extensions.DependencyInjection;

namespace LearnAsync
{
    class Program
    {
        static async Task Main()
        {

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IHashAlgorithm, MurmurHashAlgorithm>() // Register MurmurHashAlgorithm as IHashAlgorithm
                .AddSingleton<FileReader>()                          // Register FileReader
                .AddSingleton<IFilter<string>>(sp => new BloomFilterAdapter(10000000, 0.01)) // Register BloomFilterAdapter as IFilter<string>
                .AddSingleton<ChunkProcessor>()                      // Register ChunkProcessor
                .AddSingleton<FileValidator>()                       // Register FileValidator
                .AddSingleton<FileProcessor>()                       // Register FileProcessor
                .BuildServiceProvider();

            string inputFilePath = "input.txt";
            string outputFilePath = "output.txt";
            string tmpDirectory = "tempDirectory";
            int numBuckets = 100;

            string tempDirectory = Path.Combine(Directory.GetCurrentDirectory(), tmpDirectory);

            var fileProcessor = serviceProvider.GetRequiredService<FileProcessor>();

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
