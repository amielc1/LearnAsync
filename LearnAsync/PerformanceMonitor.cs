namespace LearnAsync
{
    public class PerformanceMonitor
    {
        public void LogPerformance(string stepName, TimeSpan duration)
        {
            Console.WriteLine($"{stepName} took {duration.TotalSeconds} seconds.");
        }

        public bool ValidateOutput(string outputPath)
        {
            int counter = 0;
            Console.WriteLine($"ValidateOutput file {outputPath}");
            HashSet<string> lines = new HashSet<string>();

            using (StreamReader reader = new StreamReader(outputPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (lines.Contains(line))
                    {
                        Console.WriteLine($"ValidateOutput {++counter}: line {line} already exist in the {outputPath}");
                        return false;
                    }
                    lines.Add(line);
                }
            }
            return true;
        }
    }
}
