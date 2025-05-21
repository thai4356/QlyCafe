USE [master]
GO
/****** Object:  Database [qlyCafe]    Script Date: 5/21/2025 10:57:35 AM ******/
CREATE DATABASE [qlyCafe]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'qlyCafe', FILENAME = N'D:\dotnet development\QlyCafe\Database\qlyCafe.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'qlyCafe_log', FILENAME = N'D:\dotnet development\QlyCafe\Database\qlyCafe_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
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
ALTER DATABASE [qlyCafe] SET QUERY_STORE = ON
GO
ALTER DATABASE [qlyCafe] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [qlyCafe]
GO
/****** Object:  Table [dbo].[ChiTietHDB]    Script Date: 5/21/2025 10:57:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietHDB](
	[MaHDB] [varchar](25) NOT NULL,
	[MaSP] [varchar](10) NOT NULL,
	[SoLuong] [int] NULL,
	[ThanhTien] [decimal](15, 2) NULL,
	[KhuyenMai] [nvarchar](100) NULL,
 CONSTRAINT [PK_ChiTietHDB_MaHDB_MaSP] PRIMARY KEY CLUSTERED 
(
	[MaHDB] ASC,
	[MaSP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietHDN]    Script Date: 5/21/2025 10:57:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietHDN](
	[MaHDN] [varchar](25) NOT NULL,
	[MaSP] [varchar](10) NOT NULL,
	[SoLuong] [int] NULL,
	[DonGia] [decimal](10, 2) NULL,
	[ThanhTien] [decimal](15, 2) NULL,
	[KhuyenMai] [nvarchar](100) NULL,
 CONSTRAINT [PK_ChiTietHDN_MaHDN_MaSP] PRIMARY KEY CLUSTERED 
(
	[MaHDN] ASC,
	[MaSP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CongDung]    Script Date: 5/21/2025 10:57:36 AM ******/
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
/****** Object:  Table [dbo].[HoaDonBan]    Script Date: 5/21/2025 10:57:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HoaDonBan](
	[MaHDB] [varchar](25) NOT NULL,
	[NgayBan] [date] NULL,
	[MaNV] [varchar](10) NULL,
	[MaKH] [varchar](10) NULL,
	[TongTien] [decimal](15, 2) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_HoaDonBan_MaHDB] PRIMARY KEY CLUSTERED 
(
	[MaHDB] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HoaDonNhap]    Script Date: 5/21/2025 10:57:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HoaDonNhap](
	[MaHDN] [varchar](25) NOT NULL,
	[NgayNhap] [date] NULL,
	[MaNV] [varchar](10) NULL,
	[MaNCC] [varchar](10) NULL,
	[TongTien] [decimal](15, 2) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_HoaDonNhap_MaHDN] PRIMARY KEY CLUSTERED 
(
	[MaHDN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KhachHang]    Script Date: 5/21/2025 10:57:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhachHang](
	[MaKH] [varchar](10) NOT NULL,
	[DiaChi] [nvarchar](100) NULL,
	[cccd] [int] NULL,
	[tenKH] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaKH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Loai]    Script Date: 5/21/2025 10:57:36 AM ******/
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
/****** Object:  Table [dbo].[NhaCungCap]    Script Date: 5/21/2025 10:57:36 AM ******/
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
/****** Object:  Table [dbo].[NhanVien]    Script Date: 5/21/2025 10:57:36 AM ******/
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
/****** Object:  Table [dbo].[Que]    Script Date: 5/21/2025 10:57:36 AM ******/
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
/****** Object:  Table [dbo].[SanPham]    Script Date: 5/21/2025 10:57:36 AM ******/
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
/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 5/21/2025 10:57:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[TenDangNhap] [nvarchar](50) NOT NULL,
	[MatKhau] [nvarchar](50) NULL,
	[VaiTro] [nvarchar](20) NULL,
	[MaLienKet] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[TenDangNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaSP], [SoLuong], [ThanhTien], [KhuyenMai]) VALUES (N'HDB_18052025_112530', N'SP001', 2, CAST(50000.00 AS Decimal(15, 2)), NULL)
GO
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaSP], [SoLuong], [ThanhTien], [KhuyenMai]) VALUES (N'HDB_18052025_112530', N'SP005', 1, CAST(35000.00 AS Decimal(15, 2)), N'Giảm 5,000đ trực tiếp')
GO
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaSP], [SoLuong], [ThanhTien], [KhuyenMai]) VALUES (N'HDB_18052025_113000', N'SP009', 4, CAST(60000.00 AS Decimal(15, 2)), N'Mua 3 tặng 1 (đã áp dụng)')
GO
INSERT [dbo].[ChiTietHDN] ([MaHDN], [MaSP], [SoLuong], [DonGia], [ThanhTien], [KhuyenMai]) VALUES (N'HDN_10042025_163000', N'SP005', 30, CAST(13500.00 AS Decimal(10, 2)), CAST(405000.00 AS Decimal(15, 2)), NULL)
GO
INSERT [dbo].[ChiTietHDN] ([MaHDN], [MaSP], [SoLuong], [DonGia], [ThanhTien], [KhuyenMai]) VALUES (N'HDN_18052025_143510', N'SP005', 50, CAST(14000.00 AS Decimal(10, 2)), CAST(700000.00 AS Decimal(15, 2)), NULL)
GO
INSERT [dbo].[ChiTietHDN] ([MaHDN], [MaSP], [SoLuong], [DonGia], [ThanhTien], [KhuyenMai]) VALUES (N'HDN_18052025_143510', N'SP009', 100, CAST(7500.00 AS Decimal(10, 2)), CAST(750000.00 AS Decimal(15, 2)), NULL)
GO
INSERT [dbo].[ChiTietHDN] ([MaHDN], [MaSP], [SoLuong], [DonGia], [ThanhTien], [KhuyenMai]) VALUES (N'HDN_19052025_091500', N'SP011', 15, CAST(148000.00 AS Decimal(10, 2)), CAST(2220000.00 AS Decimal(15, 2)), NULL)
GO
INSERT [dbo].[ChiTietHDN] ([MaHDN], [MaSP], [SoLuong], [DonGia], [ThanhTien], [KhuyenMai]) VALUES (N'HDN_19052025_091500', N'SP012', 10, CAST(77500.00 AS Decimal(10, 2)), CAST(775000.00 AS Decimal(15, 2)), N'Tặng kèm muỗng')
GO
INSERT [dbo].[ChiTietHDN] ([MaHDN], [MaSP], [SoLuong], [DonGia], [ThanhTien], [KhuyenMai]) VALUES (N'HDN_20052025_100515', N'SP001', 50, CAST(9800.00 AS Decimal(10, 2)), CAST(490000.00 AS Decimal(15, 2)), NULL)
GO
INSERT [dbo].[ChiTietHDN] ([MaHDN], [MaSP], [SoLuong], [DonGia], [ThanhTien], [KhuyenMai]) VALUES (N'HDN_20052025_100515', N'SP009', 200, CAST(7200.00 AS Decimal(10, 2)), CAST(1440000.00 AS Decimal(15, 2)), NULL)
GO
INSERT [dbo].[CongDung] ([MaCongDung], [TenCongDung]) VALUES (N'CD001', N'Giải khát tức thì')
GO
INSERT [dbo].[CongDung] ([MaCongDung], [TenCongDung]) VALUES (N'CD002', N'Tăng cường sự tỉnh táo')
GO
INSERT [dbo].[CongDung] ([MaCongDung], [TenCongDung]) VALUES (N'CD003', N'Bổ sung vitamin và khoáng chất')
GO
INSERT [dbo].[CongDung] ([MaCongDung], [TenCongDung]) VALUES (N'CD004', N'Món ăn nhẹ')
GO
INSERT [dbo].[CongDung] ([MaCongDung], [TenCongDung]) VALUES (N'CD005', N'Thưởng thức hương vị')
GO
INSERT [dbo].[HoaDonBan] ([MaHDB], [NgayBan], [MaNV], [MaKH], [TongTien], [IsDeleted]) VALUES (N'HDB_18052025_112530', CAST(N'2025-05-18' AS Date), N'NV001', N'KH001', CAST(85000.00 AS Decimal(15, 2)), NULL)
GO
INSERT [dbo].[HoaDonBan] ([MaHDB], [NgayBan], [MaNV], [MaKH], [TongTien], [IsDeleted]) VALUES (N'HDB_18052025_113000', CAST(N'2025-05-18' AS Date), N'NV004', N'KH003', CAST(60000.00 AS Decimal(15, 2)), NULL)
GO
INSERT [dbo].[HoaDonNhap] ([MaHDN], [NgayNhap], [MaNV], [MaNCC], [TongTien], [IsDeleted]) VALUES (N'HDN_10042025_163000', CAST(N'2025-04-10' AS Date), N'NV003', N'NCC004', CAST(405000.00 AS Decimal(15, 2)), 1)
GO
INSERT [dbo].[HoaDonNhap] ([MaHDN], [NgayNhap], [MaNV], [MaNCC], [TongTien], [IsDeleted]) VALUES (N'HDN_18052025_143510', CAST(N'2025-05-18' AS Date), N'NV002', N'NCC003', CAST(1450000.00 AS Decimal(15, 2)), 0)
GO
INSERT [dbo].[HoaDonNhap] ([MaHDN], [NgayNhap], [MaNV], [MaNCC], [TongTien], [IsDeleted]) VALUES (N'HDN_19052025_091500', CAST(N'2025-05-19' AS Date), N'NV001', N'NCC001', CAST(2995000.00 AS Decimal(15, 2)), 0)
GO
INSERT [dbo].[HoaDonNhap] ([MaHDN], [NgayNhap], [MaNV], [MaNCC], [TongTien], [IsDeleted]) VALUES (N'HDN_20052025_100515', CAST(N'2025-05-20' AS Date), N'NV001', N'NCC002', CAST(1930000.00 AS Decimal(15, 2)), 0)
GO
INSERT [dbo].[KhachHang] ([MaKH], [DiaChi], [cccd], [tenKH]) VALUES (N'KH001', N'789 Đường Cầu Giấy, Hà Nội', NULL, NULL)
GO
INSERT [dbo].[KhachHang] ([MaKH], [DiaChi], [cccd], [tenKH]) VALUES (N'KH002', N'101 Võ Văn Tần, Quận 3, TP. Hồ Chí Minh', NULL, NULL)
GO
INSERT [dbo].[KhachHang] ([MaKH], [DiaChi], [cccd], [tenKH]) VALUES (N'KH003', N'Khách vãng lai', NULL, NULL)
GO
INSERT [dbo].[KhachHang] ([MaKH], [DiaChi], [cccd], [tenKH]) VALUES (N'KH004', N'33 Ngô Thì Nhậm, Hai Bà Trưng, Hà Nội', NULL, NULL)
GO
INSERT [dbo].[KhachHang] ([MaKH], [DiaChi], [cccd], [tenKH]) VALUES (N'KH005', N'Số 10, KDC An Bình, Quận Ninh Kiều, Cần Thơ', NULL, NULL)
GO
INSERT [dbo].[Loai] ([MaLoai], [TenLoai]) VALUES (N'LOAI001', N'Cà Phê Truyền Thống')
GO
INSERT [dbo].[Loai] ([MaLoai], [TenLoai]) VALUES (N'LOAI002', N'Cà Phê Espresso')
GO
INSERT [dbo].[Loai] ([MaLoai], [TenLoai]) VALUES (N'LOAI003', N'Trà Trái Cây')
GO
INSERT [dbo].[Loai] ([MaLoai], [TenLoai]) VALUES (N'LOAI004', N'Nước Ép & Sinh Tố')
GO
INSERT [dbo].[Loai] ([MaLoai], [TenLoai]) VALUES (N'LOAI005', N'Đá Xay')
GO
INSERT [dbo].[Loai] ([MaLoai], [TenLoai]) VALUES (N'LOAI006', N'Bánh Ngọt & Ăn Vặt')
GO
INSERT [dbo].[Loai] ([MaLoai], [TenLoai]) VALUES (N'LOAI007', N'Nguyên Liệu Pha Chế')
GO
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [DiaChi], [SDT]) VALUES (N'NCC001', N'Công ty Cà Phê Ban Mê Xanh', N'150 Y Moan, TP. Buôn Ma Thuột, Đắk Lắk', N'0905123456')
GO
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [DiaChi], [SDT]) VALUES (N'NCC002', N'Nhà Phân Phối Sữa Tươi Dalatmilk Miền Nam', N'22 Võ Văn Tần, Quận 3, TP. Hồ Chí Minh', N'0988776655')
GO
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [DiaChi], [SDT]) VALUES (N'NCC003', N'Trang Trại Sữa Tươi Mộc Châu Farm', N'Thị trấn Mộc Châu, Sơn La', N'0987111222')
GO
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [DiaChi], [SDT]) VALUES (N'NCC004', N'Công ty Cổ phần Đường Lam Sơn', N'Thị trấn Lam Sơn, Thọ Xuân, Thanh Hóa', N'02373834567')
GO
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [DiaChi], [SDT]) VALUES (N'NCC005', N'Nhà Phân Phối Syrup Torani Miền Bắc', N'Số 10 Ngõ Huyện, Hoàn Kiếm, Hà Nội', N'0918765432')
GO
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [DiaChi], [SDT]) VALUES (N'NCC006', N'VinaSyrup Toàn Quốc', N'Lô B2, KCN Tân Bình, TP. Hồ Chí Minh', N'0933112233')
GO
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [DiaChi], [SDT]) VALUES (N'NCC007', N'Bột Pha Chế HorecaVN', N'789 Giải Phóng, Hoàng Mai, Hà Nội', N'0977889900')
GO
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [DiaChi], [SDT]) VALUES (N'NCC008', N'Dmm', N'dmm', N'0987111222')
GO
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [DiaChi], [SDT]) VALUES (N'NCC009', N'Công ty Bánh Kẹo Hải Hà (Cung cấp sỉ)', N'25 Trương Định, Hai Bà Trưng, Hà Nội', N'02438632041')
GO
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [DiaChi], [SDT]) VALUES (N'NCC010', N'Nhà Nhập Khẩu Trà Dilmah Chính Hãng', N'Tầng 5, Tòa nhà ABC, Quận 1, TP. Hồ Chí Minh', N'0932109876')
GO
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [DiaChi], [SDT]) VALUES (N'NCC011', N'Công ty Nước Suối Aquafina (PepsiCo)', N'Lô 2-4-6 KCN Sóng Thần 3, Bình Dương', N'1800545478')
GO
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [DiaChi], [SDT]) VALUES (N'NCC012', N'Xưởng Đá Viên Sạch Hà Thành', N'Khu tập thể Vĩnh Phúc, Ba Đình, Hà Nội', N'0977654321')
GO
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [DiaChi], [SDT]) VALUES (N'NCC013', N'Công ty Thực Phẩm Orion Vina (Chocopie, Custas)', N'KCN Mỹ Phước 2, Bến Cát, Bình Dương', N'02743567890')
GO
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [DiaChi], [SDT]) VALUES (N'NCC014', N'The Coffee Roasters - Xưởng Rang Gia Công', N'45B Lý Tự Trọng, Quận Hải Châu, Đà Nẵng', N'0908123789')
GO
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [DiaChi], [GioiTinh], [NgaySinh], [MaQue], [SDT]) VALUES (N'NV001', N'Nguyễn Văn An', N'123 Đường Láng, Đống Đa', N'Nam', CAST(N'1995-08-15' AS Date), N'QUE001', N'0901112221')
GO
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [DiaChi], [GioiTinh], [NgaySinh], [MaQue], [SDT]) VALUES (N'NV002', N'Trần Thị Bích', N'456 Lê Lợi, Quận 1', N'Nữ', CAST(N'1998-05-20' AS Date), N'QUE002', N'0902223332')
GO
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [DiaChi], [GioiTinh], [NgaySinh], [MaQue], [SDT]) VALUES (N'NV003', N'Lê Minh Hải', N'789 Hoàng Diệu, Quận Hải Châu', N'Nam', CAST(N'1992-12-01' AS Date), N'QUE003', N'0903334443')
GO
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [DiaChi], [GioiTinh], [NgaySinh], [MaQue], [SDT]) VALUES (N'NV004', N'Phạm Thị Dung', N'101 Trần Phú, Ngô Quyền', N'Nữ', CAST(N'2000-02-28' AS Date), N'QUE004', N'0904445554')
GO
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [DiaChi], [GioiTinh], [NgaySinh], [MaQue], [SDT]) VALUES (N'NV005', N'Hoàng Văn Nam', N'202 Nguyễn Văn Cừ, Ninh Kiều', N'Nam', CAST(N'1997-07-07' AS Date), N'QUE005', N'0905556665')
GO
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'QUE001', N'Hà Nội')
GO
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'QUE002', N'TP. Hồ Chí Minh')
GO
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'QUE003', N'Đà Nẵng')
GO
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'QUE004', N'Hải Phòng')
GO
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'QUE005', N'Cần Thơ')
GO
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'QUE006', N'An Giang')
GO
INSERT [dbo].[Que] ([MaQue], [TenQue]) VALUES (N'QUE007', N'Bình Dương')
GO
INSERT [dbo].[SanPham] ([MaSP], [TenSP], [MaLoai], [GiaNhap], [GiaBan], [SoLuong], [MaCongDung], [HinhAnh]) VALUES (N'SP001', N'Cà Phê Đen Đá Phin', N'LOAI001', CAST(20000.00 AS Decimal(10, 2)), CAST(25003.00 AS Decimal(10, 2)), 120, N'CD002', N'den_da_phin.jpg')
GO
INSERT [dbo].[SanPham] ([MaSP], [TenSP], [MaLoai], [GiaNhap], [GiaBan], [SoLuong], [MaCongDung], [HinhAnh]) VALUES (N'SP002', N'Cà Phê Sữa Đá Phin', N'LOAI001', CAST(12000.00 AS Decimal(10, 2)), CAST(30000.00 AS Decimal(10, 2)), 150, N'CD002', N'sua_da_phin.jpg')
GO
INSERT [dbo].[SanPham] ([MaSP], [TenSP], [MaLoai], [GiaNhap], [GiaBan], [SoLuong], [MaCongDung], [HinhAnh]) VALUES (N'SP003', N'Espresso Nóng', N'LOAI002', CAST(18000.00 AS Decimal(10, 2)), CAST(35000.00 AS Decimal(10, 2)), 80, N'CD002', N'espresso.jpg')
GO
INSERT [dbo].[SanPham] ([MaSP], [TenSP], [MaLoai], [GiaNhap], [GiaBan], [SoLuong], [MaCongDung], [HinhAnh]) VALUES (N'SP004', N'Cappuccino', N'LOAI002', CAST(22000.00 AS Decimal(10, 2)), CAST(45000.00 AS Decimal(10, 2)), 70, N'CD005', N'cappuccino.jpg')
GO
INSERT [dbo].[SanPham] ([MaSP], [TenSP], [MaLoai], [GiaNhap], [GiaBan], [SoLuong], [MaCongDung], [HinhAnh]) VALUES (N'SP005', N'Trà Đào Cam Sả', N'LOAI003', CAST(15000.00 AS Decimal(10, 2)), CAST(40000.00 AS Decimal(10, 2)), 120, N'CD001', N'tradaocamsa.jpg')
GO
INSERT [dbo].[SanPham] ([MaSP], [TenSP], [MaLoai], [GiaNhap], [GiaBan], [SoLuong], [MaCongDung], [HinhAnh]) VALUES (N'SP006', N'Trà Vải Hoa Hồng', N'LOAI003', CAST(16000.00 AS Decimal(10, 2)), CAST(42000.00 AS Decimal(10, 2)), 90, N'CD001', N'travaihoahong.jpg')
GO
INSERT [dbo].[SanPham] ([MaSP], [TenSP], [MaLoai], [GiaNhap], [GiaBan], [SoLuong], [MaCongDung], [HinhAnh]) VALUES (N'SP007', N'Nước Ép Cam Tươi', N'LOAI004', CAST(13000.00 AS Decimal(10, 2)), CAST(35000.00 AS Decimal(10, 2)), 60, N'CD003', N'camtuoi.jpg')
GO
INSERT [dbo].[SanPham] ([MaSP], [TenSP], [MaLoai], [GiaNhap], [GiaBan], [SoLuong], [MaCongDung], [HinhAnh]) VALUES (N'SP008', N'Sinh Tố Xoài', N'LOAI004', CAST(17000.00 AS Decimal(10, 2)), CAST(40000.00 AS Decimal(10, 2)), 50, N'CD003', N'sinhtoxoai.jpg')
GO
INSERT [dbo].[SanPham] ([MaSP], [TenSP], [MaLoai], [GiaNhap], [GiaBan], [SoLuong], [MaCongDung], [HinhAnh]) VALUES (N'SP009', N'Cookie Choco Chip', N'LOAI006', CAST(8000.00 AS Decimal(10, 2)), CAST(15000.00 AS Decimal(10, 2)), 200, N'CD004', N'cookie.jpg')
GO
INSERT [dbo].[SanPham] ([MaSP], [TenSP], [MaLoai], [GiaNhap], [GiaBan], [SoLuong], [MaCongDung], [HinhAnh]) VALUES (N'SP010', N'Bánh Mousse Chanh Dây', N'LOAI006', CAST(25000.00 AS Decimal(10, 2)), CAST(45000.00 AS Decimal(10, 2)), 30, N'CD004', N'moussechanhday.jpg')
GO
INSERT [dbo].[SanPham] ([MaSP], [TenSP], [MaLoai], [GiaNhap], [GiaBan], [SoLuong], [MaCongDung], [HinhAnh]) VALUES (N'SP011', N'Hạt Cà Phê Robusta (1kg)', N'LOAI007', CAST(150000.00 AS Decimal(10, 2)), CAST(250000.00 AS Decimal(10, 2)), 50, N'CD005', N'robusta_hat.jpg')
GO
INSERT [dbo].[SanPham] ([MaSP], [TenSP], [MaLoai], [GiaNhap], [GiaBan], [SoLuong], [MaCongDung], [HinhAnh]) VALUES (N'SP012', N'Siro Đường Đen (Chai 750ml)', N'LOAI007', CAST(80000.00 AS Decimal(10, 2)), CAST(120000.00 AS Decimal(10, 2)), 40, N'CD005', N'siro_duongden.jpg')
GO
INSERT [dbo].[TaiKhoan] ([TenDangNhap], [MatKhau], [VaiTro], [MaLienKet]) VALUES (N'admin', N'admin@123', N'Admin', N'NV004')
GO
INSERT [dbo].[TaiKhoan] ([TenDangNhap], [MatKhau], [VaiTro], [MaLienKet]) VALUES (N'an_nv', N'123456', N'NhanVien', N'NV001')
GO
INSERT [dbo].[TaiKhoan] ([TenDangNhap], [MatKhau], [VaiTro], [MaLienKet]) VALUES (N'bich_tt', N'123456', N'NhanVien', N'NV002')
GO
INSERT [dbo].[TaiKhoan] ([TenDangNhap], [MatKhau], [VaiTro], [MaLienKet]) VALUES (N'hai_lm', N'password123', N'NhanVien', N'NV003')
GO
ALTER TABLE [dbo].[HoaDonBan] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[HoaDonNhap] ADD  CONSTRAINT [DF_HoaDonNhap_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ChiTietHDB]  WITH CHECK ADD FOREIGN KEY([MaSP])
REFERENCES [dbo].[SanPham] ([MaSP])
GO
ALTER TABLE [dbo].[ChiTietHDB]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietHDB_HoaDonBan_MaHDB] FOREIGN KEY([MaHDB])
REFERENCES [dbo].[HoaDonBan] ([MaHDB])
GO
ALTER TABLE [dbo].[ChiTietHDB] CHECK CONSTRAINT [FK_ChiTietHDB_HoaDonBan_MaHDB]
GO
ALTER TABLE [dbo].[ChiTietHDN]  WITH CHECK ADD FOREIGN KEY([MaSP])
REFERENCES [dbo].[SanPham] ([MaSP])
GO
ALTER TABLE [dbo].[ChiTietHDN]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietHDN_HoaDonNhap_MaHDN] FOREIGN KEY([MaHDN])
REFERENCES [dbo].[HoaDonNhap] ([MaHDN])
GO
ALTER TABLE [dbo].[ChiTietHDN] CHECK CONSTRAINT [FK_ChiTietHDN_HoaDonNhap_MaHDN]
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
