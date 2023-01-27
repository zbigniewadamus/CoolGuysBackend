using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CoolGuysBackend.Contexts;
using CoolGuysBackend.Entities;
using CoolGuysBackend.UseCases._contracts;
using CoolGuysBackend.UseCases._contracts.dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CoolGuysBackend.Domain;

public class AuthService: IAuthService
{
    private readonly GlobalDbContext dbContext;
    private readonly string secretKey;
    private readonly PasswordHasher<UserDataEntity> hasher;

    public AuthService(GlobalDbContext dbContext, string secretKey)
    {
        this.dbContext = dbContext;
        this.secretKey = secretKey;
        this.hasher = new PasswordHasher<UserDataEntity>();
    }

    public string Login(LoginDto data)
    {
        var credential = dbContext.UserDataEntities.FirstOrDefault(x => x.Email == data.Email);
        if (credential == null) throw new Exception("Użytkownik nie został odnaleziony."); 
        var result = hasher.VerifyHashedPassword(credential, credential.Password, data.Password);
        if (result != PasswordVerificationResult.Success) throw new Exception("Hasło niepoprawne");
        return GenerateJwtToken(credential);
    }

    public string Register(RegisterDto data)
    {
        bool exist = dbContext.UserDataEntities.FirstOrDefault(x => x.Email == data.Email) != null;
        if (exist) throw new Exception("Użytkownik z podanym mailem został już zarejestowany.");
        if (data.Password != data.PasswordConfirm) throw new Exception("Hasła nie są takie same.");
        var newUserData = new UserDataEntity
        {
            Email = data.Email,
            User = new UserEntity()
        };
        newUserData.Password = hasher.HashPassword(newUserData, data.Password);
        var entity = dbContext.UserDataEntities.Add(newUserData);
        dbContext.SaveChanges();
        return GenerateJwtToken(entity.Entity);
    }
    private string GenerateJwtToken(UserDataEntity user)
    {
        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "coolguys",
            audience: "coolguys",
            claims: claims,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}