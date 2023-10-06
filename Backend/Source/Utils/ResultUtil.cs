#region

using System.Net;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace PoliFemoBackend.Source.Utils;

public static class ResultUtil
{
    public static ObjectResult ExceptionResult(Exception? ex)
    {
        ObjectResult objectResult =
            new(null)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Value = (object?)ex ?? ex?.Message ?? ""
            };
        return objectResult;
    }
}
