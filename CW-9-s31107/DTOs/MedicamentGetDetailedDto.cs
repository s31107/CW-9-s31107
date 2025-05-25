namespace CW_9_s31107.DTOs;

public class MedicamentGetDetailedDto
{
    public int IdMedicament { get; set; }
    public string Name { get; set; } = null!;
    public int? Dose { get; set; }
    public string Description { get; set; } = null!;
}