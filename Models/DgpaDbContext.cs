using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DgpaMapWebApi.Models;

public partial class DgpaDbContext : DbContext
{
    public DgpaDbContext(DbContextOptions<DgpaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<HistoryJob> HistoryJob { get; set; }

    public virtual DbSet<Job> Job { get; set; }

    public virtual DbSet<UpdateDate> UpdateDate { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HistoryJob>(entity =>
        {
            entity.HasKey(e => e.JOB_ID);

            entity.Property(e => e.JOB_ID).ValueGeneratedNever();
            entity.Property(e => e.Coordinate_X).HasColumnType("decimal(8, 6)");
            entity.Property(e => e.Coordinate_Y).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.ORG_ID)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ORG_NAME).HasMaxLength(50);
            entity.Property(e => e.SYSNAM).HasMaxLength(20);
            entity.Property(e => e.TITLE).HasMaxLength(50);
            entity.Property(e => e.VIEW_URL).HasMaxLength(150);
            entity.Property(e => e.WORK_PLACE_TYPE).HasMaxLength(3);
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.JOB_ID).HasName("PK_JOB");

            entity.Property(e => e.JOB_ID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Coordinate_X).HasColumnType("decimal(8, 6)");
            entity.Property(e => e.Coordinate_Y).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.ORG_ID)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ORG_NAME).HasMaxLength(50);
            entity.Property(e => e.SYSNAM).HasMaxLength(20);
            entity.Property(e => e.TITLE).HasMaxLength(50);
            entity.Property(e => e.VIEW_URL)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.WORK_PLACE_TYPE).HasMaxLength(3);
        });

        modelBuilder.Entity<UpdateDate>(entity =>
        {
            entity.HasKey(e => e.Date_ID);

            entity.Property(e => e.Date_ID).HasDefaultValueSql("(newid())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
