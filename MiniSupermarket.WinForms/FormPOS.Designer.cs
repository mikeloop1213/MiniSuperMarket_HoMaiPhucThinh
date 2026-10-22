namespace MiniSupermarket.WinForms
{
    partial class FormPOS
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            panelCart = new Panel();
            dgvCart = new DataGridView();
            panelCartActions = new Panel();
            btnRemoveItem = new Button();
            btnClearCart = new Button();
            panelBarcode = new Panel();
            txtBarcode = new TextBox();
            lblBarcode = new Label();
            panelCheckout = new Panel();
            btnCheckout = new Button();
            lblChange = new Label();
            lblChangeTitle = new Label();
            txtCashReceived = new TextBox();
            lblCashReceivedTitle = new Label();
            panelQuickCash = new FlowLayoutPanel();
            btnCashExact = new Button();
            btnCash50k = new Button();
            btnCash100k = new Button();
            btnCash200k = new Button();
            btnCash500k = new Button();
            lblTotalAmount = new Label();
            lblTotalTitle = new Label();
            groupCustomer = new GroupBox();
            btnLookupCustomer = new Button();
            lblCustomerPoints = new Label();
            lblCustomerName = new Label();
            txtCustomerPhone = new TextBox();
            lblPhone = new Label();
            panelCart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCart).BeginInit();
            panelCartActions.SuspendLayout();
            panelBarcode.SuspendLayout();
            panelCheckout.SuspendLayout();
            panelQuickCash.SuspendLayout();
            groupCustomer.SuspendLayout();
            SuspendLayout();
            // 
            // panelCart
            // 
            panelCart.Controls.Add(dgvCart);
            panelCart.Controls.Add(panelCartActions);
            panelCart.Controls.Add(panelBarcode);
            panelCart.Dock = DockStyle.Fill;
            panelCart.Location = new Point(0, 0);
            panelCart.Name = "panelCart";
            panelCart.Padding = new Padding(15);
            panelCart.Size = new Size(684, 660);
            panelCart.TabIndex = 0;
            // 
            // dgvCart
            // 
            dgvCart.AllowUserToAddRows = false;
            dgvCart.AllowUserToDeleteRows = false;
            dgvCart.BackgroundColor = Color.White;
            dgvCart.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(24, 30, 48);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvCart.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvCart.ColumnHeadersHeight = 35;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.75F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvCart.DefaultCellStyle = dataGridViewCellStyle2;
            dgvCart.Dock = DockStyle.Fill;
            dgvCart.EnableHeadersVisualStyles = false;
            dgvCart.Location = new Point(15, 80);
            dgvCart.MultiSelect = false;
            dgvCart.Name = "dgvCart";
            dgvCart.ReadOnly = true;
            dgvCart.RowHeadersVisible = false;
            dgvCart.RowTemplate.Height = 32;
            dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCart.Size = new Size(654, 515);
            dgvCart.TabIndex = 1;
            dgvCart.CellContentClick += dgvCart_CellContentClick;
            // 
            // panelCartActions
            // 
            panelCartActions.Controls.Add(btnRemoveItem);
            panelCartActions.Controls.Add(btnClearCart);
            panelCartActions.Dock = DockStyle.Bottom;
            panelCartActions.Location = new Point(15, 595);
            panelCartActions.Name = "panelCartActions";
            panelCartActions.Padding = new Padding(0, 10, 0, 0);
            panelCartActions.Size = new Size(654, 50);
            panelCartActions.TabIndex = 2;
            // 
            // btnRemoveItem
            // 
            btnRemoveItem.BackColor = Color.FromArgb(108, 117, 125);
            btnRemoveItem.Cursor = Cursors.Hand;
            btnRemoveItem.FlatAppearance.BorderSize = 0;
            btnRemoveItem.FlatStyle = FlatStyle.Flat;
            btnRemoveItem.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnRemoveItem.ForeColor = Color.White;
            btnRemoveItem.Location = new Point(335, 8);
            btnRemoveItem.Name = "btnRemoveItem";
            btnRemoveItem.Size = new Size(316, 35);
            btnRemoveItem.TabIndex = 1;
            btnRemoveItem.Text = "🗑  Xóa sản phẩm chọn";
            btnRemoveItem.UseVisualStyleBackColor = false;
            btnRemoveItem.Click += btnRemoveItem_Click;
            // 
            // btnClearCart
            // 
            btnClearCart.BackColor = Color.FromArgb(220, 53, 69);
            btnClearCart.Cursor = Cursors.Hand;
            btnClearCart.FlatAppearance.BorderSize = 0;
            btnClearCart.FlatStyle = FlatStyle.Flat;
            btnClearCart.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnClearCart.ForeColor = Color.White;
            btnClearCart.Location = new Point(3, 8);
            btnClearCart.Name = "btnClearCart";
            btnClearCart.Size = new Size(316, 35);
            btnClearCart.TabIndex = 0;
            btnClearCart.Text = "❌  Hủy toàn bộ giỏ";
            btnClearCart.UseVisualStyleBackColor = false;
            btnClearCart.Click += btnClearCart_Click;
            // 
            // panelBarcode
            // 
            panelBarcode.BackColor = Color.White;
            panelBarcode.BorderStyle = BorderStyle.FixedSingle;
            panelBarcode.Controls.Add(txtBarcode);
            panelBarcode.Controls.Add(lblBarcode);
            panelBarcode.Dock = DockStyle.Top;
            panelBarcode.Location = new Point(15, 15);
            panelBarcode.Name = "panelBarcode";
            panelBarcode.Padding = new Padding(12);
            panelBarcode.Size = new Size(654, 65);
            panelBarcode.TabIndex = 0;
            // 
            // txtBarcode
            // 
            txtBarcode.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBarcode.Font = new Font("Segoe UI", 12F);
            txtBarcode.Location = new Point(263, 15);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.PlaceholderText = "Nhập mã vạch Barcode (Ví dụ: 893500123001)...";
            txtBarcode.Size = new Size(374, 29);
            txtBarcode.TabIndex = 1;
            txtBarcode.KeyDown += txtBarcode_KeyDown;
            // 
            // lblBarcode
            // 
            lblBarcode.AutoSize = true;
            lblBarcode.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            lblBarcode.ForeColor = Color.FromArgb(24, 30, 48);
            lblBarcode.Location = new Point(108, 19);
            lblBarcode.Name = "lblBarcode";
            lblBarcode.Size = new Size(150, 19);
            lblBarcode.TabIndex = 0;
            lblBarcode.Text = "Quét mã SP (Barcode):";
            // 
            // panelCheckout
            // 
            panelCheckout.BackColor = Color.White;
            panelCheckout.BorderStyle = BorderStyle.FixedSingle;
            panelCheckout.Controls.Add(btnCheckout);
            panelCheckout.Controls.Add(lblChange);
            panelCheckout.Controls.Add(lblChangeTitle);
            panelCheckout.Controls.Add(txtCashReceived);
            panelCheckout.Controls.Add(lblCashReceivedTitle);
            panelCheckout.Controls.Add(panelQuickCash);
            panelCheckout.Controls.Add(lblTotalAmount);
            panelCheckout.Controls.Add(lblTotalTitle);
            panelCheckout.Controls.Add(groupCustomer);
            panelCheckout.Dock = DockStyle.Right;
            panelCheckout.Location = new Point(684, 0);
            panelCheckout.Name = "panelCheckout";
            panelCheckout.Padding = new Padding(15);
            panelCheckout.Size = new Size(356, 660);
            panelCheckout.TabIndex = 1;
            // 
            // btnCheckout
            // 
            btnCheckout.BackColor = Color.FromArgb(0, 192, 192);
            btnCheckout.Cursor = Cursors.Hand;
            btnCheckout.Dock = DockStyle.Bottom;
            btnCheckout.FlatAppearance.BorderSize = 0;
            btnCheckout.FlatStyle = FlatStyle.Flat;
            btnCheckout.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnCheckout.ForeColor = Color.White;
            btnCheckout.Location = new Point(15, 591);
            btnCheckout.Name = "btnCheckout";
            btnCheckout.Size = new Size(324, 52);
            btnCheckout.TabIndex = 8;
            btnCheckout.Text = "💳  THANH TOÁN (F9)";
            btnCheckout.UseVisualStyleBackColor = false;
            btnCheckout.Click += btnCheckout_Click;
            // 
            // lblChange
            // 
            lblChange.Dock = DockStyle.Top;
            lblChange.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblChange.ForeColor = Color.FromArgb(40, 167, 69);
            lblChange.Location = new Point(15, 521);
            lblChange.Name = "lblChange";
            lblChange.Size = new Size(324, 35);
            lblChange.TabIndex = 7;
            lblChange.Text = "0 đ";
            lblChange.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblChangeTitle
            // 
            lblChangeTitle.AutoSize = true;
            lblChangeTitle.Dock = DockStyle.Top;
            lblChangeTitle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblChangeTitle.ForeColor = Color.FromArgb(100, 110, 125);
            lblChangeTitle.Location = new Point(15, 499);
            lblChangeTitle.Name = "lblChangeTitle";
            lblChangeTitle.Padding = new Padding(0, 5, 0, 0);
            lblChangeTitle.Size = new Size(129, 22);
            lblChangeTitle.TabIndex = 6;
            lblChangeTitle.Text = "Tiền thừa trả khách:";
            // 
            // txtCashReceived
            // 
            txtCashReceived.Dock = DockStyle.Top;
            txtCashReceived.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            txtCashReceived.Location = new Point(15, 468);
            txtCashReceived.Name = "txtCashReceived";
            txtCashReceived.PlaceholderText = "Nhập số tiền khách đưa...";
            txtCashReceived.Size = new Size(324, 31);
            txtCashReceived.TabIndex = 5;
            txtCashReceived.TextAlign = HorizontalAlignment.Right;
            txtCashReceived.TextChanged += txtCashReceived_TextChanged;
            // 
            // lblCashReceivedTitle
            // 
            lblCashReceivedTitle.AutoSize = true;
            lblCashReceivedTitle.Dock = DockStyle.Top;
            lblCashReceivedTitle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblCashReceivedTitle.ForeColor = Color.FromArgb(100, 110, 125);
            lblCashReceivedTitle.Location = new Point(15, 441);
            lblCashReceivedTitle.Name = "lblCashReceivedTitle";
            lblCashReceivedTitle.Padding = new Padding(0, 10, 0, 0);
            lblCashReceivedTitle.Size = new Size(142, 27);
            lblCashReceivedTitle.TabIndex = 4;
            lblCashReceivedTitle.Text = "Tiền khách đưa (VNĐ):";
            // 
            // panelQuickCash
            // 
            panelQuickCash.Controls.Add(btnCashExact);
            panelQuickCash.Controls.Add(btnCash50k);
            panelQuickCash.Controls.Add(btnCash100k);
            panelQuickCash.Controls.Add(btnCash200k);
            panelQuickCash.Controls.Add(btnCash500k);
            panelQuickCash.Dock = DockStyle.Top;
            panelQuickCash.Location = new Point(15, 260);
            panelQuickCash.Name = "panelQuickCash";
            panelQuickCash.Padding = new Padding(0, 5, 0, 0);
            panelQuickCash.Size = new Size(324, 181);
            panelQuickCash.TabIndex = 3;
            // 
            // btnCashExact
            // 
            btnCashExact.BackColor = Color.FromArgb(240, 242, 245);
            btnCashExact.Cursor = Cursors.Hand;
            btnCashExact.FlatAppearance.BorderColor = Color.FromArgb(200, 205, 215);
            btnCashExact.FlatStyle = FlatStyle.Flat;
            btnCashExact.Font = new Font("Segoe UI", 8.25F);
            btnCashExact.Location = new Point(3, 8);
            btnCashExact.Name = "btnCashExact";
            btnCashExact.Size = new Size(318, 28);
            btnCashExact.TabIndex = 0;
            btnCashExact.Text = "Vừa đủ";
            btnCashExact.UseVisualStyleBackColor = false;
            btnCashExact.Click += btnCashExact_Click;
            // 
            // btnCash50k
            // 
            btnCash50k.BackColor = Color.FromArgb(240, 242, 245);
            btnCash50k.Cursor = Cursors.Hand;
            btnCash50k.FlatAppearance.BorderColor = Color.FromArgb(200, 205, 215);
            btnCash50k.FlatStyle = FlatStyle.Flat;
            btnCash50k.Font = new Font("Segoe UI", 8.25F);
            btnCash50k.Location = new Point(3, 42);
            btnCash50k.Name = "btnCash50k";
            btnCash50k.Size = new Size(318, 28);
            btnCash50k.TabIndex = 1;
            btnCash50k.Text = "50.000";
            btnCash50k.UseVisualStyleBackColor = false;
            btnCash50k.Click += btnCashQuick_Click;
            // 
            // btnCash100k
            // 
            btnCash100k.BackColor = Color.FromArgb(240, 242, 245);
            btnCash100k.Cursor = Cursors.Hand;
            btnCash100k.FlatAppearance.BorderColor = Color.FromArgb(200, 205, 215);
            btnCash100k.FlatStyle = FlatStyle.Flat;
            btnCash100k.Font = new Font("Segoe UI", 8.25F);
            btnCash100k.Location = new Point(3, 76);
            btnCash100k.Name = "btnCash100k";
            btnCash100k.Size = new Size(318, 28);
            btnCash100k.TabIndex = 2;
            btnCash100k.Text = "100.000";
            btnCash100k.UseVisualStyleBackColor = false;
            btnCash100k.Click += btnCashQuick_Click;
            // 
            // btnCash200k
            // 
            btnCash200k.BackColor = Color.FromArgb(240, 242, 245);
            btnCash200k.Cursor = Cursors.Hand;
            btnCash200k.FlatAppearance.BorderColor = Color.FromArgb(200, 205, 215);
            btnCash200k.FlatStyle = FlatStyle.Flat;
            btnCash200k.Font = new Font("Segoe UI", 8.25F);
            btnCash200k.Location = new Point(3, 110);
            btnCash200k.Name = "btnCash200k";
            btnCash200k.Size = new Size(318, 28);
            btnCash200k.TabIndex = 3;
            btnCash200k.Text = "200.000";
            btnCash200k.UseVisualStyleBackColor = false;
            btnCash200k.Click += btnCashQuick_Click;
            // 
            // btnCash500k
            // 
            btnCash500k.BackColor = Color.FromArgb(240, 242, 245);
            btnCash500k.Cursor = Cursors.Hand;
            btnCash500k.FlatAppearance.BorderColor = Color.FromArgb(200, 205, 215);
            btnCash500k.FlatStyle = FlatStyle.Flat;
            btnCash500k.Font = new Font("Segoe UI", 8.25F);
            btnCash500k.Location = new Point(3, 144);
            btnCash500k.Name = "btnCash500k";
            btnCash500k.Size = new Size(318, 28);
            btnCash500k.TabIndex = 4;
            btnCash500k.Text = "500.000";
            btnCash500k.UseVisualStyleBackColor = false;
            btnCash500k.Click += btnCashQuick_Click;
            // 
            // lblTotalAmount
            // 
            lblTotalAmount.Dock = DockStyle.Top;
            lblTotalAmount.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTotalAmount.ForeColor = Color.FromArgb(220, 53, 69);
            lblTotalAmount.Location = new Point(15, 200);
            lblTotalAmount.Name = "lblTotalAmount";
            lblTotalAmount.Size = new Size(324, 60);
            lblTotalAmount.TabIndex = 2;
            lblTotalAmount.Text = "0 đ";
            lblTotalAmount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTotalTitle
            // 
            lblTotalTitle.AutoSize = true;
            lblTotalTitle.Dock = DockStyle.Top;
            lblTotalTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotalTitle.ForeColor = Color.FromArgb(24, 30, 48);
            lblTotalTitle.Location = new Point(15, 175);
            lblTotalTitle.Name = "lblTotalTitle";
            lblTotalTitle.Padding = new Padding(0, 6, 0, 0);
            lblTotalTitle.Size = new Size(185, 25);
            lblTotalTitle.TabIndex = 1;
            lblTotalTitle.Text = "TỔNG TIỀN THANH TOÁN:";
            // 
            // groupCustomer
            // 
            groupCustomer.Controls.Add(btnLookupCustomer);
            groupCustomer.Controls.Add(lblCustomerPoints);
            groupCustomer.Controls.Add(lblCustomerName);
            groupCustomer.Controls.Add(txtCustomerPhone);
            groupCustomer.Controls.Add(lblPhone);
            groupCustomer.Dock = DockStyle.Top;
            groupCustomer.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            groupCustomer.Location = new Point(15, 15);
            groupCustomer.Name = "groupCustomer";
            groupCustomer.Padding = new Padding(10);
            groupCustomer.Size = new Size(324, 160);
            groupCustomer.TabIndex = 0;
            groupCustomer.TabStop = false;
            groupCustomer.Text = "👤 Khách hàng thành viên";
            // 
            // btnLookupCustomer
            // 
            btnLookupCustomer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLookupCustomer.BackColor = Color.FromArgb(0, 192, 192);
            btnLookupCustomer.Cursor = Cursors.Hand;
            btnLookupCustomer.FlatAppearance.BorderSize = 0;
            btnLookupCustomer.FlatStyle = FlatStyle.Flat;
            btnLookupCustomer.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnLookupCustomer.ForeColor = Color.White;
            btnLookupCustomer.Location = new Point(234, 46);
            btnLookupCustomer.Name = "btnLookupCustomer";
            btnLookupCustomer.Size = new Size(77, 26);
            btnLookupCustomer.TabIndex = 2;
            btnLookupCustomer.Text = "Tìm";
            btnLookupCustomer.UseVisualStyleBackColor = false;
            btnLookupCustomer.Click += btnLookupCustomer_Click;
            // 
            // lblCustomerPoints
            // 
            lblCustomerPoints.AutoSize = true;
            lblCustomerPoints.Font = new Font("Segoe UI", 9F);
            lblCustomerPoints.ForeColor = Color.FromArgb(100, 110, 125);
            lblCustomerPoints.Location = new Point(10, 115);
            lblCustomerPoints.Name = "lblCustomerPoints";
            lblCustomerPoints.Size = new Size(119, 15);
            lblCustomerPoints.TabIndex = 4;
            lblCustomerPoints.Text = "Điểm tích lũy: 0 điểm";
            // 
            // lblCustomerName
            // 
            lblCustomerName.AutoSize = true;
            lblCustomerName.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            lblCustomerName.ForeColor = Color.Red;
            lblCustomerName.Location = new Point(10, 88);
            lblCustomerName.Name = "lblCustomerName";
            lblCustomerName.Size = new Size(101, 19);
            lblCustomerName.TabIndex = 3;
            lblCustomerName.Text = "Khách vãng lai";
            // 
            // txtCustomerPhone
            // 
            txtCustomerPhone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCustomerPhone.Font = new Font("Segoe UI", 9.5F);
            txtCustomerPhone.Location = new Point(10, 47);
            txtCustomerPhone.Name = "txtCustomerPhone";
            txtCustomerPhone.PlaceholderText = "SĐT (VD: 0903456789)...";
            txtCustomerPhone.Size = new Size(218, 24);
            txtCustomerPhone.TabIndex = 1;
            txtCustomerPhone.KeyDown += txtCustomerPhone_KeyDown;
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.Font = new Font("Segoe UI", 9F);
            lblPhone.ForeColor = Color.FromArgb(70, 80, 95);
            lblPhone.Location = new Point(10, 26);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(79, 15);
            lblPhone.TabIndex = 0;
            lblPhone.Text = "Số điện thoại:";
            // 
            // FormPOS
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 245, 247);
            ClientSize = new Size(1040, 660);
            Controls.Add(panelCart);
            Controls.Add(panelCheckout);
            Font = new Font("Segoe UI", 9.75F);
            KeyPreview = true;
            Name = "FormPOS";
            Text = "FormPOS";
            KeyDown += FormPOS_KeyDown;
            panelCart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCart).EndInit();
            panelCartActions.ResumeLayout(false);
            panelBarcode.ResumeLayout(false);
            panelBarcode.PerformLayout();
            panelCheckout.ResumeLayout(false);
            panelCheckout.PerformLayout();
            panelQuickCash.ResumeLayout(false);
            groupCustomer.ResumeLayout(false);
            groupCustomer.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelCart;
        private DataGridView dgvCart;
        private Panel panelCartActions;
        private Button btnRemoveItem;
        private Button btnClearCart;
        private Panel panelBarcode;
        private TextBox txtBarcode;
        private Label lblBarcode;
        private Panel panelCheckout;
        private GroupBox groupCustomer;
        private Button btnLookupCustomer;
        private Label lblCustomerPoints;
        private Label lblCustomerName;
        private TextBox txtCustomerPhone;
        private Label lblPhone;
        private Label lblTotalTitle;
        private Label lblTotalAmount;
        private FlowLayoutPanel panelQuickCash;
        private Button btnCashExact;
        private Button btnCash50k;
        private Button btnCash100k;
        private Button btnCash200k;
        private Button btnCash500k;
        private Label lblCashReceivedTitle;
        private TextBox txtCashReceived;
        private Label lblChangeTitle;
        private Label lblChange;
        private Button btnCheckout;
    }
}
