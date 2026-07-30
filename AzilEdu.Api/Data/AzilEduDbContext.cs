using AzilEdu.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AzilEdu.Api.Data;

public class AzilEduDbContext : DbContext
{
    public AzilEduDbContext(DbContextOptions<AzilEduDbContext> options)
        : base(options)
    {
    }
    public DbSet<Animal> Animals => Set<Animal>();
    
    public DbSet<HousingUnit> HousingUnits => Set<HousingUnit>();
    public DbSet<AnimalStatus> AnimalStatuses => Set<AnimalStatus>();
    public DbSet<Volunteer> Volunteers => Set<Volunteer>();
    public DbSet<VolunteerStatus> VolunteerStatuses => Set<VolunteerStatus>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Animal>()
            .HasOne(animal => animal.AnimalStatus)
            .WithMany(status => status.Animals)
            .HasForeignKey(animal => animal.AnimalStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AnimalStatus>().HasData(
            new AnimalStatus { Id = 1, Name = "Dostupna za udomljenje" },
            new AnimalStatus { Id = 2, Name = "Rezervirana" },
            new AnimalStatus { Id = 3, Name = "Udomljena" },
            new AnimalStatus { Id = 4, Name = "Na liječenju" }
        );

        modelBuilder.Entity<Volunteer>()
            .HasOne(volunteer => volunteer.VolunteerStatus)
            .WithMany(status => status.Volunteers)
            .HasForeignKey(volunteer => volunteer.VolunteerStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<VolunteerStatus>().HasData(
            new VolunteerStatus { Id = 1, Name = "Novi" },
            new VolunteerStatus { Id = 2, Name = "Aktivan" },
            new VolunteerStatus { Id = 3, Name = "Privremeno nedostupan" },
            new VolunteerStatus { Id = 4, Name = "Neaktivan" }
        );
    }
}
