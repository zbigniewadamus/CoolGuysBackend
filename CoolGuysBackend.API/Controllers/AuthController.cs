using CoolGuysBackend.UseCases._contracts;
using CoolGuysBackend.UseCases._contracts.dto;
using CoolGuysBackend.UseCases.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CoolGuysBackend.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly Login login;
    private readonly Register register;

    public AuthController(Login login, Register register)
    {
        this.login = login;
        this.register = register;
    }
    [HttpPost("login")]
    public IActionResult Login([FromBody]LoginDto data)
    {
        try
        {
            var token = login.Exec(data);
            return Ok(new ResponseMessage { message = token});
        }
        catch (Exception err)
        {
            return Problem(err.Message, statusCode: 422);
        }
    }
    
    [HttpPost("register")]
    public IActionResult Register([FromBody]RegisterDto data)
    {
        try
        {
            var task = register.Exec(data);
            if (!string.IsNullOrEmpty(task))
                return Ok(new ResponseMessage { message = task });
            else
                return Problem("Coś poszło nie tak.", statusCode: 405);
        }
        catch (Exception err)
        {
            return Problem(err.Message, statusCode: 422);
        }
    }
}