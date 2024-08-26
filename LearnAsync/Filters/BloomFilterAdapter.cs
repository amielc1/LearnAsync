namespace LearnAsync.Filters;

using BloomFilter;

public class BloomFilterAdapter : IFilter<string>
{
    private readonly IBloomFilter _bloomFilter;

    public BloomFilterAdapter(int expectedItemCount, double falsePositiveRate)
    {
        _bloomFilter = FilterBuilder.Build(expectedItemCount, falsePositiveRate);
    }

    public bool Contains(string item)
    {
        return _bloomFilter.Contains(item);
    }

    public void Add(string item)
    {
        _bloomFilter.Add(item);
    }
}
