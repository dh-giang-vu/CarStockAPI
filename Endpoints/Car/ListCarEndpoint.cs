namespace CarStockApi.Endpoints.Car;

using CarStockApi.Dto.Response;
using CarStockApi.Mappers;
using CarStockApi.Models;
using Dapper;
using FastEndpoints;
using System.Data;

public class ListCarEndpoint : EndpointWithoutRequest<CarResponseList>
{
    private readonly IDbConnection _connection;

    public ListCarEndpoint(IDbConnection connection)
    {
        _connection = connection;
    }

    public override void Configure()
    {
        Get("/api/cars");
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var dealerId = User.FindFirst("DealerId")?.Value;

        if (dealerId == null)
        {
            // ThrowError("Claim DealerId not found.", 500);
            await SendAsync(new CarResponseList
            {
                Message = "Claim DealerId not found."
            }, 500);
            
            return;
        }

        var sql = "SELECT * FROM Cars WHERE DealerId = @DealerId";
        var cars = (await _connection.QueryAsync<Car>(sql, new { DealerId = int.Parse(dealerId) })).ToList();

        await SendOkAsync(cars.ToCarResponseList());
    }
}
