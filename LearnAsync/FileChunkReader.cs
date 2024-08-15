namespace LearnAsync
{
    public class FileChunkReader
    {

        public async Task GroupLinesByFirstCharacterAsync(string inputFilePath, string tempDirectory)//took 1.5 sec !!!! 
        {
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }
            Console.WriteLine($"Read from file {inputFilePath}, into temporaty files in {tempDirectory}");
            // Dictionary to hold StreamWriters for each key
            var writers = new Dictionary<char, StreamWriter>();

            try
            {
                using (StreamReader reader = new StreamReader(inputFilePath))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        // Skip empty lines
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        // Determine the key for partitioning - the first letter of the line
                        char key = char.ToUpper(line[0]);

                        // Get or create the StreamWriter for this key
                        if (!writers.TryGetValue(key, out var writer))
                        {
                            await Console.Out.WriteLineAsync($"Create StreamWriter for key {key}");
                            string tempFilePath = Path.Combine(tempDirectory, $"{key}.txt");
                            writer = new StreamWriter(tempFilePath, true);
                            writers[key] = writer;
                        }

                        // Write the line to the appropriate temporary file
                        await writer.WriteLineAsync(line);
                    }
                }
            }
            finally
            {
                // Ensure all StreamWriters are properly disposed
                foreach (var writer in writers.Values)
                {
                    await writer.FlushAsync();
                    writer.Dispose();
                }
            }
        }

        public async Task ReadKeyFromLine(string inputFilePath)//took 181 sec !!!! 
        {
            string tempDirectory = Path.Combine(Directory.GetCurrentDirectory(), nameof(tempDirectory));

            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }

            using (StreamReader reader = new StreamReader(inputFilePath))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    // Skip empty lines
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    // Determine the key for partitioning - the first letter of the line
                    char key = char.ToUpper(line[0]);

                    // Temporary file based on the partitioning key
                    string tempFilePath = Path.Combine(tempDirectory, $"{key}.txt");

                    // Write the line to the appropriate temporary file asynchronously
                    using (StreamWriter writer = new StreamWriter(tempFilePath, true))
                    {
                        await writer.WriteLineAsync(line);
                    }
                }
            }
        }



        public async IAsyncEnumerable<List<string>> ReadChunksAsync(string filePath, int chunkSizeInBytes)
        {
            List<string> chunk = new List<string>();
            int currentSize = 0;

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
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
