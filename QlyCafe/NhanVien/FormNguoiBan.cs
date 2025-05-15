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

            bool coSanPhamThieu = false;

            foreach (var item in CartSession.GioHangTam)
            {
                // Lấy số lượng tồn kho hiện tại
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

                // Cập nhật số lượng tồn kho
                int soMoi = soLuongTon - item.SoLuong;
                string sqlUpdate = $"UPDATE SanPham SET SoLuong = {soMoi} WHERE MaSP = '{item.MaSP}'";
                Function.ExecuteNonQuery(sqlUpdate);
            }

            if (!coSanPhamThieu)
            {
                MessageBox.Show("Đã cập nhật số lượng sản phẩm thành công!");
            }

            // Xóa đơn chờ
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
