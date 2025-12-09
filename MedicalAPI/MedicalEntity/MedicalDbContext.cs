using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MedicalAPI.MedicalEntity;

public partial class MedicalDbContext : DbContext
{
    public MedicalDbContext()
    {
    }

    public MedicalDbContext(DbContextOptions<MedicalDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PatientEntry> PatientEntries { get; set; }

    public virtual DbSet<TblMedicineDetail> TblMedicineDetails { get; set; }

    public virtual DbSet<TblSymptomDetail> TblSymptomDetails { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserRoleRel> UserRoleRels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-B30V5FQ;Initial Catalog=MedicalDB;User ID=sa;Encrypt=false;TrustServerCertificate=true;Password=sql@123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PatientEntry>(entity =>
        {
            entity.HasKey(e => e.PatientId);

            entity.ToTable("PatientEntry");

            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BloodGroup)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.PatientGeneder)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PatientName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Weight)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<TblMedicineDetail>(entity =>
        {
            entity.HasKey(e => e.MedId);

            entity.ToTable("Tbl_MedicineDetails");

            entity.Property(e => e.MedId).HasColumnName("MedID");
            entity.Property(e => e.MedDetails).HasMaxLength(500);
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.SymptomId).HasColumnName("SymptomID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<TblSymptomDetail>(entity =>
        {
            entity.HasKey(e => e.SymptomId);

            entity.ToTable("Tbl_SymptomDetails");

            entity.Property(e => e.SymptomId).HasColumnName("SymptomID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.SymptomDetails).HasMaxLength(500);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("Tbl_User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.LoginName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserAadharCard)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserCreateDate).HasColumnType("datetime");
            entity.Property(e => e.UserEmailId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserEmailID");
            entity.Property(e => e.UserFirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserGender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.UserLastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserMiddleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserPasswordExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Urid);

            entity.Property(e => e.Urid).HasColumnName("URID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.RoleDescription)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RoleName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserRoleRel>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("UserRole_Rel");

            entity.Property(e => e.Urid).HasColumnName("URID");
            entity.Property(e => e.Urrid)
                .ValueGeneratedOnAdd()
                .HasColumnName("URRID");
            entity.Property(e => e.Userid).HasColumnName("USERID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
