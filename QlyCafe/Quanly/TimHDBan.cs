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

            LoadComboBoxNhanVienSearch();
            LoadComboBoxKhachHangSearch();
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

        private void CustomizeDgvKQTimKiem()
        {
            if (dgvKQTimKiemHDB.DataSource == null || dgvKQTimKiemHDB.Columns.Count == 0)
            {
                return;
            }

            // Đổi tên các cột cho phù hợp với hóa đơn bán
            string colMaHDB = "MaHDB";
            string colNgayBan = "NgayBan"; // Đổi từ NgayNhap
            string colTenKH = "TenKH";     // Đổi từ TenNCC, và JOIN để lấy TenKH
            string colTenNV = "TenNV";
            string colTongTien = "TongTien";
            string colMaKH = "MaKH";       // Đổi từ MaNCC
            string colMaNV = "MaNV";

            if (dgvKQTimKiemHDB.Columns[colMaHDB] != null)
            {
                dgvKQTimKiemHDB.Columns[colMaHDB].HeaderText = "Mã HĐB"; // Đổi
                dgvKQTimKiemHDB.Columns[colMaHDB].Width = 130;
            }

            if (dgvKQTimKiemHDB.Columns[colNgayBan] != null)
            {
                dgvKQTimKiemHDB.Columns[colNgayBan].HeaderText = "Ngày Bán"; // Đổi
                dgvKQTimKiemHDB.Columns[colNgayBan].Width = 100;
                dgvKQTimKiemHDB.Columns[colNgayBan].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvKQTimKiemHDB.Columns[colNgayBan].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvKQTimKiemHDB.Columns[colTenKH] != null)
            {
                dgvKQTimKiemHDB.Columns[colTenKH].HeaderText = "Tên Khách Hàng"; // Đổi
                dgvKQTimKiemHDB.Columns[colTenKH].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvKQTimKiemHDB.Columns[colTenKH].FillWeight = 35;
            }
            if (dgvKQTimKiemHDB.Columns[colMaKH] != null) // Hiển thị thêm mã KH nếu muốn
            {
                dgvKQTimKiemHDB.Columns[colMaKH].HeaderText = "Mã KH";
                dgvKQTimKiemHDB.Columns[colMaKH].Width = 80;
                dgvKQTimKiemHDB.Columns[colMaKH].Visible = true;
            }


            if (dgvKQTimKiemHDB.Columns[colTenNV] != null)
            {
                dgvKQTimKiemHDB.Columns[colTenNV].HeaderText = "Nhân Viên Lập";
                dgvKQTimKiemHDB.Columns[colTenNV].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvKQTimKiemHDB.Columns[colTenNV].FillWeight = 30;
            }
            if (dgvKQTimKiemHDB.Columns[colMaNV] != null)
            {
                dgvKQTimKiemHDB.Columns[colMaNV].HeaderText = "Mã NV";
                dgvKQTimKiemHDB.Columns[colMaNV].Width = 80;
                dgvKQTimKiemHDB.Columns[colMaNV].Visible = true;
            }

            if (dgvKQTimKiemHDB.Columns[colTongTien] != null)
            {
                dgvKQTimKiemHDB.Columns[colTongTien].HeaderText = "Tổng Tiền";
                dgvKQTimKiemHDB.Columns[colTongTien].Width = 120;
                dgvKQTimKiemHDB.Columns[colTongTien].DefaultCellStyle.Format = "N0";
                dgvKQTimKiemHDB.Columns[colTongTien].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            dgvKQTimKiemHDB.AllowUserToAddRows = false;
            dgvKQTimKiemHDB.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvKQTimKiemHDB.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKQTimKiemHDB.MultiSelect = false;
            dgvKQTimKiemHDB.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(230, 240, 255);
        }

        private void ResetValues()
        {
            txtTimMaHDB.Text = "";
            dtpTimTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTimDenNgay.Value = DateTime.Now;

            
            if (cboTimKH.Items.Count > 0) 
                cboTimKH.SelectedIndex = 0;
            else
                cboTimKH.SelectedIndex = -1;

            if (cboTimNV.Items.Count > 0)
                cboTimNV.SelectedIndex = 0;
            else
                cboTimNV.SelectedIndex = -1;

            dgvKQTimKiemHDB.DataSource = null;
            lblSoLuongKQ.Text = "Tìm thấy: 0 kết quả";
            txtTimMaHDB.Focus();
        }

        private void PerformSearch(bool useDateFilter)
        {
            string maHDBFilter = txtTimMaHDB.Text.Trim();
            // Lấy MaKH từ cboTimKH
            string maKHFilter = (cboTimKH.SelectedIndex > 0 && cboTimKH.SelectedValue != null && cboTimKH.SelectedValue != DBNull.Value)
                                ? cboTimKH.SelectedValue.ToString()
                                : null;
            string maNVFilter = (cboTimNV.SelectedIndex > 0 && cboTimNV.SelectedValue != null && cboTimNV.SelectedValue != DBNull.Value)
                                ? cboTimNV.SelectedValue.ToString()
                                : null;

            // Câu lệnh SQL cho Hóa Đơn Bán
            string sqlBase = @"SELECT hdb.MaHDB, hdb.NgayBan, ISNULL(kh.TenKH, kh.DiaChi) AS TenKH, nv.TenNV, hdb.TongTien, hdb.MaKH, hdb.MaNV 
                       FROM dbo.HoaDonBan hdb
                       LEFT JOIN dbo.KhachHang kh ON hdb.MaKH = kh.MaKH
                       LEFT JOIN dbo.NhanVien nv ON hdb.MaNV = nv.MaNV
                       WHERE hdb.IsDeleted = 0"; // Giả sử có cột IsDeleted

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
                DateTime denNgay = dtpTimDenNgay.Value.Date.AddDays(1).AddTicks(-1);

                if (tuNgay > denNgay.Date)
                {
                    MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc.", "Lỗi Khoảng Ngày", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                conditions += " AND hdb.NgayBan BETWEEN @TuNgay AND @DenNgay"; // NgayBan
                parameters.Add(new SqlParameter("@TuNgay", SqlDbType.Date) { Value = tuNgay });
                parameters.Add(new SqlParameter("@DenNgay", SqlDbType.Date) { Value = denNgay.Date });
            }

            if (!string.IsNullOrWhiteSpace(maKHFilter)) // Lọc theo MaKH
            {
                conditions += " AND hdb.MaKH = @MaKH";
                parameters.Add(new SqlParameter("@MaKH", SqlDbType.VarChar, 10) { Value = maKHFilter });
            }

            if (!string.IsNullOrWhiteSpace(maNVFilter))
            {
                conditions += " AND hdb.MaNV = @MaNV";
                parameters.Add(new SqlParameter("@MaNV", SqlDbType.VarChar, 10) { Value = maNVFilter });
            }

            string sql = sqlBase + conditions + " ORDER BY hdb.NgayBan DESC, hdb.MaHDB DESC"; // Sắp xếp theo NgayBan

            tblHDB = Function.GetDataToTable(sql, parameters.ToArray()); // Đổi tên tblHDN
            dgvKQTimKiemHDB.DataSource = tblHDB;
            CustomizeDgvKQTimKiem();

            if (tblHDB != null)
            {
                lblSoLuongKQ.Text = $"Tìm thấy: {tblHDB.Rows.Count} kết quả.";
                if (tblHDB.Rows.Count == 0 && useDateFilter)
                {
                    MessageBox.Show("Không có bản ghi nào thỏa mãn điều kiện tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                lblSoLuongKQ.Text = "Tìm thấy: 0 kết quả.";
                if (useDateFilter)
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
