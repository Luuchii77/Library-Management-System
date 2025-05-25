using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.BLL;
using LibraryManagementSystem.Utils;

namespace LibraryManagementSystem.Forms
{
    public class MainForm : Form
    {
        private readonly LibraryManager _libraryManager;
        private TabControl tabControl;
        private TabPage tabBooks;
        private TabPage tabStudents;
        private TabPage tabTransactions;
        private DataGridView dgvBooks;
        private DataGridView dgvStudents;
        private DataGridView dgvTransactions;
        private Button btnAddBook;
        private Button btnEditBook;
        private Button btnDeleteBook;
        private Button btnAddStudent;
        private Button btnEditStudent;
        private Button btnDeleteStudent;
        private Button btnBorrowBook;
        private Button btnReturnBook;
        private Button btnUSBReader;
        private TextBox txtSearchBooks;
        private TextBox txtSearchStudents;
        private Label lblSearchBooks;
        private Label lblSearchStudents;

        public MainForm(LibraryManager libraryManager)
        {
            _libraryManager = libraryManager;
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            tabControl = new TabControl();
            tabBooks = new TabPage();
            tabStudents = new TabPage();
            tabTransactions = new TabPage();
            dgvBooks = new DataGridView();
            dgvStudents = new DataGridView();
            dgvTransactions = new DataGridView();
            btnAddBook = new Button();
            btnEditBook = new Button();
            btnDeleteBook = new Button();
            btnAddStudent = new Button();
            btnEditStudent = new Button();
            btnDeleteStudent = new Button();
            btnBorrowBook = new Button();
            btnReturnBook = new Button();
            btnUSBReader = new Button();
            txtSearchBooks = new TextBox();
            txtSearchStudents = new TextBox();
            lblSearchBooks = new Label();
            lblSearchStudents = new Label();

            // MainForm
            this.Text = "Library Management System";
            this.Size = new Size(1024, 768);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 600);

            // TabControl
            tabControl.Dock = DockStyle.Fill;
            tabControl.Controls.Add(tabBooks);
            tabControl.Controls.Add(tabStudents);
            tabControl.Controls.Add(tabTransactions);

            // TabBooks
            tabBooks.Text = "Books";
            tabBooks.Padding = new Padding(10);

            // Search Books
            lblSearchBooks.AutoSize = true;
            lblSearchBooks.Location = new Point(10, 15);
            lblSearchBooks.Text = "Search:";

            txtSearchBooks.Location = new Point(60, 12);
            txtSearchBooks.Size = new Size(200, 23);
            txtSearchBooks.TextChanged += TxtSearchBooks_TextChanged;

            // Book Buttons
            btnAddBook.Location = new Point(10, 50);
            btnAddBook.Size = new Size(100, 30);
            btnAddBook.Text = "Add Book";
            btnAddBook.Click += BtnAddBook_Click;

            btnEditBook.Location = new Point(120, 50);
            btnEditBook.Size = new Size(100, 30);
            btnEditBook.Text = "Edit Book";
            btnEditBook.Click += BtnEditBook_Click;

            btnDeleteBook.Location = new Point(230, 50);
            btnDeleteBook.Size = new Size(100, 30);
            btnDeleteBook.Text = "Delete Book";
            btnDeleteBook.Click += BtnDeleteBook_Click;

            btnBorrowBook.Location = new Point(340, 50);
            btnBorrowBook.Size = new Size(100, 30);
            btnBorrowBook.Text = "Borrow Book";
            btnBorrowBook.Click += BtnBorrowBook_Click;

            btnReturnBook.Location = new Point(450, 50);
            btnReturnBook.Size = new Size(100, 30);
            btnReturnBook.Text = "Return Book";
            btnReturnBook.Click += BtnReturnBook_Click;

            // Add USB Reader button after Return Book button
            btnUSBReader.Location = new Point(560, 50);
            btnUSBReader.Size = new Size(100, 30);
            btnUSBReader.Text = "USB Reader";
            btnUSBReader.Click += BtnUSBReader_Click;

            // Books Grid
            dgvBooks.Location = new Point(10, 90);
            dgvBooks.Size = new Size(780, 400);
            dgvBooks.Dock = DockStyle.Bottom;
            dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooks.MultiSelect = false;
            dgvBooks.AllowUserToAddRows = false;
            dgvBooks.AllowUserToDeleteRows = false;
            dgvBooks.ReadOnly = true;

            // TabStudents
            tabStudents.Text = "Students";
            tabStudents.Padding = new Padding(10);

            // Search Students
            lblSearchStudents.AutoSize = true;
            lblSearchStudents.Location = new Point(10, 15);
            lblSearchStudents.Text = "Search:";

            txtSearchStudents.Location = new Point(60, 12);
            txtSearchStudents.Size = new Size(200, 23);
            txtSearchStudents.TextChanged += TxtSearchStudents_TextChanged;

            // Student Buttons
            btnAddStudent.Location = new Point(10, 50);
            btnAddStudent.Size = new Size(100, 30);
            btnAddStudent.Text = "Add Student";
            btnAddStudent.Click += BtnAddStudent_Click;

            btnEditStudent.Location = new Point(120, 50);
            btnEditStudent.Size = new Size(100, 30);
            btnEditStudent.Text = "Edit Student";
            btnEditStudent.Click += BtnEditStudent_Click;

            btnDeleteStudent.Location = new Point(230, 50);
            btnDeleteStudent.Size = new Size(100, 30);
            btnDeleteStudent.Text = "Delete Student";
            btnDeleteStudent.Click += BtnDeleteStudent_Click;

            // Students Grid
            dgvStudents.Location = new Point(10, 90);
            dgvStudents.Size = new Size(780, 400);
            dgvStudents.Dock = DockStyle.Bottom;
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStudents.MultiSelect = false;
            dgvStudents.AllowUserToAddRows = false;
            dgvStudents.AllowUserToDeleteRows = false;
            dgvStudents.ReadOnly = true;

            // TabTransactions
            tabTransactions.Text = "Transactions";
            tabTransactions.Padding = new Padding(10);

            // Transaction Filters
            var lblDateRange = new Label
            {
                AutoSize = true,
                Location = new Point(10, 15),
                Text = "Date Range:"
            };

            var dtpStartDate = new DateTimePicker
            {
                Location = new Point(80, 12),
                Size = new Size(120, 23),
                Format = DateTimePickerFormat.Short
            };

            var lblTo = new Label
            {
                AutoSize = true,
                Location = new Point(210, 15),
                Text = "to"
            };

            var dtpEndDate = new DateTimePicker
            {
                Location = new Point(230, 12),
                Size = new Size(120, 23),
                Format = DateTimePickerFormat.Short
            };

            var chkShowReturned = new CheckBox
            {
                AutoSize = true,
                Location = new Point(370, 14),
                Text = "Show Returned",
                Checked = true
            };

            var btnFilter = new Button
            {
                Location = new Point(480, 12),
                Size = new Size(80, 23),
                Text = "Filter",
                UseVisualStyleBackColor = true
            };

            btnFilter.Click += (s, e) =>
            {
                var transactions = _libraryManager.GetFilteredTransactions(
                    dtpStartDate.Value,
                    dtpEndDate.Value,
                    null,
                    null,
                    chkShowReturned.Checked);
                dgvTransactions.DataSource = transactions;
            };

            // Transactions Grid
            dgvTransactions.Location = new Point(10, 50);
            dgvTransactions.Size = new Size(780, 440);
            dgvTransactions.Dock = DockStyle.Bottom;
            dgvTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.MultiSelect = false;
            dgvTransactions.AllowUserToAddRows = false;
            dgvTransactions.AllowUserToDeleteRows = false;
            dgvTransactions.ReadOnly = true;

            // Add columns to transactions grid
            dgvTransactions.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn
                {
                    Name = "TransactionId",
                    HeaderText = "ID",
                    Width = 50
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "BookTitle",
                    HeaderText = "Book Title",
                    Width = 200
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "StudentName",
                    HeaderText = "Student Name",
                    Width = 150
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "BorrowDate",
                    HeaderText = "Borrow Date",
                    Width = 100
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "ReturnDate",
                    HeaderText = "Return Date",
                    Width = 100
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Fine",
                    HeaderText = "Fine",
                    Width = 100
                }
            });

            // Add export and clear history buttons to the transactions tab
            var btnExportTransactions = new Button
            {
                Location = new Point(580, 12),
                Size = new Size(120, 23),
                Text = "Export Transactions",
                UseVisualStyleBackColor = true
            };
            btnExportTransactions.Click += (s, e) => ExportGridToCsv(dgvTransactions, "transactions.csv");

            var btnClearHistory = new Button
            {
                Location = new Point(710, 12),
                Size = new Size(100, 23),
                Text = "Clear History",
                UseVisualStyleBackColor = true
            };
            btnClearHistory.Click += (s, e) =>
            {
                if (MessageBox.Show("Are you sure you want to clear all transaction history?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _libraryManager.ClearAllTransactions();
                    LoadTransactions();
                }
            };

            tabTransactions.Controls.AddRange(new Control[] {
                lblDateRange,
                dtpStartDate,
                lblTo,
                dtpEndDate,
                chkShowReturned,
                btnFilter,
                dgvTransactions,
                btnExportTransactions,
                btnClearHistory
            });

            // Add export buttons for books and students
            var btnExportBooks = new Button
            {
                Location = new Point(600, 12),
                Size = new Size(120, 23),
                Text = "Export Books",
                UseVisualStyleBackColor = true
            };
            btnExportBooks.Click += (s, e) => ExportGridToCsv(dgvBooks, "books.csv");
            tabBooks.Controls.Add(btnExportBooks);

            var btnExportStudents = new Button
            {
                Location = new Point(600, 12),
                Size = new Size(120, 23),
                Text = "Export Students",
                UseVisualStyleBackColor = true
            };
            btnExportStudents.Click += (s, e) => ExportGridToCsv(dgvStudents, "students.csv");
            tabStudents.Controls.Add(btnExportStudents);

            // Add controls to forms
            tabBooks.Controls.AddRange(new Control[] { 
                lblSearchBooks, txtSearchBooks, 
                btnAddBook, btnEditBook, btnDeleteBook, btnBorrowBook, btnReturnBook, btnUSBReader,
                dgvBooks 
            });

            tabStudents.Controls.AddRange(new Control[] { 
                lblSearchStudents, txtSearchStudents,
                btnAddStudent, btnEditStudent, btnDeleteStudent,
                dgvStudents 
            });

            // Add restart application button
            var btnRestart = new Button
            {
                Location = new Point(800, 12),
                Size = new Size(120, 23),
                Text = "Restart App",
                UseVisualStyleBackColor = true
            };
            btnRestart.Click += (s, e) => RestartApplication();
            this.Controls.Add(btnRestart);

            this.Controls.Add(tabControl);
        }

        private void LoadData()
        {
            // Load Books
            _libraryManager.ClearChangeTracker();
            var books = _libraryManager.GetAllBooks();
            var displayBooks = books.Select(b => new
            {
                b.BookId,
                b.Title,
                b.Author,
                b.Category,
                b.YearPublished,
                Status = b.IsAvailable ? "Available" : "Borrowed"
            }).ToList();
            dgvBooks.DataSource = displayBooks;
            dgvBooks.Refresh();

            // Load Students
            var students = _libraryManager.GetAllStudents();
            dgvStudents.DataSource = students;

            // Load Transactions
            LoadTransactions();
        }

        private void LoadTransactions()
        {
            _libraryManager.ClearChangeTracker();
            dgvTransactions.Columns.Clear();
            var transactions = _libraryManager.GetAllTransactions();
            var displayData = transactions.Select(t => new
            {
                t.TransactionId,
                BookTitle = t.Book.Title,
                StudentName = t.Student.Name,
                BorrowDate = t.BorrowDate.ToShortDateString(),
                ReturnDate = t.ReturnDate?.ToShortDateString() ?? "Not Returned",
                Status = t.ReturnDate.HasValue ? "Returned" : "Borrowed",
                Fine = t.FineAmount?.ToString("C") ?? "N/A"
            }).ToList();

            dgvTransactions.DataSource = displayData;

            // Add Fine column if it doesn't exist (since we clear columns)
            if (!dgvTransactions.Columns.Contains("Fine"))
            {
                dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Fine",
                    HeaderText = "Fine",
                    Width = 100
                });
            }
        }

        private void TxtSearchBooks_TextChanged(object sender, EventArgs e)
        {
            var books = _libraryManager.GetAllBooks();
            var filteredBooks = books.Where(b => 
                b.Title.Contains(txtSearchBooks.Text, StringComparison.OrdinalIgnoreCase) ||
                b.Author.Contains(txtSearchBooks.Text, StringComparison.OrdinalIgnoreCase) ||
                b.Category.Contains(txtSearchBooks.Text, StringComparison.OrdinalIgnoreCase)
            ).ToList();
            dgvBooks.DataSource = filteredBooks;
        }

        private void TxtSearchStudents_TextChanged(object sender, EventArgs e)
        {
            var students = _libraryManager.GetAllStudents();
            var filteredStudents = students.Where(s =>
                s.Name.Contains(txtSearchStudents.Text, StringComparison.OrdinalIgnoreCase) ||
                s.ReferenceID.Contains(txtSearchStudents.Text, StringComparison.OrdinalIgnoreCase) ||
                (s.Email != null && s.Email.Contains(txtSearchStudents.Text, StringComparison.OrdinalIgnoreCase))
            ).ToList();
            dgvStudents.DataSource = filteredStudents;
        }

        private void BtnAddBook_Click(object sender, EventArgs e)
        {
            using (var form = new BookForm(_libraryManager))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void BtnEditBook_Click(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count > 0)
            {
                var bookId = Convert.ToInt32(dgvBooks.SelectedRows[0].Cells["BookId"].Value);
                var book = _libraryManager.GetAllBooks().FirstOrDefault(b => b.BookId == bookId);
                using (var form = new BookForm(_libraryManager, book))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
            }
        }

        private void BtnDeleteBook_Click(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count > 0)
            {
                var bookId = Convert.ToInt32(dgvBooks.SelectedRows[0].Cells["BookId"].Value);
                var book = _libraryManager.GetAllBooks().FirstOrDefault(b => b.BookId == bookId);
                if (MessageBox.Show("Are you sure you want to delete this book?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (_libraryManager.DeleteBook(book.BookId))
                    {
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete book.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnAddStudent_Click(object sender, EventArgs e)
        {
            using (var form = new StudentForm(_libraryManager))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void BtnEditStudent_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count > 0)
            {
                var student = (Student)dgvStudents.SelectedRows[0].DataBoundItem;
                using (var form = new StudentForm(_libraryManager, student))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
            }
        }

        private void BtnDeleteStudent_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count > 0)
            {
                var student = (Student)dgvStudents.SelectedRows[0].DataBoundItem;
                if (MessageBox.Show("Are you sure you want to delete this student?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (_libraryManager.DeleteStudent(student.StudentId))
                    {
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete student.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnBorrowBook_Click(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count > 0)
            {
                var bookId = Convert.ToInt32(dgvBooks.SelectedRows[0].Cells["BookId"].Value);
                var book = _libraryManager.GetAllBooks().FirstOrDefault(b => b.BookId == bookId);
                if (!book.IsAvailable)
                {
                    MessageBox.Show("This book is not available for borrowing.", "Not Available",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var form = new BorrowBookForm(_libraryManager, book))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
            }
        }

        private void BtnReturnBook_Click(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count > 0)
            {
                var bookId = Convert.ToInt32(dgvBooks.SelectedRows[0].Cells["BookId"].Value);
                var book = _libraryManager.GetAllBooks().FirstOrDefault(b => b.BookId == bookId);
                if (book.IsAvailable)
                {
                    MessageBox.Show("This book is already available.", "Already Available",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Get all active transactions for this book
                var transactions = _libraryManager.GetAllTransactions()
                    .Where(t => t.BookId == book.BookId && t.ReturnDate == null)
                    .ToList();

                if (transactions.Count == 0)
                {
                    MessageBox.Show("No active transaction found for this book.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var selectedTransaction = transactions.First();
                
                // Open the details form for confirmation
                using (var detailsForm = new ReturnBookDetailsForm(selectedTransaction))
                {
                    if (detailsForm.ShowDialog() == DialogResult.OK)
                    {
                         // Proceed with return if confirmed
                         var studentId = selectedTransaction.StudentId;

                         if (_libraryManager.ReturnBook(book.BookId, studentId))
                         {
                             MessageBox.Show($"Book returned successfully!", "Success",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);
                             LoadData(); // Refresh all grids
                         }
                         else
                         {
                             MessageBox.Show("Failed to return book.", "Error",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                         }
                    }
                    // If dialog result is not OK (Cancel), do nothing
                }
            }
        }

        // Add restart application button
        private void RestartApplication()
        {
            try
            {
                // Check for pending transactions (books not available)
                var books = _libraryManager.GetAllBooks();
                if (books.Any(b => !b.IsAvailable))
                {
                    if (MessageBox.Show(
                        "There are still books that have not been returned. Are you sure you want to clear all data and restart?", 
                        "Pending Transactions", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Warning) != DialogResult.Yes)
                    {
                        return;
                    }
                }

                // Clear all data from the database
                _libraryManager.ClearAllData();

                // Show success message
                MessageBox.Show(
                    "Application data has been cleared successfully. The application will now restart.", 
                    "Success", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                // Restart the application
                Application.Restart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred while restarting the application: {ex.Message}\n\nPlease try again or contact support if the problem persists.", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void ExportGridToCsv(DataGridView dgv, string baseFilename)
        {
            if (dgv.Rows.Count == 0) return;
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string filename = System.IO.Path.GetFileNameWithoutExtension(baseFilename) + "_" + timestamp + ".csv";
            using (var sfd = new SaveFileDialog { FileName = filename, Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (var sw = new System.IO.StreamWriter(sfd.FileName))
                    {
                        // Write headers
                        var headers = string.Join(",", dgv.Columns.Cast<DataGridViewColumn>().Select(c => c.HeaderText));
                        sw.WriteLine(headers);
                        // Write rows
                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                var cells = string.Join(",", row.Cells.Cast<DataGridViewCell>().Select(c => c.Value?.ToString()?.Replace(",", " ") ?? ""));
                                sw.WriteLine(cells);
                            }
                        }
                    }
                }
            }
        }

        private void BtnUSBReader_Click(object sender, EventArgs e)
        {
            var usbReaderForm = new USBReaderForm(this);
            usbReaderForm.ShowDialog();
        }

        public void SwitchToStudentsTab()
        {
            tabControl.SelectedTab = tabStudents;
        }
    }
}
