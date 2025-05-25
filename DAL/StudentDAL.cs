using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL
{
    public class StudentDAL
    {
        public bool StudentExists(string referenceId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Students WHERE ReferenceID = @refId", conn))
            {
                cmd.Parameters.AddWithValue("@refId", referenceId);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public void AddStudent(Student student)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string query = "INSERT INTO Students (Name, ReferenceID, Email, PhoneNumber) VALUES (@Name, @ReferenceID, @Email, @PhoneNumber)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", student.Name);
                    cmd.Parameters.AddWithValue("@ReferenceID", student.ReferenceID);
                    cmd.Parameters.AddWithValue("@Email", (object?)student.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PhoneNumber", (object?)student.PhoneNumber ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            } 
        }


        public List<Student> GetAllStudents()
        {
            var students = new List<Student>();
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new SqlCommand("SELECT * FROM Students", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        StudentId = (int)reader["StudentId"],
                        Name = reader["Name"].ToString(),
                        ReferenceID = reader["ReferenceID"].ToString(), // Fixed property name
                        Email = reader["Email"]?.ToString(),
                        PhoneNumber = reader["PhoneNumber"]?.ToString()
                    });
                }
            }
            return students;
        }
    }
}
