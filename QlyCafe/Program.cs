using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QlyCafe.Quanly;

namespace QlyCafe
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // --- Thiết lập Encoding cho Console để hiển thị tiếng Việt ---
            Console.OutputEncoding = Encoding.UTF8;
            // ----------------------------------------------------------

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //FormNguoiDung form1 = new FormNguoiDung();
            //form1.Show();

            //Login form2 = new Login();
            //Application.Run(form2); // This keeps the app alive
            test();
        }

        static void test() {
            //NhaCungCap ncc = new NhaCungCap();
            //Application.Run(ncc);

            //SanPham sp = new SanPham();
            //Application.Run(sp);

            //HoaDonNhap hdn = new HoaDonNhap();
            //Application.Run(hdn);

            TimHDNhap thdn = new TimHDNhap();
            Application.Run(thdn);
        }
    }
}
