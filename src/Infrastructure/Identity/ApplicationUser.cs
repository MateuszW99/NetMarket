using System;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Guid UserSettingsId { get; set; }
        public UserSettings UserSettings { get; set; }
    }
}