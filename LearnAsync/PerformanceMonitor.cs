using LearnAsync.Filters;

public class FileValidator
{
    public async Task LogPerformance(string stepName, TimeSpan duration)
    {
        await Console.Out.WriteLineAsync($"{stepName} took {duration.TotalSeconds} seconds.");
    }
    IFilter<string> filter;
    public FileValidator()
    {
        //filter = new BloomFilterAdapter(10000000, 0.01);
        filter = new HashSetAdapter();
    }

    public async Task<bool> ValidateOutput(string outputPath)
    {
        int counter = 0;
        await Console.Out.WriteLineAsync($"Validating output file {outputPath}");

        using (StreamReader reader = new StreamReader(outputPath))
        {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                // Negative check: If the line is not in the filter, add it
                if (!filter.Contains(line))
                {
                    filter.Add(line);
                }
                else
                {
                    // If the line is already in the filter, it's a duplicate
                    await Console.Out.WriteLineAsync($"Duplicate line found: {line} (occurrence {++counter})");
                    return false;
                }
            }
        }

        return true;
    }
}
