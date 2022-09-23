namespace PoliFemoBackend.Source.Test;

public static class Test
{
    public static void TestMain()
    {
        Console.WriteLine("Test");
        var r = Utils.News.PoliMiNewsUtil.DownloadCurrentNews();
        Console.WriteLine(r);
    }
}