using Domain.Enums;

namespace Application.Common.Interfaces
{
    public interface IFeeService
    {
        decimal GetFee(SellerLevel sellerLevel);
        decimal CalculateFee(SellerLevel sellerLevel, decimal price);
    }
}