namespace CW_9_s31107.DTOs;

public class PrescriptionPostDto
{
    public required PatientPostDto Patient { get; set; }
    public required ICollection<PrescriptionMedicamentPostDto> Medicaments { get; set; }
    public required DateOnly Date { get; set; }
    public required DateOnly DueDate { get; set; }
}