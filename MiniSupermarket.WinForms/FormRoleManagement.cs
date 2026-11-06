using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms {
    public partial class FormRoleManagement : Form {

        public FormRoleManagement() {
            InitializeComponent();
        }

        private async void FormRoleManagement_Load(object sender, EventArgs e) {
            await LoadRolesAsync();
        }

        // tai danh sach vai tro
        private async Task LoadRolesAsync() {
            try {
                var response = await ApiClientService.Client.GetAsync("roles");
                if (response.IsSuccessStatusCode) {
                    var roles = await response.Content.ReadFromJsonAsync<List<RoleDto>>();
                    dgvRoles.DataSource = roles;
                } else {
                    string err = await ApiClientService.ReadErrorMessageAsync(response, "Không tải được danh sách vai trò!");
                    MessageBox.Show(err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show("Lỗi kết nối Server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // chon dong
        private void dgvRoles_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                DataGridViewRow row = dgvRoles.Rows[e.RowIndex];
                txtId.Text = row.Cells["colId"].Value?.ToString() ?? string.Empty;
                txtRoleName.Text = row.Cells["colRoleName"].Value?.ToString() ?? string.Empty;
                txtDescription.Text = row.Cells["colDescription"]?.Value?.ToString() ?? string.Empty;
            }
        }

        private async void btnLoad_Click(object sender, EventArgs e) {
            await LoadRolesAsync();
        }

        // them vai tro
        private async void btnAdd_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtRoleName.Text)) {
                MessageBox.Show("Tên vai trò không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newRole = new {
                RoleName = txtRoleName.Text.Trim(),
                Description = txtDescription.Text.Trim()
            };

            try {
                var response = await ApiClientService.Client.PostAsJsonAsync("roles", newRole);
                if (response.IsSuccessStatusCode) {
                    MessageBox.Show("Thêm vai trò mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadRolesAsync();
                    ClearInputs();
                } else {
                    string err = await ApiClientService.ReadErrorMessageAsync(response, "Thêm mới thất bại!");
                    MessageBox.Show(err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show("Lỗi kết nối Server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // sua vai tro
        private async void btnUpdate_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(txtId.Text)) {
                MessageBox.Show("Vui lòng chọn vai trò cần sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(txtId.Text);
            var updateRole = new {
                Id = id,
                RoleName = txtRoleName.Text.Trim(),
                Description = txtDescription.Text.Trim()
            };

            try {
                var response = await ApiClientService.Client.PutAsJsonAsync($"roles/{id}", updateRole);
                if (response.IsSuccessStatusCode) {
                    MessageBox.Show("Cập nhật vai trò thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadRolesAsync();
                    ClearInputs();
                } else {
                    string err = await ApiClientService.ReadErrorMessageAsync(response, "Cập nhật thất bại!");
                    MessageBox.Show(err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show("Lỗi kết nối Server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // xoa vai tro
        private async void btnDelete_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(txtId.Text)) {
                MessageBox.Show("Vui lòng chọn vai trò cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(txtId.Text);
            var confirm = MessageBox.Show($"Bạn có chắc muốn xóa vai trò ID = {id}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes) {
                try {
                    var response = await ApiClientService.Client.DeleteAsync($"roles/{id}");
                    if (response.IsSuccessStatusCode) {
                        MessageBox.Show("Xóa vai trò thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadRolesAsync();
                        ClearInputs();
                    } else {
                        string err = await ApiClientService.ReadErrorMessageAsync(response, "Xóa thất bại!");
                        MessageBox.Show(err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                } catch (Exception ex) {
                    MessageBox.Show("Lỗi kết nối Server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearInputs() {
            txtId.Text = "";
            txtRoleName.Text = "";
            txtDescription.Text = "";
        }
    }

    // DTO vai tro phia Client
    public class RoleDto {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
