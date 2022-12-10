#region

using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Objects.Permission;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Accounts;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[ApiExplorerSettings(GroupName = "Accounts")]
[Route("v{version:apiVersion}/accounts/me")]
[Route("accounts/me")]
public class ArticleByIdController : ControllerBase
{
    [MapToApiVersion("1.0")]
    [HttpGet]
    public ObjectResult ProfileDetails()
    {
        string userid;
        var tempSub = AuthUtil.GetSubjectFromHttpRequest(Request);
        var sub = tempSub ?? "";
        var permissions = AuthUtil.GetPermissions(sub);
        using (var sha256Hash = SHA256.Create())
        {
            //From String to byte array
            var sourceBytes = Encoding.UTF8.GetBytes(sub);
            var hashBytes = sha256Hash.ComputeHash(sourceBytes);
            userid = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
        }

        var permarray = PermissionGrantObject.GetFormattedPerms(permissions);

        return new ObjectResult(new
        {
            id = userid.ToLower(),
            permissions = permarray,
            authorized_authors = AuthUtil.GetAuthorizedAuthors(sub)
        });
    }
}