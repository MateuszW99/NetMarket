namespace Application.IntegrationTests.Helpers
{
    public static class AdminUser
    {
        public static string Email => "administrator@admin.com";
        public static string Password => "Admin123_";
    } 
    public static class SupervisorUser
    {
        public static string Email => "supervisor@supervisor.com";
        public static string Password => "Supervisor123_";
    }  
    public static class DefaultUser
    {
        public static string Email => "user@user.com";
        public static string Password => "User123_";
    }
}