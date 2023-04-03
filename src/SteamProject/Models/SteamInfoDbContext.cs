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

    public virtual DbSet<AppNews> AppNews { get; set; }

    public virtual DbSet<Competition> Competitions { get; set; }

    public virtual DbSet<CompetitionGameAchievement> CompetitionGameAchievements { get; set; }

    public virtual DbSet<CompetitionPlayer> CompetitionPlayers { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameAchievement> GameAchievements { get; set; }

    public virtual DbSet<NewsItem> NewsItems { get; set; }

    public virtual DbSet<NewsItemTag> NewsItemTags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAchievement> UserAchievements { get; set; }

    public virtual DbSet<UserGameInfo> UserGameInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=SteamInfoConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppNews>(entity =>
        {
            entity.HasKey(e => e.AppId).HasName("PK__AppNews__8E2CF7F998144EC3");

            entity.Property(e => e.AppId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Competition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competit__3214EC074895BEF4");

            entity.ToTable("Competition");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Game).WithMany(p => p.Competitions)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Competition_Fk_Game");
        });

        modelBuilder.Entity<CompetitionGameAchievement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competit__3214EC07370C3AEE");

            entity.ToTable("CompetitionGameAchievement");

            entity.HasOne(d => d.Competition).WithMany(p => p.CompetitionGameAchievements)
                .HasForeignKey(d => d.CompetitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CompetitionGameAchievement_Fk_Competition");
        });

        modelBuilder.Entity<CompetitionPlayer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competit__3214EC078E9744E4");

            entity.ToTable("CompetitionPlayer");

            entity.Property(e => e.SteamId).HasMaxLength(50);

            entity.HasOne(d => d.Competition).WithMany(p => p.CompetitionPlayers)
                .HasForeignKey(d => d.CompetitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CompetitionPlayer_Fk_Competition");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Friend__3214EC07F974B622");

            entity.ToTable("Friend");

            entity.Property(e => e.AvatarFullUrl).HasMaxLength(100);
            entity.Property(e => e.AvatarUrl).HasMaxLength(100);
            entity.Property(e => e.GameExtraInfo).HasMaxLength(100);
            entity.Property(e => e.SteamId).HasMaxLength(50);
            entity.Property(e => e.SteamName).HasMaxLength(50);

            entity.HasOne(d => d.Root).WithMany(p => p.Friends)
                .HasForeignKey(d => d.RootId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Friend_Fk_User");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Game__3214EC0777D010C7");

            entity.ToTable("Game");

            entity.Property(e => e.DescLong).HasMaxLength(1024);
            entity.Property(e => e.DescShort).HasMaxLength(512);
            entity.Property(e => e.IconUrl).HasMaxLength(512);
            entity.Property(e => e.Name).HasMaxLength(512);
        });

        modelBuilder.Entity<GameAchievement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GameAchi__3214EC07786718FA");

            entity.ToTable("GameAchievement");

            entity.Property(e => e.ApiName).HasMaxLength(100);
            entity.Property(e => e.DisplayName).HasMaxLength(50);
            entity.Property(e => e.IconAchievedUrl).HasMaxLength(256);
            entity.Property(e => e.IconHiddenUrl).HasMaxLength(256);
        });

        modelBuilder.Entity<NewsItem>(entity =>
        {
            entity.HasKey(e => e.Gid).HasName("PK__NewsItem__C51E133600EF913F");

            entity.ToTable("NewsItem");

            entity.Property(e => e.Gid).HasMaxLength(255);
            entity.Property(e => e.Author).HasMaxLength(255);
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.FeedLabel).HasMaxLength(255);
            entity.Property(e => e.FeedName).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.App).WithMany(p => p.NewsItems)
                .HasForeignKey(d => d.AppId)
                .HasConstraintName("FK__NewsItem__AppId__3F3159AB");
        });

        modelBuilder.Entity<NewsItemTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NewsItem__3214EC077638C63E");

            entity.ToTable("NewsItemTag");

            entity.Property(e => e.Gid).HasMaxLength(255);
            entity.Property(e => e.Tag).HasMaxLength(255);

            entity.HasOne(d => d.GidNavigation).WithMany(p => p.NewsItemTags)
                .HasForeignKey(d => d.Gid)
                .HasConstraintName("FK__NewsItemTag__Gid__420DC656");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC076273E74B");

            entity.ToTable("User");

            entity.Property(e => e.AspNetUserId).HasMaxLength(450);
            entity.Property(e => e.AvatarUrl).HasMaxLength(100);
            entity.Property(e => e.ProfileUrl).HasMaxLength(100);
            entity.Property(e => e.SteamId).HasMaxLength(50);
            entity.Property(e => e.SteamName).HasMaxLength(50);
        });

        modelBuilder.Entity<UserAchievement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserAchi__3214EC07C0C7AA37");

            entity.ToTable("UserAchievement");

            entity.Property(e => e.UnlockTime).HasColumnType("datetime");

            entity.HasOne(d => d.Achievement).WithMany(p => p.UserAchievements)
                .HasForeignKey(d => d.AchievementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserAchievement_FK_Achievement");

            entity.HasOne(d => d.Owner).WithMany(p => p.UserAchievements)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserAchievement_Fk_User");
        });

        modelBuilder.Entity<UserGameInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserGame__3214EC0799D0FDD9");

            entity.ToTable("UserGameInfo");

            entity.HasOne(d => d.Game).WithMany(p => p.UserGameInfos)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserGameInfo_FK_Game");

            entity.HasOne(d => d.Owner).WithMany(p => p.UserGameInfos)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("UserGameInfo_FK_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
