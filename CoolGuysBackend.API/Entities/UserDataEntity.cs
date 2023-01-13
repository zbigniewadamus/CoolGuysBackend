using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolGuysBackend.Entities;

public class UserDataEntity
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public void Configure(EntityTypeBuilder<UserDataEntity> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.ToTable("UserData");
    }
}