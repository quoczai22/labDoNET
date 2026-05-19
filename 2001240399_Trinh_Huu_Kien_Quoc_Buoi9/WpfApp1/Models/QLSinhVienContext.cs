#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Bai3.Models;

public partial class QLSinhVienContext : DbContext
{
    // thêm constructor rỗng
    public QLSinhVienContext()
    {
    }

    public QLSinhVienContext(DbContextOptions<QLSinhVienContext> options)
        : base(options)
    {
    }

    // thêm OnConfiguring
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=QLSinhVien;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }

    public virtual DbSet<Khoa> Khoas { get; set; }

    public virtual DbSet<Lop> Lops { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Khoa>(entity =>
        {
            entity.HasKey(e => e.MaKhoa);

            entity.ToTable("Khoa");

            entity.Property(e => e.MaKhoa)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.TenKhoa)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Lop>(entity =>
        {
            entity.HasKey(e => e.MaLop);

            entity.ToTable("Lop");

            entity.Property(e => e.MaLop)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.MaKhoa)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.MaKhoaNavigation)
                .WithMany(p => p.Lops)
                .HasForeignKey(d => d.MaKhoa);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}