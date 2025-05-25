using System;
using System.Windows.Forms;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Forms
{
    public partial class ReturnBookDetailsForm : Form
    {
        private Label lblBookInfo;
        private Label lblStudentInfo;
        private Label lblBorrowDate;
        private Label lblFineAmount;
        private Button btnConfirmReturn;
        private Button btnCancel;

        public Transaction Transaction { get; private set; }

        public ReturnBookDetailsForm(Transaction transaction)
        {
            InitializeComponent();
            Transaction = transaction;
            DisplayTransactionDetails();
        }

        private void InitializeComponent()
        {
            lblBookInfo = new Label();
            lblStudentInfo = new Label();
            lblBorrowDate = new Label();
            lblFineAmount = new Label();
            btnConfirmReturn = new Button();
            btnCancel = new Button();
            SuspendLayout();

            // lblBookInfo
            lblBookInfo.AutoSize = true;
            lblBookInfo.Location = new System.Drawing.Point(20, 20);
            lblBookInfo.Name = "lblBookInfo";
            lblBookInfo.Size = new System.Drawing.Size(0, 15);

            // lblStudentInfo
            lblStudentInfo.AutoSize = true;
            lblStudentInfo.Location = new System.Drawing.Point(20, 50);
            lblStudentInfo.Name = "lblStudentInfo";
            lblStudentInfo.Size = new System.Drawing.Size(0, 15);

            // lblBorrowDate
            lblBorrowDate.AutoSize = true;
            lblBorrowDate.Location = new System.Drawing.Point(20, 80);
            lblBorrowDate.Name = "lblBorrowDate";
            lblBorrowDate.Size = new System.Drawing.Size(0, 15);

            // lblFineAmount
            lblFineAmount.AutoSize = true;
            lblFineAmount.Location = new System.Drawing.Point(20, 110);
            lblFineAmount.Name = "lblFineAmount";
            lblFineAmount.Size = new System.Drawing.Size(0, 15);

            // btnConfirmReturn
            btnConfirmReturn.Location = new System.Drawing.Point(60, 150);
            btnConfirmReturn.Name = "btnConfirmReturn";
            btnConfirmReturn.Size = new System.Drawing.Size(100, 30);
            btnConfirmReturn.Text = "Confirm Return";
            btnConfirmReturn.UseVisualStyleBackColor = true;
            btnConfirmReturn.Click += new EventHandler(btnConfirmReturn_Click);

            // btnCancel
            btnCancel.Location = new System.Drawing.Point(180, 150);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(100, 30);
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);

            // ReturnBookDetailsForm
            ClientSize = new System.Drawing.Size(350, 220);
            Controls.Add(lblBookInfo);
            Controls.Add(lblStudentInfo);
            Controls.Add(lblBorrowDate);
            Controls.Add(lblFineAmount);
            Controls.Add(btnConfirmReturn);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ReturnBookDetailsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Return Book Details";
            ResumeLayout(false);
            PerformLayout();
        }

        private void DisplayTransactionDetails()
        {
            lblBookInfo.Text = $"Book: {Transaction.Book?.Title ?? "N/A"}";
            lblStudentInfo.Text = $"Borrowed by: {Transaction.Student?.Name ?? "N/A"}";
            lblBorrowDate.Text = $"Borrow Date: {Transaction.BorrowDate.ToShortDateString()}";
            lblFineAmount.Text = $"Fine Amount: {Transaction.FineAmount?.ToString("C") ?? "N/A"}";
        }

        private void btnConfirmReturn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
} 