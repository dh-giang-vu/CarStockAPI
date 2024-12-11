namespace CarStockApi.Models;

public class Car
{
    public int Id { get; set; }
    public int DealerId { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public int StockLevel { get; set; }
}
