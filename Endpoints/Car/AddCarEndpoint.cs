namespace CarStockApi.Endpoints.Car;

using CarStockApi.Dto.Request.Car;
using CarStockApi.Dto.Response;
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
            await SendAsync(new GeneralResponse
            {
                Message = "Claim DealerId not found."
            }, 500);

            return;
        }

        if (await CheckCarIsInDB(r.Make, r.Model, r.Year, dealerId))
        {
            await SendAsync(new GeneralResponse
            {
                Message = "This car already exists.",
                Details = r
            }, 400);
            return;
        }

        var sql = "INSERT INTO Cars (Make, Model, StockLevel, Year, DealerId) " +
                  "VALUES (@Make, @Model, @StockLevel, @Year, @DealerId)";

        await _connection.ExecuteAsync(sql, new
        {
            r.Make,
            r.Model,
            r.Year,
            r.StockLevel,
            DealerId = dealerId
        });
        
        await SendOkAsync(new GeneralResponse
        {
            Message = "Car added successfully.",
            Details = r
        });
    }

    private async Task<bool> CheckCarIsInDB(string make, string model, int year, string dealerId)
    {
        var sql = "SELECT COUNT(*) FROM Cars WHERE Make = @Make AND Model = @Model AND Year = @Year AND DealerId = @DealerId";
        var count = await _connection.ExecuteScalarAsync<int>(sql, new
        {
            Make = make,
            Model = model,
            Year = year,
            DealerId = dealerId
        });

        return count > 0;
    }
}
