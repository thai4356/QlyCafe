using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QlyCafe.NguoiDung
{
    public partial class FormGioHang : Form
    {
        public FormGioHang()
        {
            InitializeComponent();
        }

       

        private void FormGioHang_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = CartSession.GioHangTam.Select(x => new
            {
                x.MaSP,
                x.TenSP,
                x.SoLuong,
                x.DonGia,
                x.ThanhTien
            }).ToList();
        }

      

        private void btnGioHang_Click(object sender, EventArgs e)
        {
            if (CartSession.GioHangTam.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống!");
                return;
            }

            if (CartSession.DonHangDangChoDuyet)
            {
                MessageBox.Show("Đã có đơn đang chờ duyệt!");
                return;
            }

            CartSession.DonHangDangChoDuyet = true;
            MessageBox.Show("Đơn hàng đã được gửi đi chờ phê duyệt.");
            this.Close();
        }
    }
}
