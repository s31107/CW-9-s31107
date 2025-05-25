using CW_9_s31107.Models;
using Microsoft.EntityFrameworkCore;

namespace CW_9_s31107.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var doctors = new List<Doctor>
        {
            new()
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jan.kowalski123@gmail.com",
                IdDoctor = 1
            },
            new()
            {
                FirstName = "Mirosław",
                LastName = "Nowak",
                Email = "Mirosław.Nowak123@gmail.com",
                IdDoctor = 2
            }
        };
        var patients = new List<Patient>
        {
            new()
            {
                BirthDate = DateOnly.Parse("05-11-1987"),
                FirstName = "Zbigniew",
                LastName = "Wiśniewski",
                IdPatient = 1
            }
        };
        var medicaments = new List<Medicament>
        {
            new()
            {
                Description = "aaaaa",
                Name = "bbb",
                Type = "ccc",
                IdMedicament = 1
            },
            new()
            {
                Description = "dddd",
                Name = "kkk",
                Type = "ppp",
                IdMedicament = 2
            }
        };
        modelBuilder.Entity<Medicament>().HasData(medicaments);
        modelBuilder.Entity<Patient>().HasData(patients);
        modelBuilder.Entity<Doctor>().HasData(doctors);
    }
}