namespace QlyCafe.Quanly
{
    partial class KhuyenMai
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
            this.btnTim = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboLocTrangThai = new System.Windows.Forms.ComboBox();
            this.cboLocLoaiKM = new System.Windows.Forms.ComboBox();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvKhuyenMai = new System.Windows.Forms.DataGridView();
            this.tabChiTietKhuyenMai = new System.Windows.Forms.TabControl();
            this.tpThongTinKM = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkHoatDong = new System.Windows.Forms.CheckBox();
            this.txtDKSoLuongDuocTang = new System.Windows.Forms.TextBox();
            this.lblDKSoLuongDuocTang = new System.Windows.Forms.Label();
            this.txtDKSoLuongCanMua = new System.Windows.Forms.TextBox();
            this.lblDKSoLuongCanMua = new System.Windows.Forms.Label();
            this.txtDKApDung = new System.Windows.Forms.RichTextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMoTa = new System.Windows.Forms.RichTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.dtpNgayKT = new System.Windows.Forms.DateTimePicker();
            this.dtpNgayBD = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtGiaTri = new System.Windows.Forms.TextBox();
            this.lblGiaTri = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cboLoaiKM = new System.Windows.Forms.ComboBox();
            this.txtTenKM = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMaKM = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tpSPApDung = new System.Windows.Forms.TabPage();
            this.btnBoTatCaApDung = new System.Windows.Forms.Button();
            this.btnBoChon = new System.Windows.Forms.Button();
            this.btnThemChonApDung = new System.Windows.Forms.Button();
            this.btnThemTatCaApDung = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lstSPDaApDung = new System.Windows.Forms.ListBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lstSPChuaApDung = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnThoat = new System.Windows.Forms.Button();
            this.btnNhapExcel = new System.Windows.Forms.Button();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.BtnXoa = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnThem = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhuyenMai)).BeginInit();
            this.tabChiTietKhuyenMai.SuspendLayout();
            this.tpThongTinKM.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tpSPApDung.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnTim);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cboLocTrangThai);
            this.groupBox1.Controls.Add(this.cboLocLoaiKM);
            this.groupBox1.Controls.Add(this.txtTimKiem);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1346, 51);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tìm kiếm";
            // 
            // btnTim
            // 
            this.btnTim.Location = new System.Drawing.Point(1078, 19);
            this.btnTim.Name = "btnTim";
            this.btnTim.Size = new System.Drawing.Size(75, 23);
            this.btnTim.TabIndex = 6;
            this.btnTim.Text = "Tìm";
            this.btnTim.UseVisualStyleBackColor = true;
            this.btnTim.Click += new System.EventHandler(this.btnTim_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(704, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Lọc Trạng thái";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(385, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Lọc Loại Khuyến mại";
            // 
            // cboLocTrangThai
            // 
            this.cboLocTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocTrangThai.FormattingEnabled = true;
            this.cboLocTrangThai.Location = new System.Drawing.Point(813, 20);
            this.cboLocTrangThai.Name = "cboLocTrangThai";
            this.cboLocTrangThai.Size = new System.Drawing.Size(181, 24);
            this.cboLocTrangThai.TabIndex = 3;
            // 
            // cboLocLoaiKM
            // 
            this.cboLocLoaiKM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocLoaiKM.FormattingEnabled = true;
            this.cboLocLoaiKM.Location = new System.Drawing.Point(533, 19);
            this.cboLocLoaiKM.Name = "cboLocLoaiKM";
            this.cboLocLoaiKM.Size = new System.Drawing.Size(155, 24);
            this.cboLocLoaiKM.TabIndex = 2;
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Location = new System.Drawing.Point(210, 20);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(149, 22);
            this.txtTimKiem.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tìm kiếm theo TenKM/MaKM:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvKhuyenMai);
            this.groupBox2.Location = new System.Drawing.Point(13, 70);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1356, 188);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Danh sách khuyến mãi";
            // 
            // dgvKhuyenMai
            // 
            this.dgvKhuyenMai.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKhuyenMai.Location = new System.Drawing.Point(7, 22);
            this.dgvKhuyenMai.Name = "dgvKhuyenMai";
            this.dgvKhuyenMai.RowHeadersWidth = 51;
            this.dgvKhuyenMai.RowTemplate.Height = 24;
            this.dgvKhuyenMai.Size = new System.Drawing.Size(1343, 150);
            this.dgvKhuyenMai.TabIndex = 0;
            this.dgvKhuyenMai.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvKhuyenMai_CellClick);
            // 
            // tabChiTietKhuyenMai
            // 
            this.tabChiTietKhuyenMai.Controls.Add(this.tpThongTinKM);
            this.tabChiTietKhuyenMai.Controls.Add(this.tpSPApDung);
            this.tabChiTietKhuyenMai.Location = new System.Drawing.Point(0, 21);
            this.tabChiTietKhuyenMai.Name = "tabChiTietKhuyenMai";
            this.tabChiTietKhuyenMai.SelectedIndex = 0;
            this.tabChiTietKhuyenMai.Size = new System.Drawing.Size(1351, 386);
            this.tabChiTietKhuyenMai.TabIndex = 2;
            // 
            // tpThongTinKM
            // 
            this.tpThongTinKM.Controls.Add(this.groupBox4);
            this.tpThongTinKM.Location = new System.Drawing.Point(4, 25);
            this.tpThongTinKM.Name = "tpThongTinKM";
            this.tpThongTinKM.Padding = new System.Windows.Forms.Padding(3);
            this.tpThongTinKM.Size = new System.Drawing.Size(1343, 357);
            this.tpThongTinKM.TabIndex = 0;
            this.tpThongTinKM.Text = "Thông tin Khuyến Mãi";
            this.tpThongTinKM.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkHoatDong);
            this.groupBox4.Controls.Add(this.txtDKSoLuongDuocTang);
            this.groupBox4.Controls.Add(this.lblDKSoLuongDuocTang);
            this.groupBox4.Controls.Add(this.txtDKSoLuongCanMua);
            this.groupBox4.Controls.Add(this.lblDKSoLuongCanMua);
            this.groupBox4.Controls.Add(this.txtDKApDung);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.txtMoTa);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.dtpNgayKT);
            this.groupBox4.Controls.Add(this.dtpNgayBD);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.txtGiaTri);
            this.groupBox4.Controls.Add(this.lblGiaTri);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.cboLoaiKM);
            this.groupBox4.Controls.Add(this.txtTenKM);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.txtMaKM);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Location = new System.Drawing.Point(7, 7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1330, 347);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Chit tiết khuyến mãi";
            // 
            // chkHoatDong
            // 
            this.chkHoatDong.AutoSize = true;
            this.chkHoatDong.Location = new System.Drawing.Point(18, 181);
            this.chkHoatDong.Name = "chkHoatDong";
            this.chkHoatDong.Size = new System.Drawing.Size(129, 20);
            this.chkHoatDong.TabIndex = 21;
            this.chkHoatDong.Text = "Còn Hoạt động ?";
            this.chkHoatDong.UseVisualStyleBackColor = true;
            // 
            // txtDKSoLuongDuocTang
            // 
            this.txtDKSoLuongDuocTang.Location = new System.Drawing.Point(995, 283);
            this.txtDKSoLuongDuocTang.Name = "txtDKSoLuongDuocTang";
            this.txtDKSoLuongDuocTang.Size = new System.Drawing.Size(148, 22);
            this.txtDKSoLuongDuocTang.TabIndex = 20;
            // 
            // lblDKSoLuongDuocTang
            // 
            this.lblDKSoLuongDuocTang.AutoSize = true;
            this.lblDKSoLuongDuocTang.Location = new System.Drawing.Point(811, 286);
            this.lblDKSoLuongDuocTang.Name = "lblDKSoLuongDuocTang";
            this.lblDKSoLuongDuocTang.Size = new System.Drawing.Size(178, 16);
            this.lblDKSoLuongDuocTang.TabIndex = 19;
            this.lblDKSoLuongDuocTang.Text = "Điều kiện số lượng được tăng";
            // 
            // txtDKSoLuongCanMua
            // 
            this.txtDKSoLuongCanMua.Location = new System.Drawing.Point(495, 285);
            this.txtDKSoLuongCanMua.Name = "txtDKSoLuongCanMua";
            this.txtDKSoLuongCanMua.Size = new System.Drawing.Size(148, 22);
            this.txtDKSoLuongCanMua.TabIndex = 18;
            // 
            // lblDKSoLuongCanMua
            // 
            this.lblDKSoLuongCanMua.AutoSize = true;
            this.lblDKSoLuongCanMua.Location = new System.Drawing.Point(318, 288);
            this.lblDKSoLuongCanMua.Name = "lblDKSoLuongCanMua";
            this.lblDKSoLuongCanMua.Size = new System.Drawing.Size(171, 16);
            this.lblDKSoLuongCanMua.TabIndex = 17;
            this.lblDKSoLuongCanMua.Text = "Điều Kiện số lượng cần mua";
            // 
            // txtDKApDung
            // 
            this.txtDKApDung.Location = new System.Drawing.Point(495, 181);
            this.txtDKApDung.Name = "txtDKApDung";
            this.txtDKApDung.Size = new System.Drawing.Size(463, 98);
            this.txtDKApDung.TabIndex = 16;
            this.txtDKApDung.Text = "";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(367, 181);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(114, 16);
            this.label11.TabIndex = 15;
            this.label11.Text = "Điều kiện áp dụng";
            // 
            // txtMoTa
            // 
            this.txtMoTa.Location = new System.Drawing.Point(495, 70);
            this.txtMoTa.Name = "txtMoTa";
            this.txtMoTa.Size = new System.Drawing.Size(463, 98);
            this.txtMoTa.TabIndex = 14;
            this.txtMoTa.Text = "";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(425, 73);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 16);
            this.label10.TabIndex = 13;
            this.label10.Text = "Mô tả";
            // 
            // dtpNgayKT
            // 
            this.dtpNgayKT.CustomFormat = "dd/MM/yyyy";
            this.dtpNgayKT.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayKT.Location = new System.Drawing.Point(117, 136);
            this.dtpNgayKT.Name = "dtpNgayKT";
            this.dtpNgayKT.Size = new System.Drawing.Size(121, 22);
            this.dtpNgayKT.TabIndex = 12;
            // 
            // dtpNgayBD
            // 
            this.dtpNgayBD.CustomFormat = "dd/MM/yyyy";
            this.dtpNgayBD.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayBD.Location = new System.Drawing.Point(115, 74);
            this.dtpNgayBD.Name = "dtpNgayBD";
            this.dtpNgayBD.Size = new System.Drawing.Size(123, 22);
            this.dtpNgayBD.TabIndex = 11;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 136);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 16);
            this.label8.TabIndex = 10;
            this.label8.Text = "Ngày Kết thúc";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 76);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 16);
            this.label9.TabIndex = 9;
            this.label9.Text = "Ngày Bắt Đầu";
            // 
            // txtGiaTri
            // 
            this.txtGiaTri.Location = new System.Drawing.Point(995, 16);
            this.txtGiaTri.Name = "txtGiaTri";
            this.txtGiaTri.Size = new System.Drawing.Size(148, 22);
            this.txtGiaTri.TabIndex = 8;
            // 
            // lblGiaTri
            // 
            this.lblGiaTri.AutoSize = true;
            this.lblGiaTri.Location = new System.Drawing.Point(948, 19);
            this.lblGiaTri.Name = "lblGiaTri";
            this.lblGiaTri.Size = new System.Drawing.Size(41, 16);
            this.lblGiaTri.TabIndex = 7;
            this.lblGiaTri.Text = "Giá trị";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(667, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 16);
            this.label6.TabIndex = 6;
            this.label6.Text = "Loại Khuyến mại";
            // 
            // cboLoaiKM
            // 
            this.cboLoaiKM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoaiKM.FormattingEnabled = true;
            this.cboLoaiKM.Location = new System.Drawing.Point(778, 16);
            this.cboLoaiKM.Name = "cboLoaiKM";
            this.cboLoaiKM.Size = new System.Drawing.Size(155, 24);
            this.cboLoaiKM.TabIndex = 5;
            this.cboLoaiKM.SelectedIndexChanged += new System.EventHandler(this.cboLoaiKM_SelectedIndexChanged);
            // 
            // txtTenKM
            // 
            this.txtTenKM.Location = new System.Drawing.Point(495, 18);
            this.txtTenKM.Name = "txtTenKM";
            this.txtTenKM.Size = new System.Drawing.Size(148, 22);
            this.txtTenKM.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(386, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 16);
            this.label5.TabIndex = 3;
            this.label5.Text = "Tên Khuyến mại";
            // 
            // txtMaKM
            // 
            this.txtMaKM.Location = new System.Drawing.Point(115, 22);
            this.txtMaKM.Name = "txtMaKM";
            this.txtMaKM.ReadOnly = true;
            this.txtMaKM.Size = new System.Drawing.Size(166, 22);
            this.txtMaKM.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "Mã Khuyến mại";
            // 
            // tpSPApDung
            // 
            this.tpSPApDung.Controls.Add(this.btnBoTatCaApDung);
            this.tpSPApDung.Controls.Add(this.btnBoChon);
            this.tpSPApDung.Controls.Add(this.btnThemChonApDung);
            this.tpSPApDung.Controls.Add(this.btnThemTatCaApDung);
            this.tpSPApDung.Controls.Add(this.groupBox6);
            this.tpSPApDung.Controls.Add(this.groupBox5);
            this.tpSPApDung.Location = new System.Drawing.Point(4, 25);
            this.tpSPApDung.Name = "tpSPApDung";
            this.tpSPApDung.Padding = new System.Windows.Forms.Padding(3);
            this.tpSPApDung.Size = new System.Drawing.Size(1343, 357);
            this.tpSPApDung.TabIndex = 1;
            this.tpSPApDung.Text = "Sản Phẩm được áp dụng";
            this.tpSPApDung.UseVisualStyleBackColor = true;
            // 
            // btnBoTatCaApDung
            // 
            this.btnBoTatCaApDung.Location = new System.Drawing.Point(530, 261);
            this.btnBoTatCaApDung.Name = "btnBoTatCaApDung";
            this.btnBoTatCaApDung.Size = new System.Drawing.Size(114, 23);
            this.btnBoTatCaApDung.TabIndex = 9;
            this.btnBoTatCaApDung.Text = "<< (Bỏ Tất cả)";
            this.btnBoTatCaApDung.UseVisualStyleBackColor = true;
            this.btnBoTatCaApDung.Click += new System.EventHandler(this.btnBoTatCaApDung_Click);
            // 
            // btnBoChon
            // 
            this.btnBoChon.Location = new System.Drawing.Point(530, 184);
            this.btnBoChon.Name = "btnBoChon";
            this.btnBoChon.Size = new System.Drawing.Size(114, 23);
            this.btnBoChon.TabIndex = 8;
            this.btnBoChon.Text = "< (Bỏ chọn)";
            this.btnBoChon.UseVisualStyleBackColor = true;
            this.btnBoChon.Click += new System.EventHandler(this.btnBoChon_Click);
            // 
            // btnThemChonApDung
            // 
            this.btnThemChonApDung.Location = new System.Drawing.Point(530, 109);
            this.btnThemChonApDung.Name = "btnThemChonApDung";
            this.btnThemChonApDung.Size = new System.Drawing.Size(114, 23);
            this.btnThemChonApDung.TabIndex = 7;
            this.btnThemChonApDung.Text = "> (Thêm chọn)";
            this.btnThemChonApDung.UseVisualStyleBackColor = true;
            this.btnThemChonApDung.Click += new System.EventHandler(this.btnThemChonApDung_Click);
            // 
            // btnThemTatCaApDung
            // 
            this.btnThemTatCaApDung.Location = new System.Drawing.Point(530, 33);
            this.btnThemTatCaApDung.Name = "btnThemTatCaApDung";
            this.btnThemTatCaApDung.Size = new System.Drawing.Size(114, 23);
            this.btnThemTatCaApDung.TabIndex = 6;
            this.btnThemTatCaApDung.Text = ">> (Thêm tất cả)";
            this.btnThemTatCaApDung.UseVisualStyleBackColor = true;
            this.btnThemTatCaApDung.Click += new System.EventHandler(this.btnThemTatCaApDung_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.lstSPDaApDung);
            this.groupBox6.Location = new System.Drawing.Point(704, 7);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(450, 351);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Sản phẩm đã áp dụng";
            // 
            // lstSPDaApDung
            // 
            this.lstSPDaApDung.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSPDaApDung.FormattingEnabled = true;
            this.lstSPDaApDung.ItemHeight = 20;
            this.lstSPDaApDung.Location = new System.Drawing.Point(27, 21);
            this.lstSPDaApDung.Name = "lstSPDaApDung";
            this.lstSPDaApDung.Size = new System.Drawing.Size(399, 304);
            this.lstSPDaApDung.TabIndex = 1;
            this.lstSPDaApDung.DoubleClick += new System.EventHandler(this.lstSPDaApDung_DoubleClick);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lstSPChuaApDung);
            this.groupBox5.Location = new System.Drawing.Point(7, 7);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(440, 353);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Sản Phẩm chưa áp dụng";
            // 
            // lstSPChuaApDung
            // 
            this.lstSPChuaApDung.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSPChuaApDung.FormattingEnabled = true;
            this.lstSPChuaApDung.ItemHeight = 20;
            this.lstSPChuaApDung.Location = new System.Drawing.Point(6, 21);
            this.lstSPChuaApDung.Name = "lstSPChuaApDung";
            this.lstSPChuaApDung.Size = new System.Drawing.Size(399, 304);
            this.lstSPChuaApDung.TabIndex = 0;
            this.lstSPChuaApDung.DoubleClick += new System.EventHandler(this.lstSPChuaApDung_DoubleClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnThoat);
            this.groupBox3.Controls.Add(this.btnNhapExcel);
            this.groupBox3.Controls.Add(this.btnXuatExcel);
            this.groupBox3.Controls.Add(this.BtnXoa);
            this.groupBox3.Controls.Add(this.btnSua);
            this.groupBox3.Controls.Add(this.btnHuy);
            this.groupBox3.Controls.Add(this.btnLuu);
            this.groupBox3.Controls.Add(this.btnThem);
            this.groupBox3.Controls.Add(this.tabChiTietKhuyenMai);
            this.groupBox3.Location = new System.Drawing.Point(12, 270);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1357, 447);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Các sản phẩm áp dụng";
            // 
            // btnThoat
            // 
            this.btnThoat.Location = new System.Drawing.Point(842, 413);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(73, 23);
            this.btnThoat.TabIndex = 10;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // btnNhapExcel
            // 
            this.btnNhapExcel.Location = new System.Drawing.Point(741, 413);
            this.btnNhapExcel.Name = "btnNhapExcel";
            this.btnNhapExcel.Size = new System.Drawing.Size(95, 23);
            this.btnNhapExcel.TabIndex = 9;
            this.btnNhapExcel.Text = "Nhập Excel";
            this.btnNhapExcel.UseVisualStyleBackColor = true;
            this.btnNhapExcel.Click += new System.EventHandler(this.btnNhapExcel_Click);
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.Location = new System.Drawing.Point(626, 413);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(99, 23);
            this.btnXuatExcel.TabIndex = 8;
            this.btnXuatExcel.Text = "Xuất Excel";
            this.btnXuatExcel.UseVisualStyleBackColor = true;
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.Location = new System.Drawing.Point(534, 413);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(75, 23);
            this.btnHuy.TabIndex = 7;
            this.btnHuy.Text = "Hủy Bỏ";
            this.btnHuy.UseVisualStyleBackColor = true;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.Location = new System.Drawing.Point(211, 413);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(75, 23);
            this.btnLuu.TabIndex = 6;
            this.btnLuu.Text = "Lưu";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // BtnXoa
            // 
            this.BtnXoa.Location = new System.Drawing.Point(402, 413);
            this.BtnXoa.Name = "BtnXoa";
            this.BtnXoa.Size = new System.Drawing.Size(114, 23);
            this.BtnXoa.TabIndex = 5;
            this.BtnXoa.Text = "Xóa/Vô hiệu hóa";
            this.BtnXoa.UseVisualStyleBackColor = true;
            this.BtnXoa.Click += new System.EventHandler(this.BtnXoa_Click);
            // 
            // btnSua
            // 
            this.btnSua.Location = new System.Drawing.Point(311, 413);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(75, 23);
            this.btnSua.TabIndex = 4;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnThem
            // 
            this.btnThem.Location = new System.Drawing.Point(115, 413);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(75, 23);
            this.btnThem.TabIndex = 3;
            this.btnThem.Text = "Thêm mới";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // KhuyenMai
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1381, 729);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "KhuyenMai";
            this.Text = "KhuyenMai";
            this.Load += new System.EventHandler(this.KhuyenMai_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhuyenMai)).EndInit();
            this.tabChiTietKhuyenMai.ResumeLayout(false);
            this.tpThongTinKM.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tpSPApDung.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboLocTrangThai;
        private System.Windows.Forms.ComboBox cboLocLoaiKM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnTim;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvKhuyenMai;
        private System.Windows.Forms.TabControl tabChiTietKhuyenMai;
        private System.Windows.Forms.TabPage tpThongTinKM;
        private System.Windows.Forms.TabPage tpSPApDung;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtTenKM;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMaKM;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpNgayKT;
        private System.Windows.Forms.DateTimePicker dtpNgayBD;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtGiaTri;
        private System.Windows.Forms.Label lblGiaTri;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboLoaiKM;
        private System.Windows.Forms.RichTextBox txtDKApDung;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.RichTextBox txtMoTa;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnXuatExcel;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button BtnXoa;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.Button btnNhapExcel;
        private System.Windows.Forms.ListBox lstSPChuaApDung;
        private System.Windows.Forms.ListBox lstSPDaApDung;
        private System.Windows.Forms.Button btnBoTatCaApDung;
        private System.Windows.Forms.Button btnBoChon;
        private System.Windows.Forms.Button btnThemChonApDung;
        private System.Windows.Forms.Button btnThemTatCaApDung;
        private System.Windows.Forms.TextBox txtDKSoLuongDuocTang;
        private System.Windows.Forms.Label lblDKSoLuongDuocTang;
        private System.Windows.Forms.TextBox txtDKSoLuongCanMua;
        private System.Windows.Forms.Label lblDKSoLuongCanMua;
        private System.Windows.Forms.CheckBox chkHoatDong;
    }
}