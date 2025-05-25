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
        
    }
}