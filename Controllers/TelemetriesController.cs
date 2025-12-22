using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FleetLogic.Data;
using FleetLogic.Models;

namespace FleetLogic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelemetriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TelemetriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Telemetries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Telemetry>>> GetTelemetryLogs()
        {
            return await _context.TelemetryLogs.ToListAsync();
        }

        // GET: api/Telemetries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Telemetry>> GetTelemetry(int id)
        {
            var telemetry = await _context.TelemetryLogs.FindAsync(id);

            if (telemetry == null)
            {
                return NotFound();
            }

            return telemetry;
        }

        // PUT: api/Telemetries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTelemetry(int id, Telemetry telemetry)
        {
            if (id != telemetry.Id)
            {
                return BadRequest();
            }

            _context.Entry(telemetry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TelemetryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DTO для прийому даних
        public class TelemetryDto : Telemetry
        {
            public int DrivingTimeHours { get; set; } // Час в дорозі
        }

        [HttpPost]
        public async Task<ActionResult<Telemetry>> PostTelemetry(TelemetryDto telemetryDto)
        {
            // Створюємо змінну для статусу
            string status = "OK";
            string message = "Normal operation.";

            // 1. ПЕРЕВІРКА ЧАСУ (Тахограф)
            if (telemetryDto.DrivingTimeHours > 9)
            {
                status = "FATIGUE";
                message = $"CRITICAL: Driving time {telemetryDto.DrivingTimeHours}h exceeds limit (9h)!";
            }

            // 2. ПЕРЕВІРКА ШВИДКОСТІ (Спідометр)
            // Якщо вже є втома, додаємо і перевищення
            if (telemetryDto.Speed > 90)
            {
                if (status == "FATIGUE")
                {
                    status = "DOUBLE_DANGER"; // Комбо!
                    message += " AND Speeding detected!";
                }
                else
                {
                    status = "SPEEDING";
                    message = "Speed limit exceeded!";
                }
            }

            // 3. Зберігаємо в базу
            var telemetry = new Telemetry
            {
                TruckId = telemetryDto.TruckId,
                Speed = telemetryDto.Speed,
                GpsLatitude = telemetryDto.GpsLatitude,
                GpsLongitude = telemetryDto.GpsLongitude,
                Timestamp = DateTime.UtcNow
            };

            _context.TelemetryLogs.Add(telemetry);
            await _context.SaveChangesAsync();

            // Повертаємо вердикт
            return Ok(new
            {
                status = status,
                message = message
            });
        }

        // DELETE: api/Telemetries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTelemetry(int id)
        {
            var telemetry = await _context.TelemetryLogs.FindAsync(id);
            if (telemetry == null)
            {
                return NotFound();
            }

            _context.TelemetryLogs.Remove(telemetry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TelemetryExists(int id)
        {
            return _context.TelemetryLogs.Any(e => e.Id == id);
        }
    }
}
