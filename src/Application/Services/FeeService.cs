using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        
        public Task<decimal> GetFeeRate(SellerLevel sellerLevel)
        {
            return Task.FromResult(_feesMap[sellerLevel]);
        }

        public Task<decimal> CalculateFee(SellerLevel sellerLevel, decimal price)
        {
            if (_feesMap.TryGetValue(sellerLevel, out var feeRate))
            {
                var calculatedFee = price * feeRate;
                return Task.FromResult(calculatedFee);    
            }
            throw new Exception($"Seller level: {sellerLevel.ToString()} doesn't exist.");
        }
    }
}