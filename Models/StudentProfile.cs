using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DharoneAcademy.Models;

public class StudentProfile
{
    public int Id { get; set; }

    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public AppUser? User { get; set; }

    [Required, StringLength(40)]
    public string EnrollmentId { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string CourseStream { get; set; } = "BCA";

    [StringLength(100)]
    public string? CollegeName { get; set; }

    [StringLength(40)]
    public string BatchName { get; set; } = "DA-2026-A";

    public DateTime? JoinDate { get; set; } = DateTime.Today;

    public bool IsActive { get; set; } = true;

    public ICollection<DailyTask> DailyTasks { get; set; } = new List<DailyTask>();
}
