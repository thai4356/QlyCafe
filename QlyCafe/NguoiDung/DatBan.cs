using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace QlyCafe
{
    public partial class DatBan : Form
    {
        private List<(int id, string status)> banDangChonList = new List<(int, string)>();

        public DatBan()
        {
            InitializeComponent();
            LoadDanhSachBan();
            this.btnTraBan.Click += new System.EventHandler(this.btnTraBan_Click);

        }

        private void DatBan_Load(object sender, EventArgs e)
        {
            LoadDanhSachBan();
        }

        private void LoadDanhSachBan()
        {
            string sql = "SELECT * FROM Ban";
            DataTable dtBan = Function.GetDataToTable(sql);

            ban.Controls.Clear(); // FlowLayoutPanel tên là ban
            banDangChonList.Clear();
            lblBanDangChon.Text = "Chưa chọn bàn nào.";

            foreach (DataRow row in dtBan.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                string status = row["status"].ToString();

                Button btnBan = new Button();
                btnBan.Text = $"Bàn {id}\n({status})";
                btnBan.Tag = (id, status); // Lưu tuple trong Tag
                btnBan.Width = 100;
                btnBan.Height = 70;
                btnBan.BackColor = status == "Trống" ? Color.LightGreen : Color.Orange;

                btnBan.Click += BtnBan_Click;

                ban.Controls.Add(btnBan);
            }
        }

        private void BtnBan_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            (int id, string status) data = ((int, string))btn.Tag;

            var existing = banDangChonList.Find(b => b.id == data.id);
            if (existing != default)
            {
                banDangChonList.Remove(existing);

                // ✅ Trả lại màu đúng theo trạng thái gốc
                if (data.status == "Trống")
                    btn.BackColor = Color.LightGreen;
                else if (data.status == "Đang phục vụ")
                    btn.BackColor = Color.Orange;
                else
                    btn.BackColor = SystemColors.Control; // fallback màu mặc định
            }
            else
            {
                banDangChonList.Add(data);
                btn.BackColor = Color.CornflowerBlue;
            }

            lblBanDangChon.Text = banDangChonList.Count == 0
                ? "Chưa chọn bàn nào."
                : "Đã chọn: " + string.Join(", ", banDangChonList.ConvertAll(b => $"Bàn {b.id}"));
        }


        private void btnBan_Click(object sender, EventArgs e)
        {
            

            if (banDangChonList.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một bàn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var banKhongHopLe = banDangChonList.Where(b => b.status != "Trống").ToList();
            if (banKhongHopLe.Count > 0)
            {
                string danhSach = string.Join(", ", banKhongHopLe.Select(b => $"Bàn {b.id}"));
                MessageBox.Show($"Không thể đặt các bàn đã có người: {danhSach}", "Không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var ban in banDangChonList)
            {
                //TableDetail f = new TableDetail(ban.id, ban.status);
                FormNguoiDung f = new FormNguoiDung(ban.id);
                f.Show();

                // Sau khi đặt: cập nhật trạng thái luôn
                string sql = $"UPDATE Ban SET status = N'Đang phục vụ' WHERE id = {ban.id}";
                Function.ExecuteNonQuery(sql);
            }

            LoadDanhSachBan();
        }

        private void btnTraBan_Click(object sender, EventArgs e)
        {
            if (banDangChonList.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một bàn để trả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc muốn trả lại các bàn đã chọn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                foreach (var ban in banDangChonList)
                {
                    string sql = $"UPDATE Ban SET status = N'Trống' WHERE id = {ban.id}";
                    Function.ExecuteNonQuery(sql);
                }

                MessageBox.Show("Đã trả bàn thành công.", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachBan();
            }
        }

    }
}
