using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormCategoryManagement : Form
    {
        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7080/api/")
        };

        public FormCategoryManagement()
        {
            InitializeComponent();
        }

        private async void FormCategoryManagement_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var categories = await _client.GetFromJsonAsync<List<CategoryDto>>("categories");
                dgvCategories.DataSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối Server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            var response = await _client.PostAsJsonAsync("categories", newCat);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Thêm mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync();
                ClearInputs();
            }
            else
            {
                MessageBox.Show("Thêm mới thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            var response = await _client.PutAsJsonAsync($"categories/{id}", updateCat);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync();
                ClearInputs();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Vui lòng chọn nhóm hàng cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(txtId.Text);
            var confirm = MessageBox.Show($"Bạn có chắc muốn xóa nhóm hàng ID = {id}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                var response = await _client.DeleteAsync($"categories/{id}");
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                var result = await _client.GetFromJsonAsync<List<CategoryDto>>($"categories/search?keyword={keyword}");
                dgvCategories.DataSource = result;
            }
            catch (Exception)
            {
                MessageBox.Show("Không tìm thấy kết quả phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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