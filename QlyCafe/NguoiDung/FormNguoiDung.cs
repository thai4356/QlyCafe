using QlyCafe.NguoiDung;
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
    public partial class FormNguoiDung : Form
    {
        public FormNguoiDung()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            DataTable dt = Function.GetDataToTable("SELECT * FROM SanPham");

            foreach (DataRow row in dt.Rows)
            {
                string maSP = row["MaSP"].ToString();
                string tenSP = row["TenSP"].ToString();
                decimal giaBan = Convert.ToDecimal(row["GiaBan"]);

                Panel productCard = new Panel();
                productCard.Width = 150;
                productCard.Height = 200;
                productCard.Margin = new Padding(10);
                productCard.BorderStyle = BorderStyle.FixedSingle;

                PictureBox pic = new PictureBox();
                pic.Width = 130;
                pic.Height = 100;
                pic.SizeMode = PictureBoxSizeMode.StretchImage;

                Label lblTen = new Label();
                lblTen.Text = tenSP;
                lblTen.Width = 130;
                lblTen.Top = 110;

                Label lblGia = new Label();
                lblGia.Text = giaBan + " đ";
                lblGia.Width = 130;
                lblGia.Top = 130;

                Button btn = new Button();
                btn.Text = "Mua";
                btn.Width = 130;
                btn.Top = 160;

                // Khi bấm nút "Mua"
                btn.Click += (s, e) =>
                {
                    var existing = CartSession.GioHangTam.FirstOrDefault(i => i.MaSP == maSP);
                    if (existing != null)
                        existing.SoLuong += 1;
                    else
                        CartSession.GioHangTam.Add(new CartItem
                        {
                            MaSP = maSP,
                            TenSP = tenSP,
                            SoLuong = 1,
                            DonGia = giaBan
                        });

                    MessageBox.Show("Đã thêm vào giỏ hàng!");
                };

                productCard.Controls.Add(pic);
                productCard.Controls.Add(lblTen);
                productCard.Controls.Add(lblGia);
                productCard.Controls.Add(btn);

                pic.Top = 5;
                lblTen.Left = lblGia.Left = btn.Left = pic.Left = 10;

                flowLayoutPanel1.Controls.Add(productCard);
            }
        }


        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnGioHang_Click(object sender, EventArgs e)
        {
            FormGioHang gh = new FormGioHang();
            gh.ShowDialog();
        }

    }
}
