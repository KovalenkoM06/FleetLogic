namespace FleetLogic.Models
{
    public class Alert
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Message { get; set; } = string.Empty; // Наприклад: "Перевищення часу керування!"
        public string Severity { get; set; } = "Warning"; // "Info", "Warning", "Critical"

        // До кого відноситься тривога
        public int DriverId { get; set; }
        public Driver? Driver { get; set; }
    }
}