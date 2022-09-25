namespace PoliFemoBackend.Source.Objects;

public class NewsPolimi
{
    public string? url;
    public string? title;
    public string? subtitle;
    public bool internalNews;
    public List<string> content; //list of html objects (as strings)
}