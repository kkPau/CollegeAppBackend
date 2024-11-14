using System.ComponentModel.DataAnnotations;
using CollegeApp.Validators;

namespace CollegeApp.Models;

public class StudentDTO
{
    [Required(ErrorMessage = "Student name is required")]
    [StringLength(50, ErrorMessage = "Student name must be at most 50 characters")]
    public string StudentName { get; set; }

    [Required(ErrorMessage = "Student email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Student address is required")]
    public string Address { get; set; }

    // [DateCheck]
    // [Required]
    // public DateTime Date { get; set; }

    // public string Password { get; set; }

    // [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    // public string ConfirmPassword { get; set; }
}
