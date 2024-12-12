namespace CarStockApi.Database;

using CarStockApi.Models;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data;

public class DatabaseInitializer
{
    private readonly IDbConnection _dbConnection;
    private readonly PasswordHasher<DealerCredentials> _hasher;

    public DatabaseInitializer(IDbConnection dbConnection, PasswordHasher<DealerCredentials> hasher)
    {
        _dbConnection = dbConnection;
        _hasher = hasher;
    }

    public void Initialize()
    {
        _dbConnection.Open();
        CreateTables();
        InsertSampleData();
        _dbConnection.Close();
    }

    // Create Cars and Dealers tables if they don't exist
    private void CreateTables()
    {
        var createDealersTable = @"
        CREATE TABLE IF NOT EXISTS Dealers (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Username TEXT NOT NULL,
            Password TEXT NOT NULL,
            UNIQUE(Username)
        );";

        var createCarsTable = @"
        CREATE TABLE IF NOT EXISTS Cars (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Make TEXT NOT NULL,
            Model TEXT NOT NULL,
            Year INTEGER NOT NULL,
            StockLevel INTEGER NOT NULL,
            DealerId INTEGER,
            FOREIGN KEY (DealerId) REFERENCES Dealers(Id) 
            ON DELETE CASCADE
        );";

        _dbConnection.Execute(createDealersTable);
        _dbConnection.Execute(createCarsTable);
    }

    // Add some Dealers and Cars to DB (if there are none) for testing convenience
    private void InsertSampleData()
    {
        var dealerCount = _dbConnection.ExecuteScalar<int>("SELECT COUNT(*) FROM Dealers");

        if (dealerCount == 0)
        {
            DealerCredentials dealerACreds = new() { Username = "alice123", Password = "pass" };

            _dbConnection.Execute("INSERT INTO Dealers (Name, Username, Password) VALUES (@Name, @Username, @Password)",
                new { Name = "Alice", Username = dealerACreds.Username, Password = _hasher.HashPassword(dealerACreds, dealerACreds.Password) });

            DealerCredentials dealerBCreds = new() { Username = "bobCarDealer", Password = "password123" };

            _dbConnection.Execute("INSERT INTO Dealers (Name, Username, Password) VALUES (@Name, @Username, @Password)",
                new { Name = "Bob", Username = dealerBCreds.Username, Password = _hasher.HashPassword(dealerBCreds, dealerBCreds.Password) });
        }

        var carCount = _dbConnection.ExecuteScalar<int>("SELECT COUNT(*) FROM Cars");

        if (carCount == 0)
        {
            _dbConnection.Execute("INSERT INTO Cars (Make, Model, Year, StockLevel, DealerId) VALUES (@Make, @Model, @Year, @StockLevel, @DealerId)",
                new { Make = "Toyota", Model = "Corolla", Year = 2020, StockLevel = 10, DealerId = 1 });

            _dbConnection.Execute("INSERT INTO Cars (Make, Model, Year, StockLevel, DealerId) VALUES (@Make, @Model, @Year, @StockLevel, @DealerId)",
                new { Make = "Ford", Model = "Focus", Year = 2021, StockLevel = 5, DealerId = 1 });

            _dbConnection.Execute("INSERT INTO Cars (Make, Model, Year, StockLevel, DealerId) VALUES (@Make, @Model, @Year, @StockLevel, @DealerId)",
                new { Make = "Honda", Model = "Civic", Year = 2020, StockLevel = 8, DealerId = 2 });
        }
    }
}
