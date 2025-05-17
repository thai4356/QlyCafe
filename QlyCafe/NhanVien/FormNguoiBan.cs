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
            //string maNV = UserSession.MaNguoiDung;
            //MessageBox.Show("Mã NV đang dùng: " + maNV);

            if (!CartSession.DonHangDangChoDuyet || CartSession.GioHangTam.Count == 0)
            {
                MessageBox.Show("Không có đơn hàng nào chờ duyệt!", "Thông báo");
                return;
            }

            bool coSanPhamThieu = false;
            decimal tongTien = 0;

            // Kiểm tra & cập nhật số lượng từng sản phẩm
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

            string maHDB = "HDB" + DateTime.Now.Ticks.ToString().Substring(8, 6); // HDB+6 số cuối
            string ngayBan = DateTime.Now.ToString("yyyy-MM-dd");
            string maNV = UserSession.MaNguoiDung; // Nhân viên đang duyệt đơn
            string maKH = "KH01"; // Giả sử khách hàng cố định, có thể sửa lại

            string sqlInsertHDB = $@"
            INSERT INTO HoaDonBan (MaHDB, NgayBan, MaNV, MaKH, TongTien)
            VALUES ('{maHDB}', '{ngayBan}', '{maNV}', '{maKH}', {tongTien})";
            Function.ExecuteNonQuery(sqlInsertHDB);

            foreach (var item in CartSession.GioHangTam)
            {
                string sqlCT = $"INSERT INTO ChiTietHDB VALUES ('{maHDB}', '{item.MaSP}', {item.SoLuong}, {item.ThanhTien}, N'')";
                Function.ExecuteNonQuery(sqlCT);
            }


            MessageBox.Show("Hóa đơn đã được tạo và tồn kho đã cập nhật!");


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
    }
}
