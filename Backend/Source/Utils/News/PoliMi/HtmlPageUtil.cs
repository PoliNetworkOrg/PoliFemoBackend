#region

using HtmlAgilityPack;

#endregion

namespace PoliFemoBackend.Source.Utils.News.PoliMi;

public static class HtmlPageUtil
{
    internal static HtmlDocument LoadUrl(string url)
    {
        var web = new HtmlWeb();
        var doc = web.Load(url);

        if (doc == null)
            return new HtmlDocument();
        return doc;
    }
}
