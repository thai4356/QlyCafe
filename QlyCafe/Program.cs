using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormNguoiDung form1 = new FormNguoiDung();
            form1.Show();

            Login form2 = new Login();
            Application.Run(form2); // This keeps the app alive
        }
    }
}
