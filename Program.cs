using CarStockApi.Database;
using CarStockApi.Models;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Data.SQLite;


var builder = WebApplication.CreateBuilder(args);

var signingKey = Environment.GetEnvironmentVariable("JwtSigningKey") ?? "nZ8lKA8Xd2n/j3ynAdbo6UXrvpLz8Ih/Ezi//SVtF+Y=";

builder.Services
    .AddAuthenticationJwtBearer(s => s.SigningKey = signingKey) 
    .AddAuthorization()
    .AddFastEndpoints();

builder.Services.Configure<JwtCreationOptions>(options => options.SigningKey = signingKey);

builder.Services.AddSingleton<IDbConnection>(new SQLiteConnection("Data Source=Database/carstock.db"));
builder.Services.AddSingleton<PasswordHasher<DealerCredentials>>();
builder.Services.AddSingleton<DatabaseInitializer>();

var app = builder.Build();

var dbInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
dbInitializer.Initialize();

app.UseAuthentication()
   .UseAuthorization()
   .UseFastEndpoints();

app.Run();
