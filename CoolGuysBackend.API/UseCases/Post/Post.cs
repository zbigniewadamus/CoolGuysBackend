using Azure.Core;
using CoolGuysBackend.UseCases._contracts;
using CoolGuysBackend.UseCases._contracts.dto;
using Microsoft.AspNetCore.SignalR;

namespace CoolGuysBackend.UseCases.Post;

public class Post
{
    private readonly IPostService postService;

    public Post(IPostService postService)
    {
        this.postService = postService;
    }

    public Task<int> Create(int userId, PostDto data)
    {
        return postService.Create(userId, data);
    }

    public Task<List<_contracts.Post>> ShowAll(int userId, int page)
    {
        return postService.ShowAll(userId, page);
    }

    public Task<bool> Vote(int postId, bool positive)
    {
        return postService.Vote(postId, positive);
    }

    public Task<_contracts.Post> AddImage(int postId, IFormFile data)
    {
        return postService.AddImage(postId, data);
    }
}