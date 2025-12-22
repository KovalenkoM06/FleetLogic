namespace FleetLogic.Models
{
    public class Truck
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; } = string.Empty; // Номер авто
        public string Status { get; set; } = "Parking"; // Parking, Moving, Error
        public double CurrentFuel { get; set; } // Рівень палива
    }
}