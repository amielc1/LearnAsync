namespace LearnAsync;

public class PerformanceMonitor
{
    public async Task LogPerformance(string stepName, TimeSpan duration)
    {
        await Console.Out.WriteLineAsync($"{stepName} took {duration.TotalSeconds} seconds.");
    }
     
    public async Task<bool> ValidateOutput(string outputPath)
    {
        int counter = 0;
        await Console.Out.WriteLineAsync($"Validating output file {outputPath}");
        HashSet<string> lines = new HashSet<string>();

        using (StreamReader reader = new StreamReader(outputPath))
        {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (!lines.Add(line))
                {
                    await Console.Out.WriteLineAsync($"Duplicate line found: {line} (occurrence {++counter})");
                    return false;
                }
            }
        }
        lines.Clear();
        return true;
    }
}
