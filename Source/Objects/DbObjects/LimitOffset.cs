namespace PoliFemoBackend.Source.Objects.DbObjects;

public class LimitOffset
{
    public uint limit;
    public uint offset;


    private const int MaxLimit = 100;

    public LimitOffset(uint? limit, uint? offset)
    {
        this.limit = limit ?? MaxLimit;
        this.offset = offset ?? 0;
        
        //fix values
        this.limit = Math.Max(1, this.limit); //almeno 1 dev'esserci
    }

    public string GetLimitQuery()
    {
        return "LIMIT " + ((limit < 1 || limit > 100) ? 30 : limit);
    }
}