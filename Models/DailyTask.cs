using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DharoneAcademy.Models;

public class DailyTask
{
    public int Id { get; set; }

    [Required, Display(Name = "Student")]
    public int StudentProfileId { get; set; }

    [ForeignKey(nameof(StudentProfileId))]
    [ValidateNever]
    public StudentProfile? StudentProfile { get; set; }

    [Required, DataType(DataType.Date), Display(Name = "Date")]
    public DateTime TaskDate { get; set; } = DateTime.Today;

    [Required, StringLength(20)]
    public string Attendance { get; set; } = "Present";

    [Required, StringLength(150)]
    public string TopicCovered { get; set; } = string.Empty;

    [Display(Name = "Task Title"), StringLength(200)]
    public string TaskTitle { get; set; } = string.Empty;

    [Required, StringLength(30)]
    public string Status { get; set; } = "Pending";

    [Range(0, 24)]
    public decimal HoursSpent { get; set; }

    [StringLength(1000)]
    public string? MentorFeedback { get; set; }

    [StringLength(1000)]
    public string? StudentNotes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
