#region 

using System.Text.RegularExpressions;

#endregion

namespace PoliFemoBackend.Source.Utils.Article;

public static class ArticleContentUtil {
    public static string CleanContentString(string content) {
        var result = content.Trim();
        var regex = new Regex(@"<section.*?>");
        result = regex.Replace(result, ""); // Remove all section tags 
        regex = new Regex(@"<header.*?>");
        result = regex.Replace(result, ""); // Remove all header tags 
        try {
            result = result.Split("<hr class=\"cl-right\">", 2)[1]; // Remove title, subtitle, and publish date (in case of a classic article)
        } catch (Exception) {
            var contentarray = result.Split("</header>"); // Remove title, subtitle, and publish date (in case of a special article)
            if (contentarray.Length > 1)
                contentarray = contentarray.Skip(1).ToArray();
            result = string.Join(" ", contentarray);
        }
        return result;
    }
}