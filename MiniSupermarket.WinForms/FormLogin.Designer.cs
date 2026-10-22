namespace MiniSupermarket.WinForms
{
    partial class FormLogin
    {
        private System.ComponentModel.IContainer components = null;

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
            lblUser = new Label();
            txtUser = new TextBox();
            lblPass = new Label();
            txtPass = new TextBox();
            btnLogin = new Button();
            SuspendLayout();
            // 
            // lblUser
            // 
            lblUser.AutoSize = true;
            lblUser.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblUser.Location = new Point(45, 43);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(111, 19);
            lblUser.TabIndex = 0;
            lblUser.Text = "Tên đăng nhập:";
            // 
            // txtUser
            // 
            txtUser.Font = new Font("Segoe UI", 10.5F);
            txtUser.Location = new Point(45, 77);
            txtUser.Name = "txtUser";
            txtUser.PlaceholderText = "Nhập tài khoản";
            txtUser.Size = new Size(330, 26);
            txtUser.TabIndex = 1;
            // 
            // lblPass
            // 
            lblPass.AutoSize = true;
            lblPass.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPass.Location = new Point(45, 120);
            lblPass.Name = "lblPass";
            lblPass.Size = new Size(75, 19);
            lblPass.TabIndex = 2;
            lblPass.Text = "Mật khẩu:";
            // 
            // txtPass
            // 
            txtPass.Font = new Font("Segoe UI", 10.5F);
            txtPass.Location = new Point(45, 155);
            txtPass.Name = "txtPass";
            txtPass.PlaceholderText = "Nhập mật khẩu";
            txtPass.Size = new Size(330, 26);
            txtPass.TabIndex = 3;
            txtPass.UseSystemPasswordChar = true;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.RoyalBlue;
            btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(45, 206);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(330, 69);
            btnLogin.TabIndex = 4;
            btnLogin.Text = "Đăng nhập hệ thống";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // FormLogin
            // 
            AcceptButton = btnLogin;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(420, 310);
            Controls.Add(btnLogin);
            Controls.Add(txtPass);
            Controls.Add(lblPass);
            Controls.Add(txtUser);
            Controls.Add(lblUser);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập - MiniSupermarket System";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblUser;
        private TextBox txtUser;
        private Label lblPass;
        private TextBox txtPass;
        private Button btnLogin;
    }
}
