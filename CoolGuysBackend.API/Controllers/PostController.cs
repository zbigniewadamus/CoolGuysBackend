using System.ComponentModel.DataAnnotations;
using Azure;
using Azure.Storage.Blobs.Models;
using CoolGuysBackend.Contexts;
using CoolGuysBackend.Helpers;
using CoolGuysBackend.UseCases._contracts;
using CoolGuysBackend.UseCases._contracts.dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Post = CoolGuysBackend.UseCases.Post.Post;

namespace CoolGuysBackend.Controllers;

[ApiController]
[Authorize]
[Route("api/post")]
public class PostController: ControllerBase
{
    private readonly IBlobStorageHelper blobStorageHelper;
    private readonly GlobalDbContext dbContext;
    private readonly Post post;
    private readonly TokenHelper tokenHelper;

    public PostController( GlobalDbContext dbContext, Post post, TokenHelper tokenHelper)
    {
        this.dbContext = dbContext;
        this.post = post;
        this.tokenHelper = tokenHelper;
    }

    [HttpPost("image/{id}")]
    public async Task<IActionResult> AddPostImage(IFormFile data, int id)
    {
        try
        {
            var result = await post.AddImage(id, data);
            return Ok(new ResponseMessage{message = "Dodano zdjęcie", data = result});
        }
        catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
        {
            return Problem("Zdjęcie do postu już było dodane wcześniej.", statusCode: 405);
        }
        catch (Exception err)
        {
            return Problem(err.Message, statusCode: 401);
        }
    }

    [HttpGet("all")]
    public async Task<IActionResult> ShowAll([FromHeader(Name = "Authorization")] string authorization, [FromQuery(Name = "page")] int page = 1 )
    {
        try
        {
            if (authorization == null || !authorization.StartsWith("Bearer "))
            {
                return Unauthorized();
            }

            var token = authorization.Substring("Bearer ".Length).Trim();
            int userId = tokenHelper.GetUserIdFromToken(token);
            object? data = await post.ShowAll(userId, page);
            return Ok(new ResponseMessage { message = "Sukces", data = data });
        }
        catch (Exception err)
        {
            return Problem(err.Message, statusCode: 400);
        }
    }

    [HttpPut("vote")]
    public async Task<IActionResult> Vote([FromQuery][Required] bool positive, [FromQuery][Required] int postId)
    {
        try
        {
            await post.Vote(postId, positive);
            return Ok();
        }
        catch (Exception err)
        {
            return Problem(err.Message, statusCode:400);
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromHeader(Name = "Authorization")] string authorization, [FromBody] PostDto data)
    {
        try
        {
            if (authorization == null || !authorization.StartsWith("Bearer "))
            {
                return Unauthorized();
            }

            var token = authorization.Substring("Bearer ".Length).Trim();
            int userId = tokenHelper.GetUserIdFromToken(token);
            var postID = await post.Create(userId, data);
            return Ok(new ResponseMessage{message = postID.ToString()});
        }
        catch (Exception err)
        {
            return Problem(err.Message, statusCode: 400);
        }
    }
}