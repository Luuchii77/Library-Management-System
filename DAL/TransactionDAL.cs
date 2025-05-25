using System;
using System.Data.SqlClient;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL
{
    public class TransactionDAL
    {
        public bool BorrowBook(int bookId, int studentId, DateTime dueDate, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    // Check if the book is available
                    using (var checkCmd = new SqlCommand("SELECT IsAvailable FROM Books WHERE BookId = @BookId", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@BookId", bookId);
                        var isAvailable = (bool?)checkCmd.ExecuteScalar();

                        if (!isAvailable.HasValue)
                        {
                            errorMessage = "Book not found.";
                            return false;
                        }
                        if (!isAvailable.Value)
                        {
                            errorMessage = "Book is already borrowed.";
                            return false;
                        }
                    }

                    // Insert transaction record with due date
                    using (var insertCmd = new SqlCommand(
                        "INSERT INTO Transactions (BookId, StudentId, BorrowDate, DueDate) VALUES (@BookId, @StudentId, @BorrowDate, @DueDate)", conn))
                    {
                        insertCmd.Parameters.AddWithValue("@BookId", bookId);
                        insertCmd.Parameters.AddWithValue("@StudentId", studentId);
                        insertCmd.Parameters.AddWithValue("@BorrowDate", DateTime.Now);
                        insertCmd.Parameters.AddWithValue("@DueDate", dueDate);
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
            catch (SqlException ex)
            {
                errorMessage = $"SQL Error: {ex.Message}";
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error: {ex.Message}";
                return false;
            }
        }

        public void ReturnBook(int bookId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
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
