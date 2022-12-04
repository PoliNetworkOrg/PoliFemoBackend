namespace PoliFemoBackend.Source.Objects.DbObjects;

public class LimitOffset
{
    public int limit;
    public int offset;


    private const int MaxLimit = 100;
    
    public LimitOffset(int? limit, int? offset)
    {
        this.limit = limit ?? MaxLimit;
        this.offset = offset ?? 0;

    }

    public string getLimitQuery()
    {
        return "LIMIT " + ((limit == null || limit < 1 || limit > 100) ? 30 : limit);
    }
}