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
    public partial class TableDetail : Form
    {
        //public TableDetail()
        //{
        //    InitializeComponent();
        //}

        private int banId;
        private string trangThai;

        public TableDetail(int id, string status)
        {
            InitializeComponent();
            banId = id;
            trangThai = status;
        }

        private void FormThongTinBan_Load(object sender, EventArgs e)
        {
            lblBan.Text = $"Thông tin Bàn {banId}";
            lblTrangThai.Text = $"Trạng thái: {trangThai}";

            if (trangThai != "Trống")
            {
                btnDatBan.Enabled = false;
                btnDatBan.Text = "Bàn đã có người";
            }
        }

        private void btnDatBan_Click(object sender, EventArgs e)
        {
            FormNguoiDung f = new FormNguoiDung(banId); // Truyền mã bàn sang
            f.Show();
            this.Close(); // Đóng form hiện tại nếu cần
        }

        private void TableDetail_Load(object sender, EventArgs e)
        {

        }
    }
}
