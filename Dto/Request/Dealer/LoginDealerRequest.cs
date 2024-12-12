namespace CarStockApi.Dto.Request.Dealer;

using CarStockApi.Models;

public class LoginDealerRequest
{
    public required DealerCredentials Credentials { get; set; }

}
