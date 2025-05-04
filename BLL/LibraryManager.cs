using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.BLL
{
    public class BorrowRecord
    {
        public int BookId { get; set; }
        public int StudentId { get; set; }
    }
    public class LibraryManager
    {
        private List<BorrowRecord> borrowedBooks = new List<BorrowRecord>();

        public bool BorrowBook(int bookId, int studentId)
        {
            // Check if already borrowed
            if (borrowedBooks.Any(r => r.BookId == bookId && r.StudentId == studentId))
            {
                return false;
            }

            borrowedBooks.Add(new BorrowRecord
            {
                BookId = bookId,
                StudentId = studentId
            });

            return true;
        }

        public bool ReturnBook(int bookId, int studentId)
        {
            var record = borrowedBooks.FirstOrDefault(r => r.BookId == bookId && r.StudentId == studentId);
            if (record != null)
            {
                borrowedBooks.Remove(record);
                return true;
            }

            return false;
        }

        public List<BorrowRecord> GetBorrowedBooks()
        {
            return borrowedBooks.ToList();
        }
    }
}
