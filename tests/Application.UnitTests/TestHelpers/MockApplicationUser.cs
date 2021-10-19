using System.Collections.Generic;
using Infrastructure.Identity;

namespace Application.UnitTests.TestHelpers
{
    public class MockApplicationUser : ApplicationUser
    {
        private readonly ICollection<string> roles;

        public MockApplicationUser(List<string> roles)
        {
            this.roles = roles;
        }

        public ICollection<string> Roles {
            get { return roles; }
        }
    }
}