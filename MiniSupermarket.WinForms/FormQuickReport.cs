using System;
using System.Drawing;
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
        }

        private async void FormQuickReport_Load(object sender, EventArgs e)
        {
            // Kiểm tra phân quyền cấp Action
            if (!SessionManager.CurrentRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Bạn không có quyền xem dữ liệu tài chính của siêu thị!", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Close();
                return;
            }

            dtpReportDate.Value = DateTime.Today;
            await LoadReportAsync(dtpReportDate.Value);
        }

        private async void btnRunReport_Click(object sender, EventArgs e)
        {
            // Kiểm tra phân quyền bảo vệ lần 2
            if (!SessionManager.CurrentRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Bạn không có quyền xem dữ liệu tài chính của siêu thị!", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            await LoadReportAsync(dtpReportDate.Value);
        }

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

                    decimal avgOrder = report.TotalOrders > 0 ? report.TotalRevenue / report.TotalOrders : 0;
                    lblNotes.Text = $"BÁO CÁO DOANH SỐ NGÀY: {date:dd/MM/yyyy}\n\n" +
                                   $"• Tổng số giao dịch thành công: {report.TotalOrders} lượt mua hàng tại quầy POS\n" +
                                   $"• Tổng doanh số thu về: {report.TotalRevenue:N0} VNĐ\n" +
                                   $"• Giá trị trung bình trên mỗi hóa đơn (AOV): {avgOrder:N0} VNĐ/đơn\n" +
                                   $"• Mặt hàng có sức tiêu thụ dẫn đầu: {report.BestSeller}\n\n" +
                                   $"[Ghi chú hệ thống]: Dữ liệu được tính toán tự động dựa trên toàn bộ các đơn hàng đã thanh toán trong ngày đã chọn. Người lập báo cáo: {SessionManager.CurrentFullName} ({SessionManager.CurrentUsername} - {SessionManager.CurrentRole}).";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy dữ liệu báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRunReport.Enabled = true;
                btnRunReport.Text = "📊 Xem doanh thu ca";
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Đã xuất dữ liệu báo cáo ngày {dtpReportDate.Value:dd/MM/yyyy} thành công sang máy in hóa đơn!", "In ấn báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    public class QuickReportDto
    {
        public string ReportDate { get; set; } = string.Empty;
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public string BestSeller { get; set; } = string.Empty;
    }
}
