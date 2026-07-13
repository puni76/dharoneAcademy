using System.ComponentModel.DataAnnotations;

namespace DharoneAcademy.Models.ViewModels;

public class LoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}

public class RegisterViewModel
{
    [Required, Display(Name = "Full Name"), StringLength(120)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, Phone, StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    [Required, Display(Name = "Course Stream")]
    public string CourseStream { get; set; } = "BCA";

    [Display(Name = "College")]
    public string? CollegeName { get; set; }

    [Required, DataType(DataType.Password), MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), Display(Name = "Confirm Password")]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class DashboardViewModel
{
    public string UserName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int TotalStudents { get; set; }
    public int PresentToday { get; set; }
    public int TasksPending { get; set; }
    public int TasksCompleted { get; set; }
    public int OpenEnquiries { get; set; }
    public List<DailyTask> RecentTasks { get; set; } = new();
}
