namespace LearnAsync;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class SortedChunkMerger
{
    public async Task MergeChunksAsync(List<string> tempFilePaths, string outputPath)
    {
        using (StreamWriter writer = new StreamWriter(outputPath))
        {
            foreach (var tempFilePath in tempFilePaths)
            {
                using (StreamReader reader = new StreamReader(tempFilePath))
                {
                    await Console.Out.WriteLineAsync($"Merge {tempFilePath} into {outputPath}");
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
        }
    }

}
