using FastEndpoints;

using System.Data;
using System.Data.SQLite;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();
builder.Services.AddSingleton<IDbConnection>(new SQLiteConnection("Data Source=Database/carstock.db"));

var app = builder.Build();

app.UseFastEndpoints();

app.Run();
