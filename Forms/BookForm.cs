using System;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LibraryManagementSystem.BLL;
using LibraryManagementSystem.Data;

namespace LibraryManagementSystem.Forms
{
    public partial class BookForm : Form
    {
        private readonly LibraryManager _libraryManager;
        private TextBox txtTitle;
        private TextBox txtAuthor;
        private TextBox txtCategory;
        private NumericUpDown numYearPublished;
        private Button btnSave;
        private Button btnCancel;
        private Label lblTitle;
        private Label lblAuthor;
        private Label lblCategory;
        private Label lblYearPublished;
        private Book? _book;

        public BookForm(LibraryManager libraryManager, Book? book = null)
        {
            _libraryManager = libraryManager;
            _book = book;
            InitializeComponent();
            SetupForm();
            if (book != null)
            {
                LoadBookData();
            }
        }

        private void InitializeComponent()
        {
            txtTitle = new TextBox();
            txtAuthor = new TextBox();
            txtCategory = new TextBox();
            numYearPublished = new NumericUpDown();
            btnSave = new Button();
            btnCancel = new Button();
            lblTitle = new Label();
            lblAuthor = new Label();
            lblCategory = new Label();
            lblYearPublished = new Label();
            SuspendLayout();

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(35, 15);
            lblTitle.Text = "Title:";

            // txtTitle
            txtTitle.Location = new Point(120, 17);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(250, 23);
            txtTitle.TabIndex = 0;

            // lblAuthor
            lblAuthor.AutoSize = true;
            lblAuthor.Location = new Point(20, 60);
            lblAuthor.Name = "lblAuthor";
            lblAuthor.Size = new Size(45, 15);
            lblAuthor.Text = "Author:";

            // txtAuthor
            txtAuthor.Location = new Point(120, 57);
            txtAuthor.Name = "txtAuthor";
            txtAuthor.Size = new Size(250, 23);
            txtAuthor.TabIndex = 1;

            // lblCategory
            lblCategory.AutoSize = true;
            lblCategory.Location = new Point(20, 100);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(60, 15);
            lblCategory.Text = "Category:";

            // txtCategory
            txtCategory.Location = new Point(120, 97);
            txtCategory.Name = "txtCategory";
            txtCategory.Size = new Size(250, 23);
            txtCategory.TabIndex = 2;

            // lblYearPublished
            lblYearPublished.AutoSize = true;
            lblYearPublished.Location = new Point(20, 140);
            lblYearPublished.Name = "lblYearPublished";
            lblYearPublished.Size = new Size(90, 15);
            lblYearPublished.Text = "Year Published:";

            // numYearPublished
            numYearPublished.Location = new Point(120, 137);
            numYearPublished.Name = "numYearPublished";
            numYearPublished.Size = new Size(120, 23);
            numYearPublished.TabIndex = 3;
            numYearPublished.Maximum = DateTime.Now.Year;
            numYearPublished.Minimum = 1800;
            numYearPublished.Value = DateTime.Now.Year;

            // btnSave
            btnSave.Location = new Point(120, 180);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(120, 30);
            btnSave.TabIndex = 4;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += new EventHandler(btnSave_Click);

            // btnCancel
            btnCancel.Location = new Point(250, 180);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(120, 30);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);

            // BookForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(394, 231);
            Controls.Add(lblTitle);
            Controls.Add(txtTitle);
            Controls.Add(lblAuthor);
            Controls.Add(txtAuthor);
            Controls.Add(lblCategory);
            Controls.Add(txtCategory);
            Controls.Add(lblYearPublished);
            Controls.Add(numYearPublished);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BookForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Book Information";
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupForm()
        {
            // Add any additional form setup here
        }

        private void LoadBookData()
        {
            if (_book != null)
            {
                txtTitle.Text = _book.Title;
                txtAuthor.Text = _book.Author;
                txtCategory.Text = _book.Category;
                numYearPublished.Value = _book.YearPublished;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var book = _book ?? new Book();
                book.Title = txtTitle.Text.Trim();
                book.Author = txtAuthor.Text.Trim();
                book.Category = txtCategory.Text.Trim();
                book.YearPublished = (int)numYearPublished.Value;
                book.IsAvailable = true;

                var validationContext = new ValidationContext(book);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(book, validationContext, validationResults, true))
                {
                    var errorMessage = string.Join("\n", validationResults.Select(r => r.ErrorMessage));
                    MessageBox.Show(errorMessage, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_libraryManager.SaveBook(book))
                {
                    MessageBox.Show("Book information saved successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Failed to save book information.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
} 