namespace LearnAsync
{
    public class ChunkProcessor
    {
        public string ProcessChunk(List<string> chunk)
        {
            HashSet<string> uniqueLines = new HashSet<string>(chunk);
            List<string> sortedLines = uniqueLines.ToList();
            sortedLines.Sort();

            string tempFilePath = Path.GetTempFileName();
            WriteTempFile(sortedLines, tempFilePath);

            return tempFilePath;
        }

        private void WriteTempFile(List<string> sortedLines, string tempFilePath)
        {
            using (StreamWriter writer = new StreamWriter(tempFilePath))
            {
                foreach (var line in sortedLines)
                {
                    writer.WriteLine(line);
                }
            }
        }
    }
}
