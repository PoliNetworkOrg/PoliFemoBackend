using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PoliFemoBackend.Source.Utils.Auth;

namespace PoliFemoBackend.Source.Utils.Attributes;

[AttributeUsage(AttributeTargets.All, Inherited = false)]
public class RequiresPermissionAttribute : Attribute, IActionFilter
{
    private readonly string _permValue;

    public RequiresPermissionAttribute(string permValue)
    {
        _permValue = permValue;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var sub = AuthUtil.GetSubjectFromHttpRequest(context.HttpContext.Request);
        if (AccountAuthUtil.HasPermission(sub, _permValue))
            return;
        context.Result = new ForbidResult();
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
