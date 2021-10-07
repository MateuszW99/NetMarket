using Domain.Enums;

namespace Application.Common.Interfaces
{
    public interface IFeeService
    {
        decimal GetFeeRate(SellerLevel sellerLevel);
        decimal CalculateFee(SellerLevel sellerLevel, decimal price);
    }
}