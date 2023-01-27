using System.ComponentModel.DataAnnotations;
using System.Net;
using CoolGuysBackend.Contexts;
using CoolGuysBackend.Helpers;
using CoolGuysBackend.UseCases._contracts;
using CoolGuysBackend.UseCases._contracts.dto;
using CoolGuysBackend.UseCases.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CoolGuysBackend.Controllers;

[ApiController]
[Authorize]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly GlobalDbContext dbContext;
    private readonly Search search;
    private readonly Avatar avatar;
    private readonly Friend friend;
    private readonly CurrentUser currentUser;
    private readonly Details details;
    private readonly TokenHelper tokenHelper;

    public UserController(Search search, Avatar avatar, Friend friend, CurrentUser currentUser, Details details,
        TokenHelper tokenHelper)
    {
        this.search = search;
        this.avatar = avatar;
        this.friend = friend;
        this.currentUser = currentUser;
        this.details = details;
        this.tokenHelper = tokenHelper;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrent([FromHeader(Name = "Authorization")] string authorization)
    {
        try
        {
            if (authorization == null || !authorization.StartsWith("Bearer "))
            {
                return Unauthorized();
            }

            var token = authorization.Substring("Bearer ".Length).Trim();
            int userId = tokenHelper.GetUserIdFromToken(token);
            var result = await currentUser.Show(userId);
            return Ok(new ResponseMessage { message = "Sukces", data = result });
        }
        catch (Exception err)
        {
            return Problem(err.Message);
        }
    }

    [HttpPut("me")]
    public async Task<IActionResult> AddDetails([FromHeader(Name = "Authorization")] string authorization, UserDto data)
    {
        if (authorization == null || !authorization.StartsWith("Bearer "))
        {
            return Unauthorized();
        }

        var token = authorization.Substring("Bearer ".Length).Trim();
        int userId = tokenHelper.GetUserIdFromToken(token);
        try
        {
            var user = await details.Add(userId, data);
            return Ok(new ResponseMessage { message = "Sukces", data = user });
        }
        catch (Exception err)
        {
            return Problem(err.Message);
        }
    }

    [HttpGet("friends")]
    public async Task<IActionResult> GetFriends([FromHeader(Name = "Authorization")] string authorization)
    {
        if (authorization == null || !authorization.StartsWith("Bearer "))
        {
            return Unauthorized();
        }

        var token = authorization.Substring("Bearer ".Length).Trim();
        int userId = tokenHelper.GetUserIdFromToken(token);

        var friendsList = await friend.Show(userId);
        return Ok(new ResponseMessage { message = "Sukces", data = friendsList });
    }

    [HttpPost("friends/{id}")]
    public async Task<IActionResult> AddFriend([FromHeader(Name = "Authorization")] string authorization, int id)
    {
        if (authorization == null || !authorization.StartsWith("Bearer "))
        {
            return Unauthorized();
        }

        var token = authorization.Substring("Bearer ".Length).Trim();
        int userId = tokenHelper.GetUserIdFromToken(token);
        try
        {
            await friend.Add(userId, id);
            return Ok(new ResponseMessage { message = "Sukces" });
        }
        catch (Exception err)
        {
            return Problem(err.Message);
        }
    }

    [HttpDelete("friends/{id}")]
    public async Task<IActionResult> DeleteFriend([FromHeader(Name = "Authorization")] string authorization, int id)
    {
        if (authorization == null || !authorization.StartsWith("Bearer "))
        {
            return Unauthorized();
        }

        var token = authorization.Substring("Bearer ".Length).Trim();
        int userId = tokenHelper.GetUserIdFromToken(token);
        try
        {
            await friend.Remove(userId, id);
            return Ok(new ResponseMessage { message = "Sukces" });
        }
        catch (Exception err)
        {
            return Problem(err.Message);
        }
    }

    [HttpPost("avatar")]
    public async Task<IActionResult> AddAvatar([FromHeader(Name = "Authorization")] string authorization,
        IFormFile image)
    {
        if (authorization == null || !authorization.StartsWith("Bearer "))
        {
            return Unauthorized();
        }

        var token = authorization.Substring("Bearer ".Length).Trim();
        int userId = tokenHelper.GetUserIdFromToken(token);
        try
        {
            await avatar.Add(userId, image);
            return Ok(new ResponseMessage { message = "Sukces" });
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery(Name = "Email")] [Required] string email)
    {
        try
        {
            return Ok(new ResponseMessage { message = $"{ await search.Exec(email)}" });
        }
        catch (Exception er)
        {
            return Problem(er.Message);
        }
    }
}