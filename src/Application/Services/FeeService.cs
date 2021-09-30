using System.Collections.Generic;
using Application.Common.Interfaces;
using Domain.Enums;

namespace Application.Services
{
    public class FeeService : IFeeService
    {
        private readonly Dictionary<SellerLevel, decimal> _feesMap;

        public FeeService()
        {
            _feesMap = new Dictionary<SellerLevel, decimal>()
            {
                { SellerLevel.Beginner, 0.1m },
                { SellerLevel.Intermediate, 0.085m },
                { SellerLevel.Advanced, 0.06m },
                { SellerLevel.Business, 0.04m },
            };
        }


        public decimal GetFee(SellerLevel sellerLevel)
        {
            return _feesMap[sellerLevel];
        }

        public decimal CalculateFee(SellerLevel sellerLevel, decimal price)
        {
            var fee = _feesMap[sellerLevel];
            var calculatedFee = price * (1 + fee);
            return calculatedFee;
        }
    }
}