using System.Threading.Tasks;
using Dapper;
using Discount.Api.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Api.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        #region constractor
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion


        #region get coupon
        public async Task<Coupon> GetDiscount(string productName)
        {
            await using var connection = new NpgsqlConnection(
                _configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
                "Select * from Coupon where ProductName = @ProductName", new { ProductName = productName });
            if (coupon == null)
                return new Coupon
                {
                    ProductName = "No Discount",
                    Amount = 0,
                    Description = "No Discount"
                };
            return coupon;
        }

        #endregion

        #region create
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection(
                _configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync(
                "Insert Into Coupon (ProductName,Description,Amount)",
                new Coupon
                {
                    ProductName = coupon.ProductName,
                    Amount = coupon.Amount,
                    Description = coupon.Description
                });
            return affected != 0;
        }

        #endregion

        #region update
        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection(
                _configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync(
                "Update Coupon set ProductName=@ProductName, Description=@Description, Amount=@Amount where ProductName=@ProductName",
                new Coupon
                {
                    ProductName = coupon.ProductName,
                    Amount = coupon.Amount,
                    Description = coupon.Description
                });
            return affected != 0;
        }

        #endregion


        #region delete
        public async Task<bool> DeleteDiscount(string productName)
        {
            await using var connection = new NpgsqlConnection(
                _configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync(
                "Delete From Coupon where ProductName=@ProductName",
                new { ProductName =productName });
            return affected != 0;
        }

        #endregion
    }
}
