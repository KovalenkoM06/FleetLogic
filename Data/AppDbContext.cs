using FleetLogic.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FleetLogic.Data
{
    // Цей клас — це і є твоя База Даних у коді
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Список твоїх таблиць:
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Telemetry> TelemetryLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Alert> Alerts { get; set; }
    }
}