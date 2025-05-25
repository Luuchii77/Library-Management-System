using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.BLL;
using LibraryManagementSystem.Data;

namespace LibraryManagementSystem.Forms
{
    public partial class AddStudentForm : Form
    {
        private StudentDAL studentDAL = new StudentDAL();

        public AddStudentForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string referenceId = txtReferenceId.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(referenceId))
            {
                MessageBox.Show("Name and Reference ID are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (referenceId.Length < 9 || referenceId.Length > 11)
            {
                MessageBox.Show("Reference ID must be between 9 and 11 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(email) && !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Invalid email format.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var student = new Student
            {
                Name = name,
                ReferenceID = referenceId,
                Email = email,
                PhoneNumber = phone
            };

            studentDAL.AddStudent(student);
            MessageBox.Show("Student added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
