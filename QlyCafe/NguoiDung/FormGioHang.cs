using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace QlyCafe.NguoiDung
{
    public partial class FormGioHang : Form
    {
        private BindingList<CartItem> bindingList;

        public FormGioHang()
        {
            InitializeComponent();

            // Gắn các sự kiện
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            dataGridView1.CellValidating += dataGridView1_CellValidating;
        }

        private void FormGioHang_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            // Load dữ liệu từ CartSession
            bindingList = new BindingList<CartItem>(CartSession.GioHangTam);
            dataGridView1.DataSource = bindingList;

            // Thiết lập các cột không chỉnh sửa
            dataGridView1.Columns["MaSP"].ReadOnly = true;
            dataGridView1.Columns["TenSP"].ReadOnly = true;
            dataGridView1.Columns["DonGia"].ReadOnly = true;
            dataGridView1.Columns["ThanhTien"].ReadOnly = true;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "SoLuong")
            {
                var row = dataGridView1.Rows[e.RowIndex];
                var sp = row.DataBoundItem as CartItem;
                if (sp != null)
                {
                    row.Cells["ThanhTien"].Value = sp.ThanhTien;
                    dataGridView1.Refresh();
                }
            }
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "SoLuong")
            {
                if (!int.TryParse(e.FormattedValue.ToString(), out int value) || value <= 0)
                {
                    MessageBox.Show("Số lượng phải là số nguyên dương!");
                    e.Cancel = true;
                }
            }
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
