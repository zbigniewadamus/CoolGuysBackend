using CoolGuysBackend.Contexts;
using CoolGuysBackend.Entities;
using CoolGuysBackend.UseCases._contracts;
using CoolGuysBackend.UseCases._contracts.dto;
using Microsoft.EntityFrameworkCore;

namespace CoolGuysBackend.Domain;

public class PostService : IPostService
{
    private readonly GlobalDbContext dbContext;
    private readonly IConfiguration config;
    private readonly IBlobStorageHelper blobStorageHelper;

    public PostService(GlobalDbContext dbContext, IConfiguration config, IBlobStorageHelper blobStorageHelper)
    {
        this.dbContext = dbContext;
        this.config = config;
        this.blobStorageHelper = blobStorageHelper;
    }

    public async Task<int> Create(int userId, PostDto data) {
        var x = dbContext.PostEntities.Add(new PostEntity
        {
            Description = data.Description,
            ImageUrl = "",
            Score = 0,
            UserId = userId
        });

        dbContext.SaveChanges();

        return x.Entity.Id;
    }

    public async Task<List<Post>> ShowAll(int userId, int page)
    {
        int pageSize = int.Parse(config.GetSection("MaxPerPage").Value ?? "10");
        var friendsId = dbContext.FriendEntities
            .Where(f => f.User1Id == userId || f.User2Id == userId)
            .Select(f => f.User1Id == userId ? f.User2Id : f.User1Id)
            .ToList();
        var friendPosts = dbContext.PostEntities
            .Where(p => p.UserId == userId || friendsId.Contains(p.UserId))
            .OrderByDescending(p => p.Id)
            .Skip(pageSize * (page - 1))
            .Take(pageSize)
            .ToList();

        var result = friendPosts.Select(p => new Post {
            Id = p.Id,
            Description = p.Description,
            ImageUrl = p.ImageUrl,
            Score = p.Score,
            UserId = p.UserId,
            NameSlug = getName(p.UserId)
        }).ToList();
        return result ?? new List<Post>();
    }


    private string getName(int id)
    {
        var user = dbContext.UserDataEntities.Include(x=>x.User).FirstOrDefault(x=> x.Id == id);
        
        if (!string.IsNullOrEmpty(user.User?.FirstName) && !string.IsNullOrWhiteSpace(user.User?.LastName))
        {
            return user.User.FirstName + " " + user.User.LastName;
        }

        return user.Email;
    }

    public Task<bool> Vote(int postId, bool positive)
    {
        var post = dbContext.PostEntities.FirstOrDefault(x => x.Id == postId);
        if (post == null)
        {
            throw new Exception("Cannot find post: " + postId);
        }

        post.Score = positive ? post.Score + 1 : post.Score - 1;
        dbContext.PostEntities.Update(post);
        dbContext.SaveChanges();
        return Task.FromResult(true);
    }
    
    public async Task<Post> AddImage( int id, IFormFile image)
    {
        var post = dbContext.PostEntities.FirstOrDefault(x => x.Id == id);
        if (post == null) throw new Exception("Post nie odnaleziony");
        var blob  = await blobStorageHelper.AddImage("post", id, image, false);
        if(string.IsNullOrEmpty(blob)) throw new Exception("Problem z dodaniem zdjęcia");
        post.ImageUrl = blob;
        dbContext.PostEntities.Update(post);
        dbContext.SaveChanges();
        return new Post{ Description = post.Description, ImageUrl = post.ImageUrl, Id = post.Id, NameSlug = getName(post.UserId), UserId = post.UserId, Score = post.Score};
    }
}