namespace CarStockApi.Endpoints.Car;

using CarStockApi.Dto.Request.Car;
using Dapper;
using FastEndpoints;
using System.Data;

public class AddCarEndpoint : Endpoint<AddCarRequest>
{
    private readonly IDbConnection _connection;

    public AddCarEndpoint(IDbConnection connection)
    {
        _connection = connection;
    }

    public override void Configure()
    {
        Post("/api/cars");
    }

    public override async Task HandleAsync(AddCarRequest r, CancellationToken c)
    {
        var dealerId = User.FindFirst("DealerId")?.Value;

        if (dealerId == null)
        {
            ThrowError("Claim DealerId not found.", 500);
        }

        var sql = "INSERT INTO Cars (Make, Model, StockLevel, Year, DealerId) " +
                  "VALUES (@Make, @Model, @StockLevel, @Year, @DealerId)";

        var rowsAffected = await _connection.ExecuteAsync(sql, new
        {
            r.Make,
            r.Model,
            r.Year,
            r.StockLevel,
            DealerId = dealerId
        });

        if (rowsAffected == 1)
        {
            await SendOkAsync();
        }
        else
        {
            await SendAsync("Failed to add car.", 500);
        }
    }
}
