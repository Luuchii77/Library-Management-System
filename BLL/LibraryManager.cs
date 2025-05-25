using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.DAL;
using System.Data.SqlClient;

namespace LibraryManagementSystem.BLL
{
    public class LibraryManager
    {
        private readonly LibraryDbContext _context;

        public LibraryManager(LibraryDbContext context)
        {
            _context = context;
        }

        public bool SaveStudent(Student student)
        {
            try
            {
                if (student.StudentId == 0)
                {
                    // Check if ReferenceID already exists
                    if (_context.Students.Any(s => s.ReferenceID == student.ReferenceID))
                    {
                        throw new Exception("A student with this Reference ID already exists.");
                    }
                    _context.Students.Add(student);
                }
                else
                {
                    var existingStudent = _context.Students.Find(student.StudentId);
                    if (existingStudent == null)
                    {
                        throw new Exception("Student not found.");
                    }

                    // Check if ReferenceID is being changed and if it already exists
                    if (existingStudent.ReferenceID != student.ReferenceID &&
                        _context.Students.Any(s => s.ReferenceID == student.ReferenceID))
                    {
                        throw new Exception("A student with this Reference ID already exists.");
                    }

                    _context.Entry(existingStudent).CurrentValues.SetValues(student);
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public (bool Success, string ErrorMessage) BorrowBook(int bookId, int studentId, DateTime dueDate)
        {
            var transactionDal = new DAL.TransactionDAL();
            string errorMessage;
            bool success = transactionDal.BorrowBook(bookId, studentId, dueDate, out errorMessage);

            // If successful, explicitly reload the book in the EF context
            if (success)
            {
                var book = _context.Books.Find(bookId);
                if (book != null)
                {
                    _context.Entry(book).Reload();
                }
            }

            return (success, errorMessage);
        }

        public bool ReturnBook(int bookId, int studentId)
        {
            try
            {
                var transaction = _context.Transactions
                    .FirstOrDefault(t => t.BookId == bookId && 
                                       t.StudentId == studentId && 
                                       t.ReturnDate == null);

                if (transaction == null)
                {
                    return false;
                }

                var book = _context.Books.Find(bookId);
                if (book == null)
                {
                    return false;
                }

                transaction.ReturnDate = DateTime.Now;

                // Calculate fine if applicable
                if (transaction.DueDate.HasValue)
                {
                    var fineCalculator = new LibraryManagementSystem.Utils.FineCalculator();
                    transaction.FineAmount = fineCalculator.CalculateFine(transaction.DueDate.Value, transaction.ReturnDate);
                }

                book.IsAvailable = true;
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Student> GetAllStudents()
        {
            return _context.Students.ToList();
        }

        public List<Book> GetAllBooks()
        {
            return _context.Books.ToList();
        }

        public List<Transaction> GetStudentTransactions(int studentId)
        {
            return _context.Transactions
                .Include(t => t.Book)
                .Where(t => t.StudentId == studentId)
                .OrderByDescending(t => t.BorrowDate)
                .ToList();
        }

        public bool SaveBook(Book book)
        {
            try
            {
                if (book.BookId == 0)
                {
                    _context.Books.Add(book);
                }
                else
                {
                    var existingBook = _context.Books.Find(book.BookId);
                    if (existingBook == null)
                    {
                        throw new Exception("Book not found.");
                    }

                    _context.Entry(existingBook).CurrentValues.SetValues(book);
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteBook(int bookId)
        {
            try
            {
                var book = _context.Books.Find(bookId);
                if (book == null)
                {
                    return false;
                }

                // Check if book is currently borrowed
                if (!book.IsAvailable)
                {
                    throw new Exception("Cannot delete a book that is currently borrowed.");
                }

                _context.Books.Remove(book);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteStudent(int studentId)
        {
            try
            {
                var student = _context.Students.Find(studentId);
                if (student == null)
                {
                    return false;
                }

                // Check if student has any borrowed books
                var hasBorrowedBooks = _context.Transactions
                    .Any(t => t.StudentId == studentId && t.ReturnDate == null);

                if (hasBorrowedBooks)
                {
                    throw new Exception("Cannot delete a student who has borrowed books.");
                }

                _context.Students.Remove(student);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Transaction> GetAllTransactions()
        {
            return _context.Transactions
                .Include(t => t.Book)
                .Include(t => t.Student)
                .OrderByDescending(t => t.BorrowDate)
                .ToList();
        }

        public Transaction? GetCurrentTransaction(int bookId)
        {
            return _context.Transactions
                .Include(t => t.Student)
                .FirstOrDefault(t => t.BookId == bookId && t.ReturnDate == null);
        }

        public List<Transaction> GetFilteredTransactions(
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? studentId = null,
            int? bookId = null,
            bool? isReturned = null)
        {
            var query = _context.Transactions
                .Include(t => t.Book)
                .Include(t => t.Student)
                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(t => t.BorrowDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.BorrowDate <= endDate.Value);
            }

            if (studentId.HasValue)
            {
                query = query.Where(t => t.StudentId == studentId.Value);
            }

            if (bookId.HasValue)
            {
                query = query.Where(t => t.BookId == bookId.Value);
            }

            if (isReturned.HasValue)
            {
                query = query.Where(t => (t.ReturnDate != null) == isReturned.Value);
            }

            return query.OrderByDescending(t => t.BorrowDate).ToList();
        }

        public void ClearAllTransactions()
        {
            foreach (var transaction in _context.Transactions)
            {
                _context.Transactions.Remove(transaction);
            }
            foreach (var book in _context.Books)
            {
                book.IsAvailable = true;
            }
            _context.SaveChanges();
        }

        public void ClearAllData()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                try
                {
                    // First delete all data
                    _context.Transactions.RemoveRange(_context.Transactions);
                    _context.Students.RemoveRange(_context.Students);
                    _context.Books.RemoveRange(_context.Books);
                    _context.SaveChanges();

                    // Then reset identity columns
                    using (var cmd = new SqlCommand(
                        @"DBCC CHECKIDENT ('Books', RESEED, 0);
                          DBCC CHECKIDENT ('Students', RESEED, 0);
                          DBCC CHECKIDENT ('Transactions', RESEED, 0);", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Verify the reset
                    VerifyIdentityReset(conn);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to clear data: {ex.Message}", ex);
                }
            }
        }

        private void VerifyIdentityReset(SqlConnection conn)
        {
            using (var cmd = new SqlCommand(
                @"SELECT 
                    (SELECT IDENT_CURRENT('Books')) as BooksCurrent,
                    (SELECT IDENT_CURRENT('Students')) as StudentsCurrent,
                    (SELECT IDENT_CURRENT('Transactions')) as TransactionsCurrent", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var booksCurrent = Convert.ToDecimal(reader["BooksCurrent"]);
                        var studentsCurrent = Convert.ToDecimal(reader["StudentsCurrent"]);
                        var transactionsCurrent = Convert.ToDecimal(reader["TransactionsCurrent"]);

                        if (booksCurrent != 0 || studentsCurrent != 0 || transactionsCurrent != 0)
                        {
                            throw new Exception("Identity columns were not properly reset.");
                        }
                    }
                }
            }
        }

        // Method to clear the Entity Framework Change Tracker
        public void ClearChangeTracker()
        {
            _context.ChangeTracker.Clear();
        }
    }
}
