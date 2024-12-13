namespace CarStockApi.Endpoints.Car;

using CarStockApi.Dto.Response;
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
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var dealerId = User.FindFirst("DealerId")?.Value;

        if (dealerId == null)
        {
            await SendAsync(new GeneralResponse
            {
                Message = "Claim DealerId not found."
            }, 500);

            return;
        }

        var carId = Route<int>("CarId");
        var sql = "DELETE FROM Cars WHERE Id = @Id AND DealerId = @DealerId";
        var rowsAffected = await _connection.ExecuteAsync(sql, new { Id = carId, DealerId = dealerId });

        if (rowsAffected == 1)
        {
            await SendOkAsync(new GeneralResponse
            {
                Message = "Car removed successfully.",
                Details = carId
            });
        }
        else
        {
            await SendAsync(new GeneralResponse
            {
                Message = "Not authorised to remove car.",
                Details = carId   
            }, 401);
        }
    }
}
