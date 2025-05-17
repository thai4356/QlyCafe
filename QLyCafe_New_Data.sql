USE [qlyCafe];
GO

-- =============================================
-- Bảng Dữ liệu Chính (Master Data)
-- =============================================

-- 1. Bảng Que (Quê quán)
PRINT N'Chèn dữ liệu vào bảng Que';
INSERT INTO [dbo].[Que] ([MaQue], [TenQue]) VALUES
('QUE001', N'Hà Nội'),
('QUE002', N'TP. Hồ Chí Minh'),
('QUE003', N'Đà Nẵng'),
('QUE004', N'Hải Phòng'),
('QUE005', N'Cần Thơ'),
('QUE006', N'An Giang'),
('QUE007', N'Bình Dương');
GO

-- 2. Bảng Loai (Loại Sản Phẩm)
PRINT N'Chèn dữ liệu vào bảng Loai';
INSERT INTO [dbo].[Loai] ([MaLoai], [TenLoai]) VALUES
('LOAI001', N'Cà Phê Truyền Thống'),
('LOAI002', N'Cà Phê Espresso'),
('LOAI003', N'Trà Trái Cây'),
('LOAI004', N'Nước Ép & Sinh Tố'),
('LOAI005', N'Đá Xay'),
('LOAI006', N'Bánh Ngọt & Ăn Vặt'),
('LOAI007', N'Nguyên Liệu Pha Chế');
GO

-- 3. Bảng CongDung (Công Dụng Sản Phẩm)
PRINT N'Chèn dữ liệu vào bảng CongDung';
INSERT INTO [dbo].[CongDung] ([MaCongDung], [TenCongDung]) VALUES
('CD001', N'Giải khát tức thì'),
('CD002', N'Tăng cường sự tỉnh táo'),
('CD003', N'Bổ sung vitamin và khoáng chất'),
('CD004', N'Món ăn nhẹ'),
('CD005', N'Thưởng thức hương vị');
GO

-- 4. Bảng NhaCungCap (Nhà Cung Cấp)
PRINT N'Chèn dữ liệu vào bảng NhaCungCap';
INSERT INTO [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [DiaChi], [SDT]) VALUES
('NCC001', N'Công ty Cà Phê Ban Mê Xanh', N'150 Y Moan, TP. Buôn Ma Thuột, Đắk Lắk', '0905123456'),
('NCC002', N'Nhà Phân Phối Sữa Tươi Dalatmilk Miền Nam', N'22 Võ Văn Tần, Quận 3, TP. Hồ Chí Minh', '0988776655'),
('NCC003', N'Trang Trại Sữa Tươi Mộc Châu Farm', N'Thị trấn Mộc Châu, Sơn La', '0987111222'),
('NCC004', N'Công ty Cổ phần Đường Lam Sơn', N'Thị trấn Lam Sơn, Thọ Xuân, Thanh Hóa', '02373834567'),
('NCC005', N'Nhà Phân Phối Syrup Torani Miền Bắc', N'Số 10 Ngõ Huyện, Hoàn Kiếm, Hà Nội', '0918765432'),
('NCC006', N'VinaSyrup Toàn Quốc', N'Lô B2, KCN Tân Bình, TP. Hồ Chí Minh', '0933112233'),
('NCC007', N'Bột Pha Chế HorecaVN', N'789 Giải Phóng, Hoàng Mai, Hà Nội', '0977889900');
GO

-- 5. Bảng NhanVien (Nhân Viên)
PRINT N'Chèn dữ liệu vào bảng NhanVien';
INSERT INTO [dbo].[NhanVien] ([MaNV], [TenNV], [DiaChi], [GioiTinh], [NgaySinh], [MaQue], [SDT]) VALUES
('NV001', N'Nguyễn Văn An', N'123 Đường Láng, Đống Đa', N'Nam', '1995-08-15', 'QUE001', '0901112221'),
('NV002', N'Trần Thị Bích', N'456 Lê Lợi, Quận 1', N'Nữ', '1998-05-20', 'QUE002', '0902223332'),
('NV003', N'Lê Minh Hải', N'789 Hoàng Diệu, Quận Hải Châu', N'Nam', '1992-12-01', 'QUE003', '0903334443'),
('NV004', N'Phạm Thị Dung', N'101 Trần Phú, Ngô Quyền', N'Nữ', '2000-02-28', 'QUE004', '0904445554'),
('NV005', N'Hoàng Văn Nam', N'202 Nguyễn Văn Cừ, Ninh Kiều', N'Nam', '1997-07-07', 'QUE005', '0905556665');
GO

