using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MiniSupermarket.WinForms
{
    public partial class FormDashboard : Form
    {
        // du lieu doanh thu dang hien tren bieu do (dung de xuat Excel)
        private List<DailyRevenueDto> _revenueData = new();

        public FormDashboard()
        {
            InitializeComponent();
            SetupTopProductsGrid();
            SetupChartStyle();
        }

        // cot cho luoi Top 5 san pham
        private void SetupTopProductsGrid()
        {
            dgvTopProducts.AutoGenerateColumns = false;
            dgvTopProducts.Columns.Clear();

            var colRank = new DataGridViewTextBoxColumn { DataPropertyName = nameof(TopProductRow.Rank), HeaderText = "#", Width = 35 };
            colRank.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTopProducts.Columns.Add(colRank);

            dgvTopProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(TopProductRow.ProductName),
                HeaderText = "Sản phẩm",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            var colQty = new DataGridViewTextBoxColumn { DataPropertyName = nameof(TopProductRow.TotalQuantity), HeaderText = "Đã bán", Width = 65 };
            colQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTopProducts.Columns.Add(colQty);

            var colRevenue = new DataGridViewTextBoxColumn { DataPropertyName = nameof(TopProductRow.TotalRevenue), HeaderText = "Doanh thu (đ)", Width = 105 };
            colRevenue.DefaultCellStyle.Format = "N0";
            colRevenue.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvTopProducts.Columns.Add(colRevenue);

            var colStock = new DataGridViewTextBoxColumn { DataPropertyName = nameof(TopProductRow.StockQuantity), HeaderText = "Tồn", Width = 50 };
            colStock.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTopProducts.Columns.Add(colStock);
        }

        private async void FormDashboard_Load(object? sender, EventArgs e)
        {
            if (!SessionManager.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền xem dữ liệu tài chính của siêu thị!", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close();
                return;
            }

            // mac dinh xem 7 ngay gan nhat
            dtpToDate.Value = DateTime.Today;
            dtpFromDate.Value = DateTime.Today.AddDays(-6);

            await LoadAllAsync();
        }

        private async Task LoadAllAsync()
        {
            await LoadSummaryCardsAsync();
            await LoadRevenueChartAsync();
            await LoadTopProductsAsync();
        }

        // 4 the chi so: GET api/reports/dashboard-summary
        private async Task LoadSummaryCardsAsync()
        {
            try
            {
                var response = await ApiClientService.Client.GetAsync("reports/dashboard-summary");
                if (!response.IsSuccessStatusCode)
                {
                    string err = await ApiClientService.ReadErrorMessageAsync(response, "Không tải được số liệu tổng quan!");
                    MessageBox.Show(err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var summary = await response.Content.ReadFromJsonAsync<DashboardSummaryDto>();
                if (summary == null) return;

                lblRevenueValue.Text = $"{summary.TodayRevenue:N0} đ";
                lblOrdersValue.Text = $"{summary.TotalOrders:N0}";
                lblNewCustomersValue.Text = $"{summary.NewCustomers:N0}";
                lblLowStockValue.Text = $"{summary.LowStockProducts:N0}";

                // co hang sap het -> the chuyen sang mau do
                cardLowStock.BackColor = summary.LowStockProducts > 0
                    ? Color.FromArgb(220, 53, 69)
                    : Color.FromArgb(255, 153, 0);
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Không kết nối được máy chủ. Vui lòng kiểm tra API đã chạy và mạng còn hoạt động!",
                    "Mất kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải số liệu tổng quan: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // bieu do cot: GET api/reports/revenue-by-date?fromDate=..&toDate=..
        private async Task LoadRevenueChartAsync()
        {
            DateTime from = dtpFromDate.Value.Date;
            DateTime to = dtpToDate.Value.Date;

            if (from > to)
            {
                MessageBox.Show("'Từ ngày' phải nhỏ hơn hoặc bằng 'Đến ngày'!", "Khoảng thời gian không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnViewChart.Enabled = false;
                btnViewChart.Text = "Đang tải...";

                string url = $"reports/revenue-by-date?fromDate={from:yyyy-MM-dd}&toDate={to:yyyy-MM-dd}";
                var response = await ApiClientService.Client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    string err = await ApiClientService.ReadErrorMessageAsync(response, "Không tải được doanh thu theo ngày!");
                    MessageBox.Show(err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _revenueData = await response.Content.ReadFromJsonAsync<List<DailyRevenueDto>>() ?? new List<DailyRevenueDto>();

                // doanh thu tung san pham de chia cot theo san pham
                var productRows = new List<ProductDailyRevenueDto>();
                var productResponse = await ApiClientService.Client.GetAsync(
                    $"reports/revenue-by-product?fromDate={from:yyyy-MM-dd}&toDate={to:yyyy-MM-dd}");
                if (productResponse.IsSuccessStatusCode)
                {
                    productRows = await productResponse.Content.ReadFromJsonAsync<List<ProductDailyRevenueDto>>()
                                  ?? new List<ProductDailyRevenueDto>();
                }

                DrawRevenueChart(productRows);

                decimal total = _revenueData.Sum(d => d.Revenue);
                chartRevenue.Titles[0].Text = total > 0
                    ? $"DOANH THU TỪ {from:dd/MM/yyyy} ĐẾN {to:dd/MM/yyyy}  •  TỔNG THỰC THU: {total:N0} đ"
                    : $"CHƯA CÓ DOANH THU TỪ {from:dd/MM/yyyy} ĐẾN {to:dd/MM/yyyy}";
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Không kết nối được máy chủ để tải biểu đồ doanh thu!", "Mất kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải biểu đồ doanh thu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnViewChart.Enabled = true;
                btnViewChart.Text = "📊 Xem biểu đồ";
            }
        }

        // bang mau cho cac san pham tren bieu do
        private static readonly Color[] ChartPalette =
        {
            Color.FromArgb(41, 100, 180),
            Color.FromArgb(0, 168, 150),
            Color.FromArgb(255, 153, 0),
            Color.FromArgb(220, 53, 69),
            Color.FromArgb(111, 66, 193),
            Color.FromArgb(160, 166, 175)
        };

        // dinh dang khung bieu do cho gon, de doc
        private void SetupChartStyle()
        {
            var labelFont = new Font("Segoe UI", 9F);
            var titleFont = new Font("Segoe UI", 9F, FontStyle.Bold);
            var axisColor = Color.FromArgb(100, 110, 125);

            ChartArea area = chartRevenue.ChartAreas[0];
            area.BackColor = Color.White;

            area.AxisX.Interval = 1;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.MajorTickMark.Enabled = false;
            area.AxisX.LineColor = Color.FromArgb(200, 205, 215);
            area.AxisX.LabelStyle.Font = labelFont;
            area.AxisX.LabelStyle.ForeColor = axisColor;
            area.AxisX.TitleFont = titleFont;
            area.AxisX.TitleForeColor = axisColor;

            area.AxisY.LineColor = Color.Transparent;
            area.AxisY.MajorTickMark.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(232, 235, 240);
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            area.AxisY.LabelStyle.Format = "#,##0";
            area.AxisY.LabelStyle.Font = labelFont;
            area.AxisY.LabelStyle.ForeColor = axisColor;
            area.AxisY.TitleFont = titleFont;
            area.AxisY.TitleForeColor = axisColor;

            Legend legend = chartRevenue.Legends[0];
            legend.Docking = Docking.Bottom;
            legend.Alignment = StringAlignment.Center;
            legend.Font = labelFont;
            legend.ForeColor = Color.FromArgb(70, 80, 95);

            chartRevenue.Titles[0].ForeColor = Color.FromArgb(24, 30, 48);
        }

        // ve cot chong theo san pham + duong tong doanh thu thuc thu
        private void DrawRevenueChart(List<ProductDailyRevenueDto> productRows)
        {
            chartRevenue.Series.Clear();

            var days = _revenueData.Select(d => d.SaleDate.Date).ToList();
            if (days.Count == 0) return;

            // 5 san pham doanh thu cao nhat ve rieng, con lai gop vao "San pham khac"
            var productTotals = productRows
                .GroupBy(r => r.ProductName)
                .Select(g => new { Name = g.Key, Total = g.Sum(x => x.Revenue) })
                .OrderByDescending(x => x.Total)
                .ToList();
            var topNames = productTotals.Take(5).Select(x => x.Name).ToList();
            const string otherName = "Sản phẩm khác";

            var groupNames = new List<string>(topNames);
            if (productTotals.Count > topNames.Count) groupNames.Add(otherName);

            for (int i = 0; i < groupNames.Count; i++)
            {
                string name = groupNames[i];
                var series = new Series(name)
                {
                    ChartType = SeriesChartType.StackedColumn,
                    Color = name == otherName ? ChartPalette[^1] : ChartPalette[i % (ChartPalette.Length - 1)],
                    BorderColor = Color.White,
                    BorderWidth = 1,
                    IsXValueIndexed = true,
                    ChartArea = chartRevenue.ChartAreas[0].Name,
                    Legend = chartRevenue.Legends[0].Name
                };
                series["PointWidth"] = "0.55";

                foreach (DateTime day in days)
                {
                    var dayRows = productRows.Where(r => r.SaleDate.Date == day &&
                        (name == otherName ? !topNames.Contains(r.ProductName) : r.ProductName == name)).ToList();
                    decimal value = dayRows.Sum(r => r.Revenue);
                    int qty = dayRows.Sum(r => r.Quantity);

                    int idx = series.Points.AddXY(day.ToString("dd/MM"), value);
                    series.Points[idx].ToolTip = $"{name}\n{day:dd/MM/yyyy}: {value:N0} đ ({qty} sản phẩm)";
                }
                chartRevenue.Series.Add(series);
            }

            // duong tong doanh thu thuc thu (da tru giam gia) kem so tren dinh
            var totalSeries = new Series("Tổng thực thu")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.FromArgb(24, 30, 48),
                BorderWidth = 2,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 7,
                MarkerColor = Color.White,
                MarkerBorderColor = Color.FromArgb(24, 30, 48),
                MarkerBorderWidth = 2,
                IsXValueIndexed = true,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                LabelForeColor = Color.FromArgb(24, 30, 48),
                ChartArea = chartRevenue.ChartAreas[0].Name,
                Legend = chartRevenue.Legends[0].Name
            };
            totalSeries["LabelStyle"] = "Top";

            foreach (var d in _revenueData)
            {
                int idx = totalSeries.Points.AddXY(d.SaleDate.ToString("dd/MM"), d.Revenue);
                DataPoint point = totalSeries.Points[idx];
                point.Label = d.Revenue > 0 ? d.Revenue.ToString("N0") : string.Empty;
                point.ToolTip = $"{d.SaleDate:dd/MM/yyyy}\nThực thu: {d.Revenue:N0} đ\nSố hóa đơn: {d.TotalOrders}";
            }
            chartRevenue.Series.Add(totalSeries);

            // nhieu ngay thi xoay nhan truc X cho khoi chong chu
            chartRevenue.ChartAreas[0].AxisX.LabelStyle.Angle = days.Count > 10 ? -45 : 0;
            chartRevenue.ChartAreas[0].RecalculateAxesScale();
        }

        // Top 5: GET api/reports/top-products?top=5
        private async Task LoadTopProductsAsync()
        {
            try
            {
                var response = await ApiClientService.Client.GetAsync("reports/top-products?top=5");
                if (!response.IsSuccessStatusCode)
                {
                    string err = await ApiClientService.ReadErrorMessageAsync(response, "Không tải được danh sách bán chạy!");
                    MessageBox.Show(err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var top = await response.Content.ReadFromJsonAsync<List<TopProductDto>>() ?? new List<TopProductDto>();
                dgvTopProducts.DataSource = top
                    .Select((p, i) => new TopProductRow
                    {
                        Rank = i + 1,
                        ProductName = p.ProductName,
                        TotalQuantity = p.TotalQuantity,
                        TotalRevenue = p.TotalRevenue,
                        StockQuantity = p.StockQuantity
                    })
                    .ToList();

                groupTopProducts.Text = top.Count > 0
                    ? "🏆 TOP 5 SẢN PHẨM BÁN CHẠY NHẤT"
                    : "🏆 TOP 5 SẢN PHẨM BÁN CHẠY (chưa có đơn hàng)";
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Không kết nối được máy chủ để tải Top sản phẩm!", "Mất kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải Top sản phẩm bán chạy: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnViewChart_Click(object? sender, EventArgs e)
        {
            await LoadRevenueChartAsync();
        }

        private async void btnRefresh_Click(object? sender, EventArgs e)
        {
            await LoadAllAsync();
        }

        // xuat danh sach doanh thu dang xem ra Excel / CSV
        private void btnExportRevenue_Click(object? sender, EventArgs e)
        {
            var headers = new[] { "Ngày", "Số hóa đơn", "Doanh thu (đ)" };
            var rows = _revenueData
                .Select(d => new object?[] { d.SaleDate, d.TotalOrders, d.Revenue })
                .ToList();

            if (rows.Count > 0)
            {
                rows.Add(new object?[] { "TỔNG CỘNG", _revenueData.Sum(d => d.TotalOrders), _revenueData.Sum(d => d.Revenue) });
            }

            ExportHelper.ExportWithDialog(this,
                $"DoanhThu_{dtpFromDate.Value:yyyyMMdd}_{dtpToDate.Value:yyyyMMdd}", "DoanhThu", headers, rows);
        }

        // dong hien thi tren luoi Top 5
        public class TopProductRow
        {
            public int Rank { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public int TotalQuantity { get; set; }
            public decimal TotalRevenue { get; set; }
            public int StockQuantity { get; set; }
        }
    }
}
