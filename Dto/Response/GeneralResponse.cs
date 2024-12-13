namespace CarStockApi.Dto.Response;

public class GeneralResponse
{
    public required string Message { get; set; }
    public object? Details { get; set; }
}