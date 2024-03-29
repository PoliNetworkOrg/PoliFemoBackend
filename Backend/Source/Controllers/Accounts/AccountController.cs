﻿#region

using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Objects.Permissions;
using PoliFemoBackend.Source.Utils.Account;
using PoliFemoBackend.Source.Utils.Auth;

#endregion

namespace PoliFemoBackend.Source.Controllers.Accounts;

[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = "Accounts")]
[Route("/accounts/me")]
public class ArticleByIdController : ControllerBase
{
    /// <summary>
    ///     Get basic information about the current user
    /// </summary>
    /// <remarks>
    ///     Use this endpoint to retrieve the User ID and permissions
    /// </remarks>
    /// <response code="200">Request completed successfully</response>
    /// <response code="401">Authorization error</response>
    /// <response code="500">Can't connect to the server</response>
    [HttpGet]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ObjectResult ProfileDetails()
    {
        string userid;
        var tempSub = AuthUtil.GetSubjectFromHttpRequest(Request);
        var sub = tempSub ?? "";
        var permissions = AccountAuthUtil.GetPermissions(sub);
        using (var sha256Hash = SHA256.Create())
        {
            //From String to byte array
            var sourceBytes = Encoding.UTF8.GetBytes(sub);
            var hashBytes = sha256Hash.ComputeHash(sourceBytes);
            userid = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
        }

        var permarray = Grant.GetFormattedPerms(permissions);

        return new ObjectResult(
            new
            {
                id = userid.ToLower(),
                permissions = permarray,
                authorized_authors = AccountAuthUtil.GetAuthorizedAuthors(sub)
            }
        );
    }

    /// <summary>
    ///     Delete the user's account and data
    /// </summary>
    /// <response code="200">Request completed successfully</response>
    /// <response code="400">Invalid token received</response>
    /// <response code="500">Can't connect to the server</response>
    [Authorize]
    [HttpDelete]
    [Route("/accounts/me")]
    public ObjectResult DeleteAccount()
    {
        var sub = AuthUtil.GetSubjectFromHttpRequest(Request);
        if (string.IsNullOrEmpty(sub))
            return BadRequest("");

        var r = AccountDeletionUtil.DeleteAccountSingle(sub, false);
        return r ? Ok("") : StatusCode(500, "");
    }
}
