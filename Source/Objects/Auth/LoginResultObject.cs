using Microsoft.AspNetCore.Mvc;

namespace PoliFemoBackend.Source.Objects.Auth;

public class LoginResultObject
{
    internal readonly string? Acctype;
    internal readonly string? Subject;
    public readonly ActionResult? ActionResult;

    public LoginResultObject(string? acctype, string? subject, ActionResult? actionResult)
    {
        this.Acctype = acctype;
        this.Subject = subject;
        this.ActionResult = actionResult;
    }
}