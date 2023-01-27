using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CoolGuysBackend.Entities;


[PrimaryKey(nameof(Id))]
public class PostEntity
{
    public int Id { get; set; }
    [ForeignKey(nameof(UserId))]
    public int UserId { get; set; }
    public string? Description { get; set; }
    public int Score { get; set; } = 0;
    public string ImageUrl { get; set; } = String.Empty;
}