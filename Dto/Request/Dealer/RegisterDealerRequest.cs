﻿namespace CarStockApi.Dto.Request.Dealer;

using CarStockApi.Models;

public class RegisterDealerRequest
{
    public required string Name { get; set; }
    public required DealerCredentials Credentials { get; set; }

}
