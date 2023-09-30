namespace PoliFemoBackend.Source.Objects.DbObjects;

[Serializable]
public class LimitOffset
{
    private const int MaxLimit = 100;
    private readonly uint _limit;
    private readonly uint _pageOffset;

    public LimitOffset(uint? limitParam, uint? pageOffsetParam)
    {
        _limit = limitParam ?? MaxLimit;
        _pageOffset = pageOffsetParam ?? 0;

        //fix values
        _limit = Math.Max(1, _limit); //almeno 1 dev'esserci
        _limit = Math.Min(_limit, MaxLimit); //non si può chiedere più di MaxLimit
    }

    public string GetLimitQuery()
    {
        return $"LIMIT {_pageOffset * _limit},{_limit}";
    }
}