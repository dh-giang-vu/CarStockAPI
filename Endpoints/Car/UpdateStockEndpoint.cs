namespace CarStockApi.Endpoints;

using CarStockApi.Dto.Request.Car;
using Dapper;
using FastEndpoints;
using System.Data;

public class UpdateStockEndpoint : Endpoint<UpdateStockRequest>
{
    private readonly IDbConnection _connection;

    public UpdateStockEndpoint(IDbConnection connection)
    {
        _connection = connection;
    }

    public override void Configure()
    {
        Put("api/cars");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateStockRequest r, CancellationToken c)
    {
        var sql = "UPDATE Cars SET StockLevel = @NewStockLevel WHERE Id = @CarId";
        var rowsAffected = await _connection.ExecuteAsync(sql, r);

        if (rowsAffected == 1)
        {
            await SendOkAsync();
        }
        else
        {
            ThrowError($"Error updating stock level of car {r.CarId}.", 500);
        }
    }
}
