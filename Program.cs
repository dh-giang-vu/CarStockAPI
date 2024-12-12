using CarStockApi.Database;
using CarStockApi.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Data.SQLite;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();
builder.Services.AddSingleton<IDbConnection>(new SQLiteConnection("Data Source=Database/carstock.db"));
builder.Services.AddSingleton<PasswordHasher<DealerCredentials>>();
builder.Services.AddSingleton<DatabaseInitializer>();

var app = builder.Build();

var dbInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
dbInitializer.Initialize();

app.UseFastEndpoints();

app.Run();
