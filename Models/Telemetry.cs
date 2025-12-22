namespace FleetLogic.Models
{
    public class Telemetry
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public double Speed { get; set; }
        public double GpsLatitude { get; set; }
        public double GpsLongitude { get; set; }

        // Зв'язок: Цей запис належить конкретній фурі
        public int TruckId { get; set; }
        public Truck? Truck { get; set; }
    }
}