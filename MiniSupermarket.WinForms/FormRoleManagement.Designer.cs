namespace MiniSupermarket.WinForms
{
    partial class FormRoleManagement
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
            grpList = new GroupBox();
            dgvRoles = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colRoleName = new DataGridViewTextBoxColumn();
            colDescription = new DataGridViewTextBoxColumn();
            grpInfo = new GroupBox();
            btnLoad = new Button();
            btnDelete = new Button();
            btnUpdate = new Button();
            btnAdd = new Button();
            txtDescription = new TextBox();
            lblDescription = new Label();
            txtRoleName = new TextBox();
            lblRoleName = new Label();
            txtId = new TextBox();
            lblId = new Label();
            statusStrip1 = new StatusStrip();
            lblReady = new ToolStripStatusLabel();
            grpList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRoles).BeginInit();
            grpInfo.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // grpList
            // 
            grpList.Controls.Add(dgvRoles);
            grpList.Location = new Point(12, 12);
            grpList.Name = "grpList";
            grpList.Size = new Size(475, 410);
            grpList.TabIndex = 0;
            grpList.TabStop = false;
            grpList.Text = "Danh sách Vai trò";
            // 
            // dgvRoles
            // 
            dgvRoles.AllowUserToAddRows = false;
            dgvRoles.AllowUserToDeleteRows = false;
            dgvRoles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRoles.BackgroundColor = SystemColors.Window;
            dgvRoles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRoles.Columns.AddRange(new DataGridViewColumn[] { colId, colRoleName, colDescription });
            dgvRoles.Dock = DockStyle.Fill;
            dgvRoles.Location = new Point(3, 19);
            dgvRoles.MultiSelect = false;
            dgvRoles.Name = "dgvRoles";
            dgvRoles.ReadOnly = true;
            dgvRoles.RowHeadersVisible = false;
            dgvRoles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRoles.Size = new Size(469, 388);
            dgvRoles.TabIndex = 0;
            dgvRoles.CellClick += dgvRoles_CellClick;
            // 
            // colId
            // 
            colId.DataPropertyName = "Id";
            colId.FillWeight = 25F;
            colId.HeaderText = "Mã ID";
            colId.Name = "colId";
            colId.ReadOnly = true;
            // 
            // colRoleName
            // 
            colRoleName.DataPropertyName = "RoleName";
            colRoleName.FillWeight = 45F;
            colRoleName.HeaderText = "Tên Vai trò";
            colRoleName.Name = "colRoleName";
            colRoleName.ReadOnly = true;
            // 
            // colDescription
            // 
            colDescription.DataPropertyName = "Description";
            colDescription.FillWeight = 60F;
            colDescription.HeaderText = "Mô Tả";
            colDescription.Name = "colDescription";
            colDescription.ReadOnly = true;
            // 
            // grpInfo
            // 
            grpInfo.Controls.Add(btnLoad);
            grpInfo.Controls.Add(btnDelete);
            grpInfo.Controls.Add(btnUpdate);
            grpInfo.Controls.Add(btnAdd);
            grpInfo.Controls.Add(txtDescription);
            grpInfo.Controls.Add(lblDescription);
            grpInfo.Controls.Add(txtRoleName);
            grpInfo.Controls.Add(lblRoleName);
            grpInfo.Controls.Add(txtId);
            grpInfo.Controls.Add(lblId);
            grpInfo.Location = new Point(495, 12);
            grpInfo.Name = "grpInfo";
            grpInfo.Size = new Size(307, 410);
            grpInfo.TabIndex = 1;
            grpInfo.TabStop = false;
            grpInfo.Text = "Thông tin Vai trò";
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(16, 320);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(279, 32);
            btnLoad.TabIndex = 6;
            btnLoad.Text = "Tải lại danh sách";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(215, 362);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(80, 34);
            btnDelete.TabIndex = 9;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(118, 362);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(80, 34);
            btnUpdate.TabIndex = 8;
            btnUpdate.Text = "Cập nhật";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(16, 362);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(80, 34);
            btnAdd.TabIndex = 7;
            btnAdd.Text = "Thêm mới";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(16, 172);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.PlaceholderText = "Mô tả quyền hạn vai trò...";
            txtDescription.ScrollBars = ScrollBars.Vertical;
            txtDescription.Size = new Size(279, 130);
            txtDescription.TabIndex = 5;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(16, 150);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(43, 15);
            lblDescription.TabIndex = 4;
            lblDescription.Text = "Mô Tả:";
            // 
            // txtRoleName
            // 
            txtRoleName.Font = new Font("Segoe UI", 9.75F);
            txtRoleName.Location = new Point(16, 108);
            txtRoleName.Name = "txtRoleName";
            txtRoleName.PlaceholderText = "Ví dụ: Admin, Cashier...";
            txtRoleName.Size = new Size(279, 25);
            txtRoleName.TabIndex = 3;
            // 
            // lblRoleName
            // 
            lblRoleName.AutoSize = true;
            lblRoleName.Location = new Point(16, 86);
            lblRoleName.Name = "lblRoleName";
            lblRoleName.Size = new Size(64, 15);
            lblRoleName.TabIndex = 2;
            lblRoleName.Text = "Tên Vai trò:";
            // 
            // txtId
            // 
            txtId.Location = new Point(16, 48);
            txtId.Name = "txtId";
            txtId.ReadOnly = true;
            txtId.Size = new Size(279, 23);
            txtId.TabIndex = 1;
            // 
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.Location = new Point(16, 26);
            lblId.Name = "lblId";
            lblId.Size = new Size(41, 15);
            lblId.TabIndex = 0;
            lblId.Text = "Mã ID:";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblReady });
            statusStrip1.Location = new Point(0, 431);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(802, 22);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblReady
            // 
            lblReady.Name = "lblReady";
            lblReady.Size = new Size(224, 17);
            lblReady.Text = "Sẵn sàng | Quản lý Vai trò nhân sự (Roles)";
            // 
            // FormRoleManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(802, 453);
            Controls.Add(statusStrip1);
            Controls.Add(grpInfo);
            Controls.Add(grpList);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormRoleManagement";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hệ thống quản lý siêu thị - Quản lý Vai trò (Roles)";
            Load += FormRoleManagement_Load;
            grpList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvRoles).EndInit();
            grpInfo.ResumeLayout(false);
            grpInfo.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox grpList;
        private System.Windows.Forms.DataGridView dgvRoles;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRoleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.GroupBox grpInfo;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtRoleName;
        private System.Windows.Forms.Label lblRoleName;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblReady;
    }
}
