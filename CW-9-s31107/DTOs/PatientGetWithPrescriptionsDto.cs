namespace CW_9_s31107.DTOs;

public class PatientGetWithPrescriptionsDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateOnly BirthDate { get; set; }
    public ICollection<PrescriptionGetDetailedDto> Prescriptions { get; set; } = null!;
}