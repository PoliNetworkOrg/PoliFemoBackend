using HtmlAgilityPack;
using PoliFemoBackend.Source.Utils.Html;

namespace PoliFemoBackend.Source.Objects.Articles.News;

public class HtmlNodeExtended
{
    public Dictionary<string, string?>? HtmlAttributeCollection;
    public HtmlNode? HtmlNode;

    private HtmlNodeExtended()
    {
        ;
    }

    public static HtmlNodeExtended From(HtmlNode? x)
    {
        var htmlNodeExtended = new HtmlNodeExtended
            { HtmlNode = x, HtmlAttributeCollection = FlatHtml.ToDict(x?.Attributes) };
        return htmlNodeExtended;
    }

    public Dictionary<string, string?> GetAttributes()
    {
        return HtmlAttributeCollection ?? FlatHtml.ToDict(HtmlNode?.Attributes);
    }

    public void SetAttributeValue(string src, string? s)
    {
        HtmlAttributeCollection ??= new Dictionary<string, string?>();
        HtmlAttributeCollection[src] = s;
    }
}