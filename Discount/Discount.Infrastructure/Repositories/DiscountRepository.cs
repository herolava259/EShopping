﻿using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;


namespace Discount.Infrastructure.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;

    public DiscountRepository(IConfiguration configuration)
    {
        this._configuration = configuration;
    }
    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affectedRow = await
                            connection.ExecuteAsync
                            ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

        if (affectedRow == 0)
            return false;

        return true;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affectedRow = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
                                        new {ProductName = productName});

        if (affectedRow == 0) return false;

        return true;


    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
            ("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

        if(coupon is null)
            return new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Available" };

        return coupon;

    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affectedRow = await
                            connection.ExecuteAsync
                            ("UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                                new {ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

        if(affectedRow == 0)
            return false;

        return true;
    }
}
