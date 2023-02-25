using PoliFemoBackend.Source.Enums;

namespace PoliFemoBackend.Source.Objects.Search;

/// <summary>
///     Used to store the search result in the cache
/// </summary>
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

    /// <summary>
    ///     Check if the result cached has expired
    /// </summary>
    /// <returns>True if expired, false otherwise</returns>
    /// <exception cref="ArgumentOutOfRangeException">The {alreadyExpired} enum has an invalid value</exception>
    public bool HasExpired()
    {
        return string.IsNullOrEmpty(Result) || _alreadyExpired switch
        {
            ExpireCacheEnum.NEVER_EXPIRE => false,
            ExpireCacheEnum.ALREADY_EXPIRED => true,
            ExpireCacheEnum.TIMED_EXPIRATION => _expireDate switch
            {
                null => false, //no expiration
                _ => _expireDate <= DateTime.Now
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}