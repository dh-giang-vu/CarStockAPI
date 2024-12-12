namespace CarStockApi.Dto.Response;

public class LoginDealerResponse
{
    public required string Message { get; set; }
    public string? Token { get; set; }
}
