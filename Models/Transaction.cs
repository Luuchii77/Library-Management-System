using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal? FineAmount { get; set; }

        // Navigation properties
        [ForeignKey("BookId")]
        public virtual Book? Book { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student? Student { get; set; }
    }
}
