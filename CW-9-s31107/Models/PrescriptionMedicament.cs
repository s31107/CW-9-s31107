using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CW_9_s31107.Models;

[PrimaryKey(nameof(IdMedicament), nameof(IdPrescription))]
[Table("Prescription_Medicament")]
public class PrescriptionMedicament
{
    public int IdMedicament { get; set; }
    public int IdPrescription { get; set; }
    public int? Dose { get; set; }
    [MaxLength(100)]
    public required string Details { get; set; }
    
    [ForeignKey(nameof(IdMedicament))]
    public virtual required Medicament Medicament { get; set; }
    [ForeignKey(nameof(IdPrescription))]
    public virtual required Prescription Prescription { get; set; }
}