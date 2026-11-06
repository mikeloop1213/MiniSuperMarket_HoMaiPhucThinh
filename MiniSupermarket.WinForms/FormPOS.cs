using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormPOS : Form
    {
        // gio hang hien tai
        // gio hang dung chung voi PosDraft de khong mat khi doi man hinh
        private readonly List<CartItemDto> _cart = PosDraft.Cart;

        // doi diem thuong: 50 diem = 50.000 d
        private const int PointsPerRedeem = 50;
        private const decimal DiscountPerRedeem = 50000m;

        // diem tich luy cua khach dang tra cuu
        private int _customerPoints = 0;

        public FormPOS()
        {
            InitializeComponent();
            SetupCartGrid();
            txtCustomerPhone.TextChanged += (s, e) =>
            {
                ResetLoyaltyOption();
                PosDraft.CustomerPhone = txtCustomerPhone.Text;
            };
            Load += FormPOS_Load;
        }

        // khoi phuc gio hang + SDT khach dang nhap do (neu co)
        private async void FormPOS_Load(object? sender, EventArgs e)
        {
            bool restoreUsePoints = PosDraft.UsePoints;
            UpdateCartDisplay();

            if (!string.IsNullOrWhiteSpace(PosDraft.CustomerPhone))
            {
                txtCustomerPhone.Text = PosDraft.CustomerPhone;
                await LookupCustomerAsync();
                if (restoreUsePoints && chkUsePoints.Enabled)
                {
                    chkUsePoints.Checked = true;
                }
            }

            txtBarcode.Focus();
        }

        // tong tien hang truoc khi tru diem
        private decimal GetSubtotal() => _cart.Sum(x => x.TotalPrice);

        // so tien giam khi tich chon doi diem
        private decimal GetDiscount() => chkUsePoints.Checked ? DiscountPerRedeem : 0;

        // so tien khach phai tra
        private decimal GetPayable() => Math.Max(0, GetSubtotal() - GetDiscount());

        // tat lua chon doi diem khi doi khach / chua tra cuu
        private void ResetLoyaltyOption()
        {
            _customerPoints = 0;
            chkUsePoints.Checked = false;
            chkUsePoints.Enabled = false;
        }

        private void chkUsePoints_CheckedChanged(object? sender, EventArgs e)
        {
            PosDraft.UsePoints = chkUsePoints.Checked;
            if (chkUsePoints.Checked && GetSubtotal() < DiscountPerRedeem)
            {
                MessageBox.Show($"Hóa đơn phải từ {DiscountPerRedeem:N0} đ trở lên mới được dùng {PointsPerRedeem} điểm!",
                    "Chưa đủ điều kiện đổi điểm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chkUsePoints.Checked = false;
                return;
            }
            UpdateCartDisplay();
        }

        // cau hinh cac cot DataGridView gio hang
        private void SetupCartGrid()
        {
            dgvCart.AutoGenerateColumns = false;
            dgvCart.Columns.Clear();

            // cot Ma SP
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductId",
                HeaderText = "Mã SP",
                Width = 75
            });

            // cot Ten san pham
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductName",
                HeaderText = "Tên Sản Phẩm",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // cot Don gia
            var colUnitPrice = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "UnitPrice",
                HeaderText = "Đơn Giá (đ)",
                Width = 110
            };
            colUnitPrice.DefaultCellStyle.Format = "N0";
            colUnitPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCart.Columns.Add(colUnitPrice);

            // cot So luong
            var colQuantity = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Quantity",
                HeaderText = "SL",
                Width = 70
            };
            colQuantity.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCart.Columns.Add(colQuantity);

            // cot Thanh tien
            var colTotalPrice = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TotalPrice",
                HeaderText = "Thành Tiền (đ)",
                Width = 130
            };
            colTotalPrice.DefaultCellStyle.Format = "N0";
            colTotalPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colTotalPrice.DefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            dgvCart.Columns.Add(colTotalPrice);
        }

        // bat su kien quet ma Barcode
        private async void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtBarcode.Text))
            {
                string barcode = txtBarcode.Text.Trim();
                txtBarcode.Clear();
                await AddProductToCartByBarcodeAsync(barcode);
            }
        }

        // tra cuu san pham qua ma vach va them/cong don vao gio
        private async Task AddProductToCartByBarcodeAsync(string barcode)
        {
            try
            {
                var response = await ApiClientService.Client.GetAsync($"products/barcode/{Uri.EscapeDataString(barcode)}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show($"Không tìm thấy sản phẩm có mã vạch '{barcode}'!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!response.IsSuccessStatusCode)
                {
                    string err = await ApiClientService.ReadErrorMessageAsync(response, "Lỗi khi tra cứu mã vạch!");
                    MessageBox.Show(err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var product = await response.Content.ReadFromJsonAsync<ProductDto>();
                if (product == null)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm có mã vạch này!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // chan ban khi kho da het
                if (product.StockQuantity <= 0)
                {
                    MessageBox.Show($"Sản phẩm '{product.ProductName}' hiện đã hết hàng trong kho!", "Hết hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // neu san pham da co trong gio hang -> cong don so luong
                var existingItem = _cart.FirstOrDefault(c => c.ProductId == product.ProductId);
                if (existingItem != null)
                {
                    // khong cho vuot qua so luong ton kho
                    if (existingItem.Quantity + 1 > product.StockQuantity)
                    {
                        MessageBox.Show($"Kho chỉ còn {product.StockQuantity} sản phẩm!", "Cảnh báo tồn kho", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    existingItem.Quantity++;
                }
                else
                {
                    // chua co gio hang
                    _cart.Add(new CartItemDto
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        UnitPrice = product.Price,
                        Quantity = 1
                    });
                }

                UpdateCartDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tìm thấy sản phẩm hoặc lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // cap nhat luoi gio hang va tu tinh TONG TIEN
        private void UpdateCartDisplay()
        {
            dgvCart.DataSource = null;
            dgvCart.DataSource = _cart;

            // gio hang giam xuong duoi muc doi diem -> bo chon doi diem
            if (chkUsePoints.Checked && GetSubtotal() < DiscountPerRedeem)
            {
                chkUsePoints.Checked = false;
            }

            decimal discount = GetDiscount();
            lblTotalTitle.Text = discount > 0
                ? $"TỔNG TIỀN THANH TOÁN (đã trừ {discount:N0} đ):"
                : "TỔNG TIỀN THANH TOÁN:";
            lblTotalAmount.Text = $"{GetPayable():N0} đ";
            CalculateChange();
        }


        private void txtCashReceived_TextChanged(object sender, EventArgs e)
        {
            CalculateChange();
        }

        private void CalculateChange()
        {
            decimal total = GetPayable();

            string rawText = txtCashReceived.Text.Replace(".", "").Replace(",", "").Trim();

            if (decimal.TryParse(rawText, out decimal cashReceived))
            {
                decimal change = cashReceived - total;
                if (change >= 0)
                {
                    lblChange.Text = $"{change:N0} đ";
                    lblChange.ForeColor = Color.FromArgb(40, 167, 69);
                }
                else
                {
                    lblChange.Text = "Chưa đủ tiền!";
                    lblChange.ForeColor = Color.FromArgb(220, 53, 69);
                }
            }
            else
            {
                lblChange.Text = "0 đ";
                lblChange.ForeColor = Color.FromArgb(40, 167, 69);
            }
        }

        // nut "Dua du tien"
        private void btnCashExact_Click(object sender, EventArgs e)
        {
            decimal total = GetPayable();
            txtCashReceived.Text = total > 0 ? total.ToString("N0", CultureInfo.InvariantCulture).Replace(",", ".") : "0";
        }

        // cac nut menh gia
        private void btnCashQuick_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                string amountStr = btn.Text.Replace(".", "").Trim();
                txtCashReceived.Text = amountStr;
            }
        }

        // tra cuu khach hang bang nut bam
        private async void btnLookupCustomer_Click(object sender, EventArgs e)
        {
            await LookupCustomerAsync();
        }

        // tra cuu khach hang khi tim bang sdt
        private async void txtCustomerPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await LookupCustomerAsync();
            }
        }

        // goi GET api/customers/by-phone/{phone} de hien thi ten, hang va diem tich luy
        private async Task LookupCustomerAsync()
        {
            string phone = txtCustomerPhone.Text.Trim();
            ResetLoyaltyOption();
            if (string.IsNullOrWhiteSpace(phone))
            {
                lblCustomerName.Text = "Khách vãng lai";
                lblCustomerPoints.Text = "Điểm tích lũy: 0 điểm";
                return;
            }

            try
            {
                var customer = await ApiClientService.Client.GetFromJsonAsync<CustomerDto>($"customers/by-phone/{phone}");
                if (customer != null)
                {
                    lblCustomerName.Text = $"{customer.CustomerName} ({customer.MembershipRank})";
                    lblCustomerPoints.Text = $"Điểm tích lũy: {customer.RewardPoints} điểm | Đ/C: {customer.Address}";

                    // du 50 diem -> cho phep doi diem tru tien
                    _customerPoints = customer.RewardPoints;
                    chkUsePoints.Enabled = _customerPoints >= PointsPerRedeem;
                }
                else
                {
                    lblCustomerName.Text = "Khách hàng mới";
                    lblCustomerPoints.Text = "Chưa có thông tin điểm";
                }
            }
            catch
            {
                // API tra 404 -> khach chua dang ky thanh vien
                lblCustomerName.Text = "Khách hàng chưa đăng ký";
                lblCustomerPoints.Text = "Tích điểm tự động khi thanh toán";
            }
        }

        // xoa toan bo gio hang
        private void btnClearCart_Click(object sender, EventArgs e)
        {
            if (_cart.Count == 0) return;

            if (MessageBox.Show("Bạn có chắc muốn xóa toàn bộ sản phẩm trong giỏ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _cart.Clear();
                UpdateCartDisplay();
                txtCashReceived.Clear();
            }
        }

        // xoa 1 dong dang chon trong gio
        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvCart.CurrentRow?.DataBoundItem is CartItemDto selectedItem)
            {
                _cart.Remove(selectedItem);
                UpdateCartDisplay();
            }
        }

        private async void btnCheckout_Click(object sender, EventArgs e)
        {
            await PerformCheckoutAsync();
        }

        // thanh toan
        private async Task PerformCheckoutAsync()
        {
            // gio hang rong -> khong cho thanh toan
            if (_cart.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống! Vui lòng quét sản phẩm trước khi thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // kiem tra tien khach dua phai >= tong tien
            decimal subtotal = GetSubtotal();
            decimal total = GetPayable();

            // kiem tra dieu kien doi diem phia Client
            if (chkUsePoints.Checked && (_customerPoints < PointsPerRedeem || subtotal < DiscountPerRedeem))
            {
                MessageBox.Show($"Không đủ điều kiện đổi điểm (cần ≥ {PointsPerRedeem} điểm và hóa đơn ≥ {DiscountPerRedeem:N0} đ)!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                chkUsePoints.Checked = false;
                return;
            }

            string rawText = txtCashReceived.Text.Replace(".", "").Replace(",", "").Trim();
            if (!decimal.TryParse(rawText, out decimal cashReceived) || cashReceived < total)
            {
                MessageBox.Show("Tiền khách đưa chưa đủ để thanh toán đơn hàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCashReceived.Focus();
                return;
            }

            // dong goi du lieu khop voi CheckoutRequestDto phia API
            var orderRequest = new
            {
                CashierUsername = SessionManager.CurrentUsername,
                CustomerPhone = txtCustomerPhone.Text.Trim(),
                Items = _cart.Select(i => new { i.ProductId, i.Quantity, i.UnitPrice }).ToList(),
                UsePoints = chkUsePoints.Checked
            };

            try
            {
                var response = await ApiClientService.Client.PostAsJsonAsync("orders/checkout", orderRequest);
                if (response.IsSuccessStatusCode)
                {
                    // doc so tien thuc thu, tien giam, diem tu may chu
                    decimal finalAmount = total;
                    decimal discountAmount = 0;
                    int pointsUsed = 0;
                    int addedPoints = 0;
                    try
                    {
                        using var doc = System.Text.Json.JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                        var root = doc.RootElement;
                        if (root.TryGetProperty("finalAmount", out var fa)) finalAmount = fa.GetDecimal();
                        if (root.TryGetProperty("discountAmount", out var da)) discountAmount = da.GetDecimal();
                        if (root.TryGetProperty("pointsUsed", out var pu)) pointsUsed = pu.GetInt32();
                        if (root.TryGetProperty("addedPoints", out var ap)) addedPoints = ap.GetInt32();
                    }
                    catch { /* giu so lieu tinh phia Client */ }

                    // hien thi tong tien/ tien khach dua/ tien thoi lai
                    decimal change = cashReceived - finalAmount;
                    string loyaltyInfo = pointsUsed > 0
                        ? $"\nTiền hàng: {subtotal:N0} đ\nĐổi {pointsUsed} điểm: -{discountAmount:N0} đ"
                        : string.Empty;
                    string pointsInfo = addedPoints > 0 ? $"\nĐiểm cộng thêm: +{addedPoints} điểm" : string.Empty;
                    MessageBox.Show($"Thanh toán thành công!{loyaltyInfo}\nKhách cần trả: {finalAmount:N0} đ\nKhách đưa: {cashReceived:N0} đ\nTiền thừa: {change:N0} đ{pointsInfo}\nĐã in hóa đơn thanh toán.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // reset man hinh cho khach tiep theo
                    _cart.Clear();
                    ResetLoyaltyOption();
                    UpdateCartDisplay();
                    txtCashReceived.Clear();
                    txtCustomerPhone.Clear();
                    lblCustomerName.Text = "Khách vãng lai";
                    lblCustomerPoints.Text = "Điểm tích lũy: 0 điểm";
                    txtBarcode.Focus();
                }
                else
                {

                    string reason = await ApiClientService.ReadErrorMessageAsync(response, "Thanh toán thất bại từ máy chủ!");
                    MessageBox.Show(reason, "Thanh toán không thành công", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // mat mang khi thanh toan -> bao loi
                MessageBox.Show("Lỗi kết nối máy chủ khi thanh toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // phim tat F9 = thanh toan
        private void FormPOS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F9)
            {
                btnCheckout.PerformClick();
            }
        }

        private void dgvCart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
