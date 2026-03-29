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

    public virtual DbSet<TblAiSymptomCache> TblAiSymptomCaches { get; set; }

    public virtual DbSet<TblMedicineDetail> TblMedicineDetails { get; set; }

    public virtual DbSet<TblSymptomDetail> TblSymptomDetails { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserRoleRel> UserRoleRels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-B30V5FQ;Initial Catalog=MedicalDB;User ID=sa;Encrypt=true;Password=sql@123;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PatientEntry>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__PatientE__970EC346805B8BD6");

            entity.ToTable("PatientEntry");

            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.BloodGroup).HasMaxLength(5);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PatientGeneder).HasMaxLength(10);
            entity.Property(e => e.PatientName).HasMaxLength(150);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Weight).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.User).WithMany(p => p.PatientEntries)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patient_User");
        });

        modelBuilder.Entity<TblAiSymptomCache>(entity =>
        {
            entity.HasKey(e => e.CacheId).HasName("PK__Tbl_AI_S__4EDCCD332EEB37F3");

            entity.ToTable("Tbl_AI_SymptomCache");

            entity.HasIndex(e => e.SymptomKey, "IX_SymptomKey");

            entity.Property(e => e.Aiadvice).HasColumnName("AIAdvice");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.HitCount).HasDefaultValue(1);
            entity.Property(e => e.Severity).HasMaxLength(50);
            entity.Property(e => e.SuggestedAction).HasMaxLength(500);
            entity.Property(e => e.SymptomKey).HasMaxLength(500);
        });

        modelBuilder.Entity<TblMedicineDetail>(entity =>
        {
            entity.HasKey(e => e.MedId).HasName("PK__Tbl_Medi__EB77FC36E3377663");

            entity.ToTable("Tbl_MedicineDetails");

            entity.Property(e => e.MedId).HasColumnName("MedID");
            entity.Property(e => e.MedDetails).HasMaxLength(500);
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.SymptomId).HasColumnName("SymptomID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Patient).WithMany(p => p.TblMedicineDetails)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medicine_Patient");

            entity.HasOne(d => d.Symptom).WithMany(p => p.TblMedicineDetails)
                .HasForeignKey(d => d.SymptomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medicine_Symptom");

            entity.HasOne(d => d.User).WithMany(p => p.TblMedicineDetails)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medicine_User");
        });

        modelBuilder.Entity<TblSymptomDetail>(entity =>
        {
            entity.HasKey(e => e.SymptomId).HasName("PK__Tbl_Symp__D26ED8B6C69532F9");

            entity.ToTable("Tbl_SymptomDetails");

            entity.Property(e => e.SymptomId).HasColumnName("SymptomID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.SymptomDetails).HasMaxLength(500);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Patient).WithMany(p => p.TblSymptomDetails)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Symptom_Patient");

            entity.HasOne(d => d.User).WithMany(p => p.TblSymptomDetails)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Symptom_User");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Tbl_User__1788CCAC0895F88C");

            entity.ToTable("Tbl_User");

            entity.HasIndex(e => e.UserEmailId, "UQ__Tbl_User__09C7B4CC1D08B4DF").IsUnique();

            entity.HasIndex(e => e.LoginName, "UQ__Tbl_User__DB8464FFF93AD48D").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.LoginName).HasMaxLength(100);
            entity.Property(e => e.UserAadharCard).HasMaxLength(20);
            entity.Property(e => e.UserCreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserEmailId)
                .HasMaxLength(150)
                .HasColumnName("UserEmailID");
            entity.Property(e => e.UserFirstName).HasMaxLength(100);
            entity.Property(e => e.UserGender).HasMaxLength(10);
            entity.Property(e => e.UserIsActive).HasDefaultValue(true);
            entity.Property(e => e.UserLastName).HasMaxLength(100);
            entity.Property(e => e.UserMiddleName).HasMaxLength(100);
            entity.Property(e => e.UserMobileNo).HasMaxLength(15);
            entity.Property(e => e.UserModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserPassword).HasMaxLength(255);
            entity.Property(e => e.UserPasswordExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");

            entity.HasOne(d => d.UserRole).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.UserRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Urid).HasName("PK__UserRole__AA3DE2DC1F7EDBE0");

            entity.Property(e => e.Urid).HasColumnName("URID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RoleDescription).HasMaxLength(255);
            entity.Property(e => e.RoleName).HasMaxLength(100);
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<UserRoleRel>(entity =>
        {
            entity.HasKey(e => e.Urrid).HasName("PK__UserRole__F6C2E30508EF86DE");

            entity.ToTable("UserRole_Rel");

            entity.Property(e => e.Urrid).HasColumnName("URRID");
            entity.Property(e => e.Urid).HasColumnName("URID");
            entity.Property(e => e.Userid).HasColumnName("USERID");

            entity.HasOne(d => d.Ur).WithMany(p => p.UserRoleRels)
                .HasForeignKey(d => d.Urid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rel_Role");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoleRels)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rel_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
