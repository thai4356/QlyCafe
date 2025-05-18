namespace QlyCafe.Quanly
{
    partial class TimHDNhap
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.cboTimNV = new System.Windows.Forms.ComboBox();
            this.dtpTimDenNgay = new System.Windows.Forms.DateTimePicker();
            this.cboTimNhaCC = new System.Windows.Forms.ComboBox();
            this.dtpTimTuNgay = new System.Windows.Forms.DateTimePicker();
            this.txtTimMaHDN = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDong = new System.Windows.Forms.Button();
            this.btnXemChiTiet = new System.Windows.Forms.Button();
            this.dgvKQTimKiemHDN = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.lblSoLuongKQ = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKQTimKiemHDN)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLamMoi);
            this.groupBox1.Controls.Add(this.btnTimKiem);
            this.groupBox1.Controls.Add(this.cboTimNV);
            this.groupBox1.Controls.Add(this.dtpTimDenNgay);
            this.groupBox1.Controls.Add(this.cboTimNhaCC);
            this.groupBox1.Controls.Add(this.dtpTimTuNgay);
            this.groupBox1.Controls.Add(this.txtTimMaHDN);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1067, 208);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tiêu chí tìm kiếm";
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Location = new System.Drawing.Point(562, 179);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(75, 23);
            this.btnLamMoi.TabIndex = 11;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.Location = new System.Drawing.Point(460, 179);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(75, 23);
            this.btnTimKiem.TabIndex = 10;
            this.btnTimKiem.Text = "Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = true;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // cboTimNV
            // 
            this.cboTimNV.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTimNV.FormattingEnabled = true;
            this.cboTimNV.Location = new System.Drawing.Point(739, 119);
            this.cboTimNV.Name = "cboTimNV";
            this.cboTimNV.Size = new System.Drawing.Size(191, 24);
            this.cboTimNV.TabIndex = 9;
            // 
            // dtpTimDenNgay
            // 
            this.dtpTimDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dtpTimDenNgay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimDenNgay.Location = new System.Drawing.Point(739, 74);
            this.dtpTimDenNgay.Name = "dtpTimDenNgay";
            this.dtpTimDenNgay.Size = new System.Drawing.Size(191, 22);
            this.dtpTimDenNgay.TabIndex = 8;
            this.dtpTimDenNgay.ValueChanged += new System.EventHandler(this.dtpTimDenNgay_ValueChanged);
            // 
            // cboTimNhaCC
            // 
            this.cboTimNhaCC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTimNhaCC.FormattingEnabled = true;
            this.cboTimNhaCC.Location = new System.Drawing.Point(184, 124);
            this.cboTimNhaCC.Name = "cboTimNhaCC";
            this.cboTimNhaCC.Size = new System.Drawing.Size(191, 24);
            this.cboTimNhaCC.TabIndex = 7;
            // 
            // dtpTimTuNgay
            // 
            this.dtpTimTuNgay.CustomFormat = "dd/MM/yyyy";
            this.dtpTimTuNgay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimTuNgay.Location = new System.Drawing.Point(184, 82);
            this.dtpTimTuNgay.Name = "dtpTimTuNgay";
            this.dtpTimTuNgay.Size = new System.Drawing.Size(191, 22);
            this.dtpTimTuNgay.TabIndex = 6;
            this.dtpTimTuNgay.ValueChanged += new System.EventHandler(this.dtpTimTuNgay_ValueChanged);
            // 
            // txtTimMaHDN
            // 
            this.txtTimMaHDN.Location = new System.Drawing.Point(184, 44);
            this.txtTimMaHDN.Name = "txtTimMaHDN";
            this.txtTimMaHDN.Size = new System.Drawing.Size(191, 22);
            this.txtTimMaHDN.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(664, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Nhân Viên";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(88, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Nhà Cung Cấp";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(664, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "Đến ngày";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(90, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Từ ngày";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(88, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mã HĐN";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDong);
            this.groupBox2.Controls.Add(this.btnXemChiTiet);
            this.groupBox2.Controls.Add(this.dgvKQTimKiemHDN);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.lblSoLuongKQ);
            this.groupBox2.Location = new System.Drawing.Point(13, 228);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1067, 383);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Kết quả tìm kiếm";
            // 
            // btnDong
            // 
            this.btnDong.Location = new System.Drawing.Point(120, 354);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(75, 23);
            this.btnDong.TabIndex = 13;
            this.btnDong.Text = "Đóng";
            this.btnDong.UseVisualStyleBackColor = true;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // btnXemChiTiet
            // 
            this.btnXemChiTiet.Location = new System.Drawing.Point(18, 354);
            this.btnXemChiTiet.Name = "btnXemChiTiet";
            this.btnXemChiTiet.Size = new System.Drawing.Size(86, 23);
            this.btnXemChiTiet.TabIndex = 12;
            this.btnXemChiTiet.Text = "Xem chi tiết";
            this.btnXemChiTiet.UseVisualStyleBackColor = true;
            this.btnXemChiTiet.Click += new System.EventHandler(this.btnXemChiTiet_Click);
            // 
            // dgvKQTimKiemHDN
            // 
            this.dgvKQTimKiemHDN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKQTimKiemHDN.Location = new System.Drawing.Point(18, 41);
            this.dgvKQTimKiemHDN.Name = "dgvKQTimKiemHDN";
            this.dgvKQTimKiemHDN.RowHeadersWidth = 51;
            this.dgvKQTimKiemHDN.RowTemplate.Height = 24;
            this.dgvKQTimKiemHDN.Size = new System.Drawing.Size(1030, 181);
            this.dgvKQTimKiemHDN.TabIndex = 6;
            this.dgvKQTimKiemHDN.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvKQTimKiemHDN_CellDoubleClick);
            this.dgvKQTimKiemHDN.SelectionChanged += new System.EventHandler(this.dgvKQTimKiemHDN_SelectionChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 322);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(600, 16);
            this.label7.TabIndex = 5;
            this.label7.Text = "Click đúp vào một dòng hoặc click vào một dòng rồi bấm nút Xem chi tiết để hiển t" +
    "hị thông tin hóa đơn";
            // 
            // lblSoLuongKQ
            // 
            this.lblSoLuongKQ.AutoSize = true;
            this.lblSoLuongKQ.Location = new System.Drawing.Point(24, 284);
            this.lblSoLuongKQ.Name = "lblSoLuongKQ";
            this.lblSoLuongKQ.Size = new System.Drawing.Size(61, 16);
            this.lblSoLuongKQ.TabIndex = 4;
            this.lblSoLuongKQ.Text = "Tìm thấy:";
            // 
            // TimHDNhap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1094, 629);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TimHDNhap";
            this.Text = "TimHDNhap";
            this.Load += new System.EventHandler(this.TimHDNhap_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKQTimKiemHDN)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblSoLuongKQ;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.ComboBox cboTimNV;
        private System.Windows.Forms.DateTimePicker dtpTimDenNgay;
        private System.Windows.Forms.ComboBox cboTimNhaCC;
        private System.Windows.Forms.DateTimePicker dtpTimTuNgay;
        private System.Windows.Forms.TextBox txtTimMaHDN;
        private System.Windows.Forms.Button btnDong;
        private System.Windows.Forms.Button btnXemChiTiet;
        private System.Windows.Forms.DataGridView dgvKQTimKiemHDN;
    }
}