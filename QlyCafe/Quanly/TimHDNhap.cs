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
using DocumentFormat.OpenXml.Spreadsheet;

namespace QlyCafe.Quanly
{
    
    public partial class TimHDNhap : Form
    {
        DataTable tblHDN;
        public TimHDNhap()
        {
            InitializeComponent();
        }

        private void TimHDNhap_Load(object sender, EventArgs e)
        {
            // 1. Thiết lập ReadOnly, Enabled/Disabled cho các control (nếu cần)
            // Ví dụ, nếu có các TextBox hiển thị chi tiết mà không cho sửa:
            // txtChiTietABC.ReadOnly = true;

            // 2. Gọi ResetValues để đặt lại các control tìm kiếm về trạng thái mặc định
            ResetValues();
            // dgviewKQTimKiemHDN.DataSource = null; // ResetValues có thể đã làm điều này hoặc không

            // 3. Tải dữ liệu cho ComboBoxes
            LoadComboBoxNhaCungCapSearch();
            LoadComboBoxNhanVienSearch();

            // 4. Thiết lập trạng thái nút ban đầu
            btnXemChiTiet.Enabled = false; // Chỉ bật khi có dòng được chọn

            // 5. Cấu hình DataGridView (đã có hàm CustomizeDgvKQTimKiem)
            // Hàm này sẽ được gọi trong btnTimKiem_Click sau khi có DataSource
            
            // 6. Tải dữ liệu mặc định vào DataGridView KHI FORM LOAD
            // Bằng cách gọi sự kiện Click của nút Tìm kiếm với các tiêu chí hiện tại (đã được ResetValues)
            // Điều này giả định btnTimKiem_Click sẽ tải tất cả nếu không có tiêu chí cụ thể
            PerformSearch(false);

            // Thêm KeyPress cho các TextBox tìm kiếm nếu muốn nhấn Enter để tìm
            txtTimMaHDN.KeyPress += TxtTimMaHDN_KeyPress;
        }

        private void LoadComboBoxNhaCungCapSearch()
        {
            // Cách 1: Dùng Function.FillCombo và tự thêm item "Tất cả"
            Function.FillCombo(cboTimNhaCC, "TenNCC", "MaNCC", "SELECT MaNCC, TenNCC FROM dbo.NhaCungCap ORDER BY TenNCC");
            // Chèn item "Tất cả" vào đầu
            DataTable dtNCC = (DataTable)cboTimNhaCC.DataSource;
            if (dtNCC != null)
            {
                DataRow dr = dtNCC.NewRow();
                dr["MaNCC"] = ""; // Hoặc DBNull.Value hoặc một giá trị đặc biệt bạn quy ước
                dr["TenNCC"] = "-- Tất cả Nhà Cung Cấp --";
                dtNCC.Rows.InsertAt(dr, 0);
                // cboTimNhaCC.DataSource = dtNCC; // Gán lại nếu cần, nhưng thường ComboBox tự cập nhật
                cboTimNhaCC.SelectedIndex = 0;
            }
            else // Nếu Function.FillCombo không gán DataSource là DataTable
            {
                // Hoặc nếu Function.FillCombo của bạn đã hỗ trợ thêm item "Tất cả" thì không cần làm gì thêm
                // Nếu không, bạn có thể cần một cách khác để thêm item này.
                // Ví dụ đơn giản nếu FillCombo chỉ điền item:
                // cboTimNhaCC.Items.Insert(0, new { MaNCC = "", TenNCC = "-- Tất cả Nhà Cung Cấp --" });
                // cboTimNhaCC.SelectedIndex = 0;
            }
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

        // Hàm này để tùy chỉnh các cột sau khi dgvKQTimKiemHDN có DataSource
        // Nó sẽ được gọi trong btnTimKiem_Click sau khi gán DataSource
        private void CustomizeDgvKQTimKiem()
        {
            // Đảm bảo DataGridView đã có DataSource và các cột đã được tạo
            if (dgvKQTimKiemHDN.DataSource == null || dgvKQTimKiemHDN.Columns.Count == 0)
            {
                Console.WriteLine("CustomizeDgvKQTimKiem: DataSource is null or no columns found.");
                return;
            }

            // Tên các cột trong DataSource (DataPropertyName)
            string colMaHDN = "MaHDN";
            string colNgayNhap = "NgayNhap";
            string colTenNCC = "TenNCC";    // Giả sử bạn JOIN để lấy TenNCC
            string colTenNV = "TenNV";      // Giả sử bạn JOIN để lấy TenNV
            string colTongTien = "TongTien";
            string colMaNCC = "MaNCC";      // Mã Nhà Cung Cấp
            string colMaNV = "MaNV";        // Mã Nhân Viên

            // --- Mã Hóa Đơn Nhập ---
            if (dgvKQTimKiemHDN.Columns[colMaHDN] != null)
            {
                dgvKQTimKiemHDN.Columns[colMaHDN].HeaderText = "Mã HĐN";
                dgvKQTimKiemHDN.Columns[colMaHDN].Width = 130; // Cho mã dài "HDN_ddMMyyyy_HHmmss"
            }

            // --- Ngày Nhập ---
            if (dgvKQTimKiemHDN.Columns[colNgayNhap] != null)
            {
                dgvKQTimKiemHDN.Columns[colNgayNhap].HeaderText = "Ngày Nhập";
                dgvKQTimKiemHDN.Columns[colNgayNhap].Width = 100;
                dgvKQTimKiemHDN.Columns[colNgayNhap].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvKQTimKiemHDN.Columns[colNgayNhap].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // --- Tên Nhà Cung Cấp ---
            if (dgvKQTimKiemHDN.Columns[colTenNCC] != null)
            {
                dgvKQTimKiemHDN.Columns[colTenNCC].HeaderText = "Nhà Cung Cấp";
                // Cho phép cột này co giãn để lấp đầy không gian còn lại
                dgvKQTimKiemHDN.Columns[colTenNCC].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvKQTimKiemHDN.Columns[colTenNCC].FillWeight = 35; // Ưu tiên độ rộng
            }

            // --- Tên Nhân Viên ---
            if (dgvKQTimKiemHDN.Columns[colTenNV] != null)
            {
                dgvKQTimKiemHDN.Columns[colTenNV].HeaderText = "Nhân Viên Lập";
                dgvKQTimKiemHDN.Columns[colTenNV].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvKQTimKiemHDN.Columns[colTenNV].FillWeight = 30;
            }

            // --- Tổng Tiền ---
            if (dgvKQTimKiemHDN.Columns[colTongTien] != null)
            {
                dgvKQTimKiemHDN.Columns[colTongTien].HeaderText = "Tổng Tiền";
                dgvKQTimKiemHDN.Columns[colTongTien].Width = 120;
                dgvKQTimKiemHDN.Columns[colTongTien].DefaultCellStyle.Format = "N0"; // Định dạng số (1,234,567)
                dgvKQTimKiemHDN.Columns[colTongTien].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // --- Mã Nhà Cung Cấp (MaNCC) ---
            // Bạn có thể muốn ẩn cột này nếu đã hiển thị Tên NCC
            if (dgvKQTimKiemHDN.Columns[colMaNCC] != null)
            {
                dgvKQTimKiemHDN.Columns[colMaNCC].HeaderText = "Mã NCC";
                dgvKQTimKiemHDN.Columns[colMaNCC].Width = 80;
                dgvKQTimKiemHDN.Columns[colMaNCC].Visible = true; // Đặt là false nếu muốn ẩn
            }

            // --- Mã Nhân Viên (MaNV) ---
            // Bạn có thể muốn ẩn cột này nếu đã hiển thị Tên NV
            if (dgvKQTimKiemHDN.Columns[colMaNV] != null)
            {
                dgvKQTimKiemHDN.Columns[colMaNV].HeaderText = "Mã NV";
                dgvKQTimKiemHDN.Columns[colMaNV].Width = 80;
                dgvKQTimKiemHDN.Columns[colMaNV].Visible = true; // Đặt là false nếu muốn ẩn
            }


            // Các thiết lập chung cho DataGridView
            dgvKQTimKiemHDN.AllowUserToAddRows = false;
            dgvKQTimKiemHDN.EditMode = DataGridViewEditMode.EditProgrammatically; // Không cho sửa trực tiếp trên lưới
            dgvKQTimKiemHDN.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Chọn cả dòng
            dgvKQTimKiemHDN.MultiSelect = false; // Không cho chọn nhiều dòng
            dgvKQTimKiemHDN.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(230, 240, 255); // Ví dụ màu xen kẽ cho dễ đọc
        }

        // Sự kiện để bật/tắt nút "Xem Chi Tiết" (bạn có thể đã đăng ký trong Designer hoặc ở Form_Load)
        private void dgvKQTimKiemHDN_SelectionChanged(object sender, EventArgs e)
        {
            btnXemChiTiet.Enabled = (dgvKQTimKiemHDN.SelectedRows.Count > 0);
        }

        private void ResetValues()
        {
            txtTimMaHDN.Text = "";
            dtpTimTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTimDenNgay.Value = DateTime.Now;

            if (cboTimNhaCC.Items.Count > 0)
                cboTimNhaCC.SelectedIndex = 0; // Giả sử item đầu là "Tất cả"
            else
                cboTimNhaCC.SelectedIndex = -1;

            if (cboTimNV.Items.Count > 0)
                cboTimNV.SelectedIndex = 0; // Giả sử item đầu là "Tất cả"
            else
                cboTimNV.SelectedIndex = -1;

            // Quan trọng: Xóa DataSource của DataGridView khi reset tiêu chí
            dgvKQTimKiemHDN.DataSource = null;
            lblSoLuongKQ.Text = "Tìm thấy: 0 kết quả"; // Reset thông báo số lượng

            txtTimMaHDN.Focus();
        }

        // Các hàm LoadComboBoxNhaCungCapSearch, LoadComboBoxNhanVienSearch, CustomizeDgvKQTimKiem giữ nguyên

        // Hàm btnLamMoi_Click cập nhật
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetValues();
            // Sau khi reset, gọi lại tìm kiếm để tải lại danh sách (thường là tất cả hóa đơn)
            PerformSearch(false);
        }

        // Sự kiện KeyPress cho txtTimMaHDN để tìm khi nhấn Enter (ví dụ)
        private void TxtTimMaHDN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnTimKiem_Click(sender, e); // Gọi sự kiện click của nút Tìm Kiếm
                e.Handled = true; // Ngăn tiếng 'beep'
            }
        }

        // Hàm này sẽ là nơi bạn xây dựng câu lệnh SQL và tải dữ liệu
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            PerformSearch(true);
        }

        private void PerformSearch(bool useDateFilter) // Thêm tham số để kiểm soát việc lọc theo ngày
        {
            string maHDNFilter = txtTimMaHDN.Text.Trim();
            string maNCCFilter = (cboTimNhaCC.SelectedIndex > 0 && cboTimNhaCC.SelectedValue != null && cboTimNhaCC.SelectedValue != DBNull.Value)
                                ? cboTimNhaCC.SelectedValue.ToString()
                                : null;
            string maNVFilter = (cboTimNV.SelectedIndex > 0 && cboTimNV.SelectedValue != null && cboTimNV.SelectedValue != DBNull.Value)
                                ? cboTimNV.SelectedValue.ToString()
                                : null;

            // Xây dựng câu lệnh SQL
            string sqlBase = @"SELECT hdn.MaHDN, hdn.NgayNhap, ncc.TenNCC, nv.TenNV, hdn.TongTien, hdn.MaNCC, hdn.MaNV 
                       FROM dbo.HoaDonNhap hdn
                       LEFT JOIN dbo.NhaCungCap ncc ON hdn.MaNCC = ncc.MaNCC
                       LEFT JOIN dbo.NhanVien nv ON hdn.MaNV = nv.MaNV
                       WHERE hdn.IsDeleted = 0"; // Luôn lọc các hóa đơn chưa xóa (nếu bạn đã thêm cột IsDeleted)
                                                 // Nếu chưa có cột IsDeleted, bỏ điều kiện này: WHERE 1=1

            List<SqlParameter> parameters = new List<SqlParameter>();
            string conditions = "";

            if (!string.IsNullOrWhiteSpace(maHDNFilter))
            {
                conditions += " AND hdn.MaHDN LIKE @MaHDN";
                parameters.Add(new SqlParameter("@MaHDN", SqlDbType.VarChar, 25) { Value = "%" + maHDNFilter + "%" });
            }

            if (useDateFilter) // Chỉ thêm điều kiện ngày nếu useDateFilter là true
            {
                DateTime tuNgay = dtpTimTuNgay.Value.Date;
                DateTime denNgay = dtpTimDenNgay.Value.Date.AddDays(1).AddTicks(-1);

                if (tuNgay > denNgay.Date) // Kiểm tra lại logic ngày ở đây nếu cần
                {
                    MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc cho việc tìm kiếm.",
                                   "Lỗi Khoảng Ngày Tìm Kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                conditions += " AND hdn.NgayNhap BETWEEN @TuNgay AND @DenNgay";
                parameters.Add(new SqlParameter("@TuNgay", SqlDbType.Date) { Value = tuNgay });
                parameters.Add(new SqlParameter("@DenNgay", SqlDbType.Date) { Value = denNgay.Date });
            }

            if (!string.IsNullOrWhiteSpace(maNCCFilter))
            {
                conditions += " AND hdn.MaNCC = @MaNCC";
                parameters.Add(new SqlParameter("@MaNCC", SqlDbType.VarChar, 10) { Value = maNCCFilter });
            }

            if (!string.IsNullOrWhiteSpace(maNVFilter))
            {
                conditions += " AND hdn.MaNV = @MaNV";
                parameters.Add(new SqlParameter("@MaNV", SqlDbType.VarChar, 10) { Value = maNVFilter });
            }

            string sql = sqlBase + conditions + " ORDER BY hdn.NgayNhap DESC, hdn.MaHDN DESC";

            tblHDN = Function.GetDataToTable(sql, parameters.ToArray());
            dgvKQTimKiemHDN.DataSource = tblHDN;
            CustomizeDgvKQTimKiem(); // Gọi sau khi gán DataSource

            if (tblHDN != null)
            {
                lblSoLuongKQ.Text = $"Tìm thấy: {tblHDN.Rows.Count} kết quả.";
                if (tblHDN.Rows.Count == 0 && useDateFilter) // Chỉ thông báo không có kết quả khi người dùng chủ động tìm kiếm có điều kiện
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
            btnXemChiTiet.Enabled = (tblHDN != null && tblHDN.Rows.Count > 0);
        }

        private void dgvKQTimKiemHDN_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // click vào header thì bỏ qua

            // Lấy giá trị cột MaHDN theo RowIndex
            var cell = dgvKQTimKiemHDN.Rows[e.RowIndex].Cells["MaHDN"].Value;
            if (cell == null || cell == DBNull.Value)
            {
                MessageBox.Show("Không thể lấy mã HĐN từ dòng này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string maHDNCanXem = cell.ToString();

            // Mở form chi tiết
            var frm = new HoaDonNhap(maHDNCanXem);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(this);

            // Sau khi đóng, làm mới lại nếu cần
            PerformSearch(true);
        }

        private void btnXemChiTiet_Click(object sender, EventArgs e)
        {
            if (dgvKQTimKiemHDN.SelectedRows.Count > 0)
            {
                string maHDNCanXem = string.Empty;
                object cellValue = dgvKQTimKiemHDN.SelectedRows[0].Cells["MaHDN"].Value;

                if (cellValue != null && cellValue != DBNull.Value)
                {
                    maHDNCanXem = cellValue.ToString();
                }
                else
                {
                    MessageBox.Show("Không thể lấy được mã hóa đơn từ dòng đã chọn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra xem có vô tình lấy phải tên kiểu dữ liệu không
                if (maHDNCanXem.Equals("System.Data.DataRowView", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(maHDNCanXem))
                {
                    MessageBox.Show("Mã hóa đơn không hợp lệ. Vui lòng kiểm tra lại.", "Lỗi Mã Hóa Đơn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Truyền mã hóa đơn đã được lấy đúng cho form HoaDonNhap
                HoaDonNhap frmHDNChiTiet = new HoaDonNhap(maHDNCanXem.Trim());
                frmHDNChiTiet.StartPosition = FormStartPosition.CenterScreen;
                frmHDNChiTiet.ShowDialog(this); // Sử dụng ShowDialog để form tìm kiếm chờ

                // Sau khi form chi tiết đóng, bạn có thể muốn làm mới lại danh sách tìm kiếm
                PerformSearch(true); // Giả sử PerformSearch(true) sẽ tìm kiếm lại với các tiêu chí hiện tại
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn từ danh sách để xem chi tiết.", "Chưa chọn hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        

        private void dtpTimTuNgay_ValueChanged(object sender, EventArgs e)
        {
            if (this.Visible && dtpTimTuNgay.Value > dtpTimDenNgay.Value)
            {
                MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc. Vui lòng chọn lại.",
                            "Lỗi Khoảng Ngày", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dtpTimDenNgay_ValueChanged(object sender, EventArgs e)
        {
            if (this.Visible && dtpTimDenNgay.Value < dtpTimTuNgay.Value)
            {
                MessageBox.Show("Ngày kết thúc không thể nhỏ hơn ngày bắt đầu. Vui lòng chọn lại.",
                            "Lỗi Khoảng Ngày", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
