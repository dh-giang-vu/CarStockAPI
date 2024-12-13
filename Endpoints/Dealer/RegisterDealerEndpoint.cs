namespace CarStockApi.Endpoints.Dealer;

using CarStockApi.Dto.Request.Dealer;
using Dapper;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using CarStockApi.Models;
using System.Data;
using CarStockApi.Dto.Response;

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

        try
        {
            var rowsAffected = await _connection.ExecuteAsync(sql, new
            {
                r.Name,
                r.Credentials.Username,
                Password = _hasher.HashPassword(r.Credentials, r.Credentials.Password)
            });

            if (rowsAffected == 1)
            {
                await SendOkAsync(new GeneralResponse
                {
                    Message = "Dealer registered successfully."
                });
            }
        }
        // catch error: registered with already existing username
        catch (System.Data.SQLite.SQLiteException ex) when (ex.ResultCode == System.Data.SQLite.SQLiteErrorCode.Constraint && ex.Message.Contains("UNIQUE constraint failed: Dealers.Username"))
        {
            await SendAsync(new GeneralResponse
            {
                Message = "Username is already taken.",
                Details = r.Credentials.Username
            }, 400);
        }

        // let ASP.NET handle other uncaught exceptions by itself (for now)
    }
}
