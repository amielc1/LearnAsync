using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnAsync
{
    public class SortedChunkMerger
    {
        public void MergeSortedChunks(List<string> tempFilePaths, string outputPath)
        {
            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                var readers = new List<StreamReader>();
                foreach (var filePath in tempFilePaths)
                {
                    readers.Add(new StreamReader(filePath));
                }

                var priorityQueue = new SortedList<string, int>();
                for (int i = 0; i < readers.Count; i++)
                {
                    if (!readers[i].EndOfStream)
                    {
                        string line = readers[i].ReadLine();
                        priorityQueue.Add(line, i);
                    }
                }

                string previousLine = null;
                while (priorityQueue.Count > 0)
                {
                    var smallestEntry = priorityQueue.Keys[0];
                    var readerIndex = priorityQueue[smallestEntry];

                    if (smallestEntry != previousLine)
                    {
                        writer.WriteLine(smallestEntry);
                        previousLine = smallestEntry;
                    }

                    priorityQueue.RemoveAt(0);

                    if (!readers[readerIndex].EndOfStream)
                    {
                        string nextLine = readers[readerIndex].ReadLine();
                        priorityQueue.Add(nextLine, readerIndex);
                    }
                }

                foreach (var reader in readers)
                {
                    reader.Dispose();
                }
            }
        }
    }
}
