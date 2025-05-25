namespace CW_9_s31107.DTOs;

public class PrescriptionGetDetailedDto
{
    public int IdPrescription { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
    public ICollection<MedicamentGetDetailedDto> Medicaments { get; set; } = null!;
    public DoctorGetDetailedDto Doctor { get; set; } = null!;
}