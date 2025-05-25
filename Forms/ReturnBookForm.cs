using System;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagementSystem.BLL;

namespace LibraryManagementSystem.Forms
{
    public class ReturnBookForm : Form
    {
        private readonly LibraryManager _libraryManager;
        private TextBox txtBookId;
        private TextBox txtStudentId;
        private Button btnReturn;

        public ReturnBookForm(LibraryManager libraryManager)
        {
            _libraryManager = libraryManager;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            txtBookId = new TextBox { Location = new Point(100, 30), Size = new Size(200, 23), Name = "txtBookId" };
            txtStudentId = new TextBox { Location = new Point(100, 70), Size = new Size(200, 23), Name = "txtStudentId" };
            btnReturn = new Button { Text = "Return Book", Location = new Point(100, 110), Size = new Size(100, 30) };
            btnReturn.Click += btnReturn_Click;

            Controls.Add(new Label { Text = "Book ID:", Location = new Point(20, 30), AutoSize = true });
            Controls.Add(txtBookId);
            Controls.Add(new Label { Text = "Student ID:", Location = new Point(20, 70), AutoSize = true });
            Controls.Add(txtStudentId);
            Controls.Add(btnReturn);

            Text = "Return Book";
            ClientSize = new Size(350, 180);
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtBookId.Text, out int bookId) && int.TryParse(txtStudentId.Text, out int studentId))
            {
                if (_libraryManager.ReturnBook(bookId, studentId))
                {
                    MessageBox.Show("Book returned successfully!");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Failed to return book. Please check the IDs.");
                }
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter valid IDs.");
            }
        }
    }
}
