using CoolGuysBackend.UseCases._contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CoolGuysBackend.Controllers;

[ApiController]
[Authorize]
[Route("api/post")]
public class PostController: ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new ResponseMessage { message = "ddd"});
    }
}