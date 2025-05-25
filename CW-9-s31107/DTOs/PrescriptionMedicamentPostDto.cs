using System.ComponentModel.DataAnnotations;

namespace CW_9_s31107.DTOs;

public class PrescriptionMedicamentPostDto
{
    public required int IdMedicament { get; set; }
    public int? Dose { get; set; }
    [MaxLength(100)]
    public required string Description { get; set; }
}