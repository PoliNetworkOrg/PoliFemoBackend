using PoliFemoBackend.Source.Enums;

namespace PoliFemoBackend.Source.Objects.Search;

public class SearchResultTempObject
{
    private readonly ExpireCacheEnum _alreadyExpired;
    private readonly DateTime? _expireDate;
    public readonly string? Result;

    public SearchResultTempObject(string? result, DateTime? expireDate, ExpireCacheEnum alreadyExpired)
    {
        Result = result;
        _expireDate = expireDate;
        _alreadyExpired = alreadyExpired;
    }

    public bool HasExpired()
    {
        if (string.IsNullOrEmpty(Result))
            return true;

        switch (_alreadyExpired)
        {
            case ExpireCacheEnum.NEVER_EXPIRE:
                return false;
            case ExpireCacheEnum.ALREADY_EXPIRED:
                return true;
            case ExpireCacheEnum.TIMED_EXPIRATION:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return _expireDate switch
        {
            null => false, //no expiration
            _ => _expireDate <= DateTime.Now
        };
    }
}