namespace CarStockApi.Endpoints;

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
        AllowAnonymous();
    }

    // Search by Make and/or Model
    public override async Task HandleAsync(SearchCarRequest r, CancellationToken c)
    {
        var sql = "SELECT * FROM Cars WHERE 1=1";

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

        var cars = (await _connection.QueryAsync<Car>(sql, r)).ToList();

        var carResponses = new List<CarResponse>();
        foreach (var car in cars)
        {
            carResponses.Add(car.ToCarResponse());
        }

        await SendOkAsync(carResponses);
    }
}
