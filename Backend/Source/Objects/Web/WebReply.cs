#region

using System.Net;

#endregion

namespace PoliFemoBackend.Source.Objects.Web;

public class WebReply
{
    private readonly string? _data;
    private readonly HttpStatusCode _httpStatusCode;

    public WebReply(string? s, HttpStatusCode httpStatusCode)
    {
        _data = s;
        _httpStatusCode = httpStatusCode;
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