namespace QlyCafe.Quanly
{
    partial class TimHDBan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.cboTimNV = new System.Windows.Forms.ComboBox();
            this.dtpTimDenNgay = new System.Windows.Forms.DateTimePicker();
            this.dtpTimTuNgay = new System.Windows.Forms.DateTimePicker();
            this.txtTimMaHDB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDong = new System.Windows.Forms.Button();
            this.btnXemChiTiet = new System.Windows.Forms.Button();
            this.dgvKQTimKiemHDB = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.lblSoLuongKQ = new System.Windows.Forms.Label();
            this.cboTimKH = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKQTimKiemHDB)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboTimKH);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnLamMoi);
            this.groupBox1.Controls.Add(this.btnTimKiem);
            this.groupBox1.Controls.Add(this.cboTimNV);
            this.groupBox1.Controls.Add(this.dtpTimDenNgay);
            this.groupBox1.Controls.Add(this.dtpTimTuNgay);
            this.groupBox1.Controls.Add(this.txtTimMaHDB);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1089, 244);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tiêu chí tìm kiếm";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDong);
            this.groupBox2.Controls.Add(this.btnXemChiTiet);
            this.groupBox2.Controls.Add(this.dgvKQTimKiemHDB);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.lblSoLuongKQ);
            this.groupBox2.Location = new System.Drawing.Point(13, 261);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1089, 380);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Danh sách kết quả";
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Location = new System.Drawing.Point(597, 178);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(75, 23);
            this.btnLamMoi.TabIndex = 23;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.Location = new System.Drawing.Point(495, 178);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(75, 23);
            this.btnTimKiem.TabIndex = 22;
            this.btnTimKiem.Text = "Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = true;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // cboTimNV
            // 
            this.cboTimNV.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTimNV.FormattingEnabled = true;
            this.cboTimNV.Location = new System.Drawing.Point(169, 126);
            this.cboTimNV.Name = "cboTimNV";
            this.cboTimNV.Size = new System.Drawing.Size(191, 24);
            this.cboTimNV.TabIndex = 21;
            // 
            // dtpTimDenNgay
            // 
            this.dtpTimDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dtpTimDenNgay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimDenNgay.Location = new System.Drawing.Point(495, 81);
            this.dtpTimDenNgay.Name = "dtpTimDenNgay";
            this.dtpTimDenNgay.Size = new System.Drawing.Size(191, 22);
            this.dtpTimDenNgay.TabIndex = 20;
            this.dtpTimDenNgay.ValueChanged += new System.EventHandler(this.dtpTimDenNgay_ValueChanged);
            // 
            // dtpTimTuNgay
            // 
            this.dtpTimTuNgay.CustomFormat = "dd/MM/yyyy";
            this.dtpTimTuNgay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimTuNgay.Location = new System.Drawing.Point(169, 81);
            this.dtpTimTuNgay.Name = "dtpTimTuNgay";
            this.dtpTimTuNgay.Size = new System.Drawing.Size(191, 22);
            this.dtpTimTuNgay.TabIndex = 18;
            this.dtpTimTuNgay.ValueChanged += new System.EventHandler(this.dtpTimTuNgay_ValueChanged);
            // 
            // txtTimMaHDB
            // 
            this.txtTimMaHDB.Location = new System.Drawing.Point(169, 43);
            this.txtTimMaHDB.Name = "txtTimMaHDB";
            this.txtTimMaHDB.Size = new System.Drawing.Size(191, 22);
            this.txtTimMaHDB.TabIndex = 17;
            this.txtTimMaHDB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTimMaHDB_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(75, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 16);
            this.label5.TabIndex = 16;
            this.label5.Text = "Nhân Viên";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(420, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 16);
            this.label4.TabIndex = 14;
            this.label4.Text = "Đến ngày";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(75, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "Từ ngày";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "Mã HĐB";
            // 
            // btnDong
            // 
            this.btnDong.Location = new System.Drawing.Point(131, 305);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(75, 23);
            this.btnDong.TabIndex = 18;
            this.btnDong.Text = "Đóng";
            this.btnDong.UseVisualStyleBackColor = true;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // btnXemChiTiet
            // 
            this.btnXemChiTiet.Location = new System.Drawing.Point(29, 305);
            this.btnXemChiTiet.Name = "btnXemChiTiet";
            this.btnXemChiTiet.Size = new System.Drawing.Size(86, 23);
            this.btnXemChiTiet.TabIndex = 17;
            this.btnXemChiTiet.Text = "Xem chi tiết";
            this.btnXemChiTiet.UseVisualStyleBackColor = true;
            this.btnXemChiTiet.Click += new System.EventHandler(this.btnXemChiTiet_Click);
            // 
            // dgvKQTimKiemHDB
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvKQTimKiemHDB.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvKQTimKiemHDB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvKQTimKiemHDB.DefaultCellStyle = dataGridViewCellStyle10;
            this.dgvKQTimKiemHDB.Location = new System.Drawing.Point(29, 30);
            this.dgvKQTimKiemHDB.Name = "dgvKQTimKiemHDB";
            this.dgvKQTimKiemHDB.RowHeadersWidth = 51;
            this.dgvKQTimKiemHDB.RowTemplate.Height = 24;
            this.dgvKQTimKiemHDB.Size = new System.Drawing.Size(1030, 181);
            this.dgvKQTimKiemHDB.TabIndex = 16;
            this.dgvKQTimKiemHDB.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvKQTimKiemHDB_CellDoubleClick);
            this.dgvKQTimKiemHDB.SelectionChanged += new System.EventHandler(this.dgvKQTimKiemHDB_SelectionChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(35, 273);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(600, 16);
            this.label7.TabIndex = 15;
            this.label7.Text = "Click đúp vào một dòng hoặc click vào một dòng rồi bấm nút Xem chi tiết để hiển t" +
    "hị thông tin hóa đơn";
            // 
            // lblSoLuongKQ
            // 
            this.lblSoLuongKQ.AutoSize = true;
            this.lblSoLuongKQ.Location = new System.Drawing.Point(35, 235);
            this.lblSoLuongKQ.Name = "lblSoLuongKQ";
            this.lblSoLuongKQ.Size = new System.Drawing.Size(61, 16);
            this.lblSoLuongKQ.TabIndex = 14;
            this.lblSoLuongKQ.Text = "Tìm thấy:";
            // 
            // cboTimKH
            // 
            this.cboTimKH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTimKH.FormattingEnabled = true;
            this.cboTimKH.Location = new System.Drawing.Point(495, 126);
            this.cboTimKH.Name = "cboTimKH";
            this.cboTimKH.Size = new System.Drawing.Size(191, 24);
            this.cboTimKH.TabIndex = 25;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(408, 131);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 16);
            this.label6.TabIndex = 24;
            this.label6.Text = "Khách hàng";
            // 
            // TimHDBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 653);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "TimHDBan";
            this.Text = "TimHDBan";
            this.Load += new System.EventHandler(this.TimHDBan_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKQTimKiemHDB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.ComboBox cboTimNV;
        private System.Windows.Forms.DateTimePicker dtpTimDenNgay;
        private System.Windows.Forms.DateTimePicker dtpTimTuNgay;
        private System.Windows.Forms.TextBox txtTimMaHDB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDong;
        private System.Windows.Forms.Button btnXemChiTiet;
        private System.Windows.Forms.DataGridView dgvKQTimKiemHDB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblSoLuongKQ;
        private System.Windows.Forms.ComboBox cboTimKH;
        private System.Windows.Forms.Label label6;
    }
}