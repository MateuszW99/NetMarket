namespace Application.Identity.Requests
{
    public class UserLoginRequest
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }
}