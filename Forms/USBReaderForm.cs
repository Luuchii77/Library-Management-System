using System;
using System.Windows.Forms;
using LibraryManagementSystem.Models;
using System.Linq;
using LibraryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Drawing;

namespace LibraryManagementSystem.Forms
{
    public partial class USBReaderForm : Form
    {
        private readonly LibraryDbContext _context;
        private TextBox txtRFID;
        private Label lblStatus;
        private TextBox txtStudentInfo;
        private Button btnAddStudent;
        private MainForm _mainForm;
        private Label lblUsbStatus;
        private System.Windows.Forms.Timer usbStatusTimer;
        private DateTime lastInputTime;
        private Button btnExportBorrowedBooks;

        public USBReaderForm(MainForm mainForm)
        {
            _mainForm = mainForm;
            InitializeComponent();
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var connectionString = config.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            _context = new LibraryDbContext(optionsBuilder.Options);
            InitializeUSBReaderForm();
        }

        private void InitializeUSBReaderForm()
        {
            this.Text = "USB Reader Interface";
            this.Size = new System.Drawing.Size(600, 400);

            // USB Status Label
            lblUsbStatus = new Label
            {
                Location = new System.Drawing.Point(50, 10),
                Size = new System.Drawing.Size(200, 20),
                Text = "USB Reader: Offline"
            };

            // Timer for USB status
            usbStatusTimer = new System.Windows.Forms.Timer();
            usbStatusTimer.Interval = 1000; // 1 second
            usbStatusTimer.Tick += UsbStatusTimer_Tick;
            usbStatusTimer.Start();
            lastInputTime = DateTime.MinValue;

            // RFID TextBox
            txtRFID = new TextBox
            {
                Location = new System.Drawing.Point(50, 30),
                Size = new System.Drawing.Size(200, 20),
                PlaceholderText = "Scan RFID..."
            };
            txtRFID.TextChanged += TxtRFID_TextChanged;

            // Status Label
            lblStatus = new Label
            {
                Location = new System.Drawing.Point(50, 60),
                Size = new System.Drawing.Size(500, 20),
                Text = "Ready to scan..."
            };

            // Student Info TextBox
            txtStudentInfo = new TextBox
            {
                Location = new System.Drawing.Point(50, 90),
                Size = new System.Drawing.Size(500, 180),
                Multiline = true,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 10),
                BackColor = this.BackColor
            };

            // Add Student Button
            btnAddStudent = new Button
            {
                Location = new System.Drawing.Point(50, 200),
                Size = new System.Drawing.Size(150, 30),
                Text = "Add New Student",
                Visible = false
            };
            btnAddStudent.Click += BtnAddStudent_Click;

            // Export Borrowed Books Button
            btnExportBorrowedBooks = new Button
            {
                Location = new System.Drawing.Point(50, 320),
                Size = new System.Drawing.Size(200, 30),
                Text = "Export Borrowed Books",
                Visible = false
            };
            btnExportBorrowedBooks.Click += BtnExportBorrowedBooks_Click;

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                lblUsbStatus,
                txtRFID,
                lblStatus,
                txtStudentInfo,
                btnAddStudent,
                btnExportBorrowedBooks
            });
        }

        private void TxtRFID_TextChanged(object sender, EventArgs e)
        {
            lastInputTime = DateTime.Now;
            lblUsbStatus.Text = "USB Reader: Online";
            if (txtRFID.Text.Length >= 10)
            {
                ProcessRFID(txtRFID.Text);
                txtRFID.Clear();
            }
        }

        private void UsbStatusTimer_Tick(object sender, EventArgs e)
        {
            if ((DateTime.Now - lastInputTime).TotalSeconds > 10)
            {
                lblUsbStatus.Text = "USB Reader: Offline";
            }
        }

        private void ProcessRFID(string rfid)
        {
            string scanned = rfid.Trim();
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var connectionString = config.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            using (var freshContext = new LibraryDbContext(optionsBuilder.Options))
            {
                var students = freshContext.Students.ToList();
                foreach (var student in students)
                {
                    string dbRef = student.ReferenceID?.Trim();
                    if (scanned == dbRef)
                    {
                        lblStatus.Text = "Student found";
                        var borrowedBooks = freshContext.Transactions
                            .Where(t => t.StudentId == student.StudentId && t.ReturnDate == null)
                            .Select(t => new {
                                t.Book.Title,
                                t.Book.Author,
                                t.BorrowDate
                            })
                            .ToList();
                        int count = borrowedBooks.Count();
                        string borrowedBooksInfo = $"\r\n\r\nCurrently Borrowed Books ({count}):";
                        if (count > 0)
                        {
                            foreach (var book in borrowedBooks)
                            {
                                borrowedBooksInfo += $"\r\n- Title: {book.Title}\r\n  Author: {book.Author}\r\n  Borrowed: {book.BorrowDate:yyyy-MM-dd}";
                            }
                        }
                        else
                        {
                            borrowedBooksInfo += " None";
                        }
                        borrowedBooksInfo += "\r\n\r\nTo return books, please go to the Books tab.";
                        txtStudentInfo.Text =
                            $"Name: {student.Name}\r\n" +
                            $"Reference ID: {student.ReferenceID}\r\n" +
                            $"Email: {student.Email}\r\n" +
                            $"Phone: {student.PhoneNumber}\r\n" +
                            borrowedBooksInfo;
                        btnAddStudent.Visible = false;
                        btnExportBorrowedBooks.Visible = count > 0;
                        return;
                    }
                }
                // If not found
                lblStatus.Text = "Student not found in the system";
                txtStudentInfo.Text = "This Student isn't Registered in the System";
                btnAddStudent.Visible = true;
                btnExportBorrowedBooks.Visible = false;
            }
        }

        private void BtnAddStudent_Click(object sender, EventArgs e)
        {
            // Switch to Students tab in MainForm and close this form
            _mainForm.SwitchToStudentsTab();
            this.Close();
        }

        private void BtnExportBorrowedBooks_Click(object sender, EventArgs e)
        {
            // Export the currently displayed borrowed books to a text file
            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*", FileName = "BorrowedBooks.txt" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(sfd.FileName, txtStudentInfo.Text);
                    MessageBox.Show("Borrowed books exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
} 