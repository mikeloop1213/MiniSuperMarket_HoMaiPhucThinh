namespace MiniSupermarket.WinForms
{
    partial class FormQuickReport
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            DataGridViewCellStyle headerStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle headerStyle2 = new DataGridViewCellStyle();
            panelControlBar = new Panel();
            btnExport = new Button();
            btnRunReport = new Button();
            dtpReportDate = new DateTimePicker();
            lblSelectDate = new Label();
            panelCards = new TableLayoutPanel();
            cardOrders = new Panel();
            lblTotalOrders = new Label();
            lblOrdersTitle = new Label();
            cardRevenue = new Panel();
            lblTotalRevenue = new Label();
            lblRevenueTitle = new Label();
            cardBestSeller = new Panel();
            lblBestSeller = new Label();
            lblBestSellerTitle = new Label();
            panelContent = new Panel();
            groupNotes = new GroupBox();
            lblNotes = new Label();
            groupOrders = new GroupBox();
            dgvOrders = new DataGridView();
            lblOrderDetailTitle = new Label();
            dgvOrderDetails = new DataGridView();
            lblOrderKeyword = new Label();
            txtOrderKeyword = new TextBox();
            btnSearchOrders = new Button();
            panelControlBar.SuspendLayout();
            panelCards.SuspendLayout();
            cardOrders.SuspendLayout();
            cardRevenue.SuspendLayout();
            cardBestSeller.SuspendLayout();
            panelContent.SuspendLayout();
            groupNotes.SuspendLayout();
            groupOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrders).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvOrderDetails).BeginInit();
            SuspendLayout();
            // 
            // panelControlBar
            // 
            panelControlBar.BackColor = Color.White;
            panelControlBar.BorderStyle = BorderStyle.FixedSingle;
            panelControlBar.Controls.Add(btnSearchOrders);
            panelControlBar.Controls.Add(txtOrderKeyword);
            panelControlBar.Controls.Add(lblOrderKeyword);
            panelControlBar.Controls.Add(btnExport);
            panelControlBar.Controls.Add(btnRunReport);
            panelControlBar.Controls.Add(dtpReportDate);
            panelControlBar.Controls.Add(lblSelectDate);
            panelControlBar.Dock = DockStyle.Top;
            panelControlBar.Location = new Point(0, 0);
            panelControlBar.Name = "panelControlBar";
            panelControlBar.Padding = new Padding(15, 12, 15, 12);
            panelControlBar.Size = new Size(1040, 65);
            panelControlBar.TabIndex = 0;
            // 
            // lblOrderKeyword
            // 
            lblOrderKeyword.AutoSize = true;
            lblOrderKeyword.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblOrderKeyword.ForeColor = Color.FromArgb(24, 30, 48);
            lblOrderKeyword.Location = new Point(625, 23);
            lblOrderKeyword.Name = "lblOrderKeyword";
            lblOrderKeyword.Size = new Size(76, 17);
            lblOrderKeyword.TabIndex = 4;
            lblOrderKeyword.Text = "Khách hàng:";
            // 
            // txtOrderKeyword
            // 
            txtOrderKeyword.Font = new Font("Segoe UI", 10F);
            txtOrderKeyword.Location = new Point(707, 19);
            txtOrderKeyword.Name = "txtOrderKeyword";
            txtOrderKeyword.PlaceholderText = "Tên hoặc SĐT khách...";
            txtOrderKeyword.Size = new Size(170, 25);
            txtOrderKeyword.TabIndex = 5;
            txtOrderKeyword.KeyDown += txtOrderKeyword_KeyDown;
            // 
            // btnSearchOrders
            // 
            btnSearchOrders.BackColor = Color.FromArgb(41, 100, 180);
            btnSearchOrders.Cursor = Cursors.Hand;
            btnSearchOrders.FlatAppearance.BorderSize = 0;
            btnSearchOrders.FlatStyle = FlatStyle.Flat;
            btnSearchOrders.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btnSearchOrders.ForeColor = Color.White;
            btnSearchOrders.Location = new Point(885, 16);
            btnSearchOrders.Name = "btnSearchOrders";
            btnSearchOrders.Size = new Size(135, 32);
            btnSearchOrders.TabIndex = 6;
            btnSearchOrders.Text = "🔍 Lọc đơn hàng";
            btnSearchOrders.UseVisualStyleBackColor = false;
            btnSearchOrders.Click += btnSearchOrders_Click;
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.FromArgb(108, 117, 125);
            btnExport.Cursor = Cursors.Hand;
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(480, 16);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(130, 32);
            btnExport.TabIndex = 3;
            btnExport.Text = "🖨 In báo cáo ca";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += btnExport_Click;
            // 
            // btnRunReport
            // 
            btnRunReport.BackColor = Color.FromArgb(0, 192, 192);
            btnRunReport.Cursor = Cursors.Hand;
            btnRunReport.FlatAppearance.BorderSize = 0;
            btnRunReport.FlatStyle = FlatStyle.Flat;
            btnRunReport.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btnRunReport.ForeColor = Color.White;
            btnRunReport.Location = new Point(320, 16);
            btnRunReport.Name = "btnRunReport";
            btnRunReport.Size = new Size(150, 32);
            btnRunReport.TabIndex = 2;
            btnRunReport.Text = "📊 Xem doanh thu ca";
            btnRunReport.UseVisualStyleBackColor = false;
            btnRunReport.Click += btnRunReport_Click;
            // 
            // dtpReportDate
            // 
            dtpReportDate.CustomFormat = "dd/MM/yyyy";
            dtpReportDate.Font = new Font("Segoe UI", 10F);
            dtpReportDate.Format = DateTimePickerFormat.Custom;
            dtpReportDate.Location = new Point(145, 19);
            dtpReportDate.Name = "dtpReportDate";
            dtpReportDate.Size = new Size(160, 25);
            dtpReportDate.TabIndex = 1;
            // 
            // lblSelectDate
            // 
            lblSelectDate.AutoSize = true;
            lblSelectDate.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblSelectDate.ForeColor = Color.FromArgb(24, 30, 48);
            lblSelectDate.Location = new Point(15, 22);
            lblSelectDate.Name = "lblSelectDate";
            lblSelectDate.Size = new Size(132, 19);
            lblSelectDate.TabIndex = 0;
            lblSelectDate.Text = "Chọn ngày báo cáo:";
            // 
            // panelCards
            // 
            panelCards.ColumnCount = 3;
            panelCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            panelCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            panelCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            panelCards.Controls.Add(cardOrders, 0, 0);
            panelCards.Controls.Add(cardRevenue, 1, 0);
            panelCards.Controls.Add(cardBestSeller, 2, 0);
            panelCards.Dock = DockStyle.Top;
            panelCards.Location = new Point(0, 65);
            panelCards.Name = "panelCards";
            panelCards.Padding = new Padding(15);
            panelCards.RowCount = 1;
            panelCards.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panelCards.Size = new Size(1040, 180);
            panelCards.TabIndex = 1;
            // 
            // cardOrders
            // 
            cardOrders.BackColor = Color.FromArgb(0, 192, 192);
            cardOrders.BorderStyle = BorderStyle.FixedSingle;
            cardOrders.Controls.Add(lblTotalOrders);
            cardOrders.Controls.Add(lblOrdersTitle);
            cardOrders.Dock = DockStyle.Fill;
            cardOrders.Location = new Point(18, 18);
            cardOrders.Margin = new Padding(3, 3, 10, 3);
            cardOrders.Name = "cardOrders";
            cardOrders.Padding = new Padding(15);
            cardOrders.Size = new Size(323, 144);
            cardOrders.TabIndex = 0;
            // 
            // lblTotalOrders
            // 
            lblTotalOrders.Dock = DockStyle.Fill;
            lblTotalOrders.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblTotalOrders.ForeColor = Color.White;
            lblTotalOrders.Location = new Point(15, 45);
            lblTotalOrders.Name = "lblTotalOrders";
            lblTotalOrders.Size = new Size(291, 82);
            lblTotalOrders.TabIndex = 1;
            lblTotalOrders.Text = "0 đơn";
            lblTotalOrders.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblOrdersTitle
            // 
            lblOrdersTitle.Dock = DockStyle.Top;
            lblOrdersTitle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblOrdersTitle.ForeColor = Color.FromArgb(220, 235, 255);
            lblOrdersTitle.Location = new Point(15, 15);
            lblOrdersTitle.Name = "lblOrdersTitle";
            lblOrdersTitle.Size = new Size(291, 30);
            lblOrdersTitle.TabIndex = 0;
            lblOrdersTitle.Text = "\U0001f9fe TỔNG SỐ HÓA ĐƠN TRONG NGÀY";
            lblOrdersTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardRevenue
            // 
            cardRevenue.BackColor = Color.FromArgb(0, 192, 0);
            cardRevenue.BorderStyle = BorderStyle.FixedSingle;
            cardRevenue.Controls.Add(lblTotalRevenue);
            cardRevenue.Controls.Add(lblRevenueTitle);
            cardRevenue.Dock = DockStyle.Fill;
            cardRevenue.Location = new Point(361, 18);
            cardRevenue.Margin = new Padding(10, 3, 10, 3);
            cardRevenue.Name = "cardRevenue";
            cardRevenue.Padding = new Padding(15);
            cardRevenue.Size = new Size(316, 144);
            cardRevenue.TabIndex = 1;
            // 
            // lblTotalRevenue
            // 
            lblTotalRevenue.Dock = DockStyle.Fill;
            lblTotalRevenue.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTotalRevenue.ForeColor = Color.White;
            lblTotalRevenue.Location = new Point(15, 45);
            lblTotalRevenue.Name = "lblTotalRevenue";
            lblTotalRevenue.Size = new Size(284, 82);
            lblTotalRevenue.TabIndex = 1;
            lblTotalRevenue.Text = "0 đ";
            lblTotalRevenue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblRevenueTitle
            // 
            lblRevenueTitle.Dock = DockStyle.Top;
            lblRevenueTitle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblRevenueTitle.ForeColor = Color.FromArgb(220, 255, 225);
            lblRevenueTitle.Location = new Point(15, 15);
            lblRevenueTitle.Name = "lblRevenueTitle";
            lblRevenueTitle.Size = new Size(284, 30);
            lblRevenueTitle.TabIndex = 0;
            lblRevenueTitle.Text = "💰 TỔNG DOANH THU BÁN LẺ";
            lblRevenueTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardBestSeller
            // 
            cardBestSeller.BackColor = Color.Gray;
            cardBestSeller.BorderStyle = BorderStyle.FixedSingle;
            cardBestSeller.Controls.Add(lblBestSeller);
            cardBestSeller.Controls.Add(lblBestSellerTitle);
            cardBestSeller.Dock = DockStyle.Fill;
            cardBestSeller.Location = new Point(697, 18);
            cardBestSeller.Margin = new Padding(10, 3, 3, 3);
            cardBestSeller.Name = "cardBestSeller";
            cardBestSeller.Padding = new Padding(15);
            cardBestSeller.Size = new Size(325, 144);
            cardBestSeller.TabIndex = 2;
            // 
            // lblBestSeller
            // 
            lblBestSeller.Dock = DockStyle.Fill;
            lblBestSeller.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            lblBestSeller.ForeColor = Color.White;
            lblBestSeller.Location = new Point(15, 45);
            lblBestSeller.Name = "lblBestSeller";
            lblBestSeller.Size = new Size(293, 82);
            lblBestSeller.TabIndex = 1;
            lblBestSeller.Text = "Chưa có dữ liệu";
            lblBestSeller.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblBestSellerTitle
            // 
            lblBestSellerTitle.Dock = DockStyle.Top;
            lblBestSellerTitle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblBestSellerTitle.ForeColor = Color.FromArgb(255, 245, 220);
            lblBestSellerTitle.Location = new Point(15, 15);
            lblBestSellerTitle.Name = "lblBestSellerTitle";
            lblBestSellerTitle.Size = new Size(293, 30);
            lblBestSellerTitle.TabIndex = 0;
            lblBestSellerTitle.Text = "⭐ MẶT HÀNG BÁN CHẠY NHẤT";
            lblBestSellerTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelContent
            // 
            panelContent.Controls.Add(groupOrders);
            panelContent.Controls.Add(groupNotes);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 245);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(15);
            panelContent.Size = new Size(1040, 415);
            panelContent.TabIndex = 2;
            // 
            // groupNotes
            // 
            groupNotes.BackColor = Color.White;
            groupNotes.Controls.Add(lblNotes);
            groupNotes.Dock = DockStyle.Right;
            groupNotes.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            groupNotes.Location = new Point(705, 15);
            groupNotes.Name = "groupNotes";
            groupNotes.Padding = new Padding(15);
            groupNotes.Size = new Size(320, 385);
            groupNotes.TabIndex = 0;
            groupNotes.TabStop = false;
            groupNotes.Text = "📌 Thông tin & Phân tích hiệu suất ca bán";
            // 
            // lblNotes
            // 
            lblNotes.Dock = DockStyle.Fill;
            lblNotes.Font = new Font("Segoe UI", 10F);
            lblNotes.ForeColor = Color.FromArgb(50, 60, 75);
            lblNotes.Location = new Point(15, 33);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new Size(290, 337);
            lblNotes.TabIndex = 0;
            lblNotes.Text = "Chọn ngày và bấm nút 'Xem doanh thu ca' để tải số liệu mới nhất từ hệ thống.";
            // 
            // groupOrders
            // 
            groupOrders.BackColor = Color.White;
            groupOrders.Controls.Add(dgvOrders);
            groupOrders.Controls.Add(lblOrderDetailTitle);
            groupOrders.Controls.Add(dgvOrderDetails);
            groupOrders.Dock = DockStyle.Fill;
            groupOrders.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            groupOrders.Location = new Point(15, 15);
            groupOrders.Name = "groupOrders";
            groupOrders.Padding = new Padding(10);
            groupOrders.Size = new Size(690, 385);
            groupOrders.TabIndex = 1;
            groupOrders.TabStop = false;
            groupOrders.Text = "🧾 Danh sách đơn hàng khách đã đặt";
            // 
            // dgvOrders
            // 
            dgvOrders.AllowUserToAddRows = false;
            dgvOrders.AllowUserToDeleteRows = false;
            dgvOrders.BackgroundColor = Color.White;
            headerStyle1.BackColor = Color.FromArgb(24, 30, 48);
            headerStyle1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            headerStyle1.ForeColor = Color.White;
            headerStyle1.SelectionBackColor = Color.FromArgb(24, 30, 48);
            headerStyle1.WrapMode = DataGridViewTriState.True;
            dgvOrders.ColumnHeadersDefaultCellStyle = headerStyle1;
            dgvOrders.ColumnHeadersHeight = 32;
            dgvOrders.Dock = DockStyle.Fill;
            dgvOrders.EnableHeadersVisualStyles = false;
            dgvOrders.Font = new Font("Segoe UI", 9F);
            dgvOrders.Location = new Point(10, 28);
            dgvOrders.MultiSelect = false;
            dgvOrders.Name = "dgvOrders";
            dgvOrders.ReadOnly = true;
            dgvOrders.RowHeadersVisible = false;
            dgvOrders.RowTemplate.Height = 28;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrders.Size = new Size(670, 183);
            dgvOrders.TabIndex = 0;
            dgvOrders.SelectionChanged += dgvOrders_SelectionChanged;
            // 
            // lblOrderDetailTitle
            // 
            lblOrderDetailTitle.Dock = DockStyle.Bottom;
            lblOrderDetailTitle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblOrderDetailTitle.ForeColor = Color.FromArgb(41, 100, 180);
            lblOrderDetailTitle.Location = new Point(10, 211);
            lblOrderDetailTitle.Name = "lblOrderDetailTitle";
            lblOrderDetailTitle.Padding = new Padding(0, 6, 0, 0);
            lblOrderDetailTitle.Size = new Size(670, 28);
            lblOrderDetailTitle.TabIndex = 1;
            lblOrderDetailTitle.Text = "Chi tiết đơn hàng: chọn một đơn ở bảng trên";
            // 
            // dgvOrderDetails
            // 
            dgvOrderDetails.AllowUserToAddRows = false;
            dgvOrderDetails.AllowUserToDeleteRows = false;
            dgvOrderDetails.BackgroundColor = Color.FromArgb(248, 249, 251);
            headerStyle2.BackColor = Color.FromArgb(41, 100, 180);
            headerStyle2.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            headerStyle2.ForeColor = Color.White;
            headerStyle2.SelectionBackColor = Color.FromArgb(41, 100, 180);
            dgvOrderDetails.ColumnHeadersDefaultCellStyle = headerStyle2;
            dgvOrderDetails.ColumnHeadersHeight = 30;
            dgvOrderDetails.Dock = DockStyle.Bottom;
            dgvOrderDetails.EnableHeadersVisualStyles = false;
            dgvOrderDetails.Font = new Font("Segoe UI", 9F);
            dgvOrderDetails.Location = new Point(10, 239);
            dgvOrderDetails.Name = "dgvOrderDetails";
            dgvOrderDetails.ReadOnly = true;
            dgvOrderDetails.RowHeadersVisible = false;
            dgvOrderDetails.RowTemplate.Height = 26;
            dgvOrderDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrderDetails.Size = new Size(670, 136);
            dgvOrderDetails.TabIndex = 2;
            // 
            // FormQuickReport
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 245, 247);
            ClientSize = new Size(1040, 660);
            Controls.Add(panelContent);
            Controls.Add(panelCards);
            Controls.Add(panelControlBar);
            Font = new Font("Segoe UI", 9.75F);
            Name = "FormQuickReport";
            Text = "FormQuickReport";
            Load += FormQuickReport_Load;
            panelControlBar.ResumeLayout(false);
            panelControlBar.PerformLayout();
            panelCards.ResumeLayout(false);
            cardOrders.ResumeLayout(false);
            cardRevenue.ResumeLayout(false);
            cardBestSeller.ResumeLayout(false);
            panelContent.ResumeLayout(false);
            groupNotes.ResumeLayout(false);
            groupOrders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvOrders).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvOrderDetails).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelControlBar;
        private Label lblSelectDate;
        private DateTimePicker dtpReportDate;
        private Button btnRunReport;
        private Button btnExport;
        private TableLayoutPanel panelCards;
        private Panel cardOrders;
        private Label lblOrdersTitle;
        private Label lblTotalOrders;
        private Panel cardRevenue;
        private Label lblRevenueTitle;
        private Label lblTotalRevenue;
        private Panel cardBestSeller;
        private Label lblBestSellerTitle;
        private Label lblBestSeller;
        private Panel panelContent;
        private GroupBox groupNotes;
        private Label lblNotes;
        private GroupBox groupOrders;
        private DataGridView dgvOrders;
        private Label lblOrderDetailTitle;
        private DataGridView dgvOrderDetails;
        private Label lblOrderKeyword;
        private TextBox txtOrderKeyword;
        private Button btnSearchOrders;
    }
}
