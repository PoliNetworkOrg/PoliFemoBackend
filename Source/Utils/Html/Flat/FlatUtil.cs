using PoliFemoBackend.Source.Objects.Articles.News;

namespace PoliFemoBackend.Source.Utils.Html.Flat;

public static class FlatUtil
{
    
    // Presa una lista di elementi con figli, parti dai figli e restituisci insieme a loro i genitori
    // r[a,b[c,d]] => [a(r),c(b,r),d(b,r)]
    public  static List<Hj?>? Flat(Hj thisHj)
    {
        var result = new List<Hj?>();

        if (thisHj.Children == null || thisHj.Children.Count == 0)
            return new List<Hj?> { thisHj };

        foreach (var variable in thisHj.Children)
        {
            Flat2(new List<Hj?> { thisHj }, variable, result);
        }

        return result;
    }

    private static void Flat2(List<Hj?> list, Hj variable, ICollection<Hj?> result)
    {
        if (variable.Children == null || variable.Children.Count == 0)
        {
            Flat3(list, variable, result);
            return;
        }

        //se ha dei figli estraimoli
        Flat4(list, variable, result);
    }

    private static void Flat4(List<Hj?> list, Hj variable, ICollection<Hj?> result)
    {
        var parentList = new List<Hj?>();
        foreach (var v in list)
        {
            if (v != null)
                parentList.Add(v);
        }

        parentList.Add(variable);
        if (variable.Children == null) return;
        foreach (var child in variable.Children)
        {
            Flat2(parentList, child, result);
        }
    }

    private static void Flat3(List<Hj?> list, Hj variable, ICollection<Hj?> result)
    {
        variable.Parents ??= new List<Hj>();
        foreach (var v in list)
        {
            if (v != null)
                variable.Parents.Add(v);
        }

        result.Add(variable);
    }
}