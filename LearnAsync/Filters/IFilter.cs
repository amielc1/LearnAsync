namespace LearnAsync.Filters;

public interface IFilter<T>
{
    bool Contains(T item);
    void Add(T item);
}
