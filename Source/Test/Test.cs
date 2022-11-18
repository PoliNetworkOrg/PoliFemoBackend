#region

using PoliFemoBackend.Source.Utils.News;

#endregion

namespace PoliFemoBackend.Source.Test;

public static class Test
{
    public static void TestMain()
    {
        Console.WriteLine("Test");
        //var r = PoliMiNewsUtil.DownloadCurrentNews();
        var x = PoliMiNewsUtil.GetNews();
        Console.WriteLine(x);
    }
}