﻿#region

using PoliFemoBackend.Source.Utils.Start;

#endregion

namespace PoliFemoBackend.Source.Test;

public static class Test
{
    public static void TestMain()
    {
        Console.WriteLine("Test");
        ThreadStartUtil.ThreadStartMethod();
        Console.WriteLine("Ciao");
    }
}