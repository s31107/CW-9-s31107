namespace CW_9_s31107.DTOs;

public class MedicamentGetDto
{
    public int IdMedicament { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int? Dose { get; set; }
    public string Details { get; set; } = null!;
}