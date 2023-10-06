#region

using Microsoft.AspNetCore.Mvc;

#endregion

namespace PoliFemoBackend.Source.Objects.Auth;

[Serializable]
public class LoginResultObject
{
    internal readonly string? Acctype;
    public readonly ActionResult? ActionResult;
    internal readonly string? Subject;

    public LoginResultObject(string? acctype, string? subject, ActionResult? actionResult)
    {
        Acctype = acctype;
        Subject = subject;
        ActionResult = actionResult;
    }
}
