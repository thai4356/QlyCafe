using QRCoder;
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
    public partial class FormQRBan : Form
    {
        private int banId;

        public FormQRBan(int id)
        {
            InitializeComponent();
            banId = id;
        }

        private void FormQRBan_Load(object sender, EventArgs e)
        {
            string maBan = "BAN" + banId.ToString("D2");
            lblBan.Text = "QR cho Bàn " + banId;

            // Sinh QR
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(maBan, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            QR.Image = qrCode.GetGraphic(5);
        }
    }

}
