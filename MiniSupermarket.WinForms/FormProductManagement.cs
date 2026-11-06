// =====================================================================================
// FILE: FormProductManagement.cs  -  Quan ly san pham & ton kho
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormProductManagement : Form
    {
        // nguong canh bao hang sap het kho
        private const int LowStockThreshold = 10;
        private Font? _boldStockFont;

        // bien trang thai phan trang
        private int _currentPage = 1;
        private const int PageSize = 10;
        private int _totalPages = 1;

        // cac control phan trang 
        private readonly Panel panelPaging = new Panel();
        private readonly Button btnPrevPage = new Button();
        private readonly Button btnNextPage = new Button();
        private readonly Label lblPageInfo = new Label();

        public FormProductManagement()
        {
            InitializeComponent();
            SetupProductsGrid();
            SetupPagingControls();
        }

        // tao thanh phan trang nam duoi luoi san pham:
        private void SetupPagingControls()
        {
            panelPaging.Dock = DockStyle.Bottom;
            panelPaging.Height = 44;

            btnPrevPage.Text = "◀ Trang trước";
            btnPrevPage.Size = new Size(120, 32);
            btnPrevPage.Location = new Point(0, 6);
            btnPrevPage.Click += async (s, e) => await ChangePageAsync(-1);

            lblPageInfo.AutoSize = false;
            lblPageInfo.Size = new Size(260, 32);
            lblPageInfo.Location = new Point(130, 6);
            lblPageInfo.TextAlign = ContentAlignment.MiddleCenter;
            lblPageInfo.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);

            btnNextPage.Text = "Trang sau ▶";
            btnNextPage.Size = new Size(120, 32);
            btnNextPage.Location = new Point(400, 6);
            btnNextPage.Click += async (s, e) => await ChangePageAsync(+1);

            panelPaging.Controls.Add(btnPrevPage);
            panelPaging.Controls.Add(lblPageInfo);
            panelPaging.Controls.Add(btnNextPage);

            panelGrid.Controls.Add(panelPaging);
            dgvProducts.BringToFront();
        }

        // chuyen trang
        private async Task ChangePageAsync(int delta)
        {
            int newPage = _currentPage + delta;
            if (newPage < 1 || newPage > _totalPages) return;
            _currentPage = newPage;
            await LoadProductsAsync();
        }

        // [tao cot cho DataGridView, anh xa voi thuoc tinh cua ProductDto
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

            // cot ten nhom hang co du lieu nho
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CategoryName",
                HeaderText = "Nhóm Hàng",
                Width = 160
            });
        }

        private async void FormProductManagement_Load(object sender, EventArgs e)
        {
            // thu ngan chi xem: an nhom nut Them / Cap nhat / Xoa
            groupActions.Visible = !SessionManager.IsCashier;

            await LoadCategoriesToComboAsync();
            await LoadProductsAsync();
        }

        // to mau nen do cho dong co ton kho <= 10
        private void dgvProducts_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvProducts.Rows[e.RowIndex].DataBoundItem is not ProductDto prod) return;

            if (prod.StockQuantity <= LowStockThreshold)
            {
                e.CellStyle.BackColor = Color.LightCoral;
                e.CellStyle.SelectionBackColor = Color.IndianRed;
                e.CellStyle.SelectionForeColor = Color.White;

                if (dgvProducts.Columns[e.ColumnIndex].DataPropertyName == nameof(ProductDto.StockQuantity))
                {
                    e.CellStyle.ForeColor = Color.DarkRed;
                    e.CellStyle.Font = _boldStockFont ??= new Font(dgvProducts.Font, FontStyle.Bold);
                }
            }
        }

        // cap nhat huy hieu so mat hang sap het kho
        private async Task UpdateLowStockBadgeAsync()
        {
            try
            {
                var lowStock = await ApiClientService.Client.GetFromJsonAsync<List<ProductDto>>(
                    $"products/low-stock?threshold={LowStockThreshold}");
                int count = lowStock?.Count ?? 0;

                if (count > 0)
                {
                    lblLowStockBadge.BackColor = Color.FromArgb(220, 53, 69);
                    lblLowStockBadge.Text = $"⚠ {count} mặt hàng sắp hết kho (tồn ≤ {LowStockThreshold}) - bấm để xem";
                }
                else
                {
                    lblLowStockBadge.BackColor = Color.FromArgb(40, 167, 69);
                    lblLowStockBadge.Text = "✔ Tồn kho ổn định, không có mặt hàng sắp hết";
                }
            }
            catch
            {
                lblLowStockBadge.BackColor = Color.FromArgb(108, 117, 125);
                lblLowStockBadge.Text = "Không tải được cảnh báo tồn kho";
            }
        }

        // bam huy hieu -> chi hien cac mat hang sap het kho
        private async void lblLowStockBadge_Click(object? sender, EventArgs e)
        {
            try
            {
                var lowStock = await ApiClientService.Client.GetFromJsonAsync<List<ProductDto>>(
                    $"products/low-stock?threshold={LowStockThreshold}");
                dgvProducts.DataSource = lowStock;
                lblPageInfo.Text = $"Hàng sắp hết kho: {lowStock?.Count ?? 0} sản phẩm";
                btnPrevPage.Enabled = false;
                btnNextPage.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách hàng sắp hết kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // xuat toan bo danh muc san pham kem ton kho ra Excel / CSV
        private async void btnExportExcel_Click(object? sender, EventArgs e)
        {
            try
            {
                btnExportExcel.Enabled = false;
                var all = await ApiClientService.Client.GetFromJsonAsync<List<ProductDto>>("products/search");
                if (all == null) return;

                var headers = new[] { "Mã SP", "Mã vạch", "Tên sản phẩm", "Nhóm hàng", "Đơn giá (đ)", "Tồn kho", "Trạng thái" };
                var rows = all.Select(p => new object?[]
                {
                    p.ProductId,
                    p.Barcode,
                    p.ProductName,
                    p.CategoryName,
                    p.Price,
                    p.StockQuantity,
                    p.StockQuantity <= LowStockThreshold ? "Sắp hết hàng" : "Còn hàng"
                }).ToList();

                ExportHelper.ExportWithDialog(this, $"DanhMucSanPham_{DateTime.Now:yyyyMMdd_HHmm}", "SanPham", headers, rows);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy dữ liệu để xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnExportExcel.Enabled = true;
            }
        }

        // nap danh muc nhom hang
        private async Task LoadCategoriesToComboAsync()
        {
            try
            {
                var categories = await ApiClientService.Client.GetFromJsonAsync<List<CategoryDto>>("categories");
                if (categories != null)
                {
                    // do du lieu vao ComboBox chi tiet
                    cboCategory.DataSource = new List<CategoryDto>(categories);
                    cboCategory.DisplayMember = "CategoryName";
                    cboCategory.ValueMember = "CategoryId";

                    // do du lieu vao ComboBox bo loc
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

        // tai 1 trang san pham tu GET api/products?page=..&pageSize=.. len DataGridView
        private async Task LoadProductsAsync()
        {
            try
            {
                var paged = await ApiClientService.Client.GetFromJsonAsync<PagedResultDto<ProductDto>>(
                    $"products?page={_currentPage}&pageSize={PageSize}");
                if (paged == null) return;


                if (paged.Items.Count == 0 && paged.Page > 1 && paged.TotalPages > 0)
                {
                    _currentPage = paged.TotalPages;
                    await LoadProductsAsync();
                    return;
                }

                dgvProducts.DataSource = paged.Items;
                await UpdateLowStockBadgeAsync();
                _totalPages = Math.Max(1, paged.TotalPages);
                lblPageInfo.Text = $"Trang {paged.Page}/{_totalPages}  •  Tổng {paged.TotalItems} sản phẩm";
                btnPrevPage.Enabled = paged.Page > 1;
                btnNextPage.Enabled = paged.Page < _totalPages;
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

        // them moi san pham -> POST api/products
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (SessionManager.BlockCashierEdit(this)) return;

            // Kiem tra du lieu phia Client truoc khi gui
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

                    var err = await ApiClientService.ReadErrorMessageAsync(res, "Thêm mới thất bại!");
                    MessageBox.Show("Thêm mới thất bại:\n" + err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối máy chủ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // cap nhat san pham -> PUT api/products/{id}
        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (SessionManager.BlockCashierEdit(this)) return;

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
                    var err = await ApiClientService.ReadErrorMessageAsync(res, "Cập nhật thất bại!");
                    MessageBox.Show("Cập nhật thất bại:\n" + err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối máy chủ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // xoa san pham -> DELETE api/products/{id}
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (SessionManager.BlockCashierEdit(this)) return;

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

                        var err = await ApiClientService.ReadErrorMessageAsync(res, "Không thể xóa sản phẩm!");
                        MessageBox.Show(err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // tim kiem
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

                lblPageInfo.Text = $"Kết quả tìm kiếm: {results?.Count ?? 0} sản phẩm";
                btnPrevPage.Enabled = false;
                btnNextPage.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Lam moi
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            if (cboFilterCategory.Items.Count > 0)
            {
                cboFilterCategory.SelectedIndex = 0;
            }
            ClearInputs();
            _currentPage = 1;
            await LoadProductsAsync();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        // Xoa
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
