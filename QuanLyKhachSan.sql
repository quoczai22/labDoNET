-- ============================================
-- CƠ SỞ DỮ LIỆU: QUẢN LÝ KHÁCH SẠN
-- Môn: Công nghệ .NET - Đề KT2
-- ============================================

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'QuanLyKhachSan')
    DROP DATABASE QuanLyKhachSan;
GO

CREATE DATABASE QuanLyKhachSan;
GO

USE QuanLyKhachSan;
GO

-- ============================================
-- 1. BẢNG LOAIPHONG
-- ============================================
CREATE TABLE LOAIPHONG (
    MaLoai      VARCHAR(10)     NOT NULL PRIMARY KEY,
    TenLoai     NVARCHAR(100)   NOT NULL,
    GiaLoai     DECIMAL(18, 0)  NOT NULL
);
GO

-- ============================================
-- 2. BẢNG PHONG
-- ============================================
CREATE TABLE PHONG (
    MaPhong     VARCHAR(10)     NOT NULL PRIMARY KEY,
    TenPhong    NVARCHAR(100)   NOT NULL,
    SucChua     INT             NOT NULL,
    GiaPhong    DECIMAL(18, 0)  NOT NULL,
    MaLoai      VARCHAR(10)     NOT NULL,
    TinhTrang   INT             NOT NULL DEFAULT 0,
        -- 0: Phòng trống
        -- 1: Khách đang nhận phòng
    CONSTRAINT FK_PHONG_LOAIPHONG FOREIGN KEY (MaLoai)
        REFERENCES LOAIPHONG(MaLoai)
);
GO

-- ============================================
-- 3. BẢNG KHACHHANG
-- ============================================
CREATE TABLE KHACHHANG (
    MaKH        VARCHAR(10)     NOT NULL PRIMARY KEY,
    TenKH       NVARCHAR(100)   NOT NULL,
    SoDT        VARCHAR(15)     NOT NULL,
    DiaChi      NVARCHAR(200)   NULL
);
GO

-- ============================================
-- 4. BẢNG DATPHONG
-- ============================================
CREATE TABLE DATPHONG (
    MaDatPhong  INT             NOT NULL PRIMARY KEY IDENTITY(1,1),
    MaPh        VARCHAR(10)     NOT NULL,
    MaKH        VARCHAR(10)     NOT NULL,
    NgayDat     DATE            NOT NULL DEFAULT GETDATE(),
    NgayTra     DATE            NULL,
    GioVao      TIME            NOT NULL,
    GioRa       TIME            NOT NULL,
    CONSTRAINT FK_DATPHONG_PHONG      FOREIGN KEY (MaPh)
        REFERENCES PHONG(MaPhong),
    CONSTRAINT FK_DATPHONG_KHACHHANG  FOREIGN KEY (MaKH)
        REFERENCES KHACHHANG(MaKH)
);
GO

-- ============================================
-- 5. BẢNG DICHVU
-- ============================================
CREATE TABLE DICHVU (
    MaDV        VARCHAR(10)     NOT NULL PRIMARY KEY,
    TenDV       NVARCHAR(100)   NOT NULL,
    GiaDV       DECIMAL(18, 0)  NOT NULL
);
GO

-- ============================================
-- 6. BẢNG CHITIETDATPHONG
-- ============================================
CREATE TABLE CHITIETDATPHONG (
    MaCT        INT             NOT NULL PRIMARY KEY IDENTITY(1,1),
    MaDatPhong  INT             NOT NULL,
    MaDV        VARCHAR(10)     NOT NULL,
    SoLuong     INT             NOT NULL DEFAULT 1,
    CONSTRAINT FK_CTDP_DATPHONG FOREIGN KEY (MaDatPhong)
        REFERENCES DATPHONG(MaDatPhong),
    CONSTRAINT FK_CTDP_DICHVU  FOREIGN KEY (MaDV)
        REFERENCES DICHVU(MaDV)
);
GO

-- ============================================
-- DỮ LIỆU MẪU
-- ============================================

-- LOAIPHONG
INSERT INTO LOAIPHONG (MaLoai, TenLoai, GiaLoai) VALUES
('LPH01', N'Phòng đơn',    500000),
('LPH02', N'Phòng đôi',    900000),
('LPH03', N'Phòng Suite',  2000000);
GO

-- PHONG
INSERT INTO PHONG (MaPhong, TenPhong, SucChua, GiaPhong, MaLoai, TinhTrang) VALUES
('PH01', N'Phòng Standard 101', 1, 500000,  'LPH01', 0),
('PH02', N'Phòng Standard 102', 1, 500000,  'LPH01', 1),
('PH03', N'Phòng Deluxe 201',   2, 900000,  'LPH02', 0),
('PH04', N'Phòng Deluxe 202',   2, 900000,  'LPH02', 1),
('PH05', N'Phòng Suite 301',    4, 2000000, 'LPH03', 0);
GO

-- KHACHHANG
INSERT INTO KHACHHANG (MaKH, TenKH, SoDT, DiaChi) VALUES
('KH01', N'Nguyễn Văn A',  '0901234567', N'123 Lê Lợi, Q.1, TP.HCM'),
('KH02', N'Trần Thị B',    '0912345678', N'456 Nguyễn Huệ, Q.1, TP.HCM'),
('KH03', N'Lê Văn C',      '0923456789', N'789 Điện Biên Phủ, Q.3, TP.HCM'),
('KH04', N'Phạm Thị D',    '0934567890', N'321 Võ Văn Tần, Q.3, TP.HCM');
GO

-- DICHVU
INSERT INTO DICHVU (MaDV, TenDV, GiaDV) VALUES
('DV01', N'Nước suối',     15000),
('DV02', N'Khăn tắm',      20000),
('DV03', N'Bữa sáng',      80000),
('DV04', N'Đưa đón sân bay', 200000),
('DV05', N'Giặt ủi',       50000);
GO

-- DATPHONG
INSERT INTO DATPHONG (MaPh, MaKH, NgayDat, NgayTra, GioVao, GioRa) VALUES
('PH02', 'KH01', '2026-01-23', NULL,         '14:00', '16:00'),
('PH04', 'KH02', '2026-01-23', NULL,         '13:00', '15:00');
GO

-- CHITIETDATPHONG
INSERT INTO CHITIETDATPHONG (MaDatPhong, MaDV, SoLuong) VALUES
(1, 'DV01', 3),
(1, 'DV02', 2),
(2, 'DV03', 2),
(2, 'DV05', 1);
GO

-- ============================================
-- KIỂM TRA DỮ LIỆU
-- ============================================
SELECT * FROM LOAIPHONG;
SELECT * FROM PHONG;
SELECT * FROM KHACHHANG;
SELECT * FROM DICHVU;
SELECT * FROM DATPHONG;
SELECT * FROM CHITIETDATPHONG;
GO
