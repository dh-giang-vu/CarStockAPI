namespace CarStockApi.Dto.Request.Car;

public class UpdateStockRequest
{
    public required int CarId { get; set; }
    public required int NewStockLevel { get; set; }
}
