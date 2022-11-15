#region

using PoliFemoBackend.Source.Utils.News;
using PoliFemoBackend.Source.Utils.Start;

#endregion

namespace PoliFemoBackend.Source.Test;

public static class Test
{
    public static void TestMain()
    {
        Console.WriteLine("Test");
        var r = PoliMiNewsUtil.DownloadCurrentNews();
        Console.WriteLine(r.Count());
    }
}