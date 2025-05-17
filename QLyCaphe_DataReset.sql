USE [qlyCafe];
GO

PRINT N'Bắt đầu quá trình xóa dữ liệu...';

-- Xóa dữ liệu từ các bảng chi tiết trước
PRINT N'Xóa dữ liệu bảng ChiTietHDB...';
DELETE FROM [dbo].[ChiTietHDB];
GO

PRINT N'Xóa dữ liệu bảng ChiTietHDN...';
DELETE FROM [dbo].[ChiTietHDN];
GO

-- Tiếp theo, xóa dữ liệu từ các bảng hóa đơn (master của các bảng chi tiết ở trên)
PRINT N'Xóa dữ liệu bảng HoaDonBan...';
DELETE FROM [dbo].[HoaDonBan];
GO

PRINT N'Xóa dữ liệu bảng HoaDonNhap...';
DELETE FROM [dbo].[HoaDonNhap];
GO

-- Bây giờ xóa dữ liệu từ các bảng không còn được tham chiếu bởi các bảng hóa đơn/chi tiết
-- hoặc các bảng có dữ liệu phụ thuộc đã được xóa

-- Bảng TaiKhoan có thể có MaLienKet tới NhanVien, nhưng không có FK constraint trong DDL bạn cung cấp
-- trỏ TỪ NhanVien TỚI TaiKhoan, nên có thể xóa TaiKhoan trước hoặc sau NhanVien.
-- Để an toàn, nếu MaLienKet thường là MaNV, ta có thể xóa sau NhanVien.
-- Tuy nhiên, vì không có FK cứng, thứ tự ở đây ít quan trọng hơn.
PRINT N'Xóa dữ liệu bảng TaiKhoan...';
DELETE FROM [dbo].[TaiKhoan];
GO

PRINT N'Xóa dữ liệu bảng SanPham...';
DELETE FROM [dbo].[SanPham];
GO
-- (SanPham tham chiếu tới Loai, CongDung. ChiTietHDB/HDN tham chiếu tới SanPham đã được xóa)

PRINT N'Xóa dữ liệu bảng NhanVien...';
DELETE FROM [dbo].[NhanVien];
GO
-- (NhanVien tham chiếu tới Que. HoaDonBan/Nhap tham chiếu tới NhanVien đã được xóa)

PRINT N'Xóa dữ liệu bảng KhachHang...';
DELETE FROM [dbo].[KhachHang];
GO
-- (HoaDonBan tham chiếu tới KhachHang đã được xóa)

PRINT N'Xóa dữ liệu bảng NhaCungCap...';
DELETE FROM [dbo].[NhaCungCap];
GO
-- (HoaDonNhap tham chiếu tới NhaCungCap đã được xóa)

-- Cuối cùng, xóa dữ liệu từ các bảng danh mục gốc không còn được tham chiếu
PRINT N'Xóa dữ liệu bảng CongDung...';
DELETE FROM [dbo].[CongDung];
GO

PRINT N'Xóa dữ liệu bảng Loai...';
DELETE FROM [dbo].[Loai];
GO

PRINT N'Xóa dữ liệu bảng Que...';
DELETE FROM [dbo].[Que];
GO

PRINT N'Hoàn tất xóa dữ liệu khỏi tất cả các bảng.';