namespace CarStockApi.Dto.Response;

public class CarResponse
{
    public required int Id { get; set; }
    public required string Make { get; set; }
    public required string Model { get; set; }
    public required int Year { get; set; }
    public required int StockLevel { get; set; }
}
