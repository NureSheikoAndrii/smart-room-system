namespace SmartRoomBackend.Data
{
    using SmartRoomBackend.Models;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }

        public DbSet<SensorData> Sensors { get; set; } // ТАблица БД
    }
}
