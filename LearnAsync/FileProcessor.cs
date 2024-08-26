using LearnAsync.Filters;
using System.Diagnostics;

namespace LearnAsync;

public class FileProcessor
{
    private readonly FileReader _fileReader;
    private readonly ChunkProcessor _chunkProcessor;
    private readonly FileValidator _performanceMonitor;
    private readonly string _tempDirectory;


    public FileProcessor(FileReader fileReader, ChunkProcessor chunkProcessor, FileValidator performanceMonitor,  string tempDirectory)
    {
        _fileReader = fileReader;
        _chunkProcessor = chunkProcessor;
        _performanceMonitor = performanceMonitor;
        _tempDirectory = tempDirectory;
    }

    public async Task ProcessFile(string inputFilePath, string outputFilePath, int numBuckets)
    {
        Stopwatch stopwatch = new Stopwatch();

        // Step 1: Read file into buckets
        stopwatch.Start();
        await _fileReader.GroupLinesByHashAsync(inputFilePath, _tempDirectory, numBuckets);
        stopwatch.Stop();
        await _performanceMonitor.LogPerformance("GroupLinesByHashAsync", stopwatch.Elapsed);

        // Step 2: Process each bucket
        stopwatch.Restart();
        await ProcessBuckets(outputFilePath);
        stopwatch.Stop();
        await _performanceMonitor.LogPerformance("Chunk Processing", stopwatch.Elapsed);

        // Step 3: Validate output
        bool isValid = await _performanceMonitor.ValidateOutput(outputFilePath);
        await Console.Out.WriteLineAsync($"Output validation result: {isValid}");
    }

    private async Task ProcessBuckets(string outputFilePath)
    {
        var tasks = new List<Task>();
        int procounter = 0;

        foreach (string tempFilePath in Directory.GetFiles(_tempDirectory))
        {
            tasks.Add(ProcessSingleBucket(tempFilePath, outputFilePath, procounter));
        }

        await Task.WhenAll(tasks);
    }

    private async Task ProcessSingleBucket(string tempFilePath, string outputFilePath, int procounter)
    {
        await Console.Out.WriteLineAsync($"[{Interlocked.Increment(ref procounter)}] Start processing temp file {tempFilePath}");

        var lines = await _fileReader.ReadFileAsync(tempFilePath);
        await Console.Out.WriteLineAsync($"[{procounter}] Processing {lines.Count} lines from {tempFilePath}");

        await _chunkProcessor.ProcessChunkAsync(lines, outputFilePath);
        await Console.Out.WriteLineAsync($"[{procounter}] Finished processing {tempFilePath}");
    }

    public async Task CleanUp(string tempDirectory)
    {
        if (Directory.Exists(tempDirectory))
        {
            await Console.Out.WriteLineAsync($"Deleting directory: {tempDirectory}");
            Directory.Delete(tempDirectory, true);
        }
    }
}
