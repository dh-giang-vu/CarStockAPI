namespace CarStockApi.Dto.Request.Dealer;

public class RegisterDealerRequest
{
    public required string Name { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }

}