-- 6. Bảng KhachHang (Khách Hàng)
-- Lưu ý: Bảng KhachHang hiện tại chỉ có MaKH và DiaChi. 
-- Bạn có thể cân nhắc thêm cột TenKH (Tên Khách Hàng) để quản lý tốt hơn.
PRINT N'Chèn dữ liệu vào bảng KhachHang';
INSERT INTO [dbo].[KhachHang] ([MaKH], [DiaChi]) VALUES
('KH001', N'789 Đường Cầu Giấy, Hà Nội'), -- Tên gợi ý: Trần Văn Cường
('KH002', N'101 Võ Văn Tần, Quận 3, TP. Hồ Chí Minh'), -- Tên gợi ý: Lê Thị Duyên
('KH003', N'Khách vãng lai'), -- Có thể dùng cho khách không đăng ký
('KH004', N'33 Ngô Thì Nhậm, Hai Bà Trưng, Hà Nội'), -- Tên gợi ý: Nguyễn Thu Hằng
('KH005', N'Số 10, KDC An Bình, Quận Ninh Kiều, Cần Thơ'); -- Tên gợi ý: Lý Hoàng Phúc
GO

-- 7. Bảng SanPham (Sản Phẩm)
PRINT N'Chèn dữ liệu vào bảng SanPham';
INSERT INTO [dbo].[SanPham] ([MaSP], [TenSP], [MaLoai], [GiaNhap], [GiaBan], [SoLuong], [MaCongDung], [HinhAnh]) VALUES
('SP001', N'Cà Phê Đen Đá Phin', 'LOAI001', 10000.00, 25000.00, 100, 'CD002', N'images/caphe/den_da_phin.jpg'),
('SP002', N'Cà Phê Sữa Đá Phin', 'LOAI001', 12000.00, 30000.00, 150, 'CD002', N'images/caphe/sua_da_phin.jpg'),
('SP003', N'Espresso Nóng', 'LOAI002', 18000.00, 35000.00, 80, 'CD002', N'images/caphe/espresso.jpg'),
('SP004', N'Cappuccino', 'LOAI002', 22000.00, 45000.00, 70, 'CD005', N'images/caphe/cappuccino.jpg'),
('SP005', N'Trà Đào Cam Sả', 'LOAI003', 15000.00, 40000.00, 120, 'CD001', N'images/tra/tradaocamsa.jpg'),
('SP006', N'Trà Vải Hoa Hồng', 'LOAI003', 16000.00, 42000.00, 90, 'CD001', N'images/tra/travaihoahong.jpg'),
('SP007', N'Nước Ép Cam Tươi', 'LOAI004', 13000.00, 35000.00, 60, 'CD003', N'images/nuocep/camtuoi.jpg'),
('SP008', N'Sinh Tố Xoài', 'LOAI004', 17000.00, 40000.00, 50, 'CD003', N'images/sinhto/sinhtoxoai.jpg'),
('SP009', N'Cookie Choco Chip', 'LOAI006', 8000.00, 15000.00, 200, 'CD004', N'images/banh/cookie.jpg'),
('SP010', N'Bánh Mousse Chanh Dây', 'LOAI006', 25000.00, 45000.00, 30, 'CD004', N'images/banh/moussechanhday.jpg'),
('SP011', N'Hạt Cà Phê Robusta (1kg)', 'LOAI007', 150000.00, 250000.00, 50, 'CD005', N'images/nguyenlieu/robusta_hat.jpg'),
('SP012', N'Siro Đường Đen (Chai 750ml)', 'LOAI007', 80000.00, 120000.00, 40, 'CD005', N'images/nguyenlieu/siro_duongden.jpg');
GO

