using System;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public SellerLevel SellerLevel { get; set; }
        public int SalesCompleted { get; set; }
    }
}