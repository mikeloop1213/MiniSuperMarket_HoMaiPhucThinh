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
        private readonly List<CartItemDto> _cart = new();

        public FormPOS()
        {
            InitializeComponent();
            SetupCartGrid();
        }

        private void SetupCartGrid()
        {
            dgvCart.AutoGenerateColumns = false;
            dgvCart.Columns.Clear();

            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductId",
                HeaderText = "Mã SP",
                Width = 75
            });

            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductName",
                HeaderText = "Tên Sản Phẩm",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            var colUnitPrice = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "UnitPrice",
                HeaderText = "Đơn Giá (đ)",
                Width = 110
            };
            colUnitPrice.DefaultCellStyle.Format = "N0";
            colUnitPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCart.Columns.Add(colUnitPrice);

            var colQuantity = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Quantity",
                HeaderText = "SL",
                Width = 70
            };
            colQuantity.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCart.Columns.Add(colQuantity);

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

        // Bắt sự kiện quét mã Barcode
        private async void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtBarcode.Text))
            {
                string barcode = txtBarcode.Text.Trim();
                txtBarcode.Clear();
                await AddProductToCartByBarcodeAsync(barcode);
            }
        }

        private async Task AddProductToCartByBarcodeAsync(string barcode)
        {
            try
            {
                // Gọi API tra cứu sản phẩm theo Barcode
                var product = await ApiClientService.Client.GetFromJsonAsync<ProductDto>($"products/barcode/{barcode}");
                if (product == null)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm có mã vạch này!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (product.StockQuantity <= 0)
                {
                    MessageBox.Show($"Sản phẩm '{product.ProductName}' hiện đã hết hàng trong kho!", "Hết hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var existingItem = _cart.FirstOrDefault(c => c.ProductId == product.ProductId);
                if (existingItem != null)
                {
                    if (existingItem.Quantity + 1 > product.StockQuantity)
                    {
                        MessageBox.Show($"Kho chỉ còn {product.StockQuantity} sản phẩm!", "Cảnh báo tồn kho", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    existingItem.Quantity++;
                }
                else
                {
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

        private void UpdateCartDisplay()
        {
            dgvCart.DataSource = null;
            dgvCart.DataSource = _cart;

            decimal total = _cart.Sum(x => x.TotalPrice);
            lblTotalAmount.Text = $"{total:N0} đ";
            CalculateChange();
        }

        private void txtCashReceived_TextChanged(object sender, EventArgs e)
        {
            CalculateChange();
        }

        private void CalculateChange()
        {
            decimal total = _cart.Sum(x => x.TotalPrice);
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

        private void btnCashExact_Click(object sender, EventArgs e)
        {
            decimal total = _cart.Sum(x => x.TotalPrice);
            txtCashReceived.Text = total > 0 ? total.ToString("N0", CultureInfo.InvariantCulture).Replace(",", ".") : "0";
        }

        private void btnCashQuick_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                string amountStr = btn.Text.Replace(".", "").Trim();
                txtCashReceived.Text = amountStr;
            }
        }

        private async void btnLookupCustomer_Click(object sender, EventArgs e)
        {
            await LookupCustomerAsync();
        }

        private async void txtCustomerPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await LookupCustomerAsync();
            }
        }

        private async Task LookupCustomerAsync()
        {
            string phone = txtCustomerPhone.Text.Trim();
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
                }
                else
                {
                    lblCustomerName.Text = "Khách hàng mới";
                    lblCustomerPoints.Text = "Chưa có thông tin điểm";
                }
            }
            catch
            {
                lblCustomerName.Text = "Khách hàng chưa đăng ký";
                lblCustomerPoints.Text = "Tích điểm tự động khi thanh toán";
            }
        }

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

        private async Task PerformCheckoutAsync()
        {
            if (_cart.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống! Vui lòng quét sản phẩm trước khi thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal total = _cart.Sum(x => x.TotalPrice);
            string rawText = txtCashReceived.Text.Replace(".", "").Replace(",", "").Trim();
            if (!decimal.TryParse(rawText, out decimal cashReceived) || cashReceived < total)
            {
                MessageBox.Show("Tiền khách đưa chưa đủ để thanh toán đơn hàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCashReceived.Focus();
                return;
            }

            var orderRequest = new
            {
                CashierUsername = SessionManager.CurrentUsername,
                CustomerPhone = txtCustomerPhone.Text.Trim(),
                Items = _cart.Select(i => new { i.ProductId, i.Quantity, i.UnitPrice }).ToList()
            };

            try
            {
                var response = await ApiClientService.Client.PostAsJsonAsync("orders/checkout", orderRequest);
                if (response.IsSuccessStatusCode)
                {
                    decimal change = cashReceived - total;
                    MessageBox.Show($"Thanh toán thành công!\nTổng tiền: {total:N0} đ\nKhách đưa: {cashReceived:N0} đ\nTiền thừa: {change:N0} đ\nĐã in hóa đơn thanh toán.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _cart.Clear();
                    UpdateCartDisplay();
                    txtCashReceived.Clear();
                    txtCustomerPhone.Clear();
                    lblCustomerName.Text = "Khách vãng lai";
                    lblCustomerPoints.Text = "Điểm tích lũy: 0 điểm";
                    txtBarcode.Focus();
                }
                else
                {
                    MessageBox.Show("Thanh toán thất bại từ máy chủ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối máy chủ khi thanh toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
