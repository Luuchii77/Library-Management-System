namespace LibraryManagementSystem.Forms
{
    partial class AddStudentForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // 👉 Field Declarations
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtReferenceId;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblReferenceId;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPhone;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtReferenceId = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblReferenceId = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Text = "Name:";
            this.lblName.Location = new System.Drawing.Point(30, 30);
            this.lblName.AutoSize = true;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(150, 30);
            this.txtName.Width = 200;
            // 
            // lblReferenceId
            // 
            this.lblReferenceId.Text = "Reference ID:";
            this.lblReferenceId.Location = new System.Drawing.Point(30, 70);
            this.lblReferenceId.AutoSize = true;
            // 
            // txtReferenceId
            // 
            this.txtReferenceId.Location = new System.Drawing.Point(150, 70);
            this.txtReferenceId.Width = 200;
            // 
            // lblEmail
            // 
            this.lblEmail.Text = "Email (optional):";
            this.lblEmail.Location = new System.Drawing.Point(30, 110);
            this.lblEmail.AutoSize = true;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(150, 110);
            this.txtEmail.Width = 200;
            // 
            // lblPhone
            // 
            this.lblPhone.Text = "Phone (optional):";
            this.lblPhone.Location = new System.Drawing.Point(30, 150);
            this.lblPhone.AutoSize = true;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(150, 150);
            this.txtPhone.Width = 200;
            // 
            // btnSave
            // 
            this.btnSave.Text = "Save";
            this.btnSave.Location = new System.Drawing.Point(150, 200);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // AddStudentForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(400, 270);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblReferenceId);
            this.Controls.Add(this.txtReferenceId);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.btnSave);
            this.Name = "AddStudentForm";
            this.Text = "Add Student";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
