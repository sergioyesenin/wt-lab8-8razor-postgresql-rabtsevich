using Microsoft.EntityFrameworkCore;
using wt_lab8_8razor_postgresql_rabtsevich.Models;

namespace wt_lab8_8razor_postgresql_rabtsevich.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<Airline> Airlines => Set<Airline>();
    public DbSet<Aircraft> Aircrafts => Set<Aircraft>();
    public DbSet<Flight> Flights => Set<Flight>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var airline = modelBuilder.Entity<Airline>();
        airline.ToTable("Airlines");
        airline.HasIndex(a => a.IataCode).IsUnique();

        var aircraft = modelBuilder.Entity<Aircraft>();
        aircraft.ToTable("Aircrafts");
        aircraft.HasOne(a => a.Airline)
            .WithMany()
            .HasForeignKey(a => a.AirlineId)
            .OnDelete(DeleteBehavior.Restrict);

        var flight = modelBuilder.Entity<Flight>();
        flight.ToTable("Flights");
        flight.HasIndex(f => f.FlightNumber).IsUnique();

        flight.Property(f => f.Description)
            .HasColumnName("Note")
            .HasColumnType("text");

        flight.Property(f => f.DepartureTime)
            .HasColumnType("timestamp with time zone")
            .HasConversion(
                value => DateTime.SpecifyKind(value, DateTimeKind.Utc),
                value => value);

        flight.HasOne(f => f.Airline)
            .WithMany()
            .HasForeignKey(f => f.AirlineId)
            .OnDelete(DeleteBehavior.Restrict);

        flight.HasOne(f => f.Aircraft)
            .WithMany()
            .HasForeignKey(f => f.AircraftId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}