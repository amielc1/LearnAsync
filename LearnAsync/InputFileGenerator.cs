using System.Text;

namespace LearnAsync
{
    internal class InputFileGenerator
    {

        public async Task GenerateFile(string filename = "input.txt")
        {
            string filePath = filename;
            int totalLines = 1000000; // 100 million lines
            int maxLength = 10000; // Maximum of 10,000 characters per line

            Random random = new Random();

            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8, bufferSize: 65536))
            {
                for (int i = 0; i < totalLines; i++)
                {
                    await writer.WriteLineAsync(GenerateRandomString(random, maxLength));

                    // Optionally, print progress to the console
                    if (i % 1_000 == 0)
                    {
                        await Console.Out.WriteLineAsync($"{i} lines written...");
                    }
                }
            }

            Console.WriteLine("File generation completed.");
        }

        private string GenerateRandomString(Random random, int maxLength)
        {
            int length = random.Next(1, maxLength + 1); // Random length between 1 and 10,000
            StringBuilder sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                sb.Append((char)random.Next('A', 'Z' + 1)); // Generate a random uppercase letter
            }

            return sb.ToString();
        }
    }
}
