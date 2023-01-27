using System.Text;
using CoolGuysBackend.Contexts;
using CoolGuysBackend.Domain;
using CoolGuysBackend.Helpers;
using CoolGuysBackend.UseCases._contracts;
using CoolGuysBackend.UseCases.Auth;
using CoolGuysBackend.UseCases.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Post = CoolGuysBackend.UseCases.Post.Post;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "coolguys",
        ValidAudience = "coolguys",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JwtSecret")?.Value))
    };
});
string _cors = "CorsPolicy";
builder.Services.AddCors(opt =>
{
    
    opt.AddPolicy(name: _cors, builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddDbContext<GlobalDbContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
    // options.UseInMemoryDatabase("DevelopDB");
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBlobStorageHelper, BlobStorageHelper>(x => new BlobStorageHelper( config.GetConnectionString("BlobStorage")));
builder.Services.AddScoped<TokenHelper>(x => new TokenHelper(config.GetSection("JwtSecret")?.Value));

//Auth
builder.Services.AddScoped<IAuthService, AuthService>(x => new AuthService(x.GetRequiredService<GlobalDbContext>(), config.GetSection("JwtSecret")?.Value));
builder.Services.AddScoped<Login>();
builder.Services.AddScoped<Register>();

//Post
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<Post>();

//User
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<Avatar>();
builder.Services.AddScoped<CurrentUser>();
builder.Services.AddScoped<Details>();
builder.Services.AddScoped<Friend>();
builder.Services.AddScoped<Search>();


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    app.UseCors(_cors);
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();