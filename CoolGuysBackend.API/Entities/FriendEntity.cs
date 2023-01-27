using System.ComponentModel.DataAnnotations.Schema;
using CoolGuysBackend.UseCases.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolGuysBackend.Entities;
public class FriendEntity
{
    public int Id { get; set; }
    public int User1Id { get; set; }
    public int User2Id { get; set; }
    
    [ForeignKey(nameof(User1Id))]
    public UserDataEntity User1 { get; set; }
    [ForeignKey(nameof(User2Id))]
    public  UserDataEntity User2 { get; set; }
}