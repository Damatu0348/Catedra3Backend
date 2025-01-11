using System.Text;
using api.Src.Data;
using api.Src.Helpers;
using api.Src.Interfaces;
using api.Src.Models;
using api.Src.Services;
using CloudinaryDotNet;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var cloudinarySettings = builder
    .Configuration.GetSection("CloudinarySettings")
    .Get<CloudinarySettings>();
var cloudinaryAccount = new Account(
    cloudinarySettings!.CloudName,
    cloudinarySettings.ApiKey,
    cloudinarySettings.ApiSecret
);
var cloudinary = new Cloudinary(cloudinaryAccount);
builder.Services.AddSingleton(cloudinary);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<ITokenService, TokenService>();

builder
    .Services.AddIdentity<Usuario, IdentityRole>(opt =>
    {
        opt.Password.RequireDigit = false;
        opt.Password.RequireLowercase = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequiredLength = 12;
    })
    .AddEntityFrameworkStores<ApplicationDBContext>();

builder
    .Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme =
            opt.DefaultChallengeScheme =
            opt.DefaultForbidScheme =
            opt.DefaultScheme =
            opt.DefaultSignInScheme =
            opt.DefaultSignOutScheme =
                JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:SigningKey"]
                        ?? throw new ArgumentNullException("Jwt:SigningKey")
                )
            ),
        };
    });

string connectionString =
    Environment.GetEnvironmentVariable("DATABASE_URL") ?? "Data Source=database.db";
builder.Services.AddDbContext<ApplicationDBContext>(opt => opt.UseSqlite(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
        }
    );
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDBContext>();
    await context.Database.MigrateAsync();
    _ = DataSeeder.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
