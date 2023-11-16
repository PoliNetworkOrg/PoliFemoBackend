#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Enums;

#endregion

namespace PoliFemoBackend.Source.Objects.Auth;

[Serializable]
public class LoginResultObject
{
    internal readonly AccountType Acctype;
    public readonly ActionResult? ActionResult;
    internal readonly string? Subject;

    public LoginResultObject(AccountType acctype, string? subject, ActionResult? actionResult)
    {
        Acctype = acctype;
        Subject = subject;
        ActionResult = actionResult;
    }
}
