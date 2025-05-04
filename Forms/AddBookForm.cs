using System;
using System.Windows.Forms;
using LibraryManagementSystem.BLL;

namespace LibraryManagementSystem.Forms
{
    public partial class BorrowBookForm : Form
    {
        private readonly LibraryManager libraryManager;

        public BorrowBookForm()
        {
            InitializeComponent(); // Ensure this method is defined in the designer file
            libraryManager = new LibraryManager();
        }

        private void btnBorrow_Click(object sender, EventArgs e)
        {
            int bookId = int.Parse(txtBookId.Text);
            int studentId = int.Parse(txtStudentId.Text);

            bool success = libraryManager.BorrowBook(bookId, studentId);

            if (success)
                MessageBox.Show("Book borrowed successfully!");
            else
                MessageBox.Show("Book is not available.");
        }
    }
}

