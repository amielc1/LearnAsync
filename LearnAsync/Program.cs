using LearnAsync;
using LearnAsync.HashAlgorithm;
using System.Diagnostics;

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

        List<string> tempFiles = new List<string>();
        string tempDirectory = Path.Combine(Directory.GetCurrentDirectory(), nameof(tempDirectory));
        try
        {
            Stopwatch stopwatch = new Stopwatch();

            // Step 1: Read file into files by hash
            stopwatch.Start();
            await fileReader.GroupLinesByHashAsync(inputFilePath, tempDirectory, numBuckets);
            stopwatch.Stop();
            performanceMonitor.LogPerformance("Read to Key files took", stopwatch.Elapsed);

            stopwatch.Start(); 
            var tasks = new List<Task>();

            foreach (string tempFilePath in Directory.GetFiles(tempDirectory))
            {
                tasks.Add(Task.Run(async () =>
                {
                    // Step 2: Read temp files   
                    var lines = await fileReader.ReadFileAsync(tempFilePath); 
                    // Step 2: write uniq lines into output file   
                    await chunkProcessor.ProcessChunkAsync(lines, outputFilePath);
                    //Console.WriteLine($"Processed {lines.Count} lines from file {tempFilePath}");
                }));
            }

            await Task.WhenAll(tasks);

            stopwatch.Stop();
            performanceMonitor.LogPerformance("Chunk Processing", stopwatch.Elapsed);

            // Step 4: Validate output
            bool isValid = performanceMonitor.ValidateOutput(outputFilePath);
            Console.WriteLine($"Output validation result: {isValid}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
        finally
        {
            CleanUp(tempDirectory);
        }

        Console.Read();
    }

    private static void CleanUp(string tempDirectory)
    {
        if (Directory.Exists(tempDirectory))
        {
            Console.WriteLine($"Deleting directory: {tempDirectory}");
            Directory.Delete(tempDirectory, true);
        }
    }
}
