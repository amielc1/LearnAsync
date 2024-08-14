namespace LearnAsync
{
    public class ChunkProcessor
    {
        public string ProcessChunk(List<string> chunk)
        {
            HashSet<string> uniqueLines = new HashSet<string>(chunk);

            string tempFilePath = Path.GetTempFileName();
            WriteTempFile(uniqueLines, tempFilePath);

            return tempFilePath;
        }

        private void WriteTempFile(HashSet<string> lines, string tempFilePath)
        {
            using (StreamWriter writer = new StreamWriter(tempFilePath))
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }
    }
}
