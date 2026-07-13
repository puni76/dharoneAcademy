using System.ComponentModel.DataAnnotations;

namespace DharoneAcademy.Models;

public class ContactEnquiry
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    [StringLength(80)]
    public string? CourseInterest { get; set; }

    [Required, StringLength(1000)]
    public string Message { get; set; } = string.Empty;

    public bool IsResolved { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
