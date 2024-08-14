namespace LearnAsync
{
    public class FileChunkReader
    {
        public IEnumerable<List<string>> ReadChunks(string filePath, int chunkSizeInBytes)
        {
            List<string> chunk = new List<string>();
            int currentSize = 0;

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    chunk.Add(line);
                    currentSize += System.Text.Encoding.UTF8.GetByteCount(line);

                    if (currentSize >= chunkSizeInBytes)
                    {
                        yield return chunk;
                        chunk = new List<string>();
                        currentSize = 0;
                    }
                }

                if (chunk.Count > 0)
                {
                    yield return chunk;
                }
            }
        } 
    }

}
