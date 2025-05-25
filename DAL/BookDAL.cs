using System.Collections.Generic;
using System.Data.SqlClient;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL
{
    public class BookDAL
    {
        public List<Book> GetAllBooks()
        {
            var books = new List<Book>();
            using (var conn = DatabaseHelper.GetConnection()) // Already opens connection
            using (var cmd = new SqlCommand("SELECT * FROM Books", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            BookId = (int)reader["BookId"],
                            Title = reader["Title"].ToString(),
                            IsAvailable = (bool)reader["IsAvailable"]
                        });
                    }
                }
            }
            return books;
        }

        // ADD this method:
        public void AddBook(Book book)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("INSERT INTO Books (Title, IsAvailable) VALUES (@Title, @IsAvailable)", conn))
                {
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@IsAvailable", book.IsAvailable);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
