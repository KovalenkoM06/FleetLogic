namespace FleetLogic.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // У реальному житті тут хеш, але для лаби зійде пароль
        public string Role { get; set; } = "Dispatcher"; // "Admin", "Dispatcher"
    }
}