namespace PoliFemoBackend.Source.Objects.Articles.News;

/*
class to replace Optional<T> type for Article(ex NewsPolimi).
*/

public class OptionalArticleNews{
    public OptionalArticleNews(){

    }
    public string? title {get; set;}
    public string? content {get; set;}
    public string? tag {get; set;}
    public string? subtitle {get; set;}
    public string? image {get; set;}
    public string? url {get; set;}
    public bool? internalNews {get; set;}

}