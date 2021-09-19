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
    public static class FirstUser
    {
        public static string Email => "user1@user.com";
        public static string Password => "User123_";
    }

    public static class OtherUser
    {
        public static string Email => "user2@user.com";
        public static string Password => "User456_";
    }
}