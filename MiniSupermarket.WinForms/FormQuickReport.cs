using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormQuickReport : Form
    {
        public FormQuickReport()
        {
            InitializeComponent();
            SetupOrderGrids();
        }

        // cot cho bang danh sach don hang va bang chi tiet
        private void SetupOrderGrids()
        {
            dgvOrders.AutoGenerateColumns = false;
            dgvOrders.Columns.Clear();
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(OrderSummaryDto.OrderId), HeaderText = "Mã HĐ", Width = 60 });
            var colDate = new DataGridViewTextBoxColumn { DataPropertyName = nameof(OrderSummaryDto.OrderDate), HeaderText = "Thời gian", Width = 115 };
            colDate.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            dgvOrders.Columns.Add(colDate);
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(OrderSummaryDto.CustomerName), HeaderText = "Khách hàng", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(OrderSummaryDto.CustomerPhone), HeaderText = "SĐT", Width = 95 });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(OrderSummaryDto.CashierUsername), HeaderText = "Thu ngân", Width = 85 });
            var colItems = new DataGridViewTextBoxColumn { DataPropertyName = nameof(OrderSummaryDto.ItemCount), HeaderText = "SL", Width = 40 };
            colItems.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvOrders.Columns.Add(colItems);
            var colDiscount = new DataGridViewTextBoxColumn { DataPropertyName = nameof(OrderSummaryDto.DiscountAmount), HeaderText = "Giảm (đ)", Width = 75 };
            colDiscount.DefaultCellStyle.Format = "N0";
            colDiscount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvOrders.Columns.Add(colDiscount);
            var colFinal = new DataGridViewTextBoxColumn { DataPropertyName = nameof(OrderSummaryDto.FinalAmount), HeaderText = "Thực thu (đ)", Width = 95 };
            colFinal.DefaultCellStyle.Format = "N0";
            colFinal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colFinal.DefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            dgvOrders.Columns.Add(colFinal);

            dgvOrderDetails.AutoGenerateColumns = false;
            dgvOrderDetails.Columns.Clear();
            dgvOrderDetails.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(OrderLineDto.ProductId), HeaderText = "Mã SP", Width = 60 });
            dgvOrderDetails.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(OrderLineDto.ProductName), HeaderText = "Sản phẩm", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            var colQty = new DataGridViewTextBoxColumn { DataPropertyName = nameof(OrderLineDto.Quantity), HeaderText = "SL", Width = 50 };
            colQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvOrderDetails.Columns.Add(colQty);
            var colPrice = new DataGridViewTextBoxColumn { DataPropertyName = nameof(OrderLineDto.UnitPrice), HeaderText = "Đơn giá (đ)", Width = 100 };
            colPrice.DefaultCellStyle.Format = "N0";
            colPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvOrderDetails.Columns.Add(colPrice);
            var colLine = new DataGridViewTextBoxColumn { DataPropertyName = nameof(OrderLineDto.LineTotal), HeaderText = "Thành tiền (đ)", Width = 110 };
            colLine.DefaultCellStyle.Format = "N0";
            colLine.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvOrderDetails.Columns.Add(colLine);
        }

        // tai danh sach don hang trong ngay dang chon (loc theo ten / sdt khach neu co)
        private async Task LoadOrdersAsync(DateTime date)
        {
            try
            {
                string day = date.ToString("yyyy-MM-dd");
                string url = $"orders?fromDate={day}&toDate={day}";
                string keyword = txtOrderKeyword.Text.Trim();
                if (!string.IsNullOrEmpty(keyword))
                {
                    url += $"&keyword={Uri.EscapeDataString(keyword)}";
                }

                var response = await ApiClientService.Client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    string err = await ApiClientService.ReadErrorMessageAsync(response, "Không tải được danh sách đơn hàng!");
                    MessageBox.Show(err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var orders = await response.Content.ReadFromJsonAsync<List<OrderSummaryDto>>() ?? new List<OrderSummaryDto>();
                dgvOrderDetails.DataSource = null;
                dgvOrders.DataSource = orders;

                groupOrders.Text = $"🧾 Đơn hàng ngày {date:dd/MM/yyyy}: {orders.Count} đơn  •  Thực thu {orders.Sum(o => o.FinalAmount):N0} đ";
                lblOrderDetailTitle.Text = orders.Count > 0
                    ? "Chi tiết đơn hàng: chọn một đơn ở bảng trên"
                    : "Không có đơn hàng nào phù hợp";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối khi tải danh sách đơn hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // chon 1 don -> tai chi tiet tu GET api/orders/{id}
        private async void dgvOrders_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvOrders.CurrentRow?.DataBoundItem is not OrderSummaryDto selected) return;

            try
            {
                var response = await ApiClientService.Client.GetAsync($"orders/{selected.OrderId}");
                if (!response.IsSuccessStatusCode) return;

                var order = await response.Content.ReadFromJsonAsync<OrderDetailViewDto>();
                if (order == null) return;

                dgvOrderDetails.DataSource = order.Details;
                string discount = order.DiscountAmount > 0 ? $"  •  Giảm {order.DiscountAmount:N0} đ ({order.PointsUsed} điểm)" : string.Empty;
                lblOrderDetailTitle.Text = $"Chi tiết HĐ #{order.OrderId} - {selected.CustomerName}  •  Tiền hàng {order.TotalAmount:N0} đ{discount}  •  Thực thu {order.FinalAmount:N0} đ";
            }
            catch
            {
                lblOrderDetailTitle.Text = "Không tải được chi tiết đơn hàng";
            }
        }

        private async void btnSearchOrders_Click(object? sender, EventArgs e)
        {
            await LoadOrdersAsync(dtpReportDate.Value);
        }

        private async void txtOrderKeyword_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await LoadOrdersAsync(dtpReportDate.Value);
            }
        }

        private async void FormQuickReport_Load(object sender, EventArgs e)
        {
            // kiem tra phan quyen cap Action
            if (!SessionManager.IsAdmin && !SessionManager.IsCashier)
            {
                MessageBox.Show("Bạn không có quyền xem dữ liệu tài chính của siêu thị!", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Close();
                return;
            }

            dtpReportDate.Value = DateTime.Today;
            await LoadReportAsync(dtpReportDate.Value);
            await LoadOrdersAsync(dtpReportDate.Value);
        }

        private async void btnRunReport_Click(object sender, EventArgs e)
        {
            // kiem tra phan quyen lan 2
            if (!SessionManager.IsAdmin && !SessionManager.IsCashier)
            {
                MessageBox.Show("Bạn không có quyền xem dữ liệu tài chính của siêu thị!", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            await LoadReportAsync(dtpReportDate.Value);
            await LoadOrdersAsync(dtpReportDate.Value);
        }

        // goi API bao cao theo ngay va hien thi so don, doanh thu, mat hang ban chay
        private async Task LoadReportAsync(DateTime date)
        {
            try
            {
                btnRunReport.Enabled = false;
                btnRunReport.Text = "Đang tải dữ liệu...";

                string dateParam = date.ToString("yyyy-MM-dd");
                var report = await ApiClientService.Client.GetFromJsonAsync<QuickReportDto>($"orders/report?date={dateParam}");

                if (report != null)
                {
                    lblTotalOrders.Text = $"{report.TotalOrders} đơn";
                    lblTotalRevenue.Text = $"{report.TotalRevenue:N0} đ";
                    lblBestSeller.Text = !string.IsNullOrWhiteSpace(report.BestSeller) ? report.BestSeller : "Chưa có đơn hàng";

                    // gia tri trung binh moi hoa don
                    decimal avgOrder = report.TotalOrders > 0 ? report.TotalRevenue / report.TotalOrders : 0;
                    lblNotes.Text = $"BÁO CÁO DOANH SỐ NGÀY: {date:dd/MM/yyyy}\n\n" +
                                   $"• Tổng số giao dịch thành công: {report.TotalOrders} lượt mua hàng tại quầy POS\n" +
                                   $"• Tổng doanh số thu về: {report.TotalRevenue:N0} VNĐ\n" +
                                   $"• Giá trị trung bình trên mỗi hóa đơn (AOV): {avgOrder:N0} VNĐ/đơn\n" +
                                   $"• Mặt hàng có sức tiêu thụ dẫn đầu: {report.BestSeller}\n\n" +
                                   "[Ghi chú hệ thống]: Dữ liệu được tính toán tự động dựa trên toàn bộ các đơn hàng đã thanh toán trong ngày đã chọn." +
                                   $" Người lập báo cáo: {SessionManager.CurrentFullName} ({SessionManager.CurrentUsername} - {SessionManager.CurrentRole}).";
                }
            }
            catch (Exception ex)
            {
                // loi mang -> thong bao
                MessageBox.Show("Lỗi lấy dữ liệu báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRunReport.Enabled = true;
                btnRunReport.Text = "📊 Xem doanh thu ca";
            }
        }

        // gia lap xuat bao cao ra may in
        private void btnExport_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Đã xuất dữ liệu báo cáo ngày {dtpReportDate.Value:dd/MM/yyyy} thành công sang máy in hóa đơn!", "In ấn báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    // DTO nhan ket qua bao cao tu API
    public class QuickReportDto
    {
        public string ReportDate { get; set; } = string.Empty;
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public string BestSeller { get; set; } = string.Empty;
    }

    // mot dong trong danh sach don hang (GET api/orders)
    public class OrderSummaryDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CashierUsername { get; set; } = string.Empty;
        public string CashierName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }
        public int ItemCount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
    }

    // chi tiet 1 don hang (GET api/orders/{id})
    public class OrderDetailViewDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int PointsUsed { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public List<OrderLineDto> Details { get; set; } = new();
    }

    public class OrderLineDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
