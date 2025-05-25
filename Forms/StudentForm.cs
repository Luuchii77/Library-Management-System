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
    public partial class StudentForm : Form
    {
        private readonly LibraryManager _libraryManager;
        private TextBox txtName;
        private TextBox txtReferenceID;
        private TextBox txtEmail;
        private TextBox txtPhoneNumber;
        private Button btnSave;
        private Button btnCancel;
        private Label lblName;
        private Label lblReferenceID;
        private Label lblEmail;
        private Label lblPhoneNumber;
        private Student? _student;

        public StudentForm(LibraryManager libraryManager, Student? student = null)
        {
            _libraryManager = libraryManager;
            _student = student;
            InitializeComponent();
            SetupForm();
            if (student != null)
            {
                LoadStudentData();
            }
        }

        private void InitializeComponent()
        {
            txtName = new TextBox();
            txtReferenceID = new TextBox();
            txtEmail = new TextBox();
            txtPhoneNumber = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            lblName = new Label();
            lblReferenceID = new Label();
            lblEmail = new Label();
            lblPhoneNumber = new Label();
            SuspendLayout();

            // lblName
            lblName.AutoSize = true;
            lblName.Location = new Point(20, 20);
            lblName.Name = "lblName";
            lblName.Size = new Size(50, 15);
            lblName.Text = "Name:";

            // txtName
            txtName.Location = new Point(120, 17);
            txtName.Name = "txtName";
            txtName.Size = new Size(250, 23);
            txtName.TabIndex = 0;

            // lblReferenceID
            lblReferenceID.AutoSize = true;
            lblReferenceID.Location = new Point(20, 60);
            lblReferenceID.Name = "lblReferenceID";
            lblReferenceID.Size = new Size(80, 15);
            lblReferenceID.Text = "Reference ID:";

            // txtReferenceID
            txtReferenceID.Location = new Point(120, 57);
            txtReferenceID.Name = "txtReferenceID";
            txtReferenceID.Size = new Size(250, 23);
            txtReferenceID.TabIndex = 1;

            // lblEmail
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(20, 100);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(40, 15);
            lblEmail.Text = "Email:";

            // txtEmail
            txtEmail.Location = new Point(120, 97);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(250, 23);
            txtEmail.TabIndex = 2;

            // lblPhoneNumber
            lblPhoneNumber.AutoSize = true;
            lblPhoneNumber.Location = new Point(20, 140);
            lblPhoneNumber.Name = "lblPhoneNumber";
            lblPhoneNumber.Size = new Size(90, 15);
            lblPhoneNumber.Text = "Phone Number:";

            // txtPhoneNumber
            txtPhoneNumber.Location = new Point(120, 137);
            txtPhoneNumber.Name = "txtPhoneNumber";
            txtPhoneNumber.Size = new Size(250, 23);
            txtPhoneNumber.TabIndex = 3;

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

            // StudentForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(394, 231);
            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(lblReferenceID);
            Controls.Add(txtReferenceID);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(lblPhoneNumber);
            Controls.Add(txtPhoneNumber);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "StudentForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Student Information";
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupForm()
        {
            // Add any additional form setup here
        }

        private void LoadStudentData()
        {
            if (_student != null)
            {
                txtName.Text = _student.Name;
                txtReferenceID.Text = _student.ReferenceID;
                txtEmail.Text = _student.Email;
                txtPhoneNumber.Text = _student.PhoneNumber;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var student = _student ?? new Student();
                student.Name = txtName.Text.Trim();
                student.ReferenceID = txtReferenceID.Text.Trim();
                student.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
                student.PhoneNumber = string.IsNullOrWhiteSpace(txtPhoneNumber.Text) ? null : txtPhoneNumber.Text.Trim();

                var validationContext = new ValidationContext(student);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(student, validationContext, validationResults, true))
                {
                    var errorMessage = string.Join("\n", validationResults.Select(r => r.ErrorMessage));
                    MessageBox.Show(errorMessage, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_libraryManager.SaveStudent(student))
                {
                    MessageBox.Show("Student information saved successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Failed to save student information.", "Error", 
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