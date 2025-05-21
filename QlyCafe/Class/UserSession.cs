using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlyCafe
{
    public static class UserSession
    {
        public static string TenDangNhap { get; set; }
        public static string VaiTro { get; set; }
        public static string MaNguoiDung { get; set; } // Có thể là MaKH hoặc MaNV tùy VaiTro
    }

}
