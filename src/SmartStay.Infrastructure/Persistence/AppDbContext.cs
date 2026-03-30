using Microsoft.EntityFrameworkCore;
using SmartStay.Domain.Entities;
using System.Reflection;

namespace SmartStay.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Property> Properties => Set<Property>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<ServiceConfig> ServiceConfigs => Set<ServiceConfig>();
    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<Roommate> Roommates => Set<Roommate>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<MeterReading> MeterReadings => Set<MeterReading>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<VisitorLog> VisitorLogs => Set<VisitorLog>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