-- 8. Bảng TaiKhoan (Tài Khoản)
-- Lưu ý: Mật khẩu nên được hash trong thực tế. MaLienKet có thể là MaNV.
PRINT N'Chèn dữ liệu vào bảng TaiKhoan';
INSERT INTO [dbo].[TaiKhoan] ([TenDangNhap], [MatKhau], [VaiTro], [MaLienKet]) VALUES
(N'admin', N'admin@123', N'Admin', NULL), -- Tài khoản admin tổng
(N'an_nv', N'123456', N'NhanVien', 'NV001'),
(N'bich_tt', N'123456', N'NhanVien', 'NV002'),
(N'hai_lm', N'password123', N'NhanVien', 'NV003');
GO


-- =============================================
-- Bảng Dữ liệu Giao Dịch (Transaction Data) - VÍ DỤ
-- =============================================

-- Cần có dữ liệu ở các bảng master trước khi chèn vào đây.

-- Ví dụ 1: Hóa Đơn Nhập và Chi Tiết Hóa Đơn Nhập
PRINT N'Chèn dữ liệu ví dụ vào bảng HoaDonNhap và ChiTietHDN';
DECLARE @MaHDN_Vidu1 VARCHAR(10) = 'HDN001';
DECLARE @TongTienHDN_Vidu1 DECIMAL(15,2) = 0;

-- Tính tổng tiền cho chi tiết hóa đơn nhập
SET @TongTienHDN_Vidu1 = (SELECT (20 * 140000.00) + (10 * 75000.00)); -- 20kg SP011, 10 chai SP012 (giá nhập ví dụ)

INSERT INTO [dbo].[HoaDonNhap] ([MaHDN], [NgayNhap], [MaNV], [MaNCC], [TongTien]) VALUES
(@MaHDN_Vidu1, '2025-05-16', 'NV001', 'NCC001', @TongTienHDN_Vidu1);

INSERT INTO [dbo].[ChiTietHDN] ([MaHDN], [MaSP], [SoLuong], [DonGia], [ThanhTien], [KhuyenMai]) VALUES
(@MaHDN_Vidu1, 'SP011', 20, 140000.00, (20 * 140000.00), NULL), -- Giá nhập SP011 là 140k
(@MaHDN_Vidu1, 'SP012', 10, 75000.00, (10 * 75000.00), NULL); -- Giá nhập SP012 là 75k
GO


-- Ví dụ 2: Hóa Đơn Bán và Chi Tiết Hóa Đơn Bán
PRINT N'Chèn dữ liệu ví dụ vào bảng HoaDonBan và ChiTietHDB';
DECLARE @MaHDB_Vidu1 VARCHAR(10) = 'HDB001';
DECLARE @TongTienHDB_Vidu1 DECIMAL(15,2);

-- Giả sử giá bán SP001 là 25000, SP005 là 40000
SET @TongTienHDB_Vidu1 = (2 * 25000.00) + (1 * 40000.00); 

INSERT INTO [dbo].[HoaDonBan] ([MaHDB], [NgayBan], [MaNV], [MaKH], [TongTien]) VALUES
(@MaHDB_Vidu1, '2025-05-17', 'NV002', 'KH001', @TongTienHDB_Vidu1);

INSERT INTO [dbo].[ChiTietHDB] ([MaHDB], [MaSP], [SoLuong], [ThanhTien], [KhuyenMai]) VALUES
(@MaHDB_Vidu1, 'SP001', 2, (2 * 25000.00), NULL), -- 2 Cà Phê Đen Đá Phin
(@MaHDB_Vidu1, 'SP005', 1, (1 * 40000.00), N'Giảm 5k'); -- 1 Trà Đào Cam Sả, ThanhTien có thể là sau giảm giá hoặc trước, tùy logic. Ở đây là trước giảm.
-- Nếu ThanhTien là giá cuối cùng sau khuyến mãi, bạn cần tính toán lại. Giả sử ở đây ThanhTien là giá gốc.
-- Tổng tiền hóa đơn sẽ được cập nhật dựa trên logic tính khuyến mãi của bạn.
GO

PRINT N'Hoàn tất chèn dữ liệu mẫu.';