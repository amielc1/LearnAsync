namespace LearnAsync.Filters;

public class HashSetAdapter : IFilter<string>
{
    private readonly HashSet<string> _hashSet = new HashSet<string>();

    public bool Contains(string item)
    {
        return _hashSet.Contains(item);
    }

    public void Add(string item)
    {
        _hashSet.Add(item);
    }
}
