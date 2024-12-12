namespace CarStockApi.Dto.Request.Car;

public class AddCarRequest
{
    public required int DealerId { get; set; }  // TODO: remove later for authorisation(?) (separate dealers)
    public required string Make { get; set; }
    public required string Model { get; set; }
    public required int Year { get; set; }
    public int StockLevel { get; set; }
}
