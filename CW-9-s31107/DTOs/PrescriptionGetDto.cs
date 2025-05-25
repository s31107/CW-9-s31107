namespace CW_9_s31107.DTOs;

public class PrescriptionGetDto
{
    public int IdPrescription { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
    public DoctorGetDto Doctor { get; set; } = null!;
    public PatientGetDto Patient { get; set; } = null!;
    public ICollection<MedicamentGetDto> Medicaments { get; set; } = null!;
}