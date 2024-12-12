namespace CarStockApi.Endpoints.Dealer;

using CarStockApi.Dto.Request.Dealer;
using Dapper;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using CarStockApi.Models;
using System.Data;

public class RegisterDealerEndpoint : Endpoint<RegisterDealerRequest>
{
    private readonly IDbConnection _connection;
    private readonly PasswordHasher<DealerCredentials> _hasher;

    public RegisterDealerEndpoint(IDbConnection connection, PasswordHasher<DealerCredentials> hasher)
    {
        _connection = connection;
        _hasher = hasher;
    }

    public override void Configure()
    {
        Post("/api/dealers/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterDealerRequest r, CancellationToken c)
    {
        var sql = "INSERT INTO Dealers (Name, Username, Password) VALUES (@Name, @Username, @Password)";

        var rowsAffected = await _connection.ExecuteAsync(sql, new { Name = r.Name, Username = r.Credentials.Username, Password = _hasher.HashPassword(r.Credentials, r.Credentials.Password) });

        if (rowsAffected == 1)
        {
            await SendOkAsync();
        }
        else
        {
            await SendAsync("Failed to register new dealer.", 500);
        }
    }
}
