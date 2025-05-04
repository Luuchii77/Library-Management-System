using System;
using System.Windows.Forms;
using LibraryManagementSystem.BLL;

namespace LibraryManagementSystem.Forms
{
    public partial class BorrowBookForm : Form
    {
        private readonly LibraryManager _libraryManager;

        // Constructor accepting LibraryManager as a parameter  
        public BorrowBookForm(LibraryManager libraryManager)
        {
            _libraryManager = libraryManager;
            InitializeComponent();
        }

        private void btnBorrow_Click(object sender, EventArgs e)
        {
            int bookId = int.Parse(txtBookId.Text);
            int studentId = int.Parse(txtStudentId.Text);

            if (_libraryManager.BorrowBook(bookId, studentId))
            {
                MessageBox.Show("Book borrowed successfully!");
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Failed to borrow book. Please check the details.");
            }
        }

        // Add missing declaration for txtStudentId  
        private TextBox txtStudentId;

        // Ensure txtStudentId is initialized in InitializeComponent or constructor  
        private void InitializeComponent()
        {
            txtStudentId = new TextBox();
            txtBookId = new TextBox();
            btnBorrow = new Button();
            SuspendLayout();
            // 
            // txtStudentId
            // 
            txtStudentId.Location = new Point(100, 50);
            txtStudentId.Name = "txtStudentId";
            txtStudentId.Size = new Size(200, 23);
            txtStudentId.TabIndex = 0;
            // 
            // txtBookId
            // 
            txtBookId.Location = new Point(0, 0);
            txtBookId.Name = "txtBookId";
            txtBookId.Size = new Size(100, 23);
            txtBookId.TabIndex = 0;
            // 
            // btnBorrow
            // 
            btnBorrow.Location = new Point(0, 0);
            btnBorrow.Name = "btnBorrow";
            btnBorrow.Size = new Size(75, 23);
            btnBorrow.TabIndex = 0;
            // 
            // BorrowBookForm
            // 
            ClientSize = new Size(472, 261);
            Controls.Add(txtStudentId);
            Name = "BorrowBookForm";
            ResumeLayout(false);
            PerformLayout();

            // Other initialization code for txtBookId, btnBorrow, etc.  
        }

        private TextBox txtBookId;
        private Button btnBorrow;
    }
}
