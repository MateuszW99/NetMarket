namespace Application.Identity.Requests
{
    public class ResetPasswordRequest
    {
        public string Email { get; init; }
        public string Password { get; init; }
        public string NewPassword { get; init; }
    }
}