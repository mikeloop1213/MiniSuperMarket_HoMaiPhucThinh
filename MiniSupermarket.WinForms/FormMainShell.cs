using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormMainShell : Form
    {
        // Biến lưu trữ Form con đang được kích hoạt hiển thị
        private Form? _activeForm = null;

        public FormMainShell()
        {
            InitializeComponent();
        }

        private void FormMainShell_Load(object sender, EventArgs e)
        {
            // 1. Hiển thị thông tin phiên người dùng đăng nhập
            string displayName = !string.IsNullOrWhiteSpace(SessionManager.CurrentFullName) 
                ? SessionManager.CurrentFullName 
                : SessionManager.CurrentUsername;

            lblUserInfo.Text = $"Nhân viên: {displayName} ({SessionManager.CurrentUsername}) | Vai trò: [{SessionManager.CurrentRole}]";

            // 2. Kích hoạt phân quyền giao diện theo vai trò (Role-Based Access)
            ApplyRolePermissions(SessionManager.CurrentRole);

            // 3. Mở màn hình mặc định tương ứng với vai trò
            OpenDefaultScreenByRole(SessionManager.CurrentRole);
        }

        /// <summary>
        /// Hàm nhúng động một Form con vào vùng panelMainContent
        /// </summary>
        public void OpenChildForm(Form childForm, string screenTitle, Button senderButton)
        {
            // Nếu form con đang mở cùng loại, không cần khởi tạo lại
            if (_activeForm != null)
            {
                _activeForm.Close();
                _activeForm.Dispose();
            }

            HighlightActiveButton(senderButton);

            _activeForm = childForm;
            childForm.TopLevel = false;                          // Bỏ thuộc tính cửa sổ độc lập cấp cao nhất
            childForm.FormBorderStyle = FormBorderStyle.None;     // Bỏ viền và thanh điều khiển Windows
            childForm.Dock = DockStyle.Fill;                     // Tràn toàn bộ vùng panelMainContent

            panelMainContent.Controls.Clear();                   // Dọn dẹp màn hình cũ
            panelMainContent.Controls.Add(childForm);            // Thêm form mới vào vùng chứa
            panelMainContent.Tag = childForm;

            lblTitle.Text = screenTitle;                         // Đồng bộ tên màn hình trên Header
            childForm.BringToFront();
            childForm.Show();
        }

        /// <summary>
        /// Làm nổi bật nút menu bên Sidebar đang được chọn
        /// </summary>
        private void HighlightActiveButton(Button activeButton)
        {
            foreach (Control ctrl in panelNavButtons.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.BackColor = Color.FromArgb(24, 30, 48); // Màu nền gốc sidebar
                }
            }
            activeButton.BackColor = Color.FromArgb(41, 100, 180); // Màu xanh active
        }

        /// <summary>
        /// Phân định quyền truy cập hiển thị theo vai trò người dùng (RBAC)
        /// </summary>
        private void ApplyRolePermissions(string role)
        {
            switch (role.ToUpper())
            {
                case "ADMIN":
                    // Quản trị viên: Có toàn quyền sử dụng tất cả các nút
                    btnPOS.Visible = true;
                    btnCategory.Visible = true;
                    btnProduct.Visible = true;
                    btnCustomer.Visible = true;
                    btnReports.Visible = true;
                    btnUserManage.Visible = true;
                    break;

                case "CASHIER":
                    // Thu ngân: Chỉ truy cập màn hình Bán hàng (POS) và Khách hàng
                    btnPOS.Visible = true;
                    btnCustomer.Visible = true;
                    btnCategory.Visible = false;
                    btnProduct.Visible = false;
                    btnReports.Visible = false;
                    btnUserManage.Visible = false;
                    break;

                case "WAREHOUSE":
                    // Thủ kho: Chỉ quản lý Danh mục nhóm hàng và Sản phẩm tồn kho
                    btnPOS.Visible = false;
                    btnCustomer.Visible = false;
                    btnReports.Visible = false;
                    btnUserManage.Visible = false;
                    btnCategory.Visible = true;
                    btnProduct.Visible = true;
                    break;

                default:
                    // Vai trò không xác định: Khóa toàn bộ
                    MessageBox.Show("Tài khoản chưa được cấp quyền hạn hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    break;
            }
        }

        /// <summary>
        /// Điều hướng ngay vào màn hình đúng chuyên môn của từng vai trò
        /// </summary>
        private void OpenDefaultScreenByRole(string role)
        {
            switch (role.ToUpper())
            {
                case "ADMIN":
                    OpenChildForm(new FormCategoryManagement(), "QUẢN LÝ DANH MỤC SẢN PHẨM", btnCategory);
                    break;

                case "WAREHOUSE":
                    OpenChildForm(new FormProductManagement(), "QUẢN LÝ THÔNG TIN SẢN PHẨM & KHO HÀNG", btnProduct);
                    break;

                case "CASHIER":
                    OpenChildForm(new FormPOS(), "HỆ THỐNG QUẦY BÁN HÀNG & THU NGÂN (POS)", btnPOS);
                    break;
            }
        }

        // ================= CÁC SỰ KIỆN CLICK NÚT TRÊN SIDEBAR =================

        // 1. Quản lý Danh mục (Admin & Warehouse)
        private void btnCategory_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormCategoryManagement(), "QUẢN LÝ DANH MỤC SẢN PHẨM", btnCategory);
        }

        // 2. Bán hàng POS (Admin & Cashier)
        private void btnPOS_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormPOS(), "HỆ THỐNG QUẦY BÁN HÀNG & THU NGÂN (POS)", btnPOS);
        }

        // 3. Quản lý Sản phẩm (Admin & Warehouse)
        private void btnProduct_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormProductManagement(), "QUẢN LÝ THÔNG TIN SẢN PHẨM & KHO HÀNG", btnProduct);
        }

        // 4. Quản lý Khách hàng (Admin & Cashier)
        private void btnCustomer_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormCustomerManagement(), "QUẢN LÝ KHÁCH HÀNG & TÍCH ĐIỂM", btnCustomer);
        }

        // 5. Báo cáo Doanh thu (Chỉ Admin)
        private void btnReports_Click(object sender, EventArgs e)
        {
            // Kiểm tra bảo vệ cấp Action (Double protection)
            if (!SessionManager.CurrentRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Bạn không có quyền xem dữ liệu tài chính của siêu thị!", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            OpenChildForm(new FormQuickReport(), "BÁO CÁO DOANH THU & HIỆU SUẤT CA BÁN", btnReports);
        }

        // 6. Quản trị Tài khoản Người dùng (Chỉ Admin)
        private void btnUserManage_Click(object sender, EventArgs e)
        {
            // Kiểm tra bảo vệ cấp Action
            if (!SessionManager.CurrentRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Bạn không có quyền quản trị tài khoản!", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            OpenChildForm(new FormUserManagement(), "QUẢN TRỊ TÀI KHOẢN VÀ PHÂN QUYỀN HỆ THỐNG", btnUserManage);
        }

        // 7. Đăng xuất phiên làm việc
        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất phiên làm việc?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                SessionManager.ClearSession();

                this.Hide();
                var loginForm = new FormLogin();
                loginForm.ShowDialog();
                this.Close();
            }
        }
    }
}
