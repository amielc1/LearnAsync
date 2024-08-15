using LearnAsync;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

class Program
{
    static async Task Main()
    {
        string inputFilePath = "input.txt";
        string outputFilePath = "output.txt";
        int chunkSizeInBytes = 10 * 1024 * 1024; // 10MB

        FileChunkReader fileChunkReader = new FileChunkReader();
        ChunkProcessor chunkProcessor = new ChunkProcessor();
        SortedChunkMerger sortedChunkMerger = new SortedChunkMerger();
        ResourceManager resourceManager = new ResourceManager();
        PerformanceMonitor performanceMonitor = new PerformanceMonitor();

        List<string> tempFiles = new List<string>();
        string tempDirectory = Path.Combine(Directory.GetCurrentDirectory(), nameof(tempDirectory));
        try
        {
            Stopwatch stopwatch = new Stopwatch();

            // Step 1: Read and process chunks
            stopwatch.Start();
           
            await fileChunkReader.GroupLinesByFirstCharacterAsync(inputFilePath, tempDirectory);
            stopwatch.Stop();
            performanceMonitor.LogPerformance("Read to Key files took", stopwatch.Elapsed);
          
            stopwatch.Start();

            foreach (string tempFilePath in Directory.GetFiles(tempDirectory))
            {
                // read each file 
                await foreach (var chunk in fileChunkReader.ReadChunksAsync(tempFilePath, chunkSizeInBytes))
                {
                    await chunkProcessor.ProcessChunkAsync(chunk, outputFilePath);
                    Console.WriteLine($"write Chunk with {chunk.Count} lines into file {outputFilePath}");
                }
            }

            stopwatch.Stop();
            performanceMonitor.LogPerformance("Chunk Processing", stopwatch.Elapsed);

            // Step 3: Validate output
            bool isValid = performanceMonitor.ValidateOutput(outputFilePath);
            Console.WriteLine($"Output validation result: {isValid}");

        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
        finally
        {
            // Clean up temporary files
            foreach (var tempFile in tempFiles)
            {
                //resourceManager.DeleteTempFile(tempFile);
            }
        }

        Console.Read();
    }
}
