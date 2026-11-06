using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormMainShell : Form
    {
        // bien luu tru Form con dang duoc kich hoat hien thi
        private Form? _activeForm = null;

        public FormMainShell()
        {
            InitializeComponent();
        }

        private void FormMainShell_Load(object sender, EventArgs e)
        {
            // hien thi thong tin nguoi dung dang nhap
            string displayName = !string.IsNullOrWhiteSpace(SessionManager.CurrentFullName)
                ? SessionManager.CurrentFullName
                : SessionManager.CurrentUsername;

            lblUserInfo.Text = $"Nhân viên: {displayName} ({SessionManager.CurrentUsername}) | Vai trò: [{SessionManager.CurrentRole}]";

            // kich hoat phan quyen giao dien theo vai tro
            ApplyRolePermissions(SessionManager.CurrentRole);

            // mo man hinh mac dinh tuong ung voi vai tro
            OpenDefaultScreenByRole(SessionManager.CurrentRole);
        }

        public void OpenChildForm(Form childForm, string screenTitle, Button senderButton)
        {

            if (_activeForm != null)
            {
                _activeForm.Close();
                _activeForm.Dispose();
            }

            HighlightActiveButton(senderButton);

            _activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            panelMainContent.Controls.Clear();
            panelMainContent.Controls.Add(childForm);
            panelMainContent.Tag = childForm;

            lblTitle.Text = screenTitle;
            childForm.BringToFront();
            childForm.Show();
        }


        public void OpenChildForm(Form childForm, string screenTitle)
        {
            if (_activeForm != null)
            {
                _activeForm.Close();
                _activeForm.Dispose();
            }

            _activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            panelMainContent.Controls.Clear();
            panelMainContent.Controls.Add(childForm);
            panelMainContent.Tag = childForm;

            lblTitle.Text = screenTitle;
            childForm.BringToFront();
            childForm.Show();
        }
        private void HighlightActiveButton(Button activeButton)
        {
            foreach (Control ctrl in panelNavButtons.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.BackColor = Color.FromArgb(24, 30, 48);
                }
            }
            activeButton.BackColor = Color.FromArgb(41, 100, 180);
        }

        private void ApplyRolePermissions(string role)
        {
            switch (SessionManager.ToRoleKey(role))
            {
                case "ADMIN":
                    // qan tri vien: co toan quyen su dung tat ca cac nut
                    btnDashboard.Visible = true;
                    btnPOS.Visible = true;
                    btnCategory.Visible = true;
                    btnProduct.Visible = true;
                    btnCustomer.Visible = true;
                    btnReports.Visible = true;
                    btnUserManage.Visible = true;
                    break;

                case "CASHIER":
                    // thu ngan: ban hang POS, xem San pham / Khach hang (chi xem)
                    btnDashboard.Visible = false;
                    btnPOS.Visible = true;
                    btnCustomer.Visible = true;
                    btnCategory.Visible = false;
                    btnProduct.Visible = true;
                    btnReports.Visible = true;
                    btnUserManage.Visible = false;
                    break;

                case "WAREHOUSE":
                    // thu kho: chi quan ly Danh muc nhom hang va San pham ton kho
                    btnDashboard.Visible = false;
                    btnPOS.Visible = false;
                    btnCustomer.Visible = false;
                    btnReports.Visible = false;
                    btnUserManage.Visible = false;
                    btnCategory.Visible = true;
                    btnProduct.Visible = true;
                    break;

                default:
                    // vai tro khong xac dinh: khoa toan bo
                    MessageBox.Show("Tài khoản chưa được cấp quyền hạn hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    break;
            }
        }

        private void OpenDefaultScreenByRole(string role)
        {
            switch (SessionManager.ToRoleKey(role))
            {
                case "ADMIN":
                    OpenChildForm(new FormDashboard(), "DASHBOARD TỔNG QUAN KINH DOANH", btnDashboard);
                    break;

                case "WAREHOUSE":
                    OpenChildForm(new FormProductManagement(), "QUẢN LÝ THÔNG TIN SẢN PHẨM & KHO HÀNG", btnProduct);
                    break;

                case "CASHIER":
                    OpenChildForm(new FormPOS(), "HỆ THỐNG QUẦY BÁN HÀNG & THU NGÂN (POS)", btnPOS);
                    break;
            }
        }

        // sidebar

        // dashboard tong quan (Chi Admin)
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            if (!SessionManager.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền xem dữ liệu tài chính của siêu thị!", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            OpenChildForm(new FormDashboard(), "DASHBOARD TỔNG QUAN KINH DOANH", btnDashboard);
        }

        // quan ly Danh muc (Admin & Warehouse)
        private void btnCategory_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormCategoryManagement(), "QUẢN LÝ DANH MỤC SẢN PHẨM", btnCategory);
        }

        // ban hang POS (Admin & Cashier)
        private void btnPOS_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormPOS(), "HỆ THỐNG QUẦY BÁN HÀNG & THU NGÂN (POS)", btnPOS);
        }

        // quan ly San pham (Admin & Warehouse)
        private void btnProduct_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormProductManagement(), "QUẢN LÝ THÔNG TIN SẢN PHẨM & KHO HÀNG", btnProduct);
        }

        // quan ly Khach hang (Admin & Cashier)
        private void btnCustomer_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormCustomerManagement(), "QUẢN LÝ KHÁCH HÀNG & TÍCH ĐIỂM", btnCustomer);
        }

        // bao cao Doanh thu (Chi Admin)
        private void btnReports_Click(object sender, EventArgs e)
        {
            // kiem tra bao ve cap Action
            if (!SessionManager.IsAdmin && !SessionManager.IsCashier)
            {
                MessageBox.Show("Bạn không có quyền xem dữ liệu tài chính của siêu thị!", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            OpenChildForm(new FormQuickReport(), "BÁO CÁO DOANH THU & HIỆU SUẤT CA BÁN", btnReports);
        }

        // quan tri Tai khoan Nguoi dung (Chi Admin)
        private void btnUserManage_Click(object sender, EventArgs e)
        {
            // kiem tra bao ve cap Action
            if (!SessionManager.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền quản trị tài khoản!", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            OpenChildForm(new FormUserManagement(), "QUẢN TRỊ TÀI KHOẢN VÀ PHÂN QUYỀN HỆ THỐNG", btnUserManage);
        }

        // dang xuat phien lam viec
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
