using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QlyCafe
{
    public partial class FormNguoiBan : Form
    {
        public FormNguoiBan()
        {
            InitializeComponent();
            
        }

        private void FormNguoiBan_Load(object sender, EventArgs e)
        {

        }

        private void LoadDonHangCho()
        {
            if (CartSession.DonHangDangChoDuyet)
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = CartSession.GioHangTam;
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }

        private void btnDuyetDon_Click(object sender, EventArgs e)
        {
            if (!CartSession.DonHangDangChoDuyet || CartSession.GioHangTam.Count == 0)
            {
                MessageBox.Show("Không có đơn hàng nào chờ duyệt!", "Thông báo");
                return;
            }

            string cccd = txtCCCD.Text.Trim();
            if (string.IsNullOrWhiteSpace(cccd))
            {
                MessageBox.Show("Vui lòng nhập số CCCD của khách hàng.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Tìm hoặc tạo mới MaKH
            string maKH = GetOrCreateMaKH(cccd);

            bool coSanPhamThieu = false;
            decimal tongTien = 0;

            foreach (var item in CartSession.GioHangTam)
            {
                string sqlCheck = $"SELECT SoLuong FROM SanPham WHERE MaSP = '{item.MaSP}'";
                DataTable dt = Function.GetDataToTable(sqlCheck);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show($"Sản phẩm {item.TenSP} không tồn tại!");
                    continue;
                }

                int soLuongTon = Convert.ToInt32(dt.Rows[0]["SoLuong"]);

                if (soLuongTon < item.SoLuong)
                {
                    coSanPhamThieu = true;
                    MessageBox.Show($"Không đủ số lượng cho sản phẩm {item.TenSP} (tồn: {soLuongTon}, cần: {item.SoLuong})");
                    continue;
                }

                int soMoi = soLuongTon - item.SoLuong;
                string sqlUpdate = $"UPDATE SanPham SET SoLuong = {soMoi} WHERE MaSP = '{item.MaSP}'";
                Function.ExecuteNonQuery(sqlUpdate);

                tongTien += item.ThanhTien;
            }

            if (coSanPhamThieu)
            {
                MessageBox.Show("Có sản phẩm thiếu số lượng, hóa đơn không được tạo.");
                return;
            }

            string maHDB = "HDB_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"); // HDB_18052025_112530
            string ngayBan = DateTime.Now.ToString("yyyy-MM-dd");
            string maNV = UserSession.MaNguoiDung;

            string sqlInsertHDB = $@"
    INSERT INTO HoaDonBan (MaHDB, NgayBan, MaNV, MaKH, TongTien)
    VALUES ('{maHDB}', '{ngayBan}', '{maNV}', '{maKH}', {tongTien})";
            Function.ExecuteNonQuery(sqlInsertHDB);

            foreach (var item in CartSession.GioHangTam)
            {
                string sqlCT = $"INSERT INTO ChiTietHDB VALUES ('{maHDB}', '{item.MaSP}', {item.SoLuong}, {item.ThanhTien}, N'')";
                Function.ExecuteNonQuery(sqlCT);
            }

            MessageBox.Show("✅ Hóa đơn đã được tạo thành công!", "Thành công");

            CartSession.GioHangTam.Clear();
            CartSession.DonHangDangChoDuyet = false;
            LoadDonHangCho();
        }



        private void btnTuChoiDon_Click(object sender, EventArgs e)
        {
            CartSession.GioHangTam.Clear();
            CartSession.DonHangDangChoDuyet = false;
            LoadDonHangCho();
            MessageBox.Show("Đã từ chối đơn hàng!");
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadDonHangCho();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Function.Logout(this);
        }

        private string GetOrCreateMaKH(string cccd)
        {
            string sqlCheck = $"SELECT MaKH FROM KhachHang WHERE CCCD = '{cccd}'";
            DataTable dt = Function.GetDataToTable(sqlCheck);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["MaKH"].ToString();
            }

            // Tạo mã KH mới theo KHxxx
            string sqlMax = "SELECT TOP 1 MaKH FROM KhachHang ORDER BY MaKH DESC";
            DataTable dtMax = Function.GetDataToTable(sqlMax);

            string newMaKH = "KH001";
            if (dtMax.Rows.Count > 0)
            {
                string lastMa = dtMax.Rows[0]["MaKH"].ToString(); // KH009
                int num = int.Parse(lastMa.Substring(2)) + 1;
                newMaKH = "KH" + num.ToString("D3");
            }

            // Chèn khách hàng mới
            string sqlInsert = $"INSERT INTO KhachHang (MaKH, CCCD) VALUES ('{newMaKH}', '{cccd}')";
            Function.ExecuteNonQuery(sqlInsert);

            return newMaKH;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
