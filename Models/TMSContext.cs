using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TmsApp.Models
{
    public partial class TMSContext : DbContext
    {
        public TMSContext()
        {
        }

        public TMSContext(DbContextOptions<TMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Allocation> Allocations { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<RoutePath> RoutePaths { get; set; } = null!;
        public virtual DbSet<UserDatum> UserData { get; set; } = null!;
        public virtual DbSet<Vehicle> Vehicles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=TMS;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Allocation>(entity =>
            {
                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Allocations)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_EmployeeId");

                entity.HasOne(d => d.RouteNumberNavigation)
                    .WithMany(p => p.Allocations)
                    .HasForeignKey(d => d.RouteNumber)
                    .HasConstraintName("FK_RouteNumber");

                entity.HasOne(d => d.VehicleNumberNavigation)
                    .WithMany(p => p.Allocations)
                    .HasForeignKey(d => d.VehicleNumber)
                    .HasConstraintName("FK_VehicleNumber");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.EmployeeId)
                    .ValueGeneratedNever()
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.EmpLocation)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RoutePath>(entity =>
            {
                entity.HasKey(e => e.RouteNumber)
                    .HasName("PK_RouteNumber");

                entity.ToTable("RoutePath");

                entity.Property(e => e.RouteNumber).ValueGeneratedNever();

                entity.Property(e => e.Pickup)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.VehicleStop)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.HasOne(d => d.VehicleNumberNavigation)
                    .WithMany(p => p.RoutePaths)
                    .HasForeignKey(d => d.VehicleNumber)
                    .HasConstraintName("FK__RoutePath__Vehic__24285DB4");
            });

            modelBuilder.Entity<UserDatum>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK_Username");

                entity.Property(e => e.Username)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Passcode)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(e => e.VehicleNumber)
                    .HasName("PK_VehicleNumber");

                entity.ToTable("Vehicle");

                entity.Property(e => e.VehicleNumber).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
