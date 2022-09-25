namespace PoliFemoBackend.Source.Objects;

public class NewsPolimi
{
    public List<string> content; //list of html objects (as strings)
    public bool internalNews;
    public string? subtitle;
    public string? title;
    public string? url;
}