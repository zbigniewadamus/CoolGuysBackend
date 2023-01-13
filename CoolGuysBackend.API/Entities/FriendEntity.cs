using Microsoft.EntityFrameworkCore;

namespace CoolGuysBackend.Entities;
[PrimaryKey(nameof(User1), nameof(User2))]
public class FriendEntity
{
    public int User1 { get; set; }
    public int User2 { get; set; }
    
}