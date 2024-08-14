namespace LearnAsync
{
    public class ChunkProcessor
    {
        public async Task<string> ProcessChunkAsync(List<string> chunk)
        {
            string tempFilePath = Path.GetTempFileName();

            using (StreamWriter writer = new StreamWriter(tempFilePath))
            {
                foreach (var line in chunk)
                {
                    await writer.WriteLineAsync(line);
                }
            }

            return tempFilePath;
        }
    }
}
