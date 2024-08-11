using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            HashSet<string> lines = new HashSet<string>();

            using (StreamReader reader = new StreamReader(outputPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (lines.Contains(line))
                    {
                        return false;
                    }
                    lines.Add(line);
                }
            }
            return true;
        }
    }
}
