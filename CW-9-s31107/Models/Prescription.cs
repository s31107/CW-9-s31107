using System.ComponentModel.DataAnnotations.Schema;

namespace CW_9_s31107.Models;

[Table("Prescription")]
public class Prescription
{
    public int IdPrescription { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
    
    public int IdPatient { get; set; }
    public int IdDoctor { get; set; }
    
    [ForeignKey(nameof(IdPatient))]
    public virtual required Patient Patient { get; set; }
    [ForeignKey(nameof(IdDoctor))]
    public virtual required Doctor Doctor { get; set; }
    
    public virtual required ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
}