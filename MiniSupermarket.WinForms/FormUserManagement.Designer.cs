namespace MiniSupermarket.WinForms
{
    partial class FormUserManagement
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            panelGrid = new Panel();
            dgvUsers = new DataGridView();
            panelDetails = new Panel();
            groupActions = new GroupBox();
            btnRefresh = new Button();
            btnToggleLock = new Button();
            btnResetPassword = new Button();
            btnAddUser = new Button();
            groupUserDetails = new GroupBox();
            cboRole = new ComboBox();
            lblRole = new Label();
            txtFullName = new TextBox();
            lblFullName = new Label();
            txtPassword = new TextBox();
            lblPassword = new Label();
            txtUsername = new TextBox();
            lblUsername = new Label();
            txtId = new TextBox();
            lblId = new Label();
            panelHeaderInfo = new Panel();
            lblHeaderTitle = new Label();
            panelGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            panelDetails.SuspendLayout();
            groupActions.SuspendLayout();
            groupUserDetails.SuspendLayout();
            panelHeaderInfo.SuspendLayout();
            SuspendLayout();
            // 
            // panelGrid
            // 
            panelGrid.Controls.Add(dgvUsers);
            panelGrid.Dock = DockStyle.Fill;
            panelGrid.Location = new Point(0, 55);
            panelGrid.Name = "panelGrid";
            panelGrid.Padding = new Padding(12);
            panelGrid.Size = new Size(690, 605);
            panelGrid.TabIndex = 1;
            // 
            // dgvUsers
            // 
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.BackgroundColor = Color.White;
            dgvUsers.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(24, 30, 48);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvUsers.ColumnHeadersHeight = 35;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.75F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvUsers.DefaultCellStyle = dataGridViewCellStyle2;
            dgvUsers.Dock = DockStyle.Fill;
            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.Location = new Point(12, 12);
            dgvUsers.MultiSelect = false;
            dgvUsers.Name = "dgvUsers";
            dgvUsers.ReadOnly = true;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.RowTemplate.Height = 32;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.Size = new Size(666, 581);
            dgvUsers.TabIndex = 0;
            dgvUsers.CellClick += dgvUsers_CellClick;
            // 
            // panelDetails
            // 
            panelDetails.BackColor = Color.White;
            panelDetails.BorderStyle = BorderStyle.FixedSingle;
            panelDetails.Controls.Add(groupActions);
            panelDetails.Controls.Add(groupUserDetails);
            panelDetails.Dock = DockStyle.Right;
            panelDetails.Location = new Point(690, 55);
            panelDetails.Name = "panelDetails";
            panelDetails.Padding = new Padding(12);
            panelDetails.Size = new Size(350, 605);
            panelDetails.TabIndex = 2;
            // 
            // groupActions
            // 
            groupActions.Controls.Add(btnRefresh);
            groupActions.Controls.Add(btnToggleLock);
            groupActions.Controls.Add(btnResetPassword);
            groupActions.Controls.Add(btnAddUser);
            groupActions.Dock = DockStyle.Bottom;
            groupActions.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            groupActions.Location = new Point(12, 385);
            groupActions.Name = "groupActions";
            groupActions.Padding = new Padding(10);
            groupActions.Size = new Size(324, 206);
            groupActions.TabIndex = 1;
            groupActions.TabStop = false;
            groupActions.Text = "Hành động quản trị";
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(108, 117, 125);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(166, 113);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(146, 78);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "🔄 Làm mới danh sách";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnToggleLock
            // 
            btnToggleLock.BackColor = Color.Red;
            btnToggleLock.Cursor = Cursors.Hand;
            btnToggleLock.FlatAppearance.BorderSize = 0;
            btnToggleLock.FlatStyle = FlatStyle.Flat;
            btnToggleLock.ForeColor = Color.White;
            btnToggleLock.Location = new Point(12, 113);
            btnToggleLock.Name = "btnToggleLock";
            btnToggleLock.Size = new Size(148, 78);
            btnToggleLock.TabIndex = 2;
            btnToggleLock.Text = "🔒 Khóa / Mở khóa tài khoản";
            btnToggleLock.UseVisualStyleBackColor = false;
            btnToggleLock.Click += btnToggleLock_Click;
            // 
            // btnResetPassword
            // 
            btnResetPassword.BackColor = Color.FromArgb(192, 64, 0);
            btnResetPassword.Cursor = Cursors.Hand;
            btnResetPassword.FlatAppearance.BorderSize = 0;
            btnResetPassword.FlatStyle = FlatStyle.Flat;
            btnResetPassword.ForeColor = Color.White;
            btnResetPassword.Location = new Point(166, 29);
            btnResetPassword.Name = "btnResetPassword";
            btnResetPassword.Size = new Size(146, 78);
            btnResetPassword.TabIndex = 1;
            btnResetPassword.Text = "🔑 Đặt lại mật khẩu";
            btnResetPassword.UseVisualStyleBackColor = false;
            btnResetPassword.Click += btnResetPassword_Click;
            // 
            // btnAddUser
            // 
            btnAddUser.BackColor = Color.FromArgb(0, 192, 192);
            btnAddUser.Cursor = Cursors.Hand;
            btnAddUser.FlatAppearance.BorderSize = 0;
            btnAddUser.FlatStyle = FlatStyle.Flat;
            btnAddUser.ForeColor = Color.White;
            btnAddUser.Location = new Point(13, 29);
            btnAddUser.Name = "btnAddUser";
            btnAddUser.Size = new Size(147, 78);
            btnAddUser.TabIndex = 0;
            btnAddUser.Text = "➕ Tạo tài khoản mới";
            btnAddUser.UseVisualStyleBackColor = false;
            btnAddUser.Click += btnAddUser_Click;
            // 
            // groupUserDetails
            // 
            groupUserDetails.Controls.Add(cboRole);
            groupUserDetails.Controls.Add(lblRole);
            groupUserDetails.Controls.Add(txtFullName);
            groupUserDetails.Controls.Add(lblFullName);
            groupUserDetails.Controls.Add(txtPassword);
            groupUserDetails.Controls.Add(lblPassword);
            groupUserDetails.Controls.Add(txtUsername);
            groupUserDetails.Controls.Add(lblUsername);
            groupUserDetails.Controls.Add(txtId);
            groupUserDetails.Controls.Add(lblId);
            groupUserDetails.Dock = DockStyle.Top;
            groupUserDetails.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            groupUserDetails.Location = new Point(12, 12);
            groupUserDetails.Name = "groupUserDetails";
            groupUserDetails.Padding = new Padding(10);
            groupUserDetails.Size = new Size(324, 355);
            groupUserDetails.TabIndex = 0;
            groupUserDetails.TabStop = false;
            groupUserDetails.Text = "Thông tin tài khoản";
            // 
            // cboRole
            // 
            cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRole.Font = new Font("Segoe UI", 9.5F);
            cboRole.FormattingEnabled = true;
            cboRole.Location = new Point(12, 295);
            cboRole.Name = "cboRole";
            cboRole.Size = new Size(300, 25);
            cboRole.TabIndex = 9;
            // 
            // lblRole
            // 
            lblRole.AutoSize = true;
            lblRole.Font = new Font("Segoe UI", 9F);
            lblRole.ForeColor = Color.FromArgb(70, 80, 95);
            lblRole.Location = new Point(12, 275);
            lblRole.Name = "lblRole";
            lblRole.Size = new Size(109, 15);
            lblRole.TabIndex = 8;
            lblRole.Text = "Vai trò phân quyền:";
            // 
            // txtFullName
            // 
            txtFullName.Font = new Font("Segoe UI", 9.5F);
            txtFullName.Location = new Point(12, 235);
            txtFullName.Name = "txtFullName";
            txtFullName.PlaceholderText = "Nhập họ tên nhân viên...";
            txtFullName.Size = new Size(300, 24);
            txtFullName.TabIndex = 7;
            // 
            // lblFullName
            // 
            lblFullName.AutoSize = true;
            lblFullName.Font = new Font("Segoe UI", 9F);
            lblFullName.ForeColor = Color.FromArgb(70, 80, 95);
            lblFullName.Location = new Point(12, 215);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(101, 15);
            lblFullName.TabIndex = 6;
            lblFullName.Text = "Họ tên nhân viên:";
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 9.5F);
            txtPassword.Location = new Point(12, 175);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Mật khẩu ban đầu...";
            txtPassword.Size = new Size(300, 24);
            txtPassword.TabIndex = 5;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 9F);
            lblPassword.ForeColor = Color.FromArgb(70, 80, 95);
            lblPassword.Location = new Point(12, 155);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(60, 15);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Mật khẩu:";
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Segoe UI", 9.5F);
            txtUsername.Location = new Point(12, 115);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Tên tài khoản đăng nhập...";
            txtUsername.Size = new Size(300, 24);
            txtUsername.TabIndex = 3;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 9F);
            lblUsername.ForeColor = Color.FromArgb(70, 80, 95);
            lblUsername.Location = new Point(12, 95);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(88, 15);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Tên đăng nhập:";
            // 
            // txtId
            // 
            txtId.BackColor = Color.FromArgb(240, 240, 240);
            txtId.Font = new Font("Segoe UI", 9.5F);
            txtId.Location = new Point(12, 55);
            txtId.Name = "txtId";
            txtId.ReadOnly = true;
            txtId.Size = new Size(300, 24);
            txtId.TabIndex = 1;
            // 
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.Font = new Font("Segoe UI", 9F);
            lblId.ForeColor = Color.FromArgb(70, 80, 95);
            lblId.Location = new Point(12, 35);
            lblId.Name = "lblId";
            lblId.Size = new Size(21, 15);
            lblId.TabIndex = 0;
            lblId.Text = "ID:";
            // 
            // panelHeaderInfo
            // 
            panelHeaderInfo.BackColor = Color.White;
            panelHeaderInfo.BorderStyle = BorderStyle.FixedSingle;
            panelHeaderInfo.Controls.Add(lblHeaderTitle);
            panelHeaderInfo.Dock = DockStyle.Top;
            panelHeaderInfo.Location = new Point(0, 0);
            panelHeaderInfo.Name = "panelHeaderInfo";
            panelHeaderInfo.Padding = new Padding(15, 10, 15, 10);
            panelHeaderInfo.Size = new Size(1040, 55);
            panelHeaderInfo.TabIndex = 0;
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.FromArgb(24, 30, 48);
            lblHeaderTitle.Location = new Point(15, 16);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(278, 20);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "DANH SÁCH TÀI KHOẢN NGƯỜI DÙNG";
            // 
            // FormUserManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 245, 247);
            ClientSize = new Size(1040, 660);
            Controls.Add(panelGrid);
            Controls.Add(panelDetails);
            Controls.Add(panelHeaderInfo);
            Font = new Font("Segoe UI", 9.75F);
            Name = "FormUserManagement";
            Text = "FormUserManagement";
            Load += FormUserManagement_Load;
            panelGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            panelDetails.ResumeLayout(false);
            groupActions.ResumeLayout(false);
            groupUserDetails.ResumeLayout(false);
            groupUserDetails.PerformLayout();
            panelHeaderInfo.ResumeLayout(false);
            panelHeaderInfo.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeaderInfo;
        private Label lblHeaderTitle;
        private Panel panelGrid;
        private DataGridView dgvUsers;
        private Panel panelDetails;
        private GroupBox groupUserDetails;
        private TextBox txtId;
        private Label lblId;
        private TextBox txtUsername;
        private Label lblUsername;
        private TextBox txtPassword;
        private Label lblPassword;
        private TextBox txtFullName;
        private Label lblFullName;
        private ComboBox cboRole;
        private Label lblRole;
        private GroupBox groupActions;
        private Button btnAddUser;
        private Button btnResetPassword;
        private Button btnToggleLock;
        private Button btnRefresh;
    }
}
