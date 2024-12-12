namespace CarStockApi.Endpoints.Dealer;

using CarStockApi.Dto.Request.Dealer;
using Dapper;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using CarStockApi.Models;
using System.Data;
using CarStockApi.Dto.Response;
using FastEndpoints.Security;

public class LoginDealerEndpoint : Endpoint<LoginDealerRequest, LoginDealerResponse>
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
        // Check if user exists by Username (Username is UNIQUE in database)
        var sql = "SELECT * FROM Dealers WHERE Username = @Username";
        var dealer = await _connection.QuerySingleOrDefaultAsync(sql, new { r.Credentials.Username });

        if (dealer == null)
        {
            await SendAsync(new LoginDealerResponse { Message = "Username not found." }, 401);
        }

        var result = _hasher.VerifyHashedPassword(r.Credentials, dealer.Password, r.Credentials.Password);

        // Authorisation success: send JWT Token
        if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            var jwtToken = JwtBearer.CreateToken(
                o =>
                {
                    o.ExpireAt = DateTime.UtcNow.AddDays(1);
                    o.User.Claims.Add(("DealerId", dealer.Id.ToString()));
                });

            await SendAsync(new LoginDealerResponse { Message = "Login success.", Token = jwtToken }, 200);
        }
        else
        {
            await SendAsync(new LoginDealerResponse { Message = "Wrong password." }, 401);
        }

        // TODO: rehash password if needed - low priority
        if (result == PasswordVerificationResult.SuccessRehashNeeded) 
        { 
            // rehash password
            // save rehashed password to DB
        }

    }
}
