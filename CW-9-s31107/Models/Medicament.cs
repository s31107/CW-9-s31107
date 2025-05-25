using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CW_9_s31107.Models;

[Table("Medicament")]
public class Medicament
{
    [Key]
    public int IdMedicament { get; set; }
    [MaxLength(100)]
    public required string Name { get; set; }
    [MaxLength(100)]
    public required string Description { get; set; }
    [MaxLength(100)]
    public required string Type { get; set; }

    public virtual required ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
}