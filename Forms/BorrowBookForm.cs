using System;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagementSystem.BLL;
using LibraryManagementSystem.Models;
using System.Linq;

namespace LibraryManagementSystem.Forms
{
    public partial class BorrowBookForm : Form
    {
        private readonly LibraryManager _libraryManager;
        private readonly Book _book;
        private TextBox txtBookId;
        private TextBox txtStudentName;
        private DateTimePicker dtpDueDate;
        private Button btnBorrow;
        private Label lblBookId;
        private Label lblReferenceId;
        private Label lblDueDate;
        private AutoCompleteStringCollection studentNamesAutoComplete;

        public BorrowBookForm(LibraryManager libraryManager, Book book)
        {
            _libraryManager = libraryManager;
            _book = book;
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            txtBookId = new TextBox();
            txtStudentName = new TextBox();
            dtpDueDate = new DateTimePicker();
            btnBorrow = new Button();
            lblBookId = new Label();
            lblReferenceId = new Label();
            lblDueDate = new Label();
            SuspendLayout();

            // lblBookId
            lblBookId.AutoSize = true;
            lblBookId.Location = new Point(20, 20);
            lblBookId.Name = "lblBookId";
            lblBookId.Size = new Size(50, 15);
            lblBookId.Text = "Book ID:";

            // txtBookId
            txtBookId.Location = new Point(100, 17);
            txtBookId.Name = "txtBookId";
            txtBookId.Size = new Size(200, 23);
            txtBookId.TabIndex = 0;
            txtBookId.Text = _book.BookId.ToString();
            txtBookId.ReadOnly = false;

            // lblReferenceId (renamed for Name)
            lblReferenceId.AutoSize = true;
            lblReferenceId.Location = new Point(20, 60);
            lblReferenceId.Name = "lblStudentName";
            lblReferenceId.Size = new Size(80, 15);
            lblReferenceId.Text = "Student Name:";

            // txtStudentName
            txtStudentName.Location = new Point(100, 57);
            txtStudentName.Name = "txtStudentName";
            txtStudentName.Size = new Size(200, 23);
            txtStudentName.TabIndex = 1;
            txtStudentName.TextChanged += TxtStudentName_TextChanged;

            // lblDueDate
            lblDueDate.AutoSize = true;
            lblDueDate.Location = new Point(20, 100);
            lblDueDate.Name = "lblDueDate";
            lblDueDate.Size = new Size(60, 15);
            lblDueDate.Text = "Due Date:";

            // dtpDueDate
            dtpDueDate.Location = new Point(100, 97);
            dtpDueDate.Name = "dtpDueDate";
            dtpDueDate.Size = new Size(200, 23);
            dtpDueDate.TabIndex = 2;
            dtpDueDate.Format = DateTimePickerFormat.Short;
            dtpDueDate.Value = DateTime.Now.AddDays(14); // Default to 14 days from now

            // Autocomplete setup
            studentNamesAutoComplete = new AutoCompleteStringCollection();
            txtStudentName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtStudentName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtStudentName.AutoCompleteCustomSource = studentNamesAutoComplete;

            // btnBorrow
            btnBorrow.Location = new Point(100, 140);
            btnBorrow.Name = "btnBorrow";
            btnBorrow.Size = new Size(200, 30);
            btnBorrow.TabIndex = 3;
            btnBorrow.Text = "Borrow Book";
            btnBorrow.UseVisualStyleBackColor = true;
            btnBorrow.Click += new EventHandler(btnBorrow_Click);

            // BorrowBookForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 250);
            Controls.Add(lblBookId);
            Controls.Add(txtBookId);
            Controls.Add(lblReferenceId);
            Controls.Add(txtStudentName);
            Controls.Add(lblDueDate);
            Controls.Add(dtpDueDate);
            Controls.Add(btnBorrow);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BorrowBookForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Borrow Book";
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupForm()
        {
            // Populate autocomplete with all student names
            var students = _libraryManager.GetAllStudents();
            studentNamesAutoComplete.Clear();
            studentNamesAutoComplete.AddRange(students.Select(s => s.Name).ToArray());
        }

        private void TxtStudentName_TextChanged(object sender, EventArgs e)
        {
            // Optionally, you can filter or update suggestions here if needed
        }

        private void btnBorrow_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtStudentName.Text))
                {
                    MessageBox.Show("Please enter the student's name.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!int.TryParse(txtBookId.Text.Trim(), out int bookId))
                {
                    MessageBox.Show("Invalid Book ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var book = _libraryManager.GetAllBooks().FirstOrDefault(b => b.BookId == bookId);
                if (book == null)
                {
                    MessageBox.Show("Book not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Look up student by Name (case-insensitive, exact match)
                var students = _libraryManager.GetAllStudents();
                var student = students.FirstOrDefault(s => s.Name.Equals(txtStudentName.Text.Trim(), StringComparison.OrdinalIgnoreCase));
                if (student == null)
                {
                    MessageBox.Show("Student not found.", "Not Found", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Prevent borrowing if already borrowed
                var transactions = _libraryManager.GetStudentTransactions(student.StudentId);
                if (transactions.Any(t => t.BookId == book.BookId && t.ReturnDate == null))
                {
                    MessageBox.Show("This student has already borrowed this book and has not returned it yet.", "Already Borrowed", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Get the selected due date
                DateTime dueDate = dtpDueDate.Value.Date; // Get only the date part

                // Borrow the book
                var result = _libraryManager.BorrowBook(book.BookId, student.StudentId, dueDate);
                if (result.Success)
                {
                    MessageBox.Show("Book borrowed successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show($"Failed to borrow book: {result.ErrorMessage}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 