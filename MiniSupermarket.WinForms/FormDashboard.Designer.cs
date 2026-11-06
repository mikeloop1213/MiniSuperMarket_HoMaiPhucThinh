namespace MiniSupermarket.WinForms
{
    partial class FormDashboard
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            tableCards = new TableLayoutPanel();
            cardRevenue = new Panel();
            lblRevenueValue = new Label();
            lblRevenueTitle = new Label();
            cardOrders = new Panel();
            lblOrdersValue = new Label();
            lblOrdersTitle = new Label();
            cardNewCustomers = new Panel();
            lblNewCustomersValue = new Label();
            lblNewCustomersTitle = new Label();
            cardLowStock = new Panel();
            lblLowStockValue = new Label();
            lblLowStockTitle = new Label();
            panelFilter = new Panel();
            btnExportRevenue = new Button();
            btnRefresh = new Button();
            btnViewChart = new Button();
            dtpToDate = new DateTimePicker();
            lblToDate = new Label();
            dtpFromDate = new DateTimePicker();
            lblFromDate = new Label();
            panelContent = new Panel();
            chartRevenue = new System.Windows.Forms.DataVisualization.Charting.Chart();
            groupTopProducts = new GroupBox();
            dgvTopProducts = new DataGridView();
            tableCards.SuspendLayout();
            cardRevenue.SuspendLayout();
            cardOrders.SuspendLayout();
            cardNewCustomers.SuspendLayout();
            cardLowStock.SuspendLayout();
            panelFilter.SuspendLayout();
            panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartRevenue).BeginInit();
            groupTopProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTopProducts).BeginInit();
            SuspendLayout();
            //
            // tableCards
            //
            tableCards.BackColor = Color.FromArgb(244, 245, 247);
            tableCards.ColumnCount = 4;
            tableCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableCards.Controls.Add(cardRevenue, 0, 0);
            tableCards.Controls.Add(cardOrders, 1, 0);
            tableCards.Controls.Add(cardNewCustomers, 2, 0);
            tableCards.Controls.Add(cardLowStock, 3, 0);
            tableCards.Dock = DockStyle.Top;
            tableCards.Location = new Point(0, 0);
            tableCards.Name = "tableCards";
            tableCards.Padding = new Padding(8, 10, 8, 4);
            tableCards.RowCount = 1;
            tableCards.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableCards.Size = new Size(1040, 115);
            tableCards.TabIndex = 0;
            //
            // cardRevenue
            //
            cardRevenue.BackColor = Color.FromArgb(40, 167, 69);
            cardRevenue.Controls.Add(lblRevenueValue);
            cardRevenue.Controls.Add(lblRevenueTitle);
            cardRevenue.Dock = DockStyle.Fill;
            cardRevenue.Location = new Point(14, 13);
            cardRevenue.Margin = new Padding(6, 3, 6, 3);
            cardRevenue.Name = "cardRevenue";
            cardRevenue.Padding = new Padding(12, 8, 12, 8);
            cardRevenue.Size = new Size(244, 95);
            cardRevenue.TabIndex = 0;
            //
            // lblRevenueValue
            //
            lblRevenueValue.Dock = DockStyle.Fill;
            lblRevenueValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblRevenueValue.ForeColor = Color.White;
            lblRevenueValue.Location = new Point(12, 33);
            lblRevenueValue.Name = "lblRevenueValue";
            lblRevenueValue.Size = new Size(220, 54);
            lblRevenueValue.TabIndex = 1;
            lblRevenueValue.Text = "0 đ";
            lblRevenueValue.TextAlign = ContentAlignment.MiddleLeft;
            //
            // lblRevenueTitle
            //
            lblRevenueTitle.Dock = DockStyle.Top;
            lblRevenueTitle.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblRevenueTitle.ForeColor = Color.WhiteSmoke;
            lblRevenueTitle.Location = new Point(12, 8);
            lblRevenueTitle.Name = "lblRevenueTitle";
            lblRevenueTitle.Size = new Size(220, 25);
            lblRevenueTitle.TabIndex = 0;
            lblRevenueTitle.Text = "💰 DOANH THU HÔM NAY (VNĐ)";
            //
            // cardOrders
            //
            cardOrders.BackColor = Color.FromArgb(41, 100, 180);
            cardOrders.Controls.Add(lblOrdersValue);
            cardOrders.Controls.Add(lblOrdersTitle);
            cardOrders.Dock = DockStyle.Fill;
            cardOrders.Location = new Point(270, 13);
            cardOrders.Margin = new Padding(6, 3, 6, 3);
            cardOrders.Name = "cardOrders";
            cardOrders.Padding = new Padding(12, 8, 12, 8);
            cardOrders.Size = new Size(244, 95);
            cardOrders.TabIndex = 1;
            //
            // lblOrdersValue
            //
            lblOrdersValue.Dock = DockStyle.Fill;
            lblOrdersValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblOrdersValue.ForeColor = Color.White;
            lblOrdersValue.Location = new Point(12, 33);
            lblOrdersValue.Name = "lblOrdersValue";
            lblOrdersValue.Size = new Size(220, 54);
            lblOrdersValue.TabIndex = 1;
            lblOrdersValue.Text = "0";
            lblOrdersValue.TextAlign = ContentAlignment.MiddleLeft;
            //
            // lblOrdersTitle
            //
            lblOrdersTitle.Dock = DockStyle.Top;
            lblOrdersTitle.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblOrdersTitle.ForeColor = Color.WhiteSmoke;
            lblOrdersTitle.Location = new Point(12, 8);
            lblOrdersTitle.Name = "lblOrdersTitle";
            lblOrdersTitle.Size = new Size(220, 25);
            lblOrdersTitle.TabIndex = 0;
            lblOrdersTitle.Text = "🧾 TỔNG SỐ HÓA ĐƠN ĐÃ XUẤT";
            //
            // cardNewCustomers
            //
            cardNewCustomers.BackColor = Color.FromArgb(0, 192, 192);
            cardNewCustomers.Controls.Add(lblNewCustomersValue);
            cardNewCustomers.Controls.Add(lblNewCustomersTitle);
            cardNewCustomers.Dock = DockStyle.Fill;
            cardNewCustomers.Location = new Point(526, 13);
            cardNewCustomers.Margin = new Padding(6, 3, 6, 3);
            cardNewCustomers.Name = "cardNewCustomers";
            cardNewCustomers.Padding = new Padding(12, 8, 12, 8);
            cardNewCustomers.Size = new Size(244, 95);
            cardNewCustomers.TabIndex = 2;
            //
            // lblNewCustomersValue
            //
            lblNewCustomersValue.Dock = DockStyle.Fill;
            lblNewCustomersValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblNewCustomersValue.ForeColor = Color.White;
            lblNewCustomersValue.Location = new Point(12, 33);
            lblNewCustomersValue.Name = "lblNewCustomersValue";
            lblNewCustomersValue.Size = new Size(220, 54);
            lblNewCustomersValue.TabIndex = 1;
            lblNewCustomersValue.Text = "0";
            lblNewCustomersValue.TextAlign = ContentAlignment.MiddleLeft;
            //
            // lblNewCustomersTitle
            //
            lblNewCustomersTitle.Dock = DockStyle.Top;
            lblNewCustomersTitle.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblNewCustomersTitle.ForeColor = Color.WhiteSmoke;
            lblNewCustomersTitle.Location = new Point(12, 8);
            lblNewCustomersTitle.Name = "lblNewCustomersTitle";
            lblNewCustomersTitle.Size = new Size(220, 25);
            lblNewCustomersTitle.TabIndex = 0;
            lblNewCustomersTitle.Text = "👥 KHÁCH HÀNG MỚI HÔM NAY";
            //
            // cardLowStock
            //
            cardLowStock.BackColor = Color.FromArgb(255, 153, 0);
            cardLowStock.Controls.Add(lblLowStockValue);
            cardLowStock.Controls.Add(lblLowStockTitle);
            cardLowStock.Dock = DockStyle.Fill;
            cardLowStock.Location = new Point(782, 13);
            cardLowStock.Margin = new Padding(6, 3, 6, 3);
            cardLowStock.Name = "cardLowStock";
            cardLowStock.Padding = new Padding(12, 8, 12, 8);
            cardLowStock.Size = new Size(244, 95);
            cardLowStock.TabIndex = 3;
            //
            // lblLowStockValue
            //
            lblLowStockValue.Dock = DockStyle.Fill;
            lblLowStockValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblLowStockValue.ForeColor = Color.White;
            lblLowStockValue.Location = new Point(12, 33);
            lblLowStockValue.Name = "lblLowStockValue";
            lblLowStockValue.Size = new Size(220, 54);
            lblLowStockValue.TabIndex = 1;
            lblLowStockValue.Text = "0";
            lblLowStockValue.TextAlign = ContentAlignment.MiddleLeft;
            //
            // lblLowStockTitle
            //
            lblLowStockTitle.Dock = DockStyle.Top;
            lblLowStockTitle.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblLowStockTitle.ForeColor = Color.WhiteSmoke;
            lblLowStockTitle.Location = new Point(12, 8);
            lblLowStockTitle.Name = "lblLowStockTitle";
            lblLowStockTitle.Size = new Size(220, 25);
            lblLowStockTitle.TabIndex = 0;
            lblLowStockTitle.Text = "⚠ MẶT HÀNG SẮP HẾT (≤ 10)";
            //
            // panelFilter
            //
            panelFilter.BackColor = Color.White;
            panelFilter.BorderStyle = BorderStyle.FixedSingle;
            panelFilter.Controls.Add(btnExportRevenue);
            panelFilter.Controls.Add(btnRefresh);
            panelFilter.Controls.Add(btnViewChart);
            panelFilter.Controls.Add(dtpToDate);
            panelFilter.Controls.Add(lblToDate);
            panelFilter.Controls.Add(dtpFromDate);
            panelFilter.Controls.Add(lblFromDate);
            panelFilter.Dock = DockStyle.Top;
            panelFilter.Location = new Point(0, 115);
            panelFilter.Name = "panelFilter";
            panelFilter.Size = new Size(1040, 56);
            panelFilter.TabIndex = 1;
            //
            // btnExportRevenue
            //
            btnExportRevenue.BackColor = Color.FromArgb(33, 115, 70);
            btnExportRevenue.Cursor = Cursors.Hand;
            btnExportRevenue.FlatAppearance.BorderSize = 0;
            btnExportRevenue.FlatStyle = FlatStyle.Flat;
            btnExportRevenue.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnExportRevenue.ForeColor = Color.White;
            btnExportRevenue.Location = new Point(824, 11);
            btnExportRevenue.Name = "btnExportRevenue";
            btnExportRevenue.Size = new Size(200, 32);
            btnExportRevenue.TabIndex = 6;
            btnExportRevenue.Text = "📥 Xuất Excel doanh thu";
            btnExportRevenue.UseVisualStyleBackColor = false;
            btnExportRevenue.Click += btnExportRevenue_Click;
            //
            // btnRefresh
            //
            btnRefresh.BackColor = Color.FromArgb(108, 117, 125);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(690, 11);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(126, 32);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "🔄 Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            //
            // btnViewChart
            //
            btnViewChart.BackColor = Color.FromArgb(0, 192, 192);
            btnViewChart.Cursor = Cursors.Hand;
            btnViewChart.FlatAppearance.BorderSize = 0;
            btnViewChart.FlatStyle = FlatStyle.Flat;
            btnViewChart.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnViewChart.ForeColor = Color.White;
            btnViewChart.Location = new Point(530, 11);
            btnViewChart.Name = "btnViewChart";
            btnViewChart.Size = new Size(152, 32);
            btnViewChart.TabIndex = 4;
            btnViewChart.Text = "📊 Xem biểu đồ";
            btnViewChart.UseVisualStyleBackColor = false;
            btnViewChart.Click += btnViewChart_Click;
            //
            // dtpToDate
            //
            dtpToDate.CustomFormat = "dd/MM/yyyy";
            dtpToDate.Font = new Font("Segoe UI", 9.75F);
            dtpToDate.Format = DateTimePickerFormat.Custom;
            dtpToDate.Location = new Point(365, 14);
            dtpToDate.Name = "dtpToDate";
            dtpToDate.Size = new Size(140, 25);
            dtpToDate.TabIndex = 3;
            //
            // lblToDate
            //
            lblToDate.AutoSize = true;
            lblToDate.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblToDate.ForeColor = Color.FromArgb(70, 80, 95);
            lblToDate.Location = new Point(285, 18);
            lblToDate.Name = "lblToDate";
            lblToDate.Size = new Size(70, 17);
            lblToDate.TabIndex = 2;
            lblToDate.Text = "Đến ngày:";
            //
            // dtpFromDate
            //
            dtpFromDate.CustomFormat = "dd/MM/yyyy";
            dtpFromDate.Font = new Font("Segoe UI", 9.75F);
            dtpFromDate.Format = DateTimePickerFormat.Custom;
            dtpFromDate.Location = new Point(100, 14);
            dtpFromDate.Name = "dtpFromDate";
            dtpFromDate.Size = new Size(140, 25);
            dtpFromDate.TabIndex = 1;
            //
            // lblFromDate
            //
            lblFromDate.AutoSize = true;
            lblFromDate.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblFromDate.ForeColor = Color.FromArgb(70, 80, 95);
            lblFromDate.Location = new Point(20, 18);
            lblFromDate.Name = "lblFromDate";
            lblFromDate.Size = new Size(62, 17);
            lblFromDate.TabIndex = 0;
            lblFromDate.Text = "Từ ngày:";
            //
            // panelContent
            //
            panelContent.Controls.Add(chartRevenue);
            panelContent.Controls.Add(groupTopProducts);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 171);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(12);
            panelContent.Size = new Size(1040, 489);
            panelContent.TabIndex = 2;
            //
            // chartRevenue
            //
            chartRevenue.BackColor = Color.White;
            chartArea1.AxisX.Interval = 1D;
            chartArea1.AxisX.LabelStyle.Angle = -45;
            chartArea1.AxisX.MajorGrid.LineColor = Color.Gainsboro;
            chartArea1.AxisX.Title = "Ngày bán";
            chartArea1.AxisY.LabelStyle.Format = "N0";
            chartArea1.AxisY.MajorGrid.LineColor = Color.Gainsboro;
            chartArea1.AxisY.Title = "Doanh thu (VNĐ)";
            chartArea1.Name = "ChartArea1";
            chartRevenue.ChartAreas.Add(chartArea1);
            chartRevenue.Dock = DockStyle.Fill;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Name = "Legend1";
            chartRevenue.Legends.Add(legend1);
            chartRevenue.Location = new Point(12, 12);
            chartRevenue.Name = "chartRevenue";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            series1.Color = Color.FromArgb(41, 100, 180);
            series1.LabelFormat = "N0";
            series1.Legend = "Legend1";
            series1.LegendText = "Doanh thu theo ngày";
            series1.Name = "DoanhThu";
            series1.ToolTip = "#VALX: #VALY{N0} đ";
            chartRevenue.Series.Add(series1);
            chartRevenue.Size = new Size(626, 465);
            chartRevenue.TabIndex = 0;
            title1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            title1.Name = "Title1";
            title1.Text = "BIỂU ĐỒ DOANH THU THEO NGÀY";
            chartRevenue.Titles.Add(title1);
            //
            // groupTopProducts
            //
            groupTopProducts.BackColor = Color.White;
            groupTopProducts.Controls.Add(dgvTopProducts);
            groupTopProducts.Dock = DockStyle.Right;
            groupTopProducts.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            groupTopProducts.Location = new Point(638, 12);
            groupTopProducts.Name = "groupTopProducts";
            groupTopProducts.Padding = new Padding(8);
            groupTopProducts.Size = new Size(390, 465);
            groupTopProducts.TabIndex = 1;
            groupTopProducts.TabStop = false;
            groupTopProducts.Text = "🏆 TOP 5 SẢN PHẨM BÁN CHẠY NHẤT";
            //
            // dgvTopProducts
            //
            dgvTopProducts.AllowUserToAddRows = false;
            dgvTopProducts.AllowUserToDeleteRows = false;
            dgvTopProducts.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(24, 30, 48);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvTopProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvTopProducts.ColumnHeadersHeight = 34;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvTopProducts.DefaultCellStyle = dataGridViewCellStyle2;
            dgvTopProducts.Dock = DockStyle.Fill;
            dgvTopProducts.EnableHeadersVisualStyles = false;
            dgvTopProducts.Location = new Point(8, 26);
            dgvTopProducts.MultiSelect = false;
            dgvTopProducts.Name = "dgvTopProducts";
            dgvTopProducts.ReadOnly = true;
            dgvTopProducts.RowHeadersVisible = false;
            dgvTopProducts.RowTemplate.Height = 30;
            dgvTopProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTopProducts.Size = new Size(374, 431);
            dgvTopProducts.TabIndex = 0;
            //
            // FormDashboard
            //
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 245, 247);
            ClientSize = new Size(1040, 660);
            Controls.Add(panelContent);
            Controls.Add(panelFilter);
            Controls.Add(tableCards);
            Font = new Font("Segoe UI", 9.75F);
            Name = "FormDashboard";
            Text = "FormDashboard";
            Load += FormDashboard_Load;
            tableCards.ResumeLayout(false);
            cardRevenue.ResumeLayout(false);
            cardOrders.ResumeLayout(false);
            cardNewCustomers.ResumeLayout(false);
            cardLowStock.ResumeLayout(false);
            panelFilter.ResumeLayout(false);
            panelFilter.PerformLayout();
            panelContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartRevenue).EndInit();
            groupTopProducts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTopProducts).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableCards;
        private Panel cardRevenue;
        private Label lblRevenueValue;
        private Label lblRevenueTitle;
        private Panel cardOrders;
        private Label lblOrdersValue;
        private Label lblOrdersTitle;
        private Panel cardNewCustomers;
        private Label lblNewCustomersValue;
        private Label lblNewCustomersTitle;
        private Panel cardLowStock;
        private Label lblLowStockValue;
        private Label lblLowStockTitle;
        private Panel panelFilter;
        private Label lblFromDate;
        private DateTimePicker dtpFromDate;
        private Label lblToDate;
        private DateTimePicker dtpToDate;
        private Button btnViewChart;
        private Button btnRefresh;
        private Button btnExportRevenue;
        private Panel panelContent;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRevenue;
        private GroupBox groupTopProducts;
        private DataGridView dgvTopProducts;
    }
}
