using Microsoft.AspNetCore.Mvc;

namespace PoliFemoBackend.Source.Objects.Auth;

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