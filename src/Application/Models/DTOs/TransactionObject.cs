using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Models.DTOs
{
    public class TransactionObject : IMapFrom<Transaction>
    {
        public string Id { get; set; }
        public string AssignedSupervisorId { get; set; }
        public decimal CompanyProfit { get; set; }
        
        public AskObject Ask { get; set; }
        public decimal SellerFee { get; set; }
        public decimal SellerPayout { get; set; }
        
        public BidObject Bid { get; set; }
        public decimal BuyerFee { get; set; }
        public decimal TotalBuyerCost { get; set; }
        
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Transaction, TransactionObject>()
                .ForMember(x => x.Ask,
                    opt => opt.MapFrom(y => y.Ask))
                .ForMember(x => x.Bid,
                    opt => opt.MapFrom(y => y.Bid));
        }
    }
}