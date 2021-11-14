using System.Collections.Generic;

namespace Application.Identity.Responses
{
    public class DeleteUserResponse
    {
        public bool Success { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}