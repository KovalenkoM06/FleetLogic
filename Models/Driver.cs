namespace FleetLogic.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }

        // Зв'язок: Водій може бути закріплений за фурою
        public int? CurrentTruckId { get; set; }
    }
}