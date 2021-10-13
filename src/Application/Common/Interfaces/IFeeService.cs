using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Common.Interfaces
{
    public interface IFeeService
    {
        Task<decimal> GetFeeRate(SellerLevel sellerLevel);
        Task<decimal> CalculateFee(SellerLevel sellerLevel, decimal price);
    }
}