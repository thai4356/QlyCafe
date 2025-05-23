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
            // ĐĂNG KÝ CODE PAGES ENCODING PROVIDER (THÊM DÒNG NÀY)
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // --- Thiết lập Encoding cho Console để hiển thị tiếng Việt ---
            Console.OutputEncoding = Encoding.UTF8;
            // ----------------------------------------------------------

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //DatBan form1 = new DatBan();
            //form1.Show();

            //Login form2 = new Login();
            //Application.Run(form2);
            test();
        }

        static void test() {
            BaoCaoDoanhThu form3 = new BaoCaoDoanhThu();
            Application.Run(form3);
        }
    }
}
