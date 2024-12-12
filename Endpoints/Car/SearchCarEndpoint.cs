namespace CarStockApi.Endpoints.Car;

using CarStockApi.Dto.Request.Car;
using CarStockApi.Dto.Response;
using CarStockApi.Mappers;
using CarStockApi.Models;
using Dapper;
using FastEndpoints;
using System.Data;

public class SearchCarEndpoint : Endpoint<SearchCarRequest, List<CarResponse>>
{
    private readonly IDbConnection _connection;

    public SearchCarEndpoint(IDbConnection connection)
    {
        _connection = connection;
    }

    public override void Configure()
    {
        Get("api/cars/search");
    }

    // Search by Make and/or Model
    public override async Task HandleAsync(SearchCarRequest r, CancellationToken c)
    {
        var dealerId = User.FindFirst("DealerId")?.Value;

        if (dealerId == null)
        {
            ThrowError("User has no claim named DealerId.");
        }

        var sql = "SELECT * FROM Cars WHERE DealerId = @DealerId";

        // if Make is specified then filter by Make
        if (!string.IsNullOrEmpty(r.Make))
        {
            sql += " AND Make = @Make";
        }

        // if Model is specified then filter by Model
        if (!string.IsNullOrEmpty(r.Model))
        {
            sql += " AND Model = @Model";
        }

        var cars = (await _connection.QueryAsync<Car>(sql, new 
        { 
            r.Make,
            r.Model,
            DealerId = dealerId 
        })).ToList();

        await SendOkAsync(cars.ToCarResponseList());
    }
}
