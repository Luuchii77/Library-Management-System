using System;
using System.Windows.Forms;
using LibraryManagementSystem.BLL;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Forms; // correct place for this using

namespace LibraryManagementSystem.Forms
{
    public partial class Mainform : Form
    {
        private readonly BookDAL bookDAL = new BookDAL();
        private readonly StudentDAL studentDAL = new StudentDAL();
        private readonly LibraryManager libraryManager = new LibraryManager(); // Shared instance

        public Mainform()
        {
            InitializeComponent();
            LoadBooks();
        }

        private void LoadBooks()
        {
            dgvBooks.DataSource = bookDAL.GetAllBooks();
        }

        private void btnAddBook_Click(object sender, EventArgs e)
        {
            using (var addBookForm = new AddBookForm())
            {
                if (addBookForm.ShowDialog() == DialogResult.OK)
                {
                    LoadBooks();
                }
            }
        }

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            using (var addStudentForm = new AddStudentForm())
            {
                if (addStudentForm.ShowDialog() == DialogResult.OK)
                {
                    // You can add logic here if needed
                }
            }
        }

        private void btnBorrowBook_Click(object sender, EventArgs e)
        {
            using (var borrowForm = new BorrowBookForm(libraryManager))
            {
                if (borrowForm.ShowDialog() == DialogResult.OK)
                {
                    LoadBooks();
                }
            }
        }

        private void btnReturnBook_Click(object sender, EventArgs e)
        {
            using (var returnForm = new ReturnBookForm(libraryManager))
            {
                if (returnForm.ShowDialog() == DialogResult.OK)
                {
                    LoadBooks();
                }
            }
        }
    }
}
