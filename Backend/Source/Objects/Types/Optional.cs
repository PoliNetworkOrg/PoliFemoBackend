namespace PoliFemoBackend.Source.Objects.Types;

public class Optional<T> where T : new()
{
    private readonly T? _value;
    public readonly bool IsPresent;

    public Optional(T value)
    {
        _value = value;
        IsPresent = true;
    }

    public Optional()
    {
        IsPresent = false;
    }

    public T GetValue()
    {
        return _value ?? new T();
    }
}