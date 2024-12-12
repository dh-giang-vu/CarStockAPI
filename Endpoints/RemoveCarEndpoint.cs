﻿namespace CarStockApi.Endpoints;

using Dapper;
using FastEndpoints;
using System.Data;

public class RemoveCarEndpoint : EndpointWithoutRequest
{
    private readonly IDbConnection _connection;

    public RemoveCarEndpoint(IDbConnection connection)
    {
        _connection = connection;
    }

    public override void Configure()
    {
        Delete("/api/cars/{CarId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var carId = Route<int>("CarId");
        var sql = "DELETE FROM Cars WHERE Id = @Id";
        var rowsAffected = await _connection.ExecuteAsync(sql, new { Id = carId });

        if (rowsAffected == 1)
        {
            await SendOkAsync();
        }
        else
        {
            ThrowError($"Error removing car {carId}.", 500);
        }
    }
}