namespace LearnAsync
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class SortedChunkMerger
    {
        public async Task MergeSortedChunksAsync(List<string> tempFilePaths, string outputPath)
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

        private string MergeGroup(List<string> fileGroup, string outputFilePath = null)
        {
            if (outputFilePath == null)
            {
                outputFilePath = Path.GetTempFileName();
            }

            using (var writer = new StreamWriter(outputFilePath))
            {
                var readers = fileGroup.Select(file => new StreamReader(file)).ToList();
                var queue = new SortedList<string, StreamReader>();

                // Initialize the queue with the first line of each file
                foreach (var reader in readers)
                {
                    if (!reader.EndOfStream)
                    {
                        queue.Add(reader.ReadLine(), reader);
                    }
                }

                string previousLine = null;
                while (queue.Count > 0)
                {
                    var smallestLine = queue.Keys[0];
                    var smallestReader = queue.Values[0];

                    queue.RemoveAt(0);

                    if (smallestLine != previousLine)
                    {
                        writer.WriteLine(smallestLine);
                        previousLine = smallestLine;
                    }

                    if (!smallestReader.EndOfStream)
                    {
                        queue.Add(smallestReader.ReadLine(), smallestReader);
                    }
                }

                foreach (var reader in readers)
                {
                    reader.Dispose();
                }
            }

            return outputFilePath;
        }
    }

}
