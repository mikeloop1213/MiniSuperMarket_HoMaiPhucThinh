using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormCategoryManagement : Form
    {
        public FormCategoryManagement()
        {
            InitializeComponent();
        }

        // Bổ sung phương thức cấu hình HttpClient có gắn kèm Token bảo mật
        private HttpClient GetAuthenticatedClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/api/")
            };

            // Đính kèm Token vào Header theo chuẩn Bearer Authentication
            if (!string.IsNullOrEmpty(SessionManager.JwtToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.JwtToken);
            }
            return client;
        }

        private async void FormCategoryManagement_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SessionManager.CurrentRole))
            {
                lblReady.Text = $"Sẵn sàng | Quyền: {SessionManager.CurrentRole} | API: localhost:5000";

                // Phân quyền: Thu ngân (Cashier) không được phép xóa nhóm hàng
                if (SessionManager.CurrentRole.Equals("Cashier", StringComparison.OrdinalIgnoreCase))
                {
                    btnDelete.Enabled = false;
                    btnDelete.Text = "Xóa (Khóa)";
                }
                else
                {
                    btnDelete.Enabled = true;
                    btnDelete.Text = "Xóa";
                }
            }
            await LoadDataAsync();
        }

        // Áp dụng khi gọi hàm tải dữ liệu LoadDataAsync():
        private async Task LoadDataAsync()
        {
            try
            {
                using var client = GetAuthenticatedClient(); // Sử dụng client đã gắn token
                var response = await client.GetAsync("categories");
                if (response.IsSuccessStatusCode)
                {
                    var categories = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
                    dgvCategories.DataSource = categories;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Phiên làm việc hết hạn hoặc chưa đăng nhập!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    MessageBox.Show("Bạn không có quyền truy cập dữ liệu này!", "Bị cấm truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu từ Server.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi quyền truy cập hoặc mất kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private void dgvCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCategories.Rows[e.RowIndex];
                if (row.DataBoundItem is CategoryDto category)
                {
                    txtId.Text = category.CategoryId.ToString();
                    txtCategoryName.Text = category.CategoryName;
                    txtDescription.Text = category.Description ?? string.Empty;
                }
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Tên nhóm hàng không được trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newCat = new
            {
                CategoryName = txtCategoryName.Text.Trim(),
                Description = txtDescription.Text.Trim()
            };

            try
            {
                using var client = GetAuthenticatedClient();
                var response = await client.PostAsJsonAsync("categories", newCat);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Thêm mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
                    ClearInputs();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Phiên làm việc hết hạn hoặc chưa đăng nhập!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    MessageBox.Show("Tài khoản của bạn không có quyền thêm mới nhóm hàng!", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Thêm mới thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Vui lòng chọn nhóm hàng cần sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(txtId.Text);
            var updateCat = new
            {
                CategoryId = id,
                CategoryName = txtCategoryName.Text.Trim(),
                Description = txtDescription.Text.Trim()
            };

            try
            {
                using var client = GetAuthenticatedClient();
                var response = await client.PutAsJsonAsync($"categories/{id}", updateCat);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
                    ClearInputs();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Phiên làm việc hết hạn hoặc chưa đăng nhập!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    MessageBox.Show("Tài khoản của bạn không có quyền cập nhật thông tin này!", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (SessionManager.CurrentRole.Equals("Cashier", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Tài khoản Thu ngân (Cashier) không có quyền xóa nhóm hàng! Chức năng này chỉ dành cho Quản trị viên (Admin).", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Vui lòng chọn nhóm hàng cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(txtId.Text);
            var confirm = MessageBox.Show($"Bạn có chắc muốn xóa nhóm hàng ID = {id}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using var client = GetAuthenticatedClient();
                    var response = await client.DeleteAsync($"categories/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadDataAsync();
                        ClearInputs();
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        MessageBox.Show("Phiên làm việc hết hạn hoặc chưa đăng nhập!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        MessageBox.Show("Tài khoản của bạn không có quyền xóa nhóm hàng!", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtKeyword.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                await LoadDataAsync();
                return;
            }

            try
            {
                using var client = GetAuthenticatedClient();
                var response = await client.GetAsync($"categories/search?keyword={keyword}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
                    dgvCategories.DataSource = result;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Phiên làm việc hết hạn hoặc chưa đăng nhập!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy kết quả phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpenRoles_Click(object sender, EventArgs e)
        {
            FormRoleManagement formRole = new FormRoleManagement();
            formRole.ShowDialog();
        }

        private void btnOpenCustomers_Click(object sender, EventArgs e)
        {
            FormCustomerManagement formCustomer = new FormCustomerManagement();
            formCustomer.ShowDialog();
        }

        private void ClearInputs()
        {
            txtId.Text = "";
            txtCategoryName.Text = "";
            txtDescription.Text = "";
        }
    }

    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}