using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QlyCafe.Quanly
{
    public partial class TimHDBan : Form
    {
        DataTable tblHDB;
        public TimHDBan()
        {
            InitializeComponent();
        }

        private void TimHDBan_Load(object sender, EventArgs e)
        {
            ResetValues();

            //LoadComboBoxNhanVienSearch();
            //LoadComboBoxKhachHangSearch();
            btnXemChiTiet.Enabled = false; // Chỉ bật khi có dòng được chọn
            PerformSearch(false);
        }

        private void LoadComboBoxNhanVienSearch()
        {
            Function.FillCombo(cboTimNV, "TenNV", "MaNV", "SELECT MaNV, TenNV FROM dbo.NhanVien ORDER BY TenNV");
            DataTable dtNV = (DataTable)cboTimNV.DataSource;
            if (dtNV != null)
            {
                DataRow dr = dtNV.NewRow();
                dr["MaNV"] = ""; // Hoặc DBNull.Value
                dr["TenNV"] = "-- Tất cả Nhân Viên --";
                dtNV.Rows.InsertAt(dr, 0);
                cboTimNV.SelectedIndex = 0;
            }
        }

        private void LoadComboBoxKhachHangSearch()
        {
            Function.FillCombo(cboTimNV, "TenKH", "MaKH", "SELECT MaKH, TenKH FROM dbo.KhachHang ORDER BY TenKH");
            DataTable dtKH = (DataTable)cboTimKH.DataSource;
            if (dtKH != null)
            {
                DataRow dr = dtKH.NewRow();
                dr["MaKH"] = ""; // Hoặc DBNull.Value
                dr["TenKH"] = "-- Tất cả Khách Hàng --";
                dtKH.Rows.InsertAt(dr, 0);
                cboTimKH.SelectedIndex = 0;
            }
        }

        // Phương thức mới để nạp cboMaBan
        private void LoadComboBoxMaBanSearch()
        {
            // Hiển thị dạng "Bàn X (Trạng thái)"
            string sqlLoadBan = "SELECT id, N'Bàn ' + CAST(id AS VARCHAR(10)) + N' (' + ISNULL(status, N'N/A') + N')' AS DisplayTextBan FROM dbo.Ban ORDER BY id"; //
            Function.FillCombo(cboMaBan, "DisplayTextBan", "id", sqlLoadBan); //

            DataTable dtBan = (DataTable)cboMaBan.DataSource;
            if (dtBan != null)
            {
                DataRow dr = dtBan.NewRow();
                dr["id"] = DBNull.Value; // Giá trị cho "-- Tất cả Bàn --"
                dr["DisplayTextBan"] = "-- Tất cả Bàn --";
                dtBan.Rows.InsertAt(dr, 0);
                cboMaBan.SelectedIndex = 0;
            }
        }

        private void CustomizeDgvKQTimKiem()
        {
            if (dgvKQTimKiemHDB.DataSource == null || dgvKQTimKiemHDB.Columns.Count == 0)
            {
                return;
            }

            // Tên các cột từ câu lệnh SQL (đảm bảo khớp)
            string colMaHDB = "MaHDB";
            string colNgayBan = "NgayBan";
            string colIDBan = "IDBan";
            string colTenKH = "TenKH";
            string colMaKH = "MaKH";
            string colTenNV = "TenNV";
            string colMaNV = "MaNV";
            string colTongTien = "TongTien";

            // Cài đặt cho từng cột
            if (dgvKQTimKiemHDB.Columns[colMaHDB] != null)
            {
                dgvKQTimKiemHDB.Columns[colMaHDB].HeaderText = "Mã HĐB";
                dgvKQTimKiemHDB.Columns[colMaHDB].Width = 130; // Điều chỉnh
            }

            if (dgvKQTimKiemHDB.Columns[colNgayBan] != null)
            {
                dgvKQTimKiemHDB.Columns[colNgayBan].HeaderText = "Ngày Bán";
                dgvKQTimKiemHDB.Columns[colNgayBan].Width = 90; // Điều chỉnh
                dgvKQTimKiemHDB.Columns[colNgayBan].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvKQTimKiemHDB.Columns[colNgayBan].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvKQTimKiemHDB.Columns[colIDBan] != null)
            {
                dgvKQTimKiemHDB.Columns[colIDBan].HeaderText = "Số Bàn";
                dgvKQTimKiemHDB.Columns[colIDBan].Width = 60; // Điều chỉnh
                dgvKQTimKiemHDB.Columns[colIDBan].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvKQTimKiemHDB.Columns[colIDBan].Visible = true;
            }

            if (dgvKQTimKiemHDB.Columns[colTenKH] != null)
            {
                dgvKQTimKiemHDB.Columns[colTenKH].HeaderText = "Tên Khách Hàng";
                dgvKQTimKiemHDB.Columns[colTenKH].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvKQTimKiemHDB.Columns[colTenKH].FillWeight = 60; // Tăng FillWeight để ưu tiên không gian
            }

            if (dgvKQTimKiemHDB.Columns[colMaKH] != null)
            {
                dgvKQTimKiemHDB.Columns[colMaKH].HeaderText = "Mã KH";
                dgvKQTimKiemHDB.Columns[colMaKH].Width = 70; // Điều chỉnh
                dgvKQTimKiemHDB.Columns[colMaKH].Visible = true;
            }

            if (dgvKQTimKiemHDB.Columns[colTenNV] != null)
            {
                dgvKQTimKiemHDB.Columns[colTenNV].HeaderText = "Nhân Viên Lập";
                dgvKQTimKiemHDB.Columns[colTenNV].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvKQTimKiemHDB.Columns[colTenNV].FillWeight = 70; // Điều chỉnh FillWeight
            }

            if (dgvKQTimKiemHDB.Columns[colMaNV] != null)
            {
                dgvKQTimKiemHDB.Columns[colMaNV].HeaderText = "Mã NV";
                dgvKQTimKiemHDB.Columns[colMaNV].Width = 70; // Điều chỉnh
                dgvKQTimKiemHDB.Columns[colMaNV].Visible = true;
            }

            if (dgvKQTimKiemHDB.Columns[colTongTien] != null)
            {
                dgvKQTimKiemHDB.Columns[colTongTien].HeaderText = "Tổng Tiền";
                dgvKQTimKiemHDB.Columns[colTongTien].Width = 110; // Điều chỉnh
                dgvKQTimKiemHDB.Columns[colTongTien].DefaultCellStyle.Format = "N0";
                dgvKQTimKiemHDB.Columns[colTongTien].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // Các cài đặt chung cho DataGridView
            dgvKQTimKiemHDB.AllowUserToAddRows = false;
            dgvKQTimKiemHDB.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvKQTimKiemHDB.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKQTimKiemHDB.MultiSelect = false;
            dgvKQTimKiemHDB.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(230, 240, 255);
            dgvKQTimKiemHDB.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Căn giữa tiêu đề cột
        }

        private void ResetValues()
        {
            txtTimMaHDB.Text = "";
            dtpTimTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTimDenNgay.Value = DateTime.Now;

            // Gọi các hàm load ComboBox ở đây để đảm bảo chúng được nạp lại khi reset
            LoadComboBoxNhanVienSearch();
            LoadComboBoxKhachHangSearch();
            LoadComboBoxMaBanSearch(); // Gọi hàm nạp cboMaBan

            dgvKQTimKiemHDB.DataSource = null;
            lblSoLuongKQ.Text = "Tìm thấy: 0 kết quả";
            txtTimMaHDB.Focus();
        }

        private void PerformSearch(bool useDateFilter)
        {
            string maHDBFilter = txtTimMaHDB.Text.Trim();
            string maKHFilter = (cboTimKH.SelectedIndex > 0 && cboTimKH.SelectedValue != null && cboTimKH.SelectedValue != DBNull.Value)
                                ? cboTimKH.SelectedValue.ToString()
                                : null;
            string maNVFilter = (cboTimNV.SelectedIndex > 0 && cboTimNV.SelectedValue != null && cboTimNV.SelectedValue != DBNull.Value)
                                ? cboTimNV.SelectedValue.ToString()
                                : null;

            // Lấy IDBan từ cboMaBan để lọc
            int? idBanFilter = null; //
            if (cboMaBan.SelectedIndex > 0 && cboMaBan.SelectedValue != null && cboMaBan.SelectedValue != DBNull.Value) //
            {
                idBanFilter = Convert.ToInt32(cboMaBan.SelectedValue); //
            }

            // Thêm hdb.IDBan vào câu lệnh SELECT
            string sqlBase = @"SELECT hdb.MaHDB, hdb.NgayBan, ISNULL(kh.TenKH, kh.DiaChi) AS TenKH, 
                                      nv.TenNV, hdb.TongTien, hdb.MaKH, hdb.MaNV, hdb.IDBan 
                       FROM dbo.HoaDonBan hdb
                       LEFT JOIN dbo.KhachHang kh ON hdb.MaKH = kh.MaKH
                       LEFT JOIN dbo.NhanVien nv ON hdb.MaNV = nv.MaNV
                       WHERE hdb.IsDeleted = 0"; //

            List<SqlParameter> parameters = new List<SqlParameter>();
            string conditions = "";

            if (!string.IsNullOrWhiteSpace(maHDBFilter))
            {
                conditions += " AND hdb.MaHDB LIKE @MaHDB";
                parameters.Add(new SqlParameter("@MaHDB", SqlDbType.VarChar, 25) { Value = "%" + maHDBFilter + "%" });
            }

            if (useDateFilter)
            {
                DateTime tuNgay = dtpTimTuNgay.Value.Date;
                DateTime denNgay = dtpTimDenNgay.Value.Date; // Chỉ lấy phần Date

                if (tuNgay > denNgay) // So sánh chỉ phần Date
                {
                    MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc.", "Lỗi Khoảng Ngày", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Cân nhắc reset lại ngày hoặc không thực hiện tìm kiếm
                    dtpTimTuNgay.Value = denNgay; // Gợi ý: đặt lại ngày bắt đầu = ngày kết thúc
                    return;
                }
                // Để bao gồm cả ngày kết thúc, điều kiện nên là NgayBan <= denNgay
                // Hoặc denNgay = dtpTimDenNgay.Value.Date.AddDays(1).AddTicks(-1); (nếu NgayBan có cả giờ phút)
                // Nếu NgayBan chỉ lưu Date thì BETWEEN tuNgay AND denNgay là đủ
                conditions += " AND hdb.NgayBan BETWEEN @TuNgay AND @DenNgay";
                parameters.Add(new SqlParameter("@TuNgay", SqlDbType.Date) { Value = tuNgay });
                parameters.Add(new SqlParameter("@DenNgay", SqlDbType.Date) { Value = denNgay });
            }

            if (!string.IsNullOrWhiteSpace(maKHFilter))
            {
                conditions += " AND hdb.MaKH = @MaKH";
                parameters.Add(new SqlParameter("@MaKH", SqlDbType.VarChar, 10) { Value = maKHFilter });
            }

            if (!string.IsNullOrWhiteSpace(maNVFilter))
            {
                conditions += " AND hdb.MaNV = @MaNV";
                parameters.Add(new SqlParameter("@MaNV", SqlDbType.VarChar, 10) { Value = maNVFilter });
            }

            // Thêm điều kiện lọc theo IDBan
            if (idBanFilter.HasValue) //
            {
                conditions += " AND hdb.IDBan = @IDBan"; //
                parameters.Add(new SqlParameter("@IDBan", SqlDbType.Int) { Value = idBanFilter.Value }); //
            }


            string sql = sqlBase + conditions + " ORDER BY hdb.NgayBan DESC, hdb.MaHDB DESC";

            tblHDB = Function.GetDataToTable(sql, parameters.ToArray());
            dgvKQTimKiemHDB.DataSource = tblHDB;
            CustomizeDgvKQTimKiem(); // Gọi sau khi gán DataSource

            if (tblHDB != null)
            {
                lblSoLuongKQ.Text = $"Tìm thấy: {tblHDB.Rows.Count} kết quả.";
                if (tblHDB.Rows.Count == 0 && (useDateFilter || !string.IsNullOrWhiteSpace(maHDBFilter) || idBanFilter.HasValue || maKHFilter != null || maNVFilter != null)) // Chỉ thông báo nếu có tiêu chí lọc
                {
                    MessageBox.Show("Không có bản ghi nào thỏa mãn điều kiện tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                lblSoLuongKQ.Text = "Tìm thấy: 0 kết quả.";
                if (useDateFilter || !string.IsNullOrWhiteSpace(maHDBFilter) || idBanFilter.HasValue || maKHFilter != null || maNVFilter != null)
                    MessageBox.Show("Lỗi khi tải dữ liệu hoặc không có bản ghi nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            btnXemChiTiet.Enabled = (tblHDB != null && tblHDB.Rows.Count > 0);
        }

        private void dgvKQTimKiemHDB_SelectionChanged(object sender, EventArgs e)
        {
            btnXemChiTiet.Enabled = (dgvKQTimKiemHDB.SelectedRows.Count > 0);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetValues();
            PerformSearch(false);
        }

        private void txtTimMaHDB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnTimKiem_Click(sender, e);
                e.Handled = true;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            PerformSearch(true);
        }

        private void dgvKQTimKiemHDB_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var cell = dgvKQTimKiemHDB.Rows[e.RowIndex].Cells["MaHDB"].Value; // Lấy MaHDB
            if (cell == null || cell == DBNull.Value)
            {
                MessageBox.Show("Không thể lấy mã HĐB từ dòng này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string maHDBCCanXem = cell.ToString();

            // Mở form HoaDonBan (cần tạo constructor nhận MaHDB trong HoaDonBan.cs)
            var frm = new HoaDonBan(maHDBCCanXem);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(this);

            PerformSearch(true); // Làm mới lại sau khi đóng form chi tiết
        }

        private void btnXemChiTiet_Click(object sender, EventArgs e)
        {
            if (dgvKQTimKiemHDB.SelectedRows.Count > 0)
            {
                string maHDBCCanXem = string.Empty;
                object cellValue = dgvKQTimKiemHDB.SelectedRows[0].Cells["MaHDB"].Value; // Lấy MaHDB

                if (cellValue != null && cellValue != DBNull.Value)
                {
                    maHDBCCanXem = cellValue.ToString();
                }
                else
                {
                    MessageBox.Show("Không thể lấy được mã hóa đơn bán từ dòng đã chọn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(maHDBCCanXem) || maHDBCCanXem.Equals("System.Data.DataRowView", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Mã hóa đơn bán không hợp lệ.", "Lỗi Mã Hóa Đơn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Mở form HoaDonBan
                HoaDonBan frmHDBChiTiet = new HoaDonBan(maHDBCCanXem.Trim());
                frmHDBChiTiet.StartPosition = FormStartPosition.CenterScreen;
                frmHDBChiTiet.ShowDialog(this);

                PerformSearch(true);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn bán từ danh sách để xem chi tiết.", "Chưa chọn hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dtpTimTuNgay_ValueChanged(object sender, EventArgs e)
        {
            if (this.Visible && dtpTimTuNgay.Value.Date > dtpTimDenNgay.Value.Date) // So sánh phần Date để tránh lỗi do time
            {
                MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc. Vui lòng chọn lại.",
                            "Lỗi Khoảng Ngày", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Có thể đặt lại dtpTimTuNgay.Value = dtpTimDenNgay.Value; hoặc xử lý khác
            }
        }

        private void dtpTimDenNgay_ValueChanged(object sender, EventArgs e)
        {
            if (this.Visible && dtpTimDenNgay.Value.Date < dtpTimTuNgay.Value.Date) // So sánh phần Date
            {
                MessageBox.Show("Ngày kết thúc không thể nhỏ hơn ngày bắt đầu. Vui lòng chọn lại.",
                            "Lỗi Khoảng Ngày", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Có thể đặt lại dtpTimDenNgay.Value = dtpTimTuNgay.Value;
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
