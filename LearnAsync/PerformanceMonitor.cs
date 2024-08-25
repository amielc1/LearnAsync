namespace LearnAsync;

public class PerformanceMonitor
{
    public void LogPerformance(string stepName, TimeSpan duration)
    {
        Console.WriteLine($"{stepName} took {duration.TotalSeconds} seconds.");
    }

    public async Task<bool> ValidateOutput(string outputPath)
    {
        int counter = 0;
        await Console.Out.WriteLineAsync($"ValidateOutput file {outputPath}");
        HashSet<string> lines = new HashSet<string>();

        using (StreamReader reader = new StreamReader(outputPath))
        {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (lines.Contains(line))
                {
                    await Console.Out.WriteLineAsync($"ValidateOutput {++counter}: line {line} already exist in the {outputPath}");
                    return false;
                }
                lines.Add(line);
            }
        }
        return true;
    }
}
