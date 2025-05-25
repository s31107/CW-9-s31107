using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CW_9_s31107.Models;

[Table("Doctor")]
public class Doctor
{
    [Key]
    public int IdDoctor { get; set; }

    [MaxLength(100)] public string FirstName { get; set; } = null!;
    [MaxLength(100)] public string LastName { get; set; } = null!;
    [MaxLength(100)] 
    [EmailAddress]
    public string Email { get; set; } = null!;

    public virtual ICollection<Prescription> Prescriptions { get; set; } = null!;
}