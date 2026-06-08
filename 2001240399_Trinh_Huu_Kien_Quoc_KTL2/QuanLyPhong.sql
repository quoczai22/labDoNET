-- 1. TẠO CƠ SỞ DỮ LIỆU
CREATE DATABASE QL_Karaoke;
GO

USE QL_Karaoke;
GO

-- 2. TẠO CÁC BẢNG VÀ KHÓA CHÍNH/KHÓA NGOẠI

-- Bảng LOAIPHONG
CREATE TABLE LOAIPHONG (
    MaNhom VARCHAR(10) PRIMARY KEY,
    TenNhom NVARCHAR(50)
);

-- Bảng PHONG
CREATE TABLE PHONG (
    MaPhong VARCHAR(10) PRIMARY KEY,
    TenPhong NVARCHAR(50),
    SucChua INT,
    GiaPhong DECIMAL(18, 0),
    KieuPhong INT, -- Chú thích: 1 là phòng quạt, 2 là phòng máy lạnh
    MaNhom VARCHAR(10),
    CONSTRAINT FK_PHONG_LOAIPHONG FOREIGN KEY (MaNhom) REFERENCES LOAIPHONG(MaNhom)
);

-- Bảng KHACHHANG
CREATE TABLE KHACHHANG (
    MaKhachHang VARCHAR(10) PRIMARY KEY,
    TenKH NVARCHAR(50),
    SoDT VARCHAR(15)
);

-- Bảng PHUTHU
CREATE TABLE PHUTHU (
    MaPhuThu VARCHAR(10) PRIMARY KEY,
    TenPhuThu NVARCHAR(50),
    GiaPT DECIMAL(18, 0)
);

-- Bảng DATPHONG
-- (Theo đề bài: mã đặt phòng là trường tự động tăng)
CREATE TABLE DATPHONG (
    MaDatPhong INT IDENTITY(1,1) PRIMARY KEY,
    MaPh VARCHAR(10),
    MaKH VARCHAR(10),
    NgayDat DATETIME,
    NgayTra DATETIME,
    CONSTRAINT FK_DATPHONG_PHONG FOREIGN KEY (MaPh) REFERENCES PHONG(MaPhong),
    CONSTRAINT FK_DATPHONG_KHACHHANG FOREIGN KEY (MaKH) REFERENCES KHACHHANG(MaKhachHang)
);

-- Bảng CHITIETDATPHONG
-- (Giả định MaCT cũng tự động tăng để dễ quản lý)
CREATE TABLE CHITIETDATPHONG (
    MaCT INT IDENTITY(1,1) PRIMARY KEY,
    MaDP INT,
    MaPT VARCHAR(10),
    SL INT,
    CONSTRAINT FK_CTDP_DATPHONG FOREIGN KEY (MaDP) REFERENCES DATPHONG(MaDatPhong),
    CONSTRAINT FK_CTDP_PHUTHU FOREIGN KEY (MaPT) REFERENCES PHUTHU(MaPhuThu)
);
GO

-- 3. THÊM DỮ LIỆU MẪU

-- Thêm dữ liệu LOAIPHONG (Tầng)
INSERT INTO LOAIPHONG (MaNhom, TenNhom) VALUES
('T1', N'Tầng 1'),
('T2', N'Tầng 2');

-- Thêm dữ liệu PHONG
INSERT INTO PHONG (MaPhong, TenPhong, SucChua, GiaPhong, KieuPhong, MaNhom) VALUES
('P01', N'Phòng VIP', 10, 150000, 2, 'T1'),
('P02', N'Phòng Thường', 8, 100000, 1, 'T2'),
('A1002', N'Phòng 2', 20, 300000, 2, 'T1'),
('A1003', N'Phòng 3', 15, 250000, 1, 'T1');

-- Thêm dữ liệu KHACHHANG
INSERT INTO KHACHHANG (MaKhachHang, TenKH, SoDT) VALUES
('KH01', N'Nguyễn Văn A', '0901234567'),
('KH02', N'Trần Thị B', '0987654321');

-- Thêm dữ liệu PHUTHU
INSERT INTO PHUTHU (MaPhuThu, TenPhuThu, GiaPT) VALUES
('PT01', N'Bia Tiger', 20000),
('PT02', N'Nước ngọt', 15000),
('PT03', N'Trái cây dĩa', 50000);

-- Thêm dữ liệu DATPHONG (Dữ liệu test)
INSERT INTO DATPHONG (MaPh, MaKH, NgayDat, NgayTra) VALUES
('P01', 'KH01', '2026-01-23 13:00:00', '2026-01-23 15:00:00');

-- Thêm dữ liệu CHITIETDATPHONG (Dữ liệu test)
-- Lưu ý: MaDP số 1 khớp với Identity vừa thêm ở trên
INSERT INTO CHITIETDATPHONG (MaDP, MaPT, SL) VALUES
(1, 'PT01', 5),
(1, 'PT02', 2);
GO