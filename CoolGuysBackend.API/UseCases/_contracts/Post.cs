namespace CoolGuysBackend.UseCases._contracts;

public class Post
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Description { get; set; }
    public int Score { get; set; }
}