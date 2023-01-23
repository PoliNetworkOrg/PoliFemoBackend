#region

using System.Net;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace PoliFemoBackend.Source.Utils;

// ReSharper disable once UnusedType.Global
public static class GroupsUtil
{

    public static ObjectResult ErrorInRetrievingGroups()
    {
        return new ObjectResult(new
        {
            error = "Errore durante il recupero dei gruppi"
        })
        {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };
    }
}