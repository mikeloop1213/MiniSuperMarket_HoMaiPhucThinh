using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormProductManagement : Form
    {
        public FormProductManagement()
        {
            InitializeComponent();
            SetupProductsGrid();
        }

        private void SetupProductsGrid()
        {
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.Columns.Clear();

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductId",
                HeaderText = "Mã SP",
                Width = 70
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Barcode",
                HeaderText = "Mã Vạch",
                Width = 130
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductName",
                HeaderText = "Tên Sản Phẩm",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            var colPrice = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Price",
                HeaderText = "Đơn Giá (đ)",
                Width = 115
            };
            colPrice.DefaultCellStyle.Format = "N0";
            colPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProducts.Columns.Add(colPrice);

            var colStock = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "StockQuantity",
                HeaderText = "Tồn Kho",
                Width = 85
            };
            colStock.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvProducts.Columns.Add(colStock);

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CategoryName",
                HeaderText = "Nhóm Hàng",
                Width = 160
            });
        }

        private async void FormProductManagement_Load(object sender, EventArgs e)
        {
            await LoadCategoriesToComboAsync();
            await LoadProductsAsync();
        }

        private async Task LoadCategoriesToComboAsync()
        {
            try
            {
                var categories = await ApiClientService.Client.GetFromJsonAsync<List<CategoryDto>>("categories");
                if (categories != null)
                {
                    // Đổ dữ liệu vào ComboBox chi tiết
                    cboCategory.DataSource = new List<CategoryDto>(categories);
                    cboCategory.DisplayMember = "CategoryName";
                    cboCategory.ValueMember = "CategoryId";

                    // Đổ dữ liệu vào ComboBox bộ lọc
                    var filterList = new List<CategoryDto>
                    {
                        new CategoryDto { CategoryId = 0, CategoryName = "-- Tất cả nhóm hàng --" }
                    };
                    filterList.AddRange(categories);
                    cboFilterCategory.DataSource = filterList;
                    cboFilterCategory.DisplayMember = "CategoryName";
                    cboFilterCategory.ValueMember = "CategoryId";
                    cboFilterCategory.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh mục nhóm hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                var products = await ApiClientService.Client.GetFromJsonAsync<List<ProductDto>>("products");
                dgvProducts.DataSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh sách sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProducts.Rows[e.RowIndex].DataBoundItem is ProductDto prod)
            {
                txtId.Text = prod.ProductId.ToString();
                txtBarcode.Text = prod.Barcode;
                txtProductName.Text = prod.ProductName;
                nudPrice.Value = prod.Price;
                nudStock.Value = prod.StockQuantity;
                if (prod.CategoryId > 0)
                {
                    cboCategory.SelectedValue = prod.CategoryId;
                }
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBarcode.Text) || string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ mã vạch và tên sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboCategory.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhóm hàng cho sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newProd = new
            {
                Barcode = txtBarcode.Text.Trim(),
                ProductName = txtProductName.Text.Trim(),
                Price = nudPrice.Value,
                StockQuantity = (int)nudStock.Value,
                CategoryId = Convert.ToInt32(cboCategory.SelectedValue)
            };

            try
            {
                var res = await ApiClientService.Client.PostAsJsonAsync("products", newProd);
                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show("Thêm mới sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadProductsAsync();
                    ClearInputs();
                }
                else
                {
                    var err = await res.Content.ReadAsStringAsync();
                    MessageBox.Show("Thêm mới thất bại: " + err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối máy chủ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(txtId.Text);
            var updateProd = new
            {
                ProductId = id,
                Barcode = txtBarcode.Text.Trim(),
                ProductName = txtProductName.Text.Trim(),
                Price = nudPrice.Value,
                StockQuantity = (int)nudStock.Value,
                CategoryId = Convert.ToInt32(cboCategory.SelectedValue)
            };

            try
            {
                var res = await ApiClientService.Client.PutAsJsonAsync($"products/{id}", updateProd);
                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadProductsAsync();
                }
                else
                {
                    var err = await res.Content.ReadAsStringAsync();
                    MessageBox.Show("Cập nhật thất bại: " + err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối máy chủ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(txtId.Text);
            string name = txtProductName.Text;

            if (MessageBox.Show($"Xác nhận xóa vĩnh viễn sản phẩm: '{name}' (ID: {id})?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var res = await ApiClientService.Client.DeleteAsync($"products/{id}");
                    if (res.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Đã xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadProductsAsync();
                        ClearInputs();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối máy chủ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await PerformSearchAsync();
        }

        private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await PerformSearchAsync();
            }
        }

        private async Task PerformSearchAsync()
        {
            string keyword = txtSearch.Text.Trim();
            int catId = cboFilterCategory.SelectedValue != null ? Convert.ToInt32(cboFilterCategory.SelectedValue) : 0;

            try
            {
                string url = $"products/search?keyword={Uri.EscapeDataString(keyword)}";
                if (catId > 0)
                {
                    url += $"&categoryId={catId}";
                }

                var results = await ApiClientService.Client.GetFromJsonAsync<List<ProductDto>>(url);
                dgvProducts.DataSource = results;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            if (cboFilterCategory.Items.Count > 0)
            {
                cboFilterCategory.SelectedIndex = 0;
            }
            ClearInputs();
            await LoadProductsAsync();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void ClearInputs()
        {
            txtId.Clear();
            txtBarcode.Clear();
            txtProductName.Clear();
            nudPrice.Value = 0;
            nudStock.Value = 0;
            if (cboCategory.Items.Count > 0)
            {
                cboCategory.SelectedIndex = 0;
            }
        }
    }
}
