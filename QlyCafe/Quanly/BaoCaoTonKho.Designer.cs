namespace QlyCafe.Quanly
{
    partial class BaoCaoTonKho
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cboLoaiSanPhamFilter = new System.Windows.Forms.ComboBox();
            this.chkHienThiHangSapHet = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNguongSapHet = new System.Windows.Forms.TextBox();
            this.btnApDungLoc = new System.Windows.Forms.Button();
            this.btnLamMoiDuLieu = new System.Windows.Forms.Button();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panelBaoCao = new System.Windows.Forms.Panel();
            this.tabCtrlBaoCaoTonKho = new System.Windows.Forms.TabControl();
            this.tpChiTiettonKho = new System.Windows.Forms.TabPage();
            this.tpCharts = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvBaoCaoTonKho = new System.Windows.Forms.DataGridView();
            this.panelTongKetChiTiet = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTongSoMatHang = new System.Windows.Forms.Label();
            this.lblTongGiaTriTonKho = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelBieuDoTron = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.panelBieuDoCot = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.chartTopSPGiaTriTon = new LiveCharts.WinForms.CartesianChart();
            this.pieChartGiaTriTheoLoai = new LiveCharts.WinForms.PieChart();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelBaoCao.SuspendLayout();
            this.tabCtrlBaoCaoTonKho.SuspendLayout();
            this.tpChiTiettonKho.SuspendLayout();
            this.tpCharts.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaoCaoTonKho)).BeginInit();
            this.panelTongKetChiTiet.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelBieuDoTron.SuspendLayout();
            this.panelBieuDoCot.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1173, 155);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lọc theo Loại SP";
            // 
            // cboLoaiSanPhamFilter
            // 
            this.cboLoaiSanPhamFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoaiSanPhamFilter.FormattingEnabled = true;
            this.cboLoaiSanPhamFilter.Location = new System.Drawing.Point(215, 36);
            this.cboLoaiSanPhamFilter.Name = "cboLoaiSanPhamFilter";
            this.cboLoaiSanPhamFilter.Size = new System.Drawing.Size(223, 28);
            this.cboLoaiSanPhamFilter.TabIndex = 1;
            // 
            // chkHienThiHangSapHet
            // 
            this.chkHienThiHangSapHet.AutoSize = true;
            this.chkHienThiHangSapHet.Location = new System.Drawing.Point(505, 40);
            this.chkHienThiHangSapHet.Name = "chkHienThiHangSapHet";
            this.chkHienThiHangSapHet.Size = new System.Drawing.Size(216, 24);
            this.chkHienThiHangSapHet.TabIndex = 2;
            this.chkHienThiHangSapHet.Text = "Chỉ hiển thị hàng sắp hết";
            this.chkHienThiHangSapHet.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(765, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ngưỡng Sắp hết";
            // 
            // txtNguongSapHet
            // 
            this.txtNguongSapHet.Location = new System.Drawing.Point(919, 41);
            this.txtNguongSapHet.Name = "txtNguongSapHet";
            this.txtNguongSapHet.Size = new System.Drawing.Size(175, 27);
            this.txtNguongSapHet.TabIndex = 4;
            // 
            // btnApDungLoc
            // 
            this.btnApDungLoc.Location = new System.Drawing.Point(292, 100);
            this.btnApDungLoc.Name = "btnApDungLoc";
            this.btnApDungLoc.Size = new System.Drawing.Size(137, 39);
            this.btnApDungLoc.TabIndex = 5;
            this.btnApDungLoc.Text = "Áp dụng lọc";
            this.btnApDungLoc.UseVisualStyleBackColor = true;
            // 
            // btnLamMoiDuLieu
            // 
            this.btnLamMoiDuLieu.Location = new System.Drawing.Point(505, 100);
            this.btnLamMoiDuLieu.Name = "btnLamMoiDuLieu";
            this.btnLamMoiDuLieu.Size = new System.Drawing.Size(137, 39);
            this.btnLamMoiDuLieu.TabIndex = 6;
            this.btnLamMoiDuLieu.Text = "Làm mới";
            this.btnLamMoiDuLieu.UseVisualStyleBackColor = true;
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.Location = new System.Drawing.Point(715, 100);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(137, 39);
            this.btnXuatExcel.TabIndex = 7;
            this.btnXuatExcel.Text = "Xuất Excel";
            this.btnXuatExcel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnXuatExcel);
            this.groupBox1.Controls.Add(this.txtNguongSapHet);
            this.groupBox1.Controls.Add(this.btnLamMoiDuLieu);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnApDungLoc);
            this.groupBox1.Controls.Add(this.cboLoaiSanPhamFilter);
            this.groupBox1.Controls.Add(this.chkHienThiHangSapHet);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1173, 155);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Lọc và thao tác";
            // 
            // panelBaoCao
            // 
            this.panelBaoCao.Controls.Add(this.tabCtrlBaoCaoTonKho);
            this.panelBaoCao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBaoCao.Location = new System.Drawing.Point(0, 155);
            this.panelBaoCao.Name = "panelBaoCao";
            this.panelBaoCao.Size = new System.Drawing.Size(1173, 553);
            this.panelBaoCao.TabIndex = 1;
            // 
            // tabCtrlBaoCaoTonKho
            // 
            this.tabCtrlBaoCaoTonKho.Controls.Add(this.tpChiTiettonKho);
            this.tabCtrlBaoCaoTonKho.Controls.Add(this.tpCharts);
            this.tabCtrlBaoCaoTonKho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlBaoCaoTonKho.Location = new System.Drawing.Point(0, 0);
            this.tabCtrlBaoCaoTonKho.Name = "tabCtrlBaoCaoTonKho";
            this.tabCtrlBaoCaoTonKho.SelectedIndex = 0;
            this.tabCtrlBaoCaoTonKho.Size = new System.Drawing.Size(1173, 553);
            this.tabCtrlBaoCaoTonKho.TabIndex = 0;
            // 
            // tpChiTiettonKho
            // 
            this.tpChiTiettonKho.Controls.Add(this.flowLayoutPanel1);
            this.tpChiTiettonKho.Location = new System.Drawing.Point(4, 29);
            this.tpChiTiettonKho.Name = "tpChiTiettonKho";
            this.tpChiTiettonKho.Padding = new System.Windows.Forms.Padding(3);
            this.tpChiTiettonKho.Size = new System.Drawing.Size(1201, 517);
            this.tpChiTiettonKho.TabIndex = 0;
            this.tpChiTiettonKho.Text = "Báo cáo chi tiết";
            this.tpChiTiettonKho.UseVisualStyleBackColor = true;
            // 
            // tpCharts
            // 
            this.tpCharts.Controls.Add(this.tableLayoutPanel1);
            this.tpCharts.Location = new System.Drawing.Point(4, 29);
            this.tpCharts.Name = "tpCharts";
            this.tpCharts.Padding = new System.Windows.Forms.Padding(3);
            this.tpCharts.Size = new System.Drawing.Size(1165, 520);
            this.tpCharts.TabIndex = 1;
            this.tpCharts.Text = "Biểu đồ phân tích";
            this.tpCharts.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.groupBox2);
            this.flowLayoutPanel1.Controls.Add(this.panelTongKetChiTiet);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1195, 511);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvBaoCaoTonKho);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1187, 214);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Danh sách chi tiết";
            // 
            // dgvBaoCaoTonKho
            // 
            this.dgvBaoCaoTonKho.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBaoCaoTonKho.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dgvBaoCaoTonKho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBaoCaoTonKho.Location = new System.Drawing.Point(3, 23);
            this.dgvBaoCaoTonKho.Name = "dgvBaoCaoTonKho";
            this.dgvBaoCaoTonKho.RowHeadersWidth = 51;
            this.dgvBaoCaoTonKho.RowTemplate.Height = 24;
            this.dgvBaoCaoTonKho.Size = new System.Drawing.Size(1181, 188);
            this.dgvBaoCaoTonKho.TabIndex = 0;
            // 
            // panelTongKetChiTiet
            // 
            this.panelTongKetChiTiet.Controls.Add(this.lblTongSoMatHang);
            this.panelTongKetChiTiet.Controls.Add(this.label3);
            this.panelTongKetChiTiet.Location = new System.Drawing.Point(3, 223);
            this.panelTongKetChiTiet.Name = "panelTongKetChiTiet";
            this.panelTongKetChiTiet.Size = new System.Drawing.Size(1184, 116);
            this.panelTongKetChiTiet.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblTongGiaTriTonKho);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(3, 345);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1184, 116);
            this.panel2.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(46, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(346, 42);
            this.label3.TabIndex = 1;
            this.label3.Text = "Tổng số mặt hàng:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(46, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(376, 42);
            this.label4.TabIndex = 2;
            this.label4.Text = "Tổng Giá trị tồn kho:";
            // 
            // lblTongSoMatHang
            // 
            this.lblTongSoMatHang.AutoSize = true;
            this.lblTongSoMatHang.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongSoMatHang.Location = new System.Drawing.Point(537, 37);
            this.lblTongSoMatHang.Name = "lblTongSoMatHang";
            this.lblTongSoMatHang.Size = new System.Drawing.Size(123, 42);
            this.lblTongSoMatHang.TabIndex = 2;
            this.lblTongSoMatHang.Text = "Giá trị";
            // 
            // lblTongGiaTriTonKho
            // 
            this.lblTongGiaTriTonKho.AutoSize = true;
            this.lblTongGiaTriTonKho.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongGiaTriTonKho.Location = new System.Drawing.Point(537, 38);
            this.lblTongGiaTriTonKho.Name = "lblTongGiaTriTonKho";
            this.lblTongGiaTriTonKho.Size = new System.Drawing.Size(123, 42);
            this.lblTongGiaTriTonKho.TabIndex = 3;
            this.lblTongGiaTriTonKho.Text = "Giá trị";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.95398F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.04602F));
            this.tableLayoutPanel1.Controls.Add(this.panelBieuDoCot, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelBieuDoTron, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1159, 514);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelBieuDoTron
            // 
            this.panelBieuDoTron.Controls.Add(this.pieChartGiaTriTheoLoai);
            this.panelBieuDoTron.Controls.Add(this.label5);
            this.panelBieuDoTron.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBieuDoTron.Location = new System.Drawing.Point(3, 3);
            this.panelBieuDoTron.Name = "panelBieuDoTron";
            this.panelBieuDoTron.Size = new System.Drawing.Size(561, 508);
            this.panelBieuDoTron.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(331, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "Cơ cấu Giá trị Tồn kho theo Loại Sản phẩm";
            // 
            // panelBieuDoCot
            // 
            this.panelBieuDoCot.Controls.Add(this.chartTopSPGiaTriTon);
            this.panelBieuDoCot.Controls.Add(this.label6);
            this.panelBieuDoCot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBieuDoCot.Location = new System.Drawing.Point(570, 3);
            this.panelBieuDoCot.Name = "panelBieuDoCot";
            this.panelBieuDoCot.Size = new System.Drawing.Size(586, 508);
            this.panelBieuDoCot.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(222, 20);
            this.label6.TabIndex = 0;
            this.label6.Text = "Top 5 SP giá trị tồn cao nhất";
            // 
            // chartTopSPGiaTriTon
            // 
            this.chartTopSPGiaTriTon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartTopSPGiaTriTon.Location = new System.Drawing.Point(0, 20);
            this.chartTopSPGiaTriTon.Name = "chartTopSPGiaTriTon";
            this.chartTopSPGiaTriTon.Size = new System.Drawing.Size(586, 488);
            this.chartTopSPGiaTriTon.TabIndex = 2;
            this.chartTopSPGiaTriTon.Text = "cartesianChart2";
            // 
            // pieChartGiaTriTheoLoai
            // 
            this.pieChartGiaTriTheoLoai.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pieChartGiaTriTheoLoai.Location = new System.Drawing.Point(0, 20);
            this.pieChartGiaTriTheoLoai.Name = "pieChartGiaTriTheoLoai";
            this.pieChartGiaTriTheoLoai.Size = new System.Drawing.Size(561, 488);
            this.pieChartGiaTriTheoLoai.TabIndex = 1;
            this.pieChartGiaTriTheoLoai.Text = "pieChart1";
            // 
            // BaoCaoTonKho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1173, 708);
            this.Controls.Add(this.panelBaoCao);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "BaoCaoTonKho";
            this.Text = "BaoCaoTonKho";
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelBaoCao.ResumeLayout(false);
            this.tabCtrlBaoCaoTonKho.ResumeLayout(false);
            this.tpChiTiettonKho.ResumeLayout(false);
            this.tpCharts.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaoCaoTonKho)).EndInit();
            this.panelTongKetChiTiet.ResumeLayout(false);
            this.panelTongKetChiTiet.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelBieuDoTron.ResumeLayout(false);
            this.panelBieuDoTron.PerformLayout();
            this.panelBieuDoCot.ResumeLayout(false);
            this.panelBieuDoCot.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkHienThiHangSapHet;
        private System.Windows.Forms.ComboBox cboLoaiSanPhamFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNguongSapHet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnXuatExcel;
        private System.Windows.Forms.Button btnLamMoiDuLieu;
        private System.Windows.Forms.Button btnApDungLoc;
        private System.Windows.Forms.Panel panelBaoCao;
        private System.Windows.Forms.TabControl tabCtrlBaoCaoTonKho;
        private System.Windows.Forms.TabPage tpChiTiettonKho;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TabPage tpCharts;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvBaoCaoTonKho;
        private System.Windows.Forms.Panel panelTongKetChiTiet;
        private System.Windows.Forms.Label lblTongSoMatHang;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblTongGiaTriTonKho;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelBieuDoCot;
        private LiveCharts.WinForms.CartesianChart chartTopSPGiaTriTon;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panelBieuDoTron;
        private LiveCharts.WinForms.PieChart pieChartGiaTriTheoLoai;
        private System.Windows.Forms.Label label5;
    }
}