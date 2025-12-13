using System.Runtime.CompilerServices;
using Common.Entities.BEntities;
using Common.Entities.MTMTables;
using Microsoft.EntityFrameworkCore;

namespace Common.Persistance;

public class AppDbContext : DbContext
{
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<Equipment> Equipments { get; set; }
    public DbSet<Membership> Memberships { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Sport> Sports { get; set; }
    public DbSet<TrainingSession> TrainingSessions { get; set; }
    public DbSet<User> Users { get; set; }

    public DbSet<UserRoles> UserRoles { get; set; }
    public DbSet<SessionAttendees> SessionAttendees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-RD8LV0K;User ID=svetomir;Password=svetomir;TrustServerCertificate=True;");
       
        base.OnConfiguring(optionsBuilder);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region  Models manipulation

        #region User

        modelBuilder.Entity<User>()
            .HasKey(user => user.Id);

        modelBuilder.Entity<User>()
            .HasOne<Membership>()
            .WithMany()
            .HasForeignKey(user => user.MembershipId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<UserRoles>(
                u => u
                        .HasOne(ur => ur.Role)
                        .WithMany()
                        .HasForeignKey(ur => ur.RoleId),

                u => u
                        .HasOne(ur => ur.User)
                        .WithMany()
                        .HasForeignKey(ur => ur.UserId),

                u => u.HasKey(ur => new { ur.RoleId, ur.UserId })
            );

        modelBuilder.Entity<User>()
            .HasMany(u => u.TrainingSessions)
            .WithMany(ts => ts.Users)
            .UsingEntity<SessionAttendees>(
                u => u
                        .HasOne(sa => sa.TrainingSession)
                        .WithMany()
                        .HasForeignKey(sa => sa.TrainingSessionId),

                u => u
                        .HasOne(sa => sa.User)
                        .WithMany()
                        .HasForeignKey(sa => sa.UserId),

                u => u.HasKey(sa => new { sa.TrainingSessionId, sa.UserId })
            );

        #endregion

        #region Booking

        modelBuilder.Entity<Booking>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<Booking>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne<TrainingSession>()
            .WithMany()
            .HasForeignKey(b => b.TrainingSessionId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

        #region TrainingSession

        modelBuilder.Entity<TrainingSession>()
            .HasKey(ts => ts.Id);
       
        modelBuilder.Entity<TrainingSession>()
            .HasOne<Sport>()
            .WithMany()
            .HasForeignKey(ts => ts.SportId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<TrainingSession>()
            .HasOne<Coach>()
            .WithMany()
            .HasForeignKey(ts => ts.CoachId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TrainingSession>()
            .HasOne<Hall>()
            .WithMany()
            .HasForeignKey(ts => ts.HallId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TrainingSession>()
            .HasMany(ts => ts.Equipments)
            .WithMany(e => e.TrainingSessions)
            .UsingEntity<EquipmentRentals>(
                er => er
                        .HasOne(e => e.Equipment)
                        .WithMany()
                        .HasForeignKey(e => e.EquipmentId),

                er => er
                        .HasOne(ts => ts.TrainingSession)
                        .WithMany()
                        .HasForeignKey(ts => ts.TrainingSessionId),

                er => er.HasKey(k => new { k.EquipmentId, k.TrainingSessionId })
            );
        #endregion

        #region Sport

        modelBuilder.Entity<Sport>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<Sport>()
            .HasMany(s => s.Coaches)
            .WithMany(c => c.Sports)
            .UsingEntity<CoachSports>(
                s => s
                        .HasOne(cs => cs.Coach)
                        .WithMany()
                        .HasForeignKey(cs => cs.CoachId),

                s => s
                        .HasOne(cs => cs.Sport)
                        .WithMany()
                        .HasForeignKey(cs => cs.SportId),

                s => s.HasKey(cs => new { cs.CoachId, cs.SportId })
            );

        #endregion

        #region Other Entities
        
        modelBuilder.Entity<Membership>()
            .HasKey(m => m.Id);

        modelBuilder.Entity<Equipment>()
            .HasKey(m => m.Id);
            
        modelBuilder.Entity<Coach>()
            .HasKey(m => m.Id);

        modelBuilder.Entity<Hall>()
            .HasKey(m => m.Id);

        #endregion

        #endregion

        #region Data Seeding

        modelBuilder.Entity<Role>()
            .HasData(
                new Role { Id = 1, Name = "Administrator" },
                new Role { Id = 2, Name = "User" }
            );

        modelBuilder.Entity<Membership>()
            .HasData(new Membership
            {
                Id = 1,
                Name = "Worker",
                DurationDays = int.MaxValue,
                Price = 0m
            });

        modelBuilder.Entity<User>()
            .HasData(new User
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "Adminov",
                Username = "admin",
                Phone = "0888888888",
                Email = "email@gmail.com",
                Password = "adminpass",
                MembershipId = 1
            });

        modelBuilder.Entity<UserRoles>()
            .HasData(new UserRoles
            {
                RoleId = 1,
                UserId = 1
            });
        #endregion
        
        base.OnModelCreating(modelBuilder);
    }
}
