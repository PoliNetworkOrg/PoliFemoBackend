using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Utils.Auth;

namespace PoliFemoBackend.Source.Utils.Attributes;

[AttributeUsage(AttributeTargets.All, Inherited = false)]
public class RequiresAccountTypeAttribute : Attribute, IActionFilter
{
    private readonly AccountType _accountType;

    public RequiresAccountTypeAttribute(AccountType accountType)
    {
        _accountType = accountType;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var sub = AuthUtil.GetSubjectFromHttpRequest(context.HttpContext.Request) ?? null;
        if (AccountAuthUtil.GetAccountType(sub) == _accountType)
            return;
        context.Result = new ForbidResult();
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
