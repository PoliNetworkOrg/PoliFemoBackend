#region

using System.Net;

#endregion

namespace PoliFemoBackend.Source.Objects.Web;

public class WebReply
{
    private readonly string? _data;
    private readonly HttpStatusCode _httpStatusCode;
    public readonly bool fromCache;

    public WebReply(string? s, HttpStatusCode httpStatusCode, bool fromCache)
    {
        _data = s;
        _httpStatusCode = httpStatusCode;
        this.fromCache = fromCache;
    }


    public bool IsValid()
    {
        return _httpStatusCode == HttpStatusCode.OK;
    }

    public string? GetData()
    {
        return _data;
    }
}