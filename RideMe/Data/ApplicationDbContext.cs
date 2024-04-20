using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RideMe.Models;

namespace RideMe.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<Passenger> Passengers { get; set; }

    public virtual DbSet<Ride> Rides { get; set; }

    public virtual DbSet<RideStatus> RideStatuses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserStatus> UserStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=Aya;Database=RideMe;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__admin__3213E83FB474A7C9");

            entity.ToTable("admin");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .HasColumnName("password");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__city__3213E83F7F5061D5");

            entity.ToTable("city");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__driver__3213E83F023A7C87");

            entity.ToTable("driver");

            entity.HasIndex(e => e.UserId, "UQ__driver__B9BE370EA02AD236").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Available).HasColumnName("available");
            entity.Property(e => e.AvgRating).HasColumnName("avg_rating");
            entity.Property(e => e.CarType)
                .HasMaxLength(50)
                .HasColumnName("car_type");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.Region)
                .HasMaxLength(100)
                .HasColumnName("region");
            entity.Property(e => e.Smoking).HasColumnName("smoking");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.City).WithMany(p => p.Drivers)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__driver__city_id__46E78A0C");

            entity.HasOne(d => d.User).WithOne(p => p.Driver)
                .HasForeignKey<Driver>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__driver__user_id__45F365D3");
        });

        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__passenge__3213E83F0359D320");

            entity.ToTable("passenger");

            entity.HasIndex(e => e.UserId, "UQ__passenge__B9BE370E17792936").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Passenger)
                .HasForeignKey<Passenger>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__passenger__user___4AB81AF0");
        });

        modelBuilder.Entity<Ride>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ride__3213E83F19DFBE03");

            entity.ToTable("ride");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DriverId).HasColumnName("driver_id");
            entity.Property(e => e.Feedback)
                .HasMaxLength(200)
                .HasColumnName("feedback");
            entity.Property(e => e.PassengerId).HasColumnName("passenger_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.RideDate)
                .HasColumnType("datetime")
                .HasColumnName("ride_date");
            entity.Property(e => e.RideDestination)
                .HasMaxLength(50)
                .HasColumnName("ride_destination");
            entity.Property(e => e.RideSource)
                .HasMaxLength(50)
                .HasColumnName("ride_source");
            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.HasOne(d => d.Driver).WithMany(p => p.Rides)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__ride__driver_id__4E88ABD4");

            entity.HasOne(d => d.Passenger).WithMany(p => p.Rides)
                .HasForeignKey(d => d.PassengerId)
                .HasConstraintName("FK__ride__passenger___4D94879B");

            entity.HasOne(d => d.Status).WithMany(p => p.Rides)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__ride__status_id__4F7CD00D");
        });

        modelBuilder.Entity<RideStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ride_sta__3213E83F64B4F2F6");

            entity.ToTable("ride_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__role__3213E83F74C1F6E7");

            entity.ToTable("role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user__3213E83F77213F8A");

            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phone_number");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__user__role_id__412EB0B6");

            entity.HasOne(d => d.Status).WithMany(p => p.Users)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__user__status_id__4222D4EF");
        });

        modelBuilder.Entity<UserStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_sta__3213E83F275D53B7");

            entity.ToTable("user_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
