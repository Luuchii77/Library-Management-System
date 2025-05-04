using System;
using System.Data.SqlClient;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL
{
    public class TransactionDAL
    {
        public bool BorrowBook(int bookId, int studentId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                // Check if the book is available
                using (var checkCmd = new SqlCommand("SELECT IsAvailable FROM Books WHERE BookId = @BookId", conn))
                {
                    checkCmd.Parameters.AddWithValue("@BookId", bookId);
                    var isAvailable = (bool?)checkCmd.ExecuteScalar();

                    if (!isAvailable.HasValue || !isAvailable.Value)
                        return false; // Book not found or already borrowed
                }

                // Insert transaction record
                using (var insertCmd = new SqlCommand(
                    "INSERT INTO Transactions (BookId, StudentId, BorrowDate) VALUES (@BookId, @StudentId, @BorrowDate)", conn))
                {
                    insertCmd.Parameters.AddWithValue("@BookId", bookId);
                    insertCmd.Parameters.AddWithValue("@StudentId", studentId);
                    insertCmd.Parameters.AddWithValue("@BorrowDate", DateTime.Now);
                    insertCmd.ExecuteNonQuery();
                }

                // Update book status
                using (var updateCmd = new SqlCommand("UPDATE Books SET IsAvailable = 0 WHERE BookId = @BookId", conn))
                {
                    updateCmd.Parameters.AddWithValue("@BookId", bookId);
                    updateCmd.ExecuteNonQuery();
                }

                return true;
            }
        }

        public void ReturnBook(int bookId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                // Update transaction with return date
                using (var updateTransactionCmd = new SqlCommand(
                    "UPDATE Transactions SET ReturnDate = @ReturnDate WHERE BookId = @BookId AND ReturnDate IS NULL", conn))
                {
                    updateTransactionCmd.Parameters.AddWithValue("@BookId", bookId);
                    updateTransactionCmd.Parameters.AddWithValue("@ReturnDate", DateTime.Now);
                    updateTransactionCmd.ExecuteNonQuery();
                }

                // Update book status
                using (var updateBookCmd = new SqlCommand("UPDATE Books SET IsAvailable = 1 WHERE BookId = @BookId", conn))
                {
                    updateBookCmd.Parameters.AddWithValue("@BookId", bookId);
                    updateBookCmd.ExecuteNonQuery();
                }
            }
        }
    }
}
