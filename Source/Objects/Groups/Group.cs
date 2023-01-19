using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Objects.Permissions;

public class Group
{
    public string name { get; set; }
    public string? year { get; set; }
    public string id { get; set; }
    public string? degree { get; set; }
    public string? type { get; set; }
    public string? platform { get; set;}
    public string? school { get; set; }
    public string? link_id { get; set; }
    public string? language { get; set; }
    public string? office { get; set; }

    public Group(string name, string? year, string id, string? degree, string? type, string? platform, string? school, string? link_id, string? language, string? office)
    {
        this.name = name;
        this.year = year;
        this.id = id;
        this.degree = degree;
        this.type = type;
        this.platform = platform;
        this.school = school;
        this.link_id = link_id;
        this.language = language;
        this.office = office;
    }
}