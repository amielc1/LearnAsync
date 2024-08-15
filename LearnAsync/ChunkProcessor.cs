namespace LearnAsync
{
    public class ChunkProcessor
    {
        public async Task ProcessChunkAsync(List<string> chunk, string outputfile)
        {
            var uniqlines = new HashSet<string>(chunk);

            using (StreamWriter writer = new StreamWriter(outputfile))
            {
                foreach (var line in uniqlines)
                {
                    await writer.WriteLineAsync(line);
                }
            }
        }
    }
}
