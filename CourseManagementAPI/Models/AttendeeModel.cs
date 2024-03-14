using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.Models
{
    public class AttendeeModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; } = null!;
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; } = null!;
        [Required]
        [StringLength(150, MinimumLength = 2)]
        [EmailAddress]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Please enter a valid email.")]
        public string Email { get; set; } = null!;
        [Required]
        public int CourseId { get; set; }
    }
}
