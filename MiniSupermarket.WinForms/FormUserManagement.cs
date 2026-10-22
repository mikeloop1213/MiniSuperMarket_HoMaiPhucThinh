using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormUserManagement : Form
    {
        public FormUserManagement()
        {
            InitializeComponent();
            SetupUsersGrid();
            cboRole.Items.AddRange(new string[] { "Admin", "Cashier", "Warehouse" });
            cboRole.SelectedIndex = 1; // Default Cashier
        }

        private void SetupUsersGrid()
        {
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.Columns.Clear();

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "Mã",
                Width = 60
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Username",
                HeaderText = "Tên đăng nhập",
                Width = 140
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FullName",
                HeaderText = "Họ và tên nhân viên",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Role",
                HeaderText = "Vai trò (Role)",
                Width = 120
            });

            var colStatus = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IsActive",
                HeaderText = "Trạng thái",
                Width = 130
            };
            dgvUsers.Columns.Add(colStatus);

            dgvUsers.CellFormatting += (s, e) =>
            {
                if (dgvUsers.Columns[e.ColumnIndex].DataPropertyName == "IsActive" && e.Value is bool isActive)
                {
                    e.Value = isActive ? "🟢 Hoạt động" : "🔴 Đã khóa";
                    e.FormattingApplied = true;
                }
            };
        }

        private async void FormUserManagement_Load(object sender, EventArgs e)
        {
            await LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                var users = await ApiClientService.Client.GetFromJsonAsync<List<UserDto>>("users");
                dgvUsers.DataSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy danh sách tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvUsers.Rows[e.RowIndex].DataBoundItem is UserDto user)
            {
                txtId.Text = user.Id.ToString();
                txtUsername.Text = user.Username;
                txtFullName.Text = user.FullName;
                txtPassword.Clear();
                int roleIdx = cboRole.Items.IndexOf(user.Role);
                if (roleIdx >= 0)
                {
                    cboRole.SelectedIndex = roleIdx;
                }
            }
        }

        private async void btnAddUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Tên đăng nhập và mật khẩu không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newUser = new
            {
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                FullName = string.IsNullOrWhiteSpace(txtFullName.Text) ? txtUsername.Text.Trim() : txtFullName.Text.Trim(),
                Role = cboRole.SelectedItem?.ToString() ?? "Cashier"
            };

            try
            {
                var res = await ApiClientService.Client.PostAsJsonAsync("users", newUser);
                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show("Tạo tài khoản mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadUsersAsync();
                    ClearInputs();
                }
                else
                {
                    var err = await res.Content.ReadAsStringAsync();
                    MessageBox.Show("Tạo tài khoản thất bại: " + err, "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối máy chủ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần đặt lại mật khẩu từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(txtId.Text);
            string username = txtUsername.Text;

            if (MessageBox.Show($"Bạn có chắc muốn đặt lại mật khẩu của '{username}' về mặc định: 123456?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var res = await ApiClientService.Client.PostAsync($"users/{id}/reset-password", null);
                    if (res.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Mật khẩu của tài khoản '{username}' đã được đặt lại thành công về: 123456", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Đặt lại mật khẩu thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnToggleLock_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần Khóa/Mở khóa từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(txtId.Text);
            string username = txtUsername.Text;

            // Không cho phép tự khóa chính tài khoản của mình
            if (username.Equals(SessionManager.CurrentUsername, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Không thể tự khóa tài khoản bạn đang đăng nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var res = await ApiClientService.Client.PutAsync($"users/{id}/toggle-lock", null);
                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Đã thay đổi trạng thái hoạt động của tài khoản '{username}'!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadUsersAsync();
                }
                else
                {
                    MessageBox.Show("Không thể thay đổi trạng thái tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearInputs();
            await LoadUsersAsync();
        }

        private void ClearInputs()
        {
            txtId.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            cboRole.SelectedIndex = 1;
        }
    }
}
