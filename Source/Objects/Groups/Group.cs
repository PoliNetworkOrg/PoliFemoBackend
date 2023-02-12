using PoliFemoBackend.Source.Utils.Groups;

namespace PoliFemoBackend.Source.Objects.Groups;

public class Group
{
    public Group(string name, string? year, string? degree, string? type, string? platform, string? school,
        string? link_id, string? language, string? office)
    {
        this.name = name;
        this.year = year;
        var s = platform + "/" + year + "/" + link_id;
        Console.WriteLine("s:", s);
        id = GenerateHash.generatedId(s);
        this.degree = degree;
        this.type = type;
        this.platform = platform;
        this.school = school;
        this.link_id = link_id;
        this.language = language;
        this.office = office;
    }

    public string name { get; set; }
    public string? year { get; set; }
    public string id { get; set; }
    public string? degree { get; set; }
    public string? type { get; set; }
    public string? platform { get; set; }
    public string? school { get; set; }
    public string? link_id { get; set; }
    public string? language { get; set; }
    public string? office { get; set; }
}