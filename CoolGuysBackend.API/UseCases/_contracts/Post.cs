using CoolGuysBackend.Contexts;

namespace CoolGuysBackend.UseCases._contracts;

public class Post
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public string NameSlug { get; set; }

    public string? Description { get; set; }
    public int Score { get; set; }
    public string ImageUrl { get; set; }
}