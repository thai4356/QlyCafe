namespace QlyCafe.Quanly
{
    partial class FormQuanLy
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDangXuat = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuQuanLy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHDBan = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHDNhap = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSanPham = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuKhuyenMai = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNhaCungCap = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTaiKhoan = new System.Windows.Forms.ToolStripMenuItem();
            this.tìmKiếmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTKHDBan = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTKHDNhap = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBaoCao = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDoanhThu = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHangTonKho = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tabCtrlDashboard = new System.Windows.Forms.TabControl();
            this.tpKPI = new System.Windows.Forms.TabPage();
            this.panelKPI = new System.Windows.Forms.FlowLayoutPanel();
            this.tpChart = new System.Windows.Forms.TabPage();
            this.tpQuickView = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.dvgSPSapHetHang = new System.Windows.Forms.DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dgvHDBanGanDay = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblThoiGian = new System.Windows.Forms.Label();
            this.lblNgay = new System.Windows.Forms.Label();
            this.btnThoat = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSP = new System.Windows.Forms.Button();
            this.btnKM = new System.Windows.Forms.Button();
            this.btnNCC = new System.Windows.Forms.Button();
            this.btnTK = new System.Windows.Forms.Button();
            this.btnTKHDB = new System.Windows.Forms.Button();
            this.btnTKHDN = new System.Windows.Forms.Button();
            this.btnBCDT = new System.Windows.Forms.Button();
            this.btnBCTK = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabCtrlDashboard.SuspendLayout();
            this.tpKPI.SuspendLayout();
            this.tpQuickView.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvgSPSapHetHang)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHDBanGanDay)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuQuanLy,
            this.tìmKiếmToolStripMenuItem,
            this.mnuBaoCao});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1240, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDangXuat});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(46, 24);
            this.mnuFile.Text = "File";
            // 
            // mnuDangXuat
            // 
            this.mnuDangXuat.Name = "mnuDangXuat";
            this.mnuDangXuat.Size = new System.Drawing.Size(224, 26);
            this.mnuDangXuat.Text = "Đăng Xuất";
            this.mnuDangXuat.Click += new System.EventHandler(this.mnuDangXuat_Click);
            // 
            // mnuQuanLy
            // 
            this.mnuQuanLy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHDBan,
            this.mnuHDNhap,
            this.mnuSanPham,
            this.mnuKhuyenMai,
            this.mnuNhaCungCap,
            this.mnuTaiKhoan});
            this.mnuQuanLy.Name = "mnuQuanLy";
            this.mnuQuanLy.Size = new System.Drawing.Size(75, 24);
            this.mnuQuanLy.Text = "Quản Lý";
            // 
            // mnuHDBan
            // 
            this.mnuHDBan.Name = "mnuHDBan";
            this.mnuHDBan.Size = new System.Drawing.Size(224, 26);
            this.mnuHDBan.Text = "Hóa đơn bán";
            this.mnuHDBan.Click += new System.EventHandler(this.mnuHDBan_Click);
            // 
            // mnuHDNhap
            // 
            this.mnuHDNhap.Name = "mnuHDNhap";
            this.mnuHDNhap.Size = new System.Drawing.Size(224, 26);
            this.mnuHDNhap.Text = "Hóa đơn nhập";
            this.mnuHDNhap.Click += new System.EventHandler(this.mnuHDNhap_Click);
            // 
            // mnuSanPham
            // 
            this.mnuSanPham.Name = "mnuSanPham";
            this.mnuSanPham.Size = new System.Drawing.Size(224, 26);
            this.mnuSanPham.Text = "Sản Phẩm";
            this.mnuSanPham.Click += new System.EventHandler(this.mnuSanPham_Click);
            // 
            // mnuKhuyenMai
            // 
            this.mnuKhuyenMai.Name = "mnuKhuyenMai";
            this.mnuKhuyenMai.Size = new System.Drawing.Size(224, 26);
            this.mnuKhuyenMai.Text = "Khuyến Mãi";
            this.mnuKhuyenMai.Click += new System.EventHandler(this.mnuKhuyenMai_Click);
            // 
            // mnuNhaCungCap
            // 
            this.mnuNhaCungCap.Name = "mnuNhaCungCap";
            this.mnuNhaCungCap.Size = new System.Drawing.Size(224, 26);
            this.mnuNhaCungCap.Text = "Nhà Cung Cấp";
            this.mnuNhaCungCap.Click += new System.EventHandler(this.mnuNhaCungCap_Click);
            // 
            // mnuTaiKhoan
            // 
            this.mnuTaiKhoan.Name = "mnuTaiKhoan";
            this.mnuTaiKhoan.Size = new System.Drawing.Size(224, 26);
            this.mnuTaiKhoan.Text = "Tài Khoản";
            this.mnuTaiKhoan.Click += new System.EventHandler(this.mnuTaiKhoan_Click);
            // 
            // tìmKiếmToolStripMenuItem
            // 
            this.tìmKiếmToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTKHDBan,
            this.mnuTKHDNhap});
            this.tìmKiếmToolStripMenuItem.Name = "tìmKiếmToolStripMenuItem";
            this.tìmKiếmToolStripMenuItem.Size = new System.Drawing.Size(86, 24);
            this.tìmKiếmToolStripMenuItem.Text = "Tìm Kiếm";
            // 
            // mnuTKHDBan
            // 
            this.mnuTKHDBan.Name = "mnuTKHDBan";
            this.mnuTKHDBan.Size = new System.Drawing.Size(257, 26);
            this.mnuTKHDBan.Text = "Tìm kiếm Hóa Đơn Bán";
            this.mnuTKHDBan.Click += new System.EventHandler(this.mnuTKHDBan_Click);
            // 
            // mnuTKHDNhap
            // 
            this.mnuTKHDNhap.Name = "mnuTKHDNhap";
            this.mnuTKHDNhap.Size = new System.Drawing.Size(257, 26);
            this.mnuTKHDNhap.Text = "Tìm kiếm Hóa Đơn Nhập";
            this.mnuTKHDNhap.Click += new System.EventHandler(this.mnuTKHDNhap_Click);
            // 
            // mnuBaoCao
            // 
            this.mnuBaoCao.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDoanhThu,
            this.mnuHangTonKho});
            this.mnuBaoCao.Name = "mnuBaoCao";
            this.mnuBaoCao.Size = new System.Drawing.Size(79, 24);
            this.mnuBaoCao.Text = "Báo Cáo";
            // 
            // mnuDoanhThu
            // 
            this.mnuDoanhThu.Name = "mnuDoanhThu";
            this.mnuDoanhThu.Size = new System.Drawing.Size(224, 26);
            this.mnuDoanhThu.Text = "Doanh thu";
            this.mnuDoanhThu.Click += new System.EventHandler(this.mnuDoanhThu_Click);
            // 
            // mnuHangTonKho
            // 
            this.mnuHangTonKho.Name = "mnuHangTonKho";
            this.mnuHangTonKho.Size = new System.Drawing.Size(224, 26);
            this.mnuHangTonKho.Text = "Hàng tồn kho";
            this.mnuHangTonKho.Click += new System.EventHandler(this.mnuHangTonKho_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(14, 36);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(1211, 631);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Main Content Area";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tabCtrlDashboard);
            this.groupBox3.Location = new System.Drawing.Point(313, 25);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(892, 595);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Dashboard";
            // 
            // tabCtrlDashboard
            // 
            this.tabCtrlDashboard.Controls.Add(this.tpKPI);
            this.tabCtrlDashboard.Controls.Add(this.tpChart);
            this.tabCtrlDashboard.Controls.Add(this.tpQuickView);
            this.tabCtrlDashboard.Location = new System.Drawing.Point(8, 25);
            this.tabCtrlDashboard.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabCtrlDashboard.Name = "tabCtrlDashboard";
            this.tabCtrlDashboard.SelectedIndex = 0;
            this.tabCtrlDashboard.Size = new System.Drawing.Size(878, 560);
            this.tabCtrlDashboard.TabIndex = 0;
            // 
            // tpKPI
            // 
            this.tpKPI.Controls.Add(this.panelKPI);
            this.tpKPI.Location = new System.Drawing.Point(4, 29);
            this.tpKPI.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpKPI.Name = "tpKPI";
            this.tpKPI.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpKPI.Size = new System.Drawing.Size(870, 610);
            this.tpKPI.TabIndex = 0;
            this.tpKPI.Text = "KPI Section";
            this.tpKPI.UseVisualStyleBackColor = true;
            // 
            // panelKPI
            // 
            this.panelKPI.Location = new System.Drawing.Point(8, 8);
            this.panelKPI.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelKPI.Name = "panelKPI";
            this.panelKPI.Size = new System.Drawing.Size(854, 550);
            this.panelKPI.TabIndex = 0;
            // 
            // tpChart
            // 
            this.tpChart.Location = new System.Drawing.Point(4, 27);
            this.tpChart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpChart.Name = "tpChart";
            this.tpChart.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpChart.Size = new System.Drawing.Size(870, 529);
            this.tpChart.TabIndex = 1;
            this.tpChart.Text = "Chart Section";
            this.tpChart.UseVisualStyleBackColor = true;
            // 
            // tpQuickView
            // 
            this.tpQuickView.Controls.Add(this.groupBox5);
            this.tpQuickView.Controls.Add(this.groupBox4);
            this.tpQuickView.Location = new System.Drawing.Point(4, 29);
            this.tpQuickView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpQuickView.Name = "tpQuickView";
            this.tpQuickView.Size = new System.Drawing.Size(870, 610);
            this.tpQuickView.TabIndex = 2;
            this.tpQuickView.Text = "Quick View/ Alert Section";
            this.tpQuickView.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.dvgSPSapHetHang);
            this.groupBox5.Location = new System.Drawing.Point(8, 266);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Size = new System.Drawing.Size(860, 261);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Sản phẩm sắp hết hàng";
            // 
            // dvgSPSapHetHang
            // 
            this.dvgSPSapHetHang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvgSPSapHetHang.Location = new System.Drawing.Point(8, 23);
            this.dvgSPSapHetHang.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dvgSPSapHetHang.Name = "dvgSPSapHetHang";
            this.dvgSPSapHetHang.RowHeadersWidth = 51;
            this.dvgSPSapHetHang.RowTemplate.Height = 24;
            this.dvgSPSapHetHang.Size = new System.Drawing.Size(846, 223);
            this.dvgSPSapHetHang.TabIndex = 1;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dgvHDBanGanDay);
            this.groupBox4.Location = new System.Drawing.Point(4, 4);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Size = new System.Drawing.Size(860, 254);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Các hóa đơn bán gần đây";
            // 
            // dgvHDBanGanDay
            // 
            this.dgvHDBanGanDay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHDBanGanDay.Location = new System.Drawing.Point(8, 25);
            this.dgvHDBanGanDay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvHDBanGanDay.Name = "dgvHDBanGanDay";
            this.dgvHDBanGanDay.RowHeadersWidth = 51;
            this.dgvHDBanGanDay.RowTemplate.Height = 24;
            this.dgvHDBanGanDay.Size = new System.Drawing.Size(846, 223);
            this.dgvHDBanGanDay.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Controls.Add(this.flowLayoutPanel1);
            this.groupBox2.Location = new System.Drawing.Point(8, 25);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(298, 595);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Điều hướng";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblThoiGian);
            this.panel1.Controls.Add(this.lblNgay);
            this.panel1.Controls.Add(this.btnThoat);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(4, 392);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(292, 193);
            this.panel1.TabIndex = 1;
            // 
            // lblThoiGian
            // 
            this.lblThoiGian.AutoSize = true;
            this.lblThoiGian.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThoiGian.Location = new System.Drawing.Point(59, 100);
            this.lblThoiGian.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblThoiGian.Name = "lblThoiGian";
            this.lblThoiGian.Size = new System.Drawing.Size(152, 25);
            this.lblThoiGian.TabIndex = 10;
            this.lblThoiGian.Text = "Xin chào, admin";
            // 
            // lblNgay
            // 
            this.lblNgay.AutoSize = true;
            this.lblNgay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgay.Location = new System.Drawing.Point(72, 50);
            this.lblNgay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNgay.Name = "lblNgay";
            this.lblNgay.Size = new System.Drawing.Size(128, 20);
            this.lblNgay.TabIndex = 9;
            this.lblNgay.Text = "Xin chào, admin";
            // 
            // btnThoat
            // 
            this.btnThoat.Location = new System.Drawing.Point(0, 129);
            this.btnThoat.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(288, 37);
            this.btnThoat.TabIndex = 8;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(72, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Xin chào, admin";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnSP);
            this.flowLayoutPanel1.Controls.Add(this.btnKM);
            this.flowLayoutPanel1.Controls.Add(this.btnNCC);
            this.flowLayoutPanel1.Controls.Add(this.btnTK);
            this.flowLayoutPanel1.Controls.Add(this.btnTKHDB);
            this.flowLayoutPanel1.Controls.Add(this.btnTKHDN);
            this.flowLayoutPanel1.Controls.Add(this.btnBCDT);
            this.flowLayoutPanel1.Controls.Add(this.btnBCTK);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(298, 360);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnSP
            // 
            this.btnSP.Location = new System.Drawing.Point(4, 4);
            this.btnSP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSP.Name = "btnSP";
            this.btnSP.Size = new System.Drawing.Size(288, 37);
            this.btnSP.TabIndex = 2;
            this.btnSP.Text = "Quản Lý Sản Phẩm";
            this.btnSP.UseVisualStyleBackColor = true;
            this.btnSP.Click += new System.EventHandler(this.btnSP_Click);
            // 
            // btnKM
            // 
            this.btnKM.Location = new System.Drawing.Point(4, 49);
            this.btnKM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKM.Name = "btnKM";
            this.btnKM.Size = new System.Drawing.Size(288, 37);
            this.btnKM.TabIndex = 3;
            this.btnKM.Text = "Quản Lý Khuyến Mãi";
            this.btnKM.UseVisualStyleBackColor = true;
            this.btnKM.Click += new System.EventHandler(this.btnKM_Click);
            // 
            // btnNCC
            // 
            this.btnNCC.Location = new System.Drawing.Point(4, 94);
            this.btnNCC.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnNCC.Name = "btnNCC";
            this.btnNCC.Size = new System.Drawing.Size(288, 37);
            this.btnNCC.TabIndex = 4;
            this.btnNCC.Text = "Quản Lý Nhà Cung Cấp";
            this.btnNCC.UseVisualStyleBackColor = true;
            this.btnNCC.Click += new System.EventHandler(this.btnNCC_Click);
            // 
            // btnTK
            // 
            this.btnTK.Location = new System.Drawing.Point(4, 139);
            this.btnTK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTK.Name = "btnTK";
            this.btnTK.Size = new System.Drawing.Size(288, 37);
            this.btnTK.TabIndex = 5;
            this.btnTK.Text = "Quản Lý Tài Khoản";
            this.btnTK.UseVisualStyleBackColor = true;
            this.btnTK.Click += new System.EventHandler(this.btnTK_Click);
            // 
            // btnTKHDB
            // 
            this.btnTKHDB.Location = new System.Drawing.Point(4, 184);
            this.btnTKHDB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTKHDB.Name = "btnTKHDB";
            this.btnTKHDB.Size = new System.Drawing.Size(288, 37);
            this.btnTKHDB.TabIndex = 6;
            this.btnTKHDB.Text = "Tìm Kiếm Hóa Đơn Bán";
            this.btnTKHDB.UseVisualStyleBackColor = true;
            this.btnTKHDB.Click += new System.EventHandler(this.btnTKHDB_Click);
            // 
            // btnTKHDN
            // 
            this.btnTKHDN.Location = new System.Drawing.Point(4, 229);
            this.btnTKHDN.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTKHDN.Name = "btnTKHDN";
            this.btnTKHDN.Size = new System.Drawing.Size(288, 37);
            this.btnTKHDN.TabIndex = 7;
            this.btnTKHDN.Text = "Tìm Kiếm Hóa Đơn Nhập";
            this.btnTKHDN.UseVisualStyleBackColor = true;
            this.btnTKHDN.Click += new System.EventHandler(this.btnTKHDN_Click);
            // 
            // btnBCDT
            // 
            this.btnBCDT.Location = new System.Drawing.Point(4, 274);
            this.btnBCDT.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBCDT.Name = "btnBCDT";
            this.btnBCDT.Size = new System.Drawing.Size(288, 37);
            this.btnBCDT.TabIndex = 8;
            this.btnBCDT.Text = "Báo cáo doanh thu";
            this.btnBCDT.UseVisualStyleBackColor = true;
            this.btnBCDT.Click += new System.EventHandler(this.btnBCDT_Click);
            // 
            // btnBCTK
            // 
            this.btnBCTK.Location = new System.Drawing.Point(4, 319);
            this.btnBCTK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBCTK.Name = "btnBCTK";
            this.btnBCTK.Size = new System.Drawing.Size(288, 37);
            this.btnBCTK.TabIndex = 9;
            this.btnBCTK.Text = "Báo cáo tồn kho";
            this.btnBCTK.UseVisualStyleBackColor = true;
            this.btnBCTK.Click += new System.EventHandler(this.btnBCTK_Click);
            // 
            // FormQuanLy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1240, 674);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormQuanLy";
            this.Text = "FormQuanLy";
            this.Load += new System.EventHandler(this.FormQuanLy_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tabCtrlDashboard.ResumeLayout(false);
            this.tpKPI.ResumeLayout(false);
            this.tpQuickView.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dvgSPSapHetHang)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHDBanGanDay)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuQuanLy;
        private System.Windows.Forms.ToolStripMenuItem tìmKiếmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuBaoCao;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TabControl tabCtrlDashboard;
        private System.Windows.Forms.TabPage tpKPI;
        private System.Windows.Forms.TabPage tpChart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel panelKPI;
        private System.Windows.Forms.TabPage tpQuickView;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView dvgSPSapHetHang;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dgvHDBanGanDay;
        private System.Windows.Forms.ToolStripMenuItem mnuDangXuat;
        private System.Windows.Forms.ToolStripMenuItem mnuHDBan;
        private System.Windows.Forms.ToolStripMenuItem mnuHDNhap;
        private System.Windows.Forms.ToolStripMenuItem mnuSanPham;
        private System.Windows.Forms.ToolStripMenuItem mnuKhuyenMai;
        private System.Windows.Forms.ToolStripMenuItem mnuNhaCungCap;
        private System.Windows.Forms.ToolStripMenuItem mnuTaiKhoan;
        private System.Windows.Forms.ToolStripMenuItem mnuTKHDBan;
        private System.Windows.Forms.ToolStripMenuItem mnuTKHDNhap;
        private System.Windows.Forms.ToolStripMenuItem mnuDoanhThu;
        private System.Windows.Forms.ToolStripMenuItem mnuHangTonKho;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSP;
        private System.Windows.Forms.Button btnKM;
        private System.Windows.Forms.Button btnNCC;
        private System.Windows.Forms.Button btnTK;
        private System.Windows.Forms.Button btnTKHDB;
        private System.Windows.Forms.Button btnTKHDN;
        private System.Windows.Forms.Label lblThoiGian;
        private System.Windows.Forms.Label lblNgay;
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBCDT;
        private System.Windows.Forms.Button btnBCTK;
    }
}