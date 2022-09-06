#region

using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Profile;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[ApiExplorerSettings(GroupName = "Account")]
[Route("v{version:apiVersion}/accounts/me")]
[Route("accounts/me")]
public class ArticleByIdController : ControllerBase
{
    [MapToApiVersion("1.0")]
    [HttpGet]
    public ObjectResult ProfileDetails()
    {
        string userid;
        string sub;
        string? tempsub = AuthUtil.GetSubject(Request.Headers["Authorization"]);
        sub = tempsub == null ? "" : ((string)tempsub.ToString());
        string?[] permissions = AuthUtil.getPermissions(sub);
        using (SHA256 sha256Hash = SHA256.Create())
            {
                //From String to byte array
                byte[] sourceBytes = Encoding.UTF8.GetBytes(sub);
                byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                userid = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            }

        return new ObjectResult(new {
            id = userid.ToLower(),
            permissions = permissions
        });
    }
}