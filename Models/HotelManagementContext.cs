using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Hotel_Management.Models;

public partial class HotelManagementContext : IdentityDbContext<ApplicationUser>
{
    public HotelManagementContext()
    {
    }

    public HotelManagementContext(DbContextOptions<HotelManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingsRoomDetail> BookingsRoomDetails { get; set; }

    public virtual DbSet<BookingsServiceDetail> BookingsServiceDetails { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<Hotelfacility> Hotelfacilities { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Roomimage> Roomimages { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        // Loai bỏ "AspNet" prefix trong ten bang do Identity tao ra
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName != null && tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("bookings");

            entity.HasIndex(e => e.StaffId, "Staff_Id");

            entity.HasIndex(e => e.UserId, "User_Id");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.StaffId)
                .HasDefaultValueSql("'0'")
                .HasColumnName("Staff_Id");
            entity.Property(e => e.TotalPrice).HasPrecision(20, 6);
            entity.Property(e => e.TotalPriceRooms).HasPrecision(20, 6);
            entity.Property(e => e.TotalPriceServices).HasPrecision(20, 6);
            entity.Property(e => e.UserId)
                .HasDefaultValueSql("'0'")
                .HasColumnName("User_Id");

            entity.HasOne(d => d.Staff).WithMany(p => p.BookingStaffs)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_bookings_staff");

            entity.HasOne(d => d.User).WithMany(p => p.BookingUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_bookings_users");
        });

        modelBuilder.Entity<BookingsRoomDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("bookings_room_detail");

            entity.HasIndex(e => e.BookingId, "Booking_Id");

            entity.HasIndex(e => e.RoomId, "Room_Id");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BookingId).HasColumnName("Booking_Id");
            entity.Property(e => e.Price).HasPrecision(20, 6);
            entity.Property(e => e.RoomId).HasColumnName("Room_Id");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingsRoomDetails)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_bookings_room_detail_bookings");

            entity.HasOne(d => d.Room).WithMany(p => p.BookingsRoomDetails)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_bookings_room_detail_rooms");
        });

        modelBuilder.Entity<BookingsServiceDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("bookings_service_detail");

            entity.HasIndex(e => e.BookingId, "Booking_Id");

            entity.HasIndex(e => e.ServiceId, "Service_Id");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BookingId).HasColumnName("Booking_Id");
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingsServiceDetails)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_bookings_service_detail_bookings");

            entity.HasOne(d => d.Service).WithMany(p => p.BookingsServiceDetails)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__services");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("hotels");

            entity.HasIndex(e => e.CreatedBy, "CreatedBy");

            entity.Property(e => e.Address).HasColumnType("text");
            entity.Property(e => e.CreatedBy).HasMaxLength(450);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Rating).HasDefaultValueSql("'0'");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Hotels)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("hotels_ibfk_1");
        });

        modelBuilder.Entity<Hotelfacility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("hotelfacilities");

            entity.HasIndex(e => e.HotelId, "HotelId");

            entity.Property(e => e.FacilityName).HasMaxLength(255);

            entity.HasOne(d => d.Hotel).WithMany(p => p.Hotelfacilities)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("hotelfacilities_ibfk_1");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("payments");

            entity.HasIndex(e => e.BookingId, "BookingId");

            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("payments_ibfk_1");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("reviews");

            entity.HasIndex(e => e.HotelId, "HotelId");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Comment).HasColumnType("text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.Hotel).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("reviews_ibfk_1");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("reviews_ibfk_2");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("rooms");

            entity.HasIndex(e => e.HotelId, "HotelId");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Image).HasColumnType("text");
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.RoomNumber).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(100);

            entity.HasOne(d => d.Hotel).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("rooms_ibfk_1");
        });

        modelBuilder.Entity<Roomimage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roomimages");

            entity.HasIndex(e => e.RoomId, "RoomId");

            entity.Property(e => e.ImageUrl).HasColumnType("text");

            entity.HasOne(d => d.Room).WithMany(p => p.Roomimages)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("roomimages_ibfk_1");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("services");

            entity.HasIndex(e => e.HotelId, "HotelId");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasPrecision(18, 2);

            entity.HasOne(d => d.Hotel).WithMany(p => p.Services)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("services_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
