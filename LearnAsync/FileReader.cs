using LearnAsync.HashAlgorithm;

namespace LearnAsync
{
    public class FileReader
    {
        private readonly IHashAlgorithm _hashAlgorithm;

        public FileReader(IHashAlgorithm hashAlgorithm)
        {
            _hashAlgorithm = hashAlgorithm;
        }

        public async Task GroupLinesByHashAsync(string inputFilePath, string tempDirectory, int numBuckets)
        {
            Console.WriteLine("GroupLinesByHashAsync Start");
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }

            await Console.Out.WriteLineAsync($"Read from file {inputFilePath}, into temporaty files in {tempDirectory}");
            await Console.Out.WriteLineAsync($"Divide input file into [{numBuckets}] buckets");
            StreamWriter[] tempWriters = new StreamWriter[numBuckets];

            try
            {
                for (int i = 0; i < numBuckets; i++)
                {
                    tempWriters[i] = new StreamWriter(Path.Combine(tempDirectory, $"bucket_{i}.txt"));
                }
                using (StreamReader reader = new StreamReader(inputFilePath))
                {
                    int cycleCounter = 0;
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        int bucket = Math.Abs(_hashAlgorithm.ComputeHash(line)) % numBuckets;
                        await tempWriters[bucket].WriteLineAsync(line);
                        Interlocked.Increment(ref cycleCounter);
                        if (cycleCounter % 100000 == 0)
                        {
                            await Console.Out.WriteLineAsync($"{DateTime.Now.ToLongTimeString()} [{bucket}] write into bucket (*100k)");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                foreach (var writer in tempWriters)
                {
                    await writer.FlushAsync();
                    writer.Dispose();
                }
            }
        }

        public async Task<List<string>> ReadFileAsync(string filePath)
        {
            var lines = new List<string>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }
    }
}
