namespace LibraryManagementSystem.Forms
{
    partial class Mainform
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvBooks;
        private Button btnAddBook;
        private Button btnAddStudent;
        private Button btnBorrowBook;
        private Button btnReturnBook;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvBooks = new DataGridView();
            this.btnAddBook = new Button();
            this.btnAddStudent = new Button();
            this.btnBorrowBook = new Button();
            this.btnReturnBook = new Button();
            this.SuspendLayout();

            // Add Book Button
            this.btnAddBook.Text = "Add Book";
            this.btnAddBook.Location = new System.Drawing.Point(20, 20);
            this.btnAddBook.Size = new System.Drawing.Size(100, 30);
            this.btnAddBook.Click += new System.EventHandler(this.btnAddBook_Click);

            // Add Student Button
            this.btnAddStudent.Text = "Add Student";
            this.btnAddStudent.Location = new System.Drawing.Point(130, 20);
            this.btnAddStudent.Size = new System.Drawing.Size(100, 30);
            this.btnAddStudent.Click += new System.EventHandler(this.btnAddStudent_Click);

            // Borrow Book Button
            this.btnBorrowBook.Text = "Borrow Book";
            this.btnBorrowBook.Location = new System.Drawing.Point(240, 20);
            this.btnBorrowBook.Size = new System.Drawing.Size(100, 30);
            this.btnBorrowBook.Click += new System.EventHandler(this.btnBorrowBook_Click);

            // Return Book Button
            this.btnReturnBook.Text = "Return Book";
            this.btnReturnBook.Location = new System.Drawing.Point(350, 20);
            this.btnReturnBook.Size = new System.Drawing.Size(100, 30);
            this.btnReturnBook.Click += new System.EventHandler(this.btnReturnBook_Click);

            // DataGridView
            this.dgvBooks.Location = new System.Drawing.Point(20, 70);
            this.dgvBooks.Size = new System.Drawing.Size(750, 400);
            this.dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Mainform
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.btnAddBook);
            this.Controls.Add(this.btnAddStudent);
            this.Controls.Add(this.btnBorrowBook);
            this.Controls.Add(this.btnReturnBook);
            this.Controls.Add(this.dgvBooks);
            this.Text = "Library Management System";
            this.ResumeLayout(false);
        }
    }
}
