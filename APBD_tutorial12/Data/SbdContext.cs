using System;
using System.Collections.Generic;
using APBD_tutorial12.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_tutorial12.Data;

public partial class SbdContext : DbContext
{
    public SbdContext()
    {
    }

    public SbdContext(DbContextOptions<SbdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attraction> Attractions { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingAttraction> BookingAttractions { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarRental> CarRentals { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Client1> Clients1 { get; set; }

    public virtual DbSet<ClientTrip> ClientTrips { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;TrustServerCertificate=True",
                options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            );
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("s31848");

        modelBuilder.Entity<Attraction>(entity =>
        {
            entity.HasKey(e => e.AttractionId).HasName("Attraction_pk");

            entity.ToTable("Attraction");

            entity.Property(e => e.AttractionId)
                .ValueGeneratedNever()
                .HasColumnName("attraction_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("Booking_pk");

            entity.ToTable("Booking");

            entity.Property(e => e.BookingId)
                .ValueGeneratedNever()
                .HasColumnName("booking_id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.GuestId).HasColumnName("guest_id");

            entity.HasOne(d => d.Employee).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Booking_Employee");

            entity.HasOne(d => d.Guest).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Booking_Guest");
        });

        modelBuilder.Entity<BookingAttraction>(entity =>
        {
            entity.HasKey(e => new { e.BookingId, e.AttractionId }).HasName("Booking_Attraction_pk");

            entity.ToTable("Booking_Attraction");

            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.AttractionId).HasColumnName("attraction_id");
            entity.Property(e => e.Amount).HasColumnName("amount");

            entity.HasOne(d => d.Attraction).WithMany(p => p.BookingAttractions)
                .HasForeignKey(d => d.AttractionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Booking_Attraction_Attraction");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingAttractions)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Booking_Attraction_Booking");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cars_pk");

            entity.ToTable("cars");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ColorId).HasColumnName("ColorID");
            entity.Property(e => e.ModelId).HasColumnName("ModelID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Vin)
                .HasMaxLength(17)
                .HasColumnName("VIN");

            entity.HasOne(d => d.Color).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ColorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cars_colors");

            entity.HasOne(d => d.Model).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cars_models");
        });

        modelBuilder.Entity<CarRental>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("car_rentals_pk");

            entity.ToTable("car_rentals");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarId).HasColumnName("CarID");
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.DateFrom).HasColumnType("datetime");
            entity.Property(e => e.DateTo).HasColumnType("datetime");

            entity.HasOne(d => d.Car).WithMany(p => p.CarRentals)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("car_rentals_cars");

            entity.HasOne(d => d.Client).WithMany(p => p.CarRentals)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("car_rentals_clients");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("Client_pk");

            entity.ToTable("Client");

            entity.Property(e => e.Email).HasMaxLength(120);
            entity.Property(e => e.FirstName).HasMaxLength(120);
            entity.Property(e => e.LastName).HasMaxLength(120);
            entity.Property(e => e.Pesel).HasMaxLength(120);
            entity.Property(e => e.Telephone).HasMaxLength(120);
        });

        modelBuilder.Entity<Client1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clients_pk");

            entity.ToTable("clients");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(100);
        });

        modelBuilder.Entity<ClientTrip>(entity =>
        {
            entity.HasKey(e => new { e.IdClient, e.IdTrip }).HasName("Client_Trip_pk");

            entity.ToTable("Client_Trip");

            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.RegisteredAt).HasColumnType("datetime");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.ClientTrips)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Table_5_Client");

            entity.HasOne(d => d.IdTripNavigation).WithMany(p => p.ClientTrips)
                .HasForeignKey(d => d.IdTrip)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Table_5_Trip");
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("colors_pk");

            entity.ToTable("colors");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.IdCountry).HasName("Country_pk");

            entity.ToTable("Country");

            entity.Property(e => e.Name).HasMaxLength(120);

            entity.HasMany(d => d.IdTrips).WithMany(p => p.IdCountries)
                .UsingEntity<Dictionary<string, object>>(
                    "CountryTrip",
                    r => r.HasOne<Trip>().WithMany()
                        .HasForeignKey("IdTrip")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Country_Trip_Trip"),
                    l => l.HasOne<Country>().WithMany()
                        .HasForeignKey("IdCountry")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Country_Trip_Country"),
                    j =>
                    {
                        j.HasKey("IdCountry", "IdTrip").HasName("Country_Trip_pk");
                        j.ToTable("Country_Trip");
                    });
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("Employee_pk");

            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("employee_id");
            entity.Property(e => e.EmployeeNumber)
                .HasMaxLength(22)
                .HasColumnName("employee_number");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
        });

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.GuestId).HasName("Guest_pk");

            entity.ToTable("Guest");

            entity.Property(e => e.GuestId)
                .ValueGeneratedNever()
                .HasColumnName("guest_id");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("datetime")
                .HasColumnName("date_of_birth");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("models_pk");

            entity.ToTable("models");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.IdTrip).HasName("Trip_pk");

            entity.ToTable("Trip");

            entity.Property(e => e.DateFrom).HasColumnType("datetime");
            entity.Property(e => e.DateTo).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(220);
            entity.Property(e => e.Name).HasMaxLength(120);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
