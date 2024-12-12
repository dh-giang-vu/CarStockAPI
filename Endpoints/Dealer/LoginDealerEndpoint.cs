namespace CarStockApi.Endpoints.Dealer;

using CarStockApi.Dto.Request.Dealer;
using Dapper;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using CarStockApi.Models;
using System.Data;

public class LoginDealerEndpoint : Endpoint<LoginDealerRequest>
{
    private readonly IDbConnection _connection;
    private readonly PasswordHasher<DealerCredentials> _hasher;

    public LoginDealerEndpoint(IDbConnection connection, PasswordHasher<DealerCredentials> hasher)
    {
        _connection = connection;
        _hasher = hasher;
    }

    public override void Configure()
    {
        Post("/api/dealers/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginDealerRequest r, CancellationToken c)
    {
        var sql = "SELECT * FROM Dealers WHERE Username = @Username";
        var dealer = await _connection.QuerySingleOrDefaultAsync(sql, new { r.Credentials.Username });

        if (dealer == null)
        {
            await SendUnauthorizedAsync();
            return;
        }

        var result = _hasher.VerifyHashedPassword(r.Credentials, dealer.Password, r.Credentials.Password);

        if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            await SendOkAsync();
        }
        else
        {
            await SendUnauthorizedAsync();
            return;
        }

        if (result == PasswordVerificationResult.SuccessRehashNeeded) 
        { 
            // TODO: Rehash password
        }

    }
}
