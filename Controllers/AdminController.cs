using FleetLogic.Data;
using FleetLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text;

namespace FleetLogic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // 1. ФУНКЦІЯ АДМІНІСТРУВАННЯ: Експорт даних (Backup)
        // GET: api/Admin/export/drivers
        [HttpGet("export/drivers")]
        public async Task<IActionResult> ExportDrivers()
        {
            // Витягуємо всіх водіїв з бази
            var drivers = await _context.Drivers.ToListAsync();

            // Перетворюємо їх у текст JSON
            var json = JsonSerializer.Serialize(drivers, new JsonSerializerOptions { WriteIndented = true });

            // Перетворюємо текст у байти для файлу
            var fileBytes = Encoding.UTF8.GetBytes(json);

            // Віддаємо файл користувачу
            return File(fileBytes, "application/json", "drivers_backup.json");
        }
    }
}