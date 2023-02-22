using Microsoft.AspNetCore.Mvc;

namespace PoliFemoBackend.Source.Objects.Auth;

public class LoginResultObject
{
    internal readonly string? acctype;
    internal readonly string? subject;
    public ActionResult? actionResult;

    public LoginResultObject(string? acctype, string? subject, ActionResult? actionResult)
    {
        this.acctype = acctype;
        this.subject = subject;
        this.actionResult = actionResult;
    }
}