USE [master]
GO
/****** Object:  Database [qlyCafe]    Script Date: 5/15/2025 4:02:58 PM ******/
CREATE DATABASE [qlyCafe]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'qlyCafe', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\qlyCafe.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'qlyCafe_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\qlyCafe_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [qlyCafe] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [qlyCafe].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [qlyCafe] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [qlyCafe] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [qlyCafe] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [qlyCafe] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [qlyCafe] SET ARITHABORT OFF 
GO
ALTER DATABASE [qlyCafe] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [qlyCafe] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [qlyCafe] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [qlyCafe] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [qlyCafe] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [qlyCafe] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [qlyCafe] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [qlyCafe] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [qlyCafe] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [qlyCafe] SET  ENABLE_BROKER 
GO
ALTER DATABASE [qlyCafe] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [qlyCafe] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [qlyCafe] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [qlyCafe] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [qlyCafe] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [qlyCafe] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [qlyCafe] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [qlyCafe] SET RECOVERY FULL 
GO
ALTER DATABASE [qlyCafe] SET  MULTI_USER 
GO
ALTER DATABASE [qlyCafe] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [qlyCafe] SET DB_CHAINING OFF 
GO
ALTER DATABASE [qlyCafe] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [qlyCafe] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [qlyCafe] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [qlyCafe] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'qlyCafe', N'ON'
GO
ALTER DATABASE [qlyCafe] SET QUERY_STORE = ON
GO
ALTER DATABASE [qlyCafe] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [qlyCafe]
GO
/****** Object:  Table [dbo].[ChiTietHDB]    Script Date: 5/15/2025 4:02:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietHDB](
	[MaHDB] [varchar](10) NOT NULL,
	[MaSP] [varchar](10) NOT NULL,
	[SoLuong] [int] NULL,
	[ThanhTien] [decimal](15, 2) NULL,
	[KhuyenMai] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaHDB] ASC,
	[MaSP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietHDN]    Script Date: 5/15/2025 4:02:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietHDN](
	[MaHDN] [varchar](10) NOT NULL,
	[MaSP] [varchar](10) NOT NULL,
	[SoLuong] [int] NULL,
	[DonGia] [decimal](10, 2) NULL,
	[ThanhTien] [decimal](15, 2) NULL,
	[KhuyenMai] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaHDN] ASC,
	[MaSP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CongDung]    Script Date: 5/15/2025 4:02:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CongDung](
	[MaCongDung] [varchar](10) NOT NULL,
	[TenCongDung] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaCongDung] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HoaDonBan]    Script Date: 5/15/2025 4:02:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HoaDonBan](
	[MaHDB] [varchar](10) NOT NULL,
	[NgayBan] [date] NULL,
	[MaNV] [varchar](10) NULL,
	[MaKH] [varchar](10) NULL,
	[TongTien] [decimal](15, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaHDB] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HoaDonNhap]    Script Date: 5/15/2025 4:02:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HoaDonNhap](
	[MaHDN] [varchar](10) NOT NULL,
	[NgayNhap] [date] NULL,
	[MaNV] [varchar](10) NULL,
	[MaNCC] [varchar](10) NULL,
	[TongTien] [decimal](15, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaHDN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KhachHang]    Script Date: 5/15/2025 4:02:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhachHang](
	[MaKH] [varchar](10) NOT NULL,
	[DiaChi] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaKH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Loai]    Script Date: 5/15/2025 4:02:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Loai](
	[MaLoai] [varchar](10) NOT NULL,
	[TenLoai] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaLoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhaCungCap]    Script Date: 5/15/2025 4:02:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhaCungCap](
	[MaNCC] [varchar](10) NOT NULL,
	[TenNCC] [nvarchar](50) NULL,
	[DiaChi] [nvarchar](100) NULL,
	[SDT] [varchar](15) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaNCC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhanVien]    Script Date: 5/15/2025 4:02:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhanVien](
	[MaNV] [varchar](10) NOT NULL,
	[TenNV] [nvarchar](50) NULL,
	[DiaChi] [nvarchar](100) NULL,
	[GioiTinh] [nvarchar](10) NULL,
	[NgaySinh] [date] NULL,
	[MaQue] [varchar](10) NULL,
	[SDT] [varchar](15) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Que]    Script Date: 5/15/2025 4:02:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Que](
	[MaQue] [varchar](10) NOT NULL,
	[TenQue] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaQue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SanPham]    Script Date: 5/15/2025 4:02:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SanPham](
	[MaSP] [varchar](10) NOT NULL,
	[TenSP] [nvarchar](100) NULL,
	[MaLoai] [varchar](10) NULL,
	[GiaNhap] [decimal](10, 2) NULL,
	[GiaBan] [decimal](10, 2) NULL,
	[SoLuong] [int] NULL,
	[MaCongDung] [varchar](10) NULL,
	[HinhAnh] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaSP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 5/15/2025 4:02:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[TenDangNhap] [nvarchar](50) NOT NULL,
	[MatKhau] [nvarchar](50) NULL,
	[VaiTro] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[TenDangNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[CongDung] ([MaCongDung], [TenCongDung]) VALUES (N'CD01', N'Giải khát')
INSERT [dbo].[CongDung] ([MaCongDung], [TenCongDung]) VALUES (N'CD02', N'Giữ tỉnh táo')
GO
INSERT [dbo].[KhachHang] ([MaKH], [DiaChi]) VALUES (N'KH01', N'456 Bùi Thị Xuân, Q1')
INSERT [dbo].[KhachHang] ([MaKH], [DiaChi]) VALUES (N'KH02', N'789 Lê Lợi, Q3')
GO
INSERT [dbo].[Loai] ([MaLoai], [TenLoai]) VALUES (N'L01', N'Cà phê')
INSERT [dbo].[Loai] ([MaLoai], [TenLoai]) VALUES (N'L02', N'Nước ép')
GO
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [DiaChi], [SDT]) VALUES (N'NCC01', N'Công ty Cà Phê ABC', N'12 Nguyễn Huệ', N'023456789')
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [DiaChi], [SDT]) VALUES (N'NCC02', N'Trung Nguyên', N'89 Pasteur', N'0789123456')
GO
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [DiaChi], [GioiTinh], [NgaySinh], [MaQue], [SDT]) VALUES (N'NV01', N'Nguyễn Văn A', N'123 ABC', N'Nam', CAST(N'1990-01-01' AS Date), N'Q01', N'0123456789')
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [DiaChi], [GioiTinh], [NgaySinh], [MaQue], [SDT]) VALUES (N'NV02', N'Trần Thị B', N'456 XYZ', N'Nữ', CAST(N'1992-05-10' AS Date), N'Q02', N'0987654321')
GO
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q01', N'An Giang')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q02', N'Bà Rịa - Vũng Tàu')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q03', N'Bạc Liêu')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q04', N'Bắc Giang')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q05', N'Bắc Kạn')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q06', N'Bắc Ninh')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q07', N'Bến Tre')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q08', N'Bình Dương')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q09', N'Bình Định')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q10', N'Bình Phước')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q11', N'Bình Thuận')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q12', N'Cà Mau')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q13', N'Cao Bằng')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q14', N'Cần Thơ')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q15', N'Đà Nẵng')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q16', N'Đắk Lắk')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q17', N'Đắk Nông')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q18', N'Điện Biên')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q19', N'Đồng Nai')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q20', N'Đồng Tháp')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q21', N'Gia Lai')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q22', N'Hà Giang')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q23', N'Hà Nam')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q24', N'Hà Nội')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q25', N'Hà Tĩnh')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q26', N'Hải Dương')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q27', N'Hải Phòng')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q28', N'Hậu Giang')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q29', N'Hòa Bình')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q30', N'Hưng Yên')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q31', N'Khánh Hòa')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q32', N'Kiên Giang')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q33', N'Kon Tum')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q34', N'Lai Châu')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q35', N'Lạng Sơn')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q36', N'Lào Cai')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q37', N'Lâm Đồng')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q38', N'Long An')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q39', N'Nam Định')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q40', N'Nghệ An')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q41', N'Ninh Bình')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q42', N'Ninh Thuận')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q43', N'Phú Thọ')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q44', N'Phú Yên')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q45', N'Quảng Bình')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q46', N'Quảng Nam')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q47', N'Quảng Ngãi')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q48', N'Quảng Ninh')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q49', N'Quảng Trị')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q50', N'Sóc Trăng')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q51', N'Sơn La')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q52', N'Tây Ninh')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q53', N'Thái Bình')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q54', N'Thái Nguyên')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q55', N'Thanh Hóa')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q56', N'Thừa Thiên Huế')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q57', N'Tiền Giang')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q58', N'TP. Hồ Chí Minh')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q59', N'Trà Vinh')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q60', N'Tuyên Quang')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q61', N'Vĩnh Long')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q62', N'Vĩnh Phúc')
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'Q63', N'Yên Bái')
GO
INSERT [dbo].[SanPham] ([MaSP], [TenSP], [MaLoai], [GiaNhap], [GiaBan], [SoLuong], [MaCongDung], [HinhAnh]) VALUES (N'SP01', N'Cà phê sữa', N'L01', CAST(15000.00 AS Decimal(10, 2)), CAST(25000.00 AS Decimal(10, 2)), 99, N'CD02', N'cafe_sua.jpg')
INSERT [dbo].[SanPham] ([MaSP], [TenSP], [MaLoai], [GiaNhap], [GiaBan], [SoLuong], [MaCongDung], [HinhAnh]) VALUES (N'SP02', N'Nước ép cam', N'L02', CAST(10000.00 AS Decimal(10, 2)), CAST(20000.00 AS Decimal(10, 2)), 50, N'CD01', N'cam.jpg')
GO
INSERT [dbo].[TaiKhoan] ([TenDangNhap], [MatKhau], [VaiTro]) VALUES (N'admin1', N'admin', N'QuanLy')
INSERT [dbo].[TaiKhoan] ([TenDangNhap], [MatKhau], [VaiTro]) VALUES (N'seller1', N'abc', N'NguoiBan')
INSERT [dbo].[TaiKhoan] ([TenDangNhap], [MatKhau], [VaiTro]) VALUES (N'user1', N'123', N'NguoiDung')
GO
ALTER TABLE [dbo].[ChiTietHDB]  WITH CHECK ADD FOREIGN KEY([MaHDB])
REFERENCES [dbo].[HoaDonBan] ([MaHDB])
GO
ALTER TABLE [dbo].[ChiTietHDB]  WITH CHECK ADD FOREIGN KEY([MaSP])
REFERENCES [dbo].[SanPham] ([MaSP])
GO
ALTER TABLE [dbo].[ChiTietHDN]  WITH CHECK ADD FOREIGN KEY([MaHDN])
REFERENCES [dbo].[HoaDonNhap] ([MaHDN])
GO
ALTER TABLE [dbo].[ChiTietHDN]  WITH CHECK ADD FOREIGN KEY([MaSP])
REFERENCES [dbo].[SanPham] ([MaSP])
GO
ALTER TABLE [dbo].[HoaDonBan]  WITH CHECK ADD FOREIGN KEY([MaKH])
REFERENCES [dbo].[KhachHang] ([MaKH])
GO
ALTER TABLE [dbo].[HoaDonBan]  WITH CHECK ADD FOREIGN KEY([MaNV])
REFERENCES [dbo].[NhanVien] ([MaNV])
GO
ALTER TABLE [dbo].[HoaDonNhap]  WITH CHECK ADD FOREIGN KEY([MaNCC])
REFERENCES [dbo].[NhaCungCap] ([MaNCC])
GO
ALTER TABLE [dbo].[HoaDonNhap]  WITH CHECK ADD FOREIGN KEY([MaNV])
REFERENCES [dbo].[NhanVien] ([MaNV])
GO
ALTER TABLE [dbo].[NhanVien]  WITH CHECK ADD FOREIGN KEY([MaQue])
REFERENCES [dbo].[Que] ([MaQue])
GO
ALTER TABLE [dbo].[SanPham]  WITH CHECK ADD FOREIGN KEY([MaCongDung])
REFERENCES [dbo].[CongDung] ([MaCongDung])
GO
ALTER TABLE [dbo].[SanPham]  WITH CHECK ADD FOREIGN KEY([MaLoai])
REFERENCES [dbo].[Loai] ([MaLoai])
GO
USE [master]
GO
ALTER DATABASE [qlyCafe] SET  READ_WRITE 
GO
