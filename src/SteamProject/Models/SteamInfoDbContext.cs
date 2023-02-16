using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SteamProject.Models;

public partial class SteamInfoDbContext : DbContext
{
    public SteamInfoDbContext()
    {
    }

    public SteamInfoDbContext(DbContextOptions<SteamInfoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameAchievement> GameAchievements { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAchievement> UserAchievements { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Friend__3214EC0742CB7377");

            entity.ToTable("Friend");

            entity.Property(e => e.AvatarUrl).HasMaxLength(100);
            entity.Property(e => e.GameExtraInfo).HasMaxLength(100);
            entity.Property(e => e.SteamName).HasMaxLength(50);

            entity.HasOne(d => d.Root).WithMany(p => p.Friends)
                .HasForeignKey(d => d.RootId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Friend_Fk_User");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Game__3214EC07539EC757");

            entity.ToTable("Game");

            entity.Property(e => e.DescLong).HasMaxLength(1024);
            entity.Property(e => e.DescShort).HasMaxLength(512);
            entity.Property(e => e.IconUrl).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Owner).WithMany(p => p.Games)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Game_Fk_User");
        });

        modelBuilder.Entity<GameAchievement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GameAchi__3214EC0713F11CFD");

            entity.ToTable("GameAchievement");

            entity.Property(e => e.ApiName).HasMaxLength(100);
            entity.Property(e => e.DisplayName).HasMaxLength(50);
            entity.Property(e => e.IconAchievedUrl).HasMaxLength(100);
            entity.Property(e => e.IconHiddenUrl).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC0723228B7D");

            entity.ToTable("User");

            entity.Property(e => e.AspNetUserId).HasMaxLength(450);
            entity.Property(e => e.AvatarUrl).HasMaxLength(100);
            entity.Property(e => e.ProfileUrl).HasMaxLength(100);
            entity.Property(e => e.SteamId).HasMaxLength(50);
            entity.Property(e => e.SteamName).HasMaxLength(50);
        });

        modelBuilder.Entity<UserAchievement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserAchi__3214EC077426530A");

            entity.ToTable("UserAchievement");

            entity.Property(e => e.ApiName).HasMaxLength(100);
            entity.Property(e => e.DisplayName).HasMaxLength(50);

            entity.HasOne(d => d.Owner).WithMany(p => p.UserAchievements)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserAchievement_Fk_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
