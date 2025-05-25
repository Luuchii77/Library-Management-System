using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Reference ID is required")]
        [RegularExpression(@"^\d{9,11}$", ErrorMessage = "Reference ID must be 9-11 digits")]
        public string ReferenceID { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string? Email { get; set; }

        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be 11 digits")]
        public string? PhoneNumber { get; set; }

        // Navigation property for transactions
        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}
