namespace CarStockApi.Endpoints.Car;

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
    }

    public override async Task HandleAsync(UpdateStockRequest r, CancellationToken c)
    {
        var dealerId = User.FindFirst("DealerId")?.Value;

        if (dealerId == null)
        {
            ThrowError("Claim DealerId not found.", 500);
        }

        var sql = "UPDATE Cars SET StockLevel = @NewStockLevel WHERE Id = @CarId AND DealerId = @DealerId";
        var rowsAffected = await _connection.ExecuteAsync(sql, new 
        {
            r.NewStockLevel,
            r.CarId,
            DealerId = dealerId
        });

        if (rowsAffected == 1)
        {
            await SendOkAsync();
        }
        else
        {
            await SendAsync($"Not authorised to update stock level of car {r.CarId}.", 401);
        }
    }
}
