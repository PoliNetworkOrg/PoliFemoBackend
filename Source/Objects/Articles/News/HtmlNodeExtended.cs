using HtmlAgilityPack;

namespace PoliFemoBackend.Source.Objects.Articles.News;

public class HtmlNodeExtended
{
    public HtmlNode? HtmlNode;
    public Dictionary<string,string?>? HtmlAttributeCollection;

    private HtmlNodeExtended()
    {
        ;
    }

    public static HtmlNodeExtended From(HtmlNode? x)
    {
        var htmlNodeExtended = new HtmlNodeExtended(){ HtmlNode = x, HtmlAttributeCollection = Utils.Html.FlatHtml.ToDict(x?.Attributes)};
        return htmlNodeExtended;
    }

    public Dictionary<string,string?> GetAttributes()
    {
        return this.HtmlAttributeCollection ?? Utils.Html.FlatHtml.ToDict(this.HtmlNode?.Attributes);
    }

    public void SetAttributeValue(string src, string? s)
    {
        this.HtmlAttributeCollection ??= new Dictionary<string, string?>();
        this.HtmlAttributeCollection[src] = s;
    }
}