namespace MiniSupermarket.WinForms
{
    partial class FormMainShell
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
            panelSidebar = new Panel();
            panelNavButtons = new Panel();
            btnUserManage = new Button();
            btnReports = new Button();
            btnCustomer = new Button();
            btnProduct = new Button();
            btnCategory = new Button();
            btnPOS = new Button();
            panelUserFooter = new Panel();
            btnLogout = new Button();
            panelLogo = new Panel();
            lblLogoSub = new Label();
            lblLogo = new Label();
            panelTopHeader = new Panel();
            lblUserInfo = new Label();
            lblTitle = new Label();
            panelMainContent = new Panel();
            panelSidebar.SuspendLayout();
            panelNavButtons.SuspendLayout();
            panelUserFooter.SuspendLayout();
            panelLogo.SuspendLayout();
            panelTopHeader.SuspendLayout();
            SuspendLayout();
            // 
            // panelSidebar
            // 
            panelSidebar.BackColor = Color.FromArgb(24, 30, 48);
            panelSidebar.Controls.Add(panelNavButtons);
            panelSidebar.Controls.Add(panelUserFooter);
            panelSidebar.Controls.Add(panelLogo);
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Location = new Point(0, 0);
            panelSidebar.Name = "panelSidebar";
            panelSidebar.Size = new Size(240, 720);
            panelSidebar.TabIndex = 0;
            // 
            // panelNavButtons
            // 
            panelNavButtons.AutoScroll = true;
            panelNavButtons.BackColor = Color.Gray;
            panelNavButtons.Controls.Add(btnUserManage);
            panelNavButtons.Controls.Add(btnReports);
            panelNavButtons.Controls.Add(btnCustomer);
            panelNavButtons.Controls.Add(btnProduct);
            panelNavButtons.Controls.Add(btnCategory);
            panelNavButtons.Controls.Add(btnPOS);
            panelNavButtons.Dock = DockStyle.Fill;
            panelNavButtons.Location = new Point(0, 75);
            panelNavButtons.Name = "panelNavButtons";
            panelNavButtons.Padding = new Padding(0, 10, 0, 0);
            panelNavButtons.Size = new Size(240, 580);
            panelNavButtons.TabIndex = 1;
            // 
            // btnUserManage
            // 
            btnUserManage.Cursor = Cursors.Hand;
            btnUserManage.Dock = DockStyle.Top;
            btnUserManage.FlatAppearance.BorderSize = 0;
            btnUserManage.FlatStyle = FlatStyle.Flat;
            btnUserManage.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnUserManage.ForeColor = Color.White;
            btnUserManage.Location = new Point(0, 255);
            btnUserManage.Name = "btnUserManage";
            btnUserManage.Padding = new Padding(15, 0, 0, 0);
            btnUserManage.Size = new Size(240, 49);
            btnUserManage.TabIndex = 5;
            btnUserManage.Text = "🛡  Quản trị Tài khoản";
            btnUserManage.TextAlign = ContentAlignment.MiddleLeft;
            btnUserManage.UseVisualStyleBackColor = true;
            btnUserManage.Click += btnUserManage_Click;
            // 
            // btnReports
            // 
            btnReports.Cursor = Cursors.Hand;
            btnReports.Dock = DockStyle.Top;
            btnReports.FlatAppearance.BorderSize = 0;
            btnReports.FlatStyle = FlatStyle.Flat;
            btnReports.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnReports.ForeColor = Color.White;
            btnReports.Location = new Point(0, 206);
            btnReports.Name = "btnReports";
            btnReports.Padding = new Padding(15, 0, 0, 0);
            btnReports.Size = new Size(240, 49);
            btnReports.TabIndex = 4;
            btnReports.Text = "📊  Báo cáo Doanh thu";
            btnReports.TextAlign = ContentAlignment.MiddleLeft;
            btnReports.UseVisualStyleBackColor = true;
            btnReports.Click += btnReports_Click;
            // 
            // btnCustomer
            // 
            btnCustomer.Cursor = Cursors.Hand;
            btnCustomer.Dock = DockStyle.Top;
            btnCustomer.FlatAppearance.BorderSize = 0;
            btnCustomer.FlatStyle = FlatStyle.Flat;
            btnCustomer.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnCustomer.ForeColor = Color.White;
            btnCustomer.Location = new Point(0, 157);
            btnCustomer.Name = "btnCustomer";
            btnCustomer.Padding = new Padding(15, 0, 0, 0);
            btnCustomer.Size = new Size(240, 49);
            btnCustomer.TabIndex = 3;
            btnCustomer.Text = "👥  Quản lý Khách hàng";
            btnCustomer.TextAlign = ContentAlignment.MiddleLeft;
            btnCustomer.UseVisualStyleBackColor = true;
            btnCustomer.Click += btnCustomer_Click;
            // 
            // btnProduct
            // 
            btnProduct.Cursor = Cursors.Hand;
            btnProduct.Dock = DockStyle.Top;
            btnProduct.FlatAppearance.BorderSize = 0;
            btnProduct.FlatStyle = FlatStyle.Flat;
            btnProduct.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnProduct.ForeColor = Color.White;
            btnProduct.Location = new Point(0, 108);
            btnProduct.Name = "btnProduct";
            btnProduct.Padding = new Padding(15, 0, 0, 0);
            btnProduct.Size = new Size(240, 49);
            btnProduct.TabIndex = 2;
            btnProduct.Text = "📦  Quản lý Sản phẩm";
            btnProduct.TextAlign = ContentAlignment.MiddleLeft;
            btnProduct.UseVisualStyleBackColor = true;
            btnProduct.Click += btnProduct_Click;
            // 
            // btnCategory
            // 
            btnCategory.Cursor = Cursors.Hand;
            btnCategory.Dock = DockStyle.Top;
            btnCategory.FlatAppearance.BorderSize = 0;
            btnCategory.FlatStyle = FlatStyle.Flat;
            btnCategory.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnCategory.ForeColor = Color.White;
            btnCategory.Location = new Point(0, 59);
            btnCategory.Name = "btnCategory";
            btnCategory.Padding = new Padding(15, 0, 0, 0);
            btnCategory.Size = new Size(240, 49);
            btnCategory.TabIndex = 1;
            btnCategory.Text = "📁  Quản lý Danh mục";
            btnCategory.TextAlign = ContentAlignment.MiddleLeft;
            btnCategory.UseVisualStyleBackColor = true;
            btnCategory.Click += btnCategory_Click;
            // 
            // btnPOS
            // 
            btnPOS.Cursor = Cursors.Hand;
            btnPOS.Dock = DockStyle.Top;
            btnPOS.FlatAppearance.BorderSize = 0;
            btnPOS.FlatStyle = FlatStyle.Flat;
            btnPOS.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnPOS.ForeColor = Color.White;
            btnPOS.Location = new Point(0, 10);
            btnPOS.Name = "btnPOS";
            btnPOS.Padding = new Padding(15, 0, 0, 0);
            btnPOS.Size = new Size(240, 49);
            btnPOS.TabIndex = 0;
            btnPOS.Text = "\U0001f6d2  Bán hàng (POS)";
            btnPOS.TextAlign = ContentAlignment.MiddleLeft;
            btnPOS.UseVisualStyleBackColor = true;
            btnPOS.Click += btnPOS_Click;
            // 
            // panelUserFooter
            // 
            panelUserFooter.BackColor = Color.FromArgb(0, 192, 192);
            panelUserFooter.Controls.Add(btnLogout);
            panelUserFooter.Dock = DockStyle.Bottom;
            panelUserFooter.Location = new Point(0, 655);
            panelUserFooter.Name = "panelUserFooter";
            panelUserFooter.Padding = new Padding(12);
            panelUserFooter.Size = new Size(240, 65);
            panelUserFooter.TabIndex = 2;
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.FromArgb(220, 53, 69);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Dock = DockStyle.Fill;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(12, 12);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(216, 41);
            btnLogout.TabIndex = 0;
            btnLogout.Text = "🚪  Đăng xuất";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // panelLogo
            // 
            panelLogo.BackColor = Color.FromArgb(0, 192, 192);
            panelLogo.Controls.Add(lblLogoSub);
            panelLogo.Controls.Add(lblLogo);
            panelLogo.Dock = DockStyle.Top;
            panelLogo.Location = new Point(0, 0);
            panelLogo.Name = "panelLogo";
            panelLogo.Size = new Size(240, 75);
            panelLogo.TabIndex = 0;
            // 
            // lblLogoSub
            // 
            lblLogoSub.AutoSize = true;
            lblLogoSub.Font = new Font("Segoe UI", 8F);
            lblLogoSub.ForeColor = Color.FromArgb(150, 160, 180);
            lblLogoSub.Location = new Point(18, 45);
            lblLogoSub.Name = "lblLogoSub";
            lblLogoSub.Size = new Size(158, 13);
            lblLogoSub.TabIndex = 1;
            lblLogoSub.Text = "Hệ thống Quản lý Bán lẻ Mini";
            // 
            // lblLogo
            // 
            lblLogo.AutoSize = true;
            lblLogo.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            lblLogo.ForeColor = Color.White;
            lblLogo.Location = new Point(15, 18);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new Size(160, 25);
            lblLogo.TabIndex = 0;
            lblLogo.Text = "\U0001f6d2 Mini ZenMart";
            // 
            // panelTopHeader
            // 
            panelTopHeader.BackColor = Color.White;
            panelTopHeader.Controls.Add(lblUserInfo);
            panelTopHeader.Controls.Add(lblTitle);
            panelTopHeader.Dock = DockStyle.Top;
            panelTopHeader.Location = new Point(240, 0);
            panelTopHeader.Name = "panelTopHeader";
            panelTopHeader.Size = new Size(1040, 60);
            panelTopHeader.TabIndex = 1;
            // 
            // lblUserInfo
            // 
            lblUserInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblUserInfo.Font = new Font("Segoe UI", 10F);
            lblUserInfo.ForeColor = Color.FromArgb(70, 80, 95);
            lblUserInfo.Location = new Point(480, 19);
            lblUserInfo.Name = "lblUserInfo";
            lblUserInfo.Size = new Size(540, 23);
            lblUserInfo.TabIndex = 1;
            lblUserInfo.Text = "Nhân viên: ... | Vai trò: [...]";
            lblUserInfo.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(24, 30, 48);
            lblTitle.Location = new Point(20, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(239, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "BÀN LÀM VIỆC HỆ THỐNG";
            // 
            // panelMainContent
            // 
            panelMainContent.BackColor = Color.FromArgb(244, 245, 247);
            panelMainContent.Dock = DockStyle.Fill;
            panelMainContent.Location = new Point(240, 60);
            panelMainContent.Name = "panelMainContent";
            panelMainContent.Size = new Size(1040, 660);
            panelMainContent.TabIndex = 2;
            // 
            // FormMainShell
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 720);
            Controls.Add(panelMainContent);
            Controls.Add(panelTopHeader);
            Controls.Add(panelSidebar);
            Font = new Font("Segoe UI", 9.75F);
            MinimumSize = new Size(1100, 650);
            Name = "FormMainShell";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hệ thống Quản lý Bán lẻ & Tồn kho Siêu thị Mini";
            Load += FormMainShell_Load;
            panelSidebar.ResumeLayout(false);
            panelNavButtons.ResumeLayout(false);
            panelUserFooter.ResumeLayout(false);
            panelLogo.ResumeLayout(false);
            panelLogo.PerformLayout();
            panelTopHeader.ResumeLayout(false);
            panelTopHeader.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelSidebar;
        private Panel panelLogo;
        private Label lblLogo;
        private Label lblLogoSub;
        private Panel panelNavButtons;
        private Button btnPOS;
        private Button btnCategory;
        private Button btnProduct;
        private Button btnCustomer;
        private Button btnReports;
        private Button btnUserManage;
        private Panel panelUserFooter;
        private Button btnLogout;
        private Panel panelTopHeader;
        private Label lblTitle;
        private Label lblUserInfo;
        private Panel panelMainContent;
    }
}
