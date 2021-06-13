using System.Collections.Generic;

namespace Application.Identity.Responses
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}