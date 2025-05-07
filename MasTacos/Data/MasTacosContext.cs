using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MasTacos.Models;


namespace MasTacos.Data
{
    public class MasTacosContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public MasTacosContext(DbContextOptions<MasTacosContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<SurveyResponse> SurveyResponses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Customer configuration
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // TimeSlot configuration
            modelBuilder.Entity<TimeSlot>()
                .HasIndex(t => new { t.DayOfWeek, t.StartTime });

            // Reservation configuration
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.TimeSlot)
                .WithMany()
                .HasForeignKey(r => r.TimeSlotId)
                .OnDelete(DeleteBehavior.SetNull);

            // Order configuration
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.TimeSlot)
                .WithMany()
                .HasForeignKey(o => o.TimeSlotId)
                .OnDelete(DeleteBehavior.SetNull);

            // OrderItem configuration
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.MenuItem)
                .WithMany()
                .HasForeignKey(oi => oi.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // SurveyResponse configuration
            modelBuilder.Entity<SurveyResponse>()
                .HasOne(s => s.Customer)
                .WithMany(c => c.SurveyResponses)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SurveyResponse>()
                .HasOne(s => s.Order)
                .WithOne(o => o.SurveyResponse)
                .HasForeignKey<SurveyResponse>(s => s.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SurveyResponse>()
                .HasIndex(s => s.OrderId)
                .IsUnique();
        }
    }
}