using LearnAsync;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

class Program
{
    static void Main()
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

        try
        {
            Stopwatch stopwatch = new Stopwatch();

            // Step 1: Read and process chunks
            stopwatch.Start();
            foreach (var chunk in fileChunkReader.ReadChunks(inputFilePath, chunkSizeInBytes))
            {
                string tempFilePath = chunkProcessor.ProcessChunk(chunk);
                tempFiles.Add(tempFilePath);
            }
            stopwatch.Stop();
            performanceMonitor.LogPerformance("Chunk Processing", stopwatch.Elapsed);

            // Step 2: Merge sorted chunks
            stopwatch.Restart();
            sortedChunkMerger.MergeSortedChunks(tempFiles, outputFilePath);
            stopwatch.Stop();
            performanceMonitor.LogPerformance("Merging Chunks", stopwatch.Elapsed);

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
