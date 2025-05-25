using System.ComponentModel.DataAnnotations;

namespace CW_9_s31107.DTOs;

public class PatientPostDto
{
    public required int IdPatient { get; set; }
    [MaxLength(100)]
    public required string FirstName { get; set; }
    [MaxLength(100)]
    public required string LastName { get; set; }
    public required DateOnly Birthdate { get; set; }
    public required int IdDoctor { get; set; }
}