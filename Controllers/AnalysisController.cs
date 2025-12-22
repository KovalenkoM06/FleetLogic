using FleetLogic.Data;
using FleetLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetLogic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AnalysisController(AppDbContext context)
        {
            _context = context;
        }

        // 2. БІЗНЕС-ЛОГІКА: Перевірка втоми водія
        // POST: api/Analysis/check-fatigue/5
        [HttpPost("check-fatigue/{truckId}")]
        public async Task<IActionResult> CheckDriverFatigue(int truckId)
        {
            // Крок 1: Знаходимо, хто зараз за кермом цієї фури
            var truck = await _context.Trucks.FindAsync(truckId);
            if (truck == null) return NotFound("Фура не знайдена");

            // Шукаємо водія, який закріплений за цією фурою
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.CurrentTruckId == truckId);
            if (driver == null) return BadRequest("У цієї фури немає водія зараз.");

            // Крок 2: Математична обробка (Бізнес-логіка)
            // Рахуємо кількість записів телеметрії за останні 24 години
            var startTime = DateTime.UtcNow.AddHours(-24);

            var logsCount = await _context.TelemetryLogs
                .Where(t => t.TruckId == truckId && t.Timestamp > startTime)
                .CountAsync();

            // УМОВА ЛОГІКИ: Припустимо, що 1 лог = 1 хвилина їзди.
            // Якщо більше 10 логів (хвилин) - він втомився (для тесту ставимо мале число)
            int fatigueThreshold = 10;

            if (logsCount > fatigueThreshold)
            {
                // Крок 3: Створення результату (Alert)
                var alert = new Alert
                {
                    DriverId = driver.Id,
                    Timestamp = DateTime.UtcNow,
                    Severity = "High",
                    Message = $"Водій {driver.FullName} перепрацював! Зафіксовано {logsCount} одиниць активності."
                };

                _context.Alerts.Add(alert);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Status = "DANGER",
                    Message = "Тривога створена! Водій втомився.",
                    ActivityCount = logsCount
                });
            }

            return Ok(new
            {
                Status = "OK",
                Message = "Водій в нормальному стані",
                ActivityCount = logsCount
            });
        }
    }
}