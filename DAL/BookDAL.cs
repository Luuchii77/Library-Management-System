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
                            IsAvailable = (bool)reader["Available"]
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
                using (var cmd = new SqlCommand("INSERT INTO Books (Title, Available) VALUES (@Title, @Available)", conn))
                {
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@Available", book.IsAvailable);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
