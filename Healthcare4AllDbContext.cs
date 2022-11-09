using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HealthCare4All
{
    public partial class Healthcare4AllDbContext : DbContext
    {
        public Healthcare4AllDbContext()
        {
        }

        public Healthcare4AllDbContext(DbContextOptions<Healthcare4AllDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; } = null!;
        public virtual DbSet<Treatment> Treatments { get; set; } = null!;
        public virtual DbSet<TreatmentTime> TreatmentTimes { get; set; } = null!;
        public virtual DbSet<UserInfo> UserInfos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Healthcare4All;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");

                entity.Property(e => e.BulidingNumber)
                    .HasMaxLength(20)
                    .HasColumnName("bulidingNumber");

                entity.Property(e => e.City)
                    .HasMaxLength(20)
                    .HasColumnName("city");

                entity.Property(e => e.CreatorId).HasColumnName("creatorID");

                entity.Property(e => e.PatientId).HasColumnName("patientID");

                entity.Property(e => e.Postalcode)
                    .HasMaxLength(20)
                    .HasColumnName("postalcode");

                entity.Property(e => e.State)
                    .HasMaxLength(20)
                    .HasColumnName("state");

                entity.Property(e => e.Street)
                    .HasMaxLength(20)
                    .HasColumnName("street");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");
            });

            modelBuilder.Entity<Treatment>(entity =>
            {
                entity.Property(e => e.TreatmentId).HasColumnName("treatmentID");

                entity.Property(e => e.Comments)
                    .HasColumnType("text")
                    .HasColumnName("comments");

                entity.Property(e => e.CreatorId).HasColumnName("creatorID");

                entity.Property(e => e.Dose)
                    .HasMaxLength(10)
                    .HasColumnName("dose");

                entity.Property(e => e.IsPrescription).HasColumnName("isPrescription");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .HasColumnName("name");

                entity.Property(e => e.PatientId).HasColumnName("patientID");
            });

            modelBuilder.Entity<TreatmentTime>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");

                entity.Property(e => e.TreatmentId).HasColumnName("treatmentID");

                entity.HasOne(d => d.Treatment)
                    .WithMany()
                    .HasForeignKey(d => d.TreatmentId)
                    .HasConstraintName("FK__Treatment__treat__412EB0B6");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserInfo__CB9A1CDF4F14FC05");

                entity.HasIndex(e => e.UserName, "UQ__UserInfo__66DCF95C688C7829")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.Property(e => e.BirthDay)
                    .HasColumnType("date")
                    .HasColumnName("birthDay");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(20)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(20)
                    .HasColumnName("lastName");

                entity.Property(e => e.MaxPriviledge).HasColumnName("maxPriviledge");

                entity.Property(e => e.Password)
                    .HasMaxLength(30)
                    .HasColumnName("password");

                entity.Property(e => e.UserName)
                    .HasMaxLength(30)
                    .HasColumnName("userName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
