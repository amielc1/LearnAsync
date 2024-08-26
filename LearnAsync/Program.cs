using LearnAsync.Filters;
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
            IFilter<string> bloomFilter = new BloomFilterAdapter(10000000, 0.01);
            ChunkProcessor chunkProcessor = new ChunkProcessor(bloomFilter);
            FileValidator fileValidator = new FileValidator();
            await fileValidator.ValidateOutput(outputFilePath);
            string tempDirectory = Path.Combine(Directory.GetCurrentDirectory(), nameof(tempDirectory));

            FileProcessor fileProcessor = new FileProcessor(fileReader, chunkProcessor, fileValidator, tempDirectory);

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
