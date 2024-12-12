namespace CarStockApi.Mappers;

using CarStockApi.Dto.Response;
using CarStockApi.Models;

public static class CarToCarResponse
{
    public static CarResponse ToCarResponse(this Car car)
    {
        return new CarResponse
        {
            Id = car.Id,
            Make = car.Make,
            Model = car.Model,
            Year = car.Year,
            StockLevel = car.StockLevel
        };
    }
}
