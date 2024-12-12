namespace CarStockApi.Endpoints;

using CarStockApi.Dto.Response;
using CarStockApi.Mappers;
using CarStockApi.Models;
using Dapper;
using FastEndpoints;
using System.Data;

public class ListCarEndpoint : EndpointWithoutRequest<List<CarResponse>>
{
    private readonly IDbConnection _connection;

    public ListCarEndpoint(IDbConnection connection)
    {
        _connection = connection;
    }

    public override void Configure()
    {
        Get("/api/cars");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var sql = "SELECT * FROM Cars";
        var cars = (await _connection.QueryAsync<Car>(sql)).ToList();

        await SendOkAsync(cars.ToCarResponseList());
    }
}
