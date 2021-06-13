namespace Application.Identity.Requests
{
    public class UserRegistrationRequest
    {
        public string Email { get; init; }
        public string UserName { get; init; }
        public string Password { get; init; }
    }
}