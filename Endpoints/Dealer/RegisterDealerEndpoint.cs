namespace CarStockApi.Endpoints.Dealer;

using CarStockApi.Dto.Request.Dealer;
using Dapper;
using FastEndpoints;
using System.Data;

public class RegisterDealerEndpoint : Endpoint<RegisterDealerRequest>
{
    private readonly IDbConnection _connection;

    public RegisterDealerEndpoint(IDbConnection connection)
    {
        _connection = connection;
    }

    public override void Configure()
    {
        Post("/api/dealers");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterDealerRequest r, CancellationToken c)
    {
        var sql = "INSERT INTO Dealers (Name, Username, Password) VALUES (@Name, @Username, @Password)";

        var rowsAffected = await _connection.ExecuteAsync(sql, r);

        if (rowsAffected == 1)
        {
            await SendOkAsync();
        }
        else
        {
            ThrowError("Failed to register new dealer.", 500);
        }
    }
}
