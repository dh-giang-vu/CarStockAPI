namespace CarStockApi.Database;

using CarStockApi.Models;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Xml.Linq;

public class DatabaseInitializer
{
    private readonly IDbConnection _dbConnection;

    public DatabaseInitializer(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
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
            PasswordHasher<Dealer> passwordHasher = new PasswordHasher<Dealer>();

            Dealer dealerA = new Dealer();
            dealerA.Name = "Alice";
            dealerA.Username = "alice123";
            dealerA.Password = "pass";

            _dbConnection.Execute("INSERT INTO Dealers (Name, Username, Password) VALUES (@Name, @Username, @Password)",
                new { Name = dealerA.Name, Username = dealerA.Username, Password = passwordHasher.HashPassword(dealerA, dealerA.Password) });

            Dealer dealerB = new Dealer();
            dealerB.Name = "Bob";
            dealerB.Username = "carDealerBob";
            dealerB.Password = "password123";

            _dbConnection.Execute("INSERT INTO Dealers (Name, Username, Password) VALUES (@Name, @Username, @Password)",
                new { Name = dealerB.Name, Username = dealerB.Username, Password = passwordHasher.HashPassword(dealerB, dealerB.Password) });
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
