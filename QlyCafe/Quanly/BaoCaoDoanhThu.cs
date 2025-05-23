using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts.Wpf;
using LiveCharts;

namespace QlyCafe.Quanly
{
    public partial class BaoCaoDoanhThu : Form
    {
        public BaoCaoDoanhThu()
        {
            InitializeComponent();
        }

        private void BaoCaoDoanhThu_Load(object sender, EventArgs e)
        {
            LoadComboBoxes();
            SetDefaultDates();
            InitializeSummaryLabels();
            SetupChartComboBoxes();

            btnXemBaoCao.Click += new EventHandler(btnXemBaoCao_Click);
            dgvChiTietDoanhThu.RowPostPaint += dgvChiTietDoanhThu_RowPostPaint;
            // Tự động tải dữ liệu báo cáo khi form load
            btnXemBaoCao_Click(this, EventArgs.Empty); // Gọi sự kiện click của nút Xem Báo Cáo
        }

        private void LoadComboBoxes()
        {
            LoadNhanVienComboBox();
            LoadLoaiSanPhamComboBox();
            LoadSanPhamComboBox(); // Load tất cả sản phẩm ban đầu
        }

        private void LoadNhanVienComboBox()
        {
            try
            {
                string sqlNhanVien = "SELECT MaNV, TenNV FROM NhanVien WHERE MaNV IS NOT NULL AND TenNV IS NOT NULL ORDER BY TenNV";
                DataTable dtNhanVien = Function.GetDataToTable(sqlNhanVien);

                DataRow allRowNhanVien = dtNhanVien.NewRow();
                allRowNhanVien["MaNV"] = "";
                allRowNhanVien["TenNV"] = "Tất cả nhân viên";
                dtNhanVien.Rows.InsertAt(allRowNhanVien, 0);

                Function.FillCombo(cboNhanVien, "TenNV", "MaNV", dtNhanVien);
                cboNhanVien.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLoaiSanPhamComboBox()
        {
            try
            {
                string sqlLoaiSP = "SELECT MaLoai, TenLoai FROM Loai WHERE MaLoai IS NOT NULL AND TenLoai IS NOT NULL ORDER BY TenLoai";
                DataTable dtLoaiSP = Function.GetDataToTable(sqlLoaiSP);

                DataRow allRowLoaiSP = dtLoaiSP.NewRow();
                allRowLoaiSP["MaLoai"] = "";
                allRowLoaiSP["TenLoai"] = "Tất cả loại";
                dtLoaiSP.Rows.InsertAt(allRowLoaiSP, 0);

                Function.FillCombo(cboLoaiSanPham, "TenLoai", "MaLoai", dtLoaiSP);
                cboLoaiSanPham.SelectedIndex = 0;
                // Gán sự kiện sau khi đã load xong dữ liệu
                cboLoaiSanPham.SelectedIndexChanged += CboLoaiSanPham_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách loại sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSanPhamComboBox(string maLoai = "")
        {
            try
            {
                string sqlSanPham;
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (string.IsNullOrEmpty(maLoai)) // Nếu không có mã loại hoặc là "Tất cả loại"
                {
                    sqlSanPham = "SELECT MaSP, TenSP FROM SanPham WHERE MaSP IS NOT NULL AND TenSP IS NOT NULL ORDER BY TenSP";
                }
                else
                {
                    sqlSanPham = "SELECT MaSP, TenSP FROM SanPham WHERE MaLoai = @MaLoai AND MaSP IS NOT NULL AND TenSP IS NOT NULL ORDER BY TenSP";
                    parameters.Add(new SqlParameter("@MaLoai", maLoai));
                }

                DataTable dtSanPham = Function.GetDataToTable(sqlSanPham, parameters.ToArray());

                DataRow allRowSanPham = dtSanPham.NewRow();
                allRowSanPham["MaSP"] = "";
                allRowSanPham["TenSP"] = "Tất cả sản phẩm";
                dtSanPham.Rows.InsertAt(allRowSanPham, 0);

                Function.FillCombo(cboSanPham, "TenSP", "MaSP", dtSanPham);
                cboSanPham.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CboLoaiSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLoaiSanPham.SelectedValue != null)
            {
                string selectedMaLoai = cboLoaiSanPham.SelectedValue.ToString();
                LoadSanPhamComboBox(selectedMaLoai);
            }
            // Không cần else vì LoadSanPhamComboBox đã xử lý trường hợp maLoai rỗng
        }

        private void SetDefaultDates()
        {
            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpDenNgay.Value = DateTime.Now;
        }

        private void InitializeSummaryLabels()
        {
            // Các label này nằm trong các panel con của panelTongQuan
            lblTongDoanhThu.Text = "0 VNĐ";
            lblTongSoHDB.Text = "0";
            lblTrungBinhHoaDon.Text = "0 VNĐ";
            lblSanPhamBanChay.Text = "N/A";
            lblNhanVienHieuQua.Text = "N/A";
        }

        private void SetupChartComboBoxes()
        {
            // Sử dụng tên control từ file Designer (cboLoaiBieuDo và cboHienThiTheo)
            cboLoaiBieuDo.Items.Clear();
            cboLoaiBieuDo.Items.Add("Doanh thu theo Ngày");
            cboLoaiBieuDo.Items.Add("Doanh thu theo Loại Sản Phẩm");
            cboLoaiBieuDo.Items.Add("Doanh thu theo Sản Phẩm");
            cboLoaiBieuDo.Items.Add("Doanh thu theo Nhân Viên");
            if (cboLoaiBieuDo.Items.Count > 0)
                cboLoaiBieuDo.SelectedIndex = 0;

            cboHienThiTheo.Items.Clear();
            cboHienThiTheo.Items.Add("Tổng Tiền");
            // cboHienThiTheo.Items.Add("Số Lượng Bán"); // Bỏ comment nếu muốn thêm tùy chọn này
            if (cboHienThiTheo.Items.Count > 0)
                cboHienThiTheo.SelectedIndex = 0;

            // Gắn sự kiện SelectedIndexChanged
            // Xóa các sự kiện cũ đi nếu có để tránh gắn nhiều lần
            cboLoaiBieuDo.SelectedIndexChanged -= ChartFilter_SelectedIndexChanged;
            cboHienThiTheo.SelectedIndexChanged -= ChartFilter_SelectedIndexChanged;

            cboLoaiBieuDo.SelectedIndexChanged += ChartFilter_SelectedIndexChanged;
            cboHienThiTheo.SelectedIndexChanged += ChartFilter_SelectedIndexChanged;
        }

        // Sự kiện chung cho cả hai ComboBox lọc của biểu đồ
        private void ChartFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Chỉ gọi btnXemBaoCao_Click nếu form đã load xong và các ComboBox đã có item được chọn
            if (this.Visible && cboLoaiBieuDo.SelectedItem != null && cboHienThiTheo.SelectedItem != null)
            {
                btnXemBaoCao_Click(sender, e); // Gọi lại để cập nhật toàn bộ báo cáo bao gồm cả biểu đồ
            }
        }

        private void btnXemBaoCao_Click(object sender, EventArgs e)
        {
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);

            string maNV = cboNhanVien.SelectedValue?.ToString() ?? "";
            string maLoaiSP = cboLoaiSanPham.SelectedValue?.ToString() ?? "";
            string maSPFilter = cboSanPham.SelectedValue?.ToString() ?? ""; // Đổi tên để tránh nhầm lẫn với cột MaSP

            Func<List<SqlParameter>> createBaseParameters = () => new List<SqlParameter>
            {
                new SqlParameter("@TuNgay", tuNgay),
                new SqlParameter("@DenNgay", denNgay)
            };

            Action<List<SqlParameter>, string, object> addParameterIfNotEmpty = (paramList, paramName, value) =>
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    paramList.Add(new SqlParameter(paramName, value));
                }
            };

            StringBuilder baseWhereHdb = new StringBuilder(" WHERE hdb.IsDeleted = 0 AND hdb.NgayBan >= @TuNgay AND hdb.NgayBan <= @DenNgay ");
            if (!string.IsNullOrEmpty(maNV))
            {
                baseWhereHdb.Append(" AND hdb.MaNV = @MaNV ");
            }

            StringBuilder productRelatedWhere = new StringBuilder(" WHERE hdb.IsDeleted = 0 AND hdb.NgayBan >= @TuNgay AND hdb.NgayBan <= @DenNgay ");
            if (!string.IsNullOrEmpty(maNV))
            {
                productRelatedWhere.Append(" AND hdb.MaNV = @MaNV ");
            }
            if (!string.IsNullOrEmpty(maLoaiSP))
            {
                productRelatedWhere.Append(" AND sp.MaLoai = @MaLoaiSP ");
            }
            if (!string.IsNullOrEmpty(maSPFilter)) //Sử dụng maSPFilter
            {
                productRelatedWhere.Append(" AND ct.MaSP = @MaSP "); //Sử dụng @MaSP ở đây vì trong SQL là MaSP
            }

            // 1. Tổng Doanh Thu
            List<SqlParameter> tongDoanhThuParams = createBaseParameters();
            addParameterIfNotEmpty(tongDoanhThuParams, "@MaNV", maNV);
            addParameterIfNotEmpty(tongDoanhThuParams, "@MaLoaiSP", maLoaiSP);
            addParameterIfNotEmpty(tongDoanhThuParams, "@MaSP", maSPFilter); //Sử dụng maSPFilter

            string sqlTongDoanhThu;
            decimal tongDoanhThu = 0;

            if (!string.IsNullOrEmpty(maLoaiSP) || !string.IsNullOrEmpty(maSPFilter)) //Sử dụng maSPFilter
            {
                sqlTongDoanhThu = $@"SELECT SUM(ct.ThanhTien) 
                                   FROM HoaDonBan hdb
                                   JOIN ChiTietHDB ct ON hdb.MaHDB = ct.MaHDB
                                   JOIN SanPham sp ON ct.MaSP = sp.MaSP 
                                   {productRelatedWhere.ToString()}";
            }
            else
            {
                sqlTongDoanhThu = $"SELECT SUM(hdb.TongTien) FROM HoaDonBan hdb {baseWhereHdb.ToString()}";
                tongDoanhThuParams.RemoveAll(p => p.ParameterName == "@MaLoaiSP" || p.ParameterName == "@MaSP");
            }
            string resultTongDoanhThu = Function.GetFieldValue(sqlTongDoanhThu, tongDoanhThuParams.ToArray());
            if (!string.IsNullOrEmpty(resultTongDoanhThu) && decimal.TryParse(resultTongDoanhThu, out decimal parsedRevenue))
            {
                tongDoanhThu = parsedRevenue;
            }
            lblTongDoanhThu.Text = tongDoanhThu.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " VNĐ";

            // 2. Tổng Số Hóa Đơn Bán
            List<SqlParameter> tongSoHDBParams = createBaseParameters();
            addParameterIfNotEmpty(tongSoHDBParams, "@MaNV", maNV);
            addParameterIfNotEmpty(tongSoHDBParams, "@MaLoaiSP", maLoaiSP);
            addParameterIfNotEmpty(tongSoHDBParams, "@MaSP", maSPFilter); //Sử dụng maSPFilter

            string sqlTongSoHDB;
            int tongSoHDB = 0;
            if (!string.IsNullOrEmpty(maLoaiSP) || !string.IsNullOrEmpty(maSPFilter)) //Sử dụng maSPFilter
            {
                sqlTongSoHDB = $@"SELECT COUNT(DISTINCT hdb.MaHDB) 
                                FROM HoaDonBan hdb
                                JOIN ChiTietHDB ct ON hdb.MaHDB = ct.MaHDB
                                JOIN SanPham sp ON ct.MaSP = sp.MaSP
                                {productRelatedWhere.ToString()}";
            }
            else
            {
                sqlTongSoHDB = $"SELECT COUNT(hdb.MaHDB) FROM HoaDonBan hdb {baseWhereHdb.ToString()}";
                tongSoHDBParams.RemoveAll(p => p.ParameterName == "@MaLoaiSP" || p.ParameterName == "@MaSP");
            }
            string resultTongSoHDB = Function.GetFieldValue(sqlTongSoHDB, tongSoHDBParams.ToArray());
            if (!string.IsNullOrEmpty(resultTongSoHDB) && int.TryParse(resultTongSoHDB, out int parsedCount))
            {
                tongSoHDB = parsedCount;
            }
            lblTongSoHDB.Text = tongSoHDB.ToString();

            // 3. Trung Bình/Hóa Đơn
            decimal trungBinhHoaDon = (tongSoHDB > 0) ? (tongDoanhThu / tongSoHDB) : 0;
            lblTrungBinhHoaDon.Text = trungBinhHoaDon.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " VNĐ";

            // 4. Sản Phẩm Bán Chạy (Theo Doanh Thu)
            List<SqlParameter> spBanChayParams = createBaseParameters();
            addParameterIfNotEmpty(spBanChayParams, "@MaNV", maNV);
            addParameterIfNotEmpty(spBanChayParams, "@MaLoaiSP", maLoaiSP);
            addParameterIfNotEmpty(spBanChayParams, "@MaSP", maSPFilter); //Sử dụng maSPFilter

            string sqlSPBanChay = $@"SELECT TOP 1 sp.TenSP 
                                   FROM HoaDonBan hdb 
                                   JOIN ChiTietHDB ct ON hdb.MaHDB = ct.MaHDB 
                                   JOIN SanPham sp ON ct.MaSP = sp.MaSP
                                   {productRelatedWhere.ToString()}
                                   GROUP BY sp.MaSP, sp.TenSP 
                                   ORDER BY SUM(ct.ThanhTien) DESC";
            string spBanChay = Function.GetFieldValue(sqlSPBanChay, spBanChayParams.ToArray());
            lblSanPhamBanChay.Text = string.IsNullOrEmpty(spBanChay) ? "N/A" : spBanChay;

            // 5. Nhân Viên Hiệu Quả (Theo Doanh Thu)
            string nhanVienHieuQua = "N/A";
            List<SqlParameter> nvHieuQuaParams = createBaseParameters(); // Bắt đầu với params cơ bản
            addParameterIfNotEmpty(nvHieuQuaParams, "@MaLoaiSP", maLoaiSP);
            addParameterIfNotEmpty(nvHieuQuaParams, "@MaSP", maSPFilter);


            if (!string.IsNullOrEmpty(maNV))
            {
                addParameterIfNotEmpty(nvHieuQuaParams, "@MaNV", maNV); // Thêm MaNV vào params nếu có
                string sqlCheckNVRevenue;
                if (!string.IsNullOrEmpty(maLoaiSP) || !string.IsNullOrEmpty(maSPFilter)) //Sử dụng maSPFilter
                {
                    sqlCheckNVRevenue = $@"SELECT SUM(ct.ThanhTien) 
                                           FROM HoaDonBan hdb
                                           JOIN ChiTietHDB ct ON hdb.MaHDB = ct.MaHDB
                                           JOIN SanPham sp ON ct.MaSP = sp.MaSP
                                           {productRelatedWhere.ToString()}"; // productRelatedWhere đã bao gồm MaNV
                }
                else
                {
                    sqlCheckNVRevenue = $"SELECT SUM(hdb.TongTien) FROM HoaDonBan hdb {baseWhereHdb.ToString()}"; // baseWhereHdb đã bao gồm MaNV
                }
                string nvRevenueResult = Function.GetFieldValue(sqlCheckNVRevenue, nvHieuQuaParams.ToArray());
                if (!string.IsNullOrEmpty(nvRevenueResult) && decimal.TryParse(nvRevenueResult, out decimal parsedNvRevenue) && parsedNvRevenue > 0)
                {
                    nhanVienHieuQua = cboNhanVien.Text;
                }
            }
            else
            {
                StringBuilder topNvProductWhere = new StringBuilder(" WHERE hdb.IsDeleted = 0 AND hdb.NgayBan >= @TuNgay AND hdb.NgayBan <= @DenNgay ");
                if (!string.IsNullOrEmpty(maLoaiSP))
                {
                    topNvProductWhere.Append(" AND sp.MaLoai = @MaLoaiSP ");
                }
                if (!string.IsNullOrEmpty(maSPFilter)) //Sử dụng maSPFilter
                {
                    topNvProductWhere.Append(" AND ct.MaSP = @MaSP ");
                }

                string sqlNVHieuQua = $@"SELECT TOP 1 nv.TenNV 
                                       FROM HoaDonBan hdb 
                                       JOIN NhanVien nv ON hdb.MaNV = nv.MaNV 
                                       LEFT JOIN ChiTietHDB ct ON hdb.MaHDB = ct.MaHDB
                                       LEFT JOIN SanPham sp ON ct.MaSP = sp.MaSP
                                       {topNvProductWhere.ToString()}
                                       GROUP BY nv.MaNV, nv.TenNV 
                                       ORDER BY SUM(CASE WHEN ct.MaSP IS NOT NULL THEN ct.ThanhTien ELSE hdb.TongTien END) DESC";
                // nvHieuQuaParams đã được chuẩn bị ở trên (không chứa MaNV)
                nhanVienHieuQua = Function.GetFieldValue(sqlNVHieuQua, nvHieuQuaParams.ToArray());
                if (string.IsNullOrEmpty(nhanVienHieuQua)) nhanVienHieuQua = "N/A";
            }
            lblNhanVienHieuQua.Text = nhanVienHieuQua;

            // Gọi hàm load dữ liệu cho DataGridView
            LoadDetailedReportDataGridView(productRelatedWhere.ToString(), createBaseParameters, addParameterIfNotEmpty, maNV, maLoaiSP, maSPFilter);

            // --- Load Chart Data ---
            LoadChartData(tuNgay, denNgay, maNV, maLoaiSP, maSPFilter, baseWhereHdb.ToString(), productRelatedWhere.ToString(), createBaseParameters, addParameterIfNotEmpty);
        }

        private void LoadDetailedReportDataGridView(string whereClauseSql, Func<List<SqlParameter>> createBaseParamsFunc, Action<List<SqlParameter>, string, object> addParamFunc, string maNV, string maLoaiSP, string maSPFilter)
        {
            try
            {
                string sql = $@"
                    SELECT 
                        hdb.MaHDB AS [Mã HĐ],
                        hdb.NgayBan AS [Ngày Bán],
                        nv.TenNV AS [Nhân Viên],
                        ISNULL(kh.tenKH, N'Khách vãng lai') AS [Khách Hàng],
                        sp.TenSP AS [Sản Phẩm], 
                        l.TenLoai AS [Loại SP],
                        ct.SoLuong AS [SL],
                        (CASE WHEN ct.SoLuong <> 0 THEN ct.ThanhTien / ct.SoLuong ELSE 0 END) AS [Đơn Giá],
                        ct.ThanhTien AS [Thành Tiền],
                        ISNULL(km.TenKM, N'Không có') AS [Khuyến Mãi]
                    FROM HoaDonBan hdb
                    INNER JOIN ChiTietHDB ct ON hdb.MaHDB = ct.MaHDB
                    INNER JOIN SanPham sp ON ct.MaSP = sp.MaSP
                    INNER JOIN NhanVien nv ON hdb.MaNV = nv.MaNV
                    LEFT JOIN KhachHang kh ON hdb.MaKH = kh.MaKH
                    LEFT JOIN Loai l ON sp.MaLoai = l.MaLoai
                    LEFT JOIN KhuyenMai km ON ct.MaKM = km.MaKM
                    {whereClauseSql}
                    ORDER BY hdb.NgayBan DESC, hdb.MaHDB DESC, sp.TenSP ASC";

                List<SqlParameter> parameters = createBaseParamsFunc();
                addParamFunc(parameters, "@MaNV", maNV);
                addParamFunc(parameters, "@MaLoaiSP", maLoaiSP);
                addParamFunc(parameters, "@MaSP", maSPFilter);

                DataTable dtChiTiet = Function.GetDataToTable(sql, parameters.ToArray());

                // Quan trọng: Thiết lập AutoGenerateColumns trước khi gán DataSource
                dgvChiTietDoanhThu.AutoGenerateColumns = true;
                dgvChiTietDoanhThu.DataSource = null;
                //dgvChiTietDoanhThu.Columns.Clear(); // Không cần nếu AutoGenerateColumns = true và bạn muốn cột tự tạo lại

                if (dtChiTiet != null)
                {
                    dgvChiTietDoanhThu.DataSource = dtChiTiet;
                    // Chỉ gọi CustomizeDataGridView nếu có cột được tạo
                    if (dgvChiTietDoanhThu.Columns.Count > 0)
                    {
                        CustomizeDataGridView();
                    }
                    dgvChiTietDoanhThu.Refresh();
                }
                else
                {
                    dgvChiTietDoanhThu.Rows.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết báo cáo doanh thu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvChiTietDoanhThu.DataSource = null;
            }
        }

        private void dgvChiTietDoanhThu_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // Vẽ số thứ tự cho hàng
            using (SolidBrush b = new SolidBrush(dgvChiTietDoanhThu.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }


        private void CustomizeDataGridView()
        {
            dgvChiTietDoanhThu.AutoGenerateColumns = false; // Tắt tự động tạo cột nếu bạn muốn định nghĩa cột hoàn toàn bằng tay, nhưng ở đây ta đã có tên cột từ SQL

            if (dgvChiTietDoanhThu.Columns["Mã HĐ"] != null)
            {
                dgvChiTietDoanhThu.Columns["Mã HĐ"].HeaderText = "Mã HĐ";
                dgvChiTietDoanhThu.Columns["Mã HĐ"].Width = 120;
            }
            if (dgvChiTietDoanhThu.Columns["Ngày Bán"] != null)
            {
                dgvChiTietDoanhThu.Columns["Ngày Bán"].HeaderText = "Ngày Bán";
                dgvChiTietDoanhThu.Columns["Ngày Bán"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvChiTietDoanhThu.Columns["Ngày Bán"].Width = 130;
            }
            if (dgvChiTietDoanhThu.Columns["Nhân Viên"] != null)
            {
                dgvChiTietDoanhThu.Columns["Nhân Viên"].HeaderText = "Nhân Viên";
                dgvChiTietDoanhThu.Columns["Nhân Viên"].Width = 150;
            }
            if (dgvChiTietDoanhThu.Columns["Khách Hàng"] != null)
            {
                dgvChiTietDoanhThu.Columns["Khách Hàng"].HeaderText = "Khách Hàng";
                dgvChiTietDoanhThu.Columns["Khách Hàng"].Width = 150;
            }
            if (dgvChiTietDoanhThu.Columns["Sản Phẩm"] != null)
            {
                dgvChiTietDoanhThu.Columns["Sản Phẩm"].HeaderText = "Sản Phẩm";
                dgvChiTietDoanhThu.Columns["Sản Phẩm"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvChiTietDoanhThu.Columns["Sản Phẩm"].MinimumWidth = 180;
            }
            if (dgvChiTietDoanhThu.Columns["Loại SP"] != null)
            {
                dgvChiTietDoanhThu.Columns["Loại SP"].HeaderText = "Loại SP";
                dgvChiTietDoanhThu.Columns["Loại SP"].Width = 120;
            }
            if (dgvChiTietDoanhThu.Columns["SL"] != null)
            {
                dgvChiTietDoanhThu.Columns["SL"].HeaderText = "SL";
                dgvChiTietDoanhThu.Columns["SL"].Width = 60;
                dgvChiTietDoanhThu.Columns["SL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dgvChiTietDoanhThu.Columns["Đơn Giá"] != null)
            {
                dgvChiTietDoanhThu.Columns["Đơn Giá"].HeaderText = "Đơn Giá";
                dgvChiTietDoanhThu.Columns["Đơn Giá"].Width = 100;
                dgvChiTietDoanhThu.Columns["Đơn Giá"].DefaultCellStyle.Format = "N0";
                dgvChiTietDoanhThu.Columns["Đơn Giá"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dgvChiTietDoanhThu.Columns["Thành Tiền"] != null)
            {
                dgvChiTietDoanhThu.Columns["Thành Tiền"].HeaderText = "Thành Tiền";
                dgvChiTietDoanhThu.Columns["Thành Tiền"].Width = 120;
                dgvChiTietDoanhThu.Columns["Thành Tiền"].DefaultCellStyle.Format = "N0";
                dgvChiTietDoanhThu.Columns["Thành Tiền"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dgvChiTietDoanhThu.Columns["Khuyến Mãi"] != null)
            {
                dgvChiTietDoanhThu.Columns["Khuyến Mãi"].HeaderText = "Khuyến Mãi";
                dgvChiTietDoanhThu.Columns["Khuyến Mãi"].Width = 120;
            }

            dgvChiTietDoanhThu.AllowUserToAddRows = false;
            dgvChiTietDoanhThu.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvChiTietDoanhThu.RowHeadersVisible = true; // Hiển thị RowHeader để vẽ STT
            dgvChiTietDoanhThu.RowHeadersWidth = 50; // Điều chỉnh độ rộng RowHeader cho STT
        }

        // HÀM MỚI ĐỂ LOAD BIỂU ĐỒ
        private void LoadChartData(DateTime tuNgay, DateTime denNgay, string maNV, string maLoaiSP, string maSPFilter,
                                   string baseWhereHdbSql, string productRelatedWhereSql,
                                   Func<List<SqlParameter>> createBaseParamsFunc,
                                   Action<List<SqlParameter>, string, object> addParamFunc)
        {
            if (cboLoaiBieuDo.SelectedItem == null || cboHienThiTheo.SelectedItem == null)
            {
                chartRevenue.Series.Clear();
                chartRevenue.AxisX.Clear();
                chartRevenue.AxisY.Clear();
                return;
            }

            string chartType = cboLoaiBieuDo.SelectedItem.ToString();
            string valueType = cboHienThiTheo.SelectedItem.ToString(); // Hiện tại chỉ có "Tổng Tiền"

            string valueField = "SUM(ct.ThanhTien)"; // Mặc định cho "Tổng Tiền"
            string valueFieldHDBOnly = "SUM(hdb.TongTien)"; // Khi không join ChiTietHDB
            string yAxisTitle = "Tổng Tiền (VNĐ)";

            // Nếu có "Số Lượng Bán" thì sẽ thay đổi valueField và yAxisTitle ở đây
            // if (valueType == "Số Lượng Bán") { ... }

            DataTable dtChart = null;
            string sqlChart = "";
            List<SqlParameter> chartParams = createBaseParamsFunc();
            addParamFunc(chartParams, "@MaNV", maNV);
            addParamFunc(chartParams, "@MaLoaiSP", maLoaiSP);
            addParamFunc(chartParams, "@MaSP", maSPFilter);


            chartRevenue.Series.Clear();
            chartRevenue.AxisX.Clear();
            chartRevenue.AxisY.Clear();
            chartRevenue.LegendLocation = LegendLocation.Right;

            try
            {
                switch (chartType)
                {
                    case "Doanh thu theo Ngày":
                        // Nếu không có bộ lọc sản phẩm/loại sản phẩm, sử dụng TongTien từ HoaDonBan
                        if (string.IsNullOrEmpty(maLoaiSP) && string.IsNullOrEmpty(maSPFilter))
                        {
                            List<SqlParameter> dailyParams = createBaseParamsFunc(); // Chỉ cần ngày tháng, và MaNV nếu có
                            addParamFunc(dailyParams, "@MaNV", maNV);
                            sqlChart = $@"SELECT CONVERT(date, hdb.NgayBan) AS Category, {valueFieldHDBOnly} AS Value 
                                        FROM HoaDonBan hdb 
                                        {baseWhereHdbSql} 
                                        GROUP BY CONVERT(date, hdb.NgayBan) 
                                        ORDER BY Category ASC";
                            dtChart = Function.GetDataToTable(sqlChart, dailyParams.ToArray());
                        }
                        else // Có bộ lọc sản phẩm/loại, dùng ThanhTien từ ChiTietHDB
                        {
                            sqlChart = $@"SELECT CONVERT(date, hdb.NgayBan) AS Category, {valueField} AS Value 
                                        FROM HoaDonBan hdb
                                        JOIN ChiTietHDB ct ON hdb.MaHDB = ct.MaHDB
                                        JOIN SanPham sp ON ct.MaSP = sp.MaSP
                                        {productRelatedWhereSql} 
                                        GROUP BY CONVERT(date, hdb.NgayBan) 
                                        ORDER BY Category ASC";
                            dtChart = Function.GetDataToTable(sqlChart, chartParams.ToArray());
                        }

                        if (dtChart != null && dtChart.Rows.Count > 0)
                        {
                            var dayLabels = new List<string>();
                            var revenueValues = new ChartValues<decimal>();
                            Dictionary<DateTime, decimal> dailyData = new Dictionary<DateTime, decimal>();

                            // Điền dữ liệu cho tất cả các ngày trong khoảng, kể cả ngày không có doanh thu
                            for (DateTime date = tuNgay.Date; date <= denNgay.Date; date = date.AddDays(1))
                            {
                                dailyData[date] = 0;
                            }
                            foreach (DataRow row in dtChart.Rows)
                            {
                                dailyData[Convert.ToDateTime(row["Category"])] = Convert.ToDecimal(row["Value"]);
                            }

                            foreach (var item in dailyData.OrderBy(kvp => kvp.Key))
                            {
                                dayLabels.Add(item.Key.ToString("dd/MM"));
                                revenueValues.Add(item.Value);
                            }

                            chartRevenue.Series = new SeriesCollection
                            {
                                new LineSeries
                                {
                                    Title = yAxisTitle.Split('(')[0].Trim(),
                                    Values = revenueValues,
                                    DataLabels = true,
                                    PointGeometrySize = 10
                                }
                            };
                            chartRevenue.AxisX.Add(new Axis { Title = "Ngày", Labels = dayLabels, Separator = new Separator { Step = Math.Max(1, dayLabels.Count / 10), IsEnabled = false } });
                            chartRevenue.AxisY.Add(new Axis { Title = yAxisTitle, LabelFormatter = value => value.ToString("N0") });
                        }
                        break;

                    case "Doanh thu theo Loại Sản Phẩm":
                        sqlChart = $@"SELECT l.TenLoai AS Category, {valueField} AS Value 
                                    FROM HoaDonBan hdb
                                    JOIN ChiTietHDB ct ON hdb.MaHDB = ct.MaHDB
                                    JOIN SanPham sp ON ct.MaSP = sp.MaSP
                                    JOIN Loai l ON sp.MaLoai = l.MaLoai
                                    {productRelatedWhereSql}
                                    GROUP BY l.MaLoai, l.TenLoai
                                    HAVING {valueField} > 0
                                    ORDER BY Value DESC";
                        dtChart = Function.GetDataToTable(sqlChart, chartParams.ToArray());
                        if (dtChart != null && dtChart.Rows.Count > 0)
                        {
                            chartRevenue.Series = new SeriesCollection();
                            var categoryLabels = new List<string>();
                            foreach (DataRow row in dtChart.Rows)
                            {
                                categoryLabels.Add(row["Category"].ToString());
                                chartRevenue.Series.Add(new ColumnSeries
                                {
                                    Title = row["Category"].ToString(),
                                    Values = new ChartValues<decimal> { Convert.ToDecimal(row["Value"]) },
                                    DataLabels = true,
                                    MaxColumnWidth = 50
                                });
                            }
                            // Với ColumnSeries, Labels cho AxisX nên được set theo tên Category
                            chartRevenue.AxisX.Add(new Axis { Title = "Loại Sản Phẩm", Labels = categoryLabels, Separator = new Separator { Step = 1, IsEnabled = false } });
                            chartRevenue.AxisY.Add(new Axis { Title = yAxisTitle, LabelFormatter = value => value.ToString("N0"), MinValue = 0 });
                        }
                        break;

                    case "Doanh thu theo Sản Phẩm":
                        sqlChart = $@"SELECT TOP 10 sp.TenSP AS Category, {valueField} AS Value 
                                    FROM HoaDonBan hdb
                                    JOIN ChiTietHDB ct ON hdb.MaHDB = ct.MaHDB
                                    JOIN SanPham sp ON ct.MaSP = sp.MaSP
                                    {productRelatedWhereSql}
                                    GROUP BY sp.MaSP, sp.TenSP
                                    HAVING {valueField} > 0
                                    ORDER BY Value DESC"; // Lấy top 10 sản phẩm
                        dtChart = Function.GetDataToTable(sqlChart, chartParams.ToArray());
                        if (dtChart != null && dtChart.Rows.Count > 0)
                        {
                            chartRevenue.Series = new SeriesCollection();
                            var productLabels = new List<string>();
                            foreach (DataRow row in dtChart.Rows)
                            {
                                productLabels.Add(row["Category"].ToString());
                                chartRevenue.Series.Add(new ColumnSeries
                                {
                                    Title = row["Category"].ToString(),
                                    Values = new ChartValues<decimal> { Convert.ToDecimal(row["Value"]) },
                                    DataLabels = true,
                                    MaxColumnWidth = 50
                                });
                            }
                            chartRevenue.AxisX.Add(new Axis { Title = "Sản Phẩm (Top 10)", Labels = productLabels, LabelsRotation = 15, Separator = new Separator { Step = 1, IsEnabled = false } });
                            chartRevenue.AxisY.Add(new Axis { Title = yAxisTitle, LabelFormatter = value => value.ToString("N0"), MinValue = 0 });
                        }
                        break;

                    case "Doanh thu theo Nhân Viên":
                        // Tạo params riêng cho truy vấn này, không bao gồm MaNV nếu đang tìm tất cả nhân viên
                        List<SqlParameter> nvChartParams = createBaseParamsFunc();
                        addParamFunc(nvChartParams, "@MaLoaiSP", maLoaiSP);
                        addParamFunc(nvChartParams, "@MaSP", maSPFilter);

                        string nvWhereClause = productRelatedWhereSql;
                        if (string.IsNullOrEmpty(maNV)) // Nếu không lọc theo NV cụ thể, bỏ điều kiện MaNV khỏi where clause
                        {
                            nvWhereClause = nvWhereClause.Replace("AND hdb.MaNV = @MaNV", "");
                        }
                        else
                        {
                            addParamFunc(nvChartParams, "@MaNV", maNV); // Nếu có lọc MaNV, thêm vào params
                        }

                        sqlChart = $@"SELECT nv.TenNV AS Category, SUM(CASE WHEN ct.MaSP IS NOT NULL THEN ct.ThanhTien ELSE hdb.TongTien END) AS Value
                                    FROM HoaDonBan hdb
                                    JOIN NhanVien nv ON hdb.MaNV = nv.MaNV
                                    LEFT JOIN ChiTietHDB ct ON hdb.MaHDB = ct.MaHDB 
                                    LEFT JOIN SanPham sp ON ct.MaSP = sp.MaSP 
                                    {nvWhereClause}
                                    GROUP BY nv.MaNV, nv.TenNV
                                    HAVING SUM(CASE WHEN ct.MaSP IS NOT NULL THEN ct.ThanhTien ELSE hdb.TongTien END) > 0
                                    ORDER BY Value DESC";
                        dtChart = Function.GetDataToTable(sqlChart, nvChartParams.ToArray());
                        if (dtChart != null && dtChart.Rows.Count > 0)
                        {
                            chartRevenue.Series = new SeriesCollection();
                            var employeeLabels = new List<string>();
                            foreach (DataRow row in dtChart.Rows)
                            {
                                employeeLabels.Add(row["Category"].ToString());
                                chartRevenue.Series.Add(new ColumnSeries
                                {
                                    Title = row["Category"].ToString(),
                                    Values = new ChartValues<decimal> { Convert.ToDecimal(row["Value"]) },
                                    DataLabels = true,
                                    MaxColumnWidth = 50
                                });
                            }
                            chartRevenue.AxisX.Add(new Axis { Title = "Nhân Viên", Labels = employeeLabels, LabelsRotation = 10, Separator = new Separator { Step = 1, IsEnabled = false } });
                            chartRevenue.AxisY.Add(new Axis { Title = yAxisTitle, LabelFormatter = value => value.ToString("N0"), MinValue = 0 });
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu biểu đồ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                chartRevenue.Series.Clear(); // Xóa biểu đồ nếu có lỗi
            }

            if (dtChart == null || dtChart.Rows.Count == 0)
            {
                // Hiển thị thông báo không có dữ liệu hoặc để biểu đồ trống
                // MessageBox.Show("Không có dữ liệu để hiển thị trên biểu đồ với bộ lọc hiện tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chartRevenue.Series.Clear();
                chartRevenue.AxisX.Clear();
                chartRevenue.AxisY.Clear();
            }
        }
    }
}



