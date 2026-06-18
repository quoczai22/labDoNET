USE master;
GO

IF DB_ID(N'QLSinhVien_Buoi11') IS NOT NULL
BEGIN
    ALTER DATABASE QLSinhVien_Buoi11 SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QLSinhVien_Buoi11;
END
GO

CREATE DATABASE QLSinhVien_Buoi11;
GO

USE QLSinhVien_Buoi11;
GO

CREATE TABLE Khoa
(
    MaKhoa VARCHAR(5) NOT NULL PRIMARY KEY,
    TenKhoa NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE Lop
(
    MaLop VARCHAR(10) NOT NULL PRIMARY KEY,
    TenLop NVARCHAR(50) NOT NULL,
    MaKhoa VARCHAR(5) NOT NULL,
    CONSTRAINT FK_Lop_Khoa FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa)
);
GO

CREATE TABLE MonHoc
(
    MaMon VARCHAR(10) NOT NULL PRIMARY KEY,
    TenMon NVARCHAR(50) NOT NULL,
    SoTinChi INT NOT NULL,
    CONSTRAINT CK_MonHoc_SoTinChi CHECK (SoTinChi > 0 AND SoTinChi <= 10)
);
GO

CREATE TABLE SinhVien
(
    MaSV VARCHAR(10) NOT NULL PRIMARY KEY,
    HoTen NVARCHAR(50) NOT NULL,
    GioiTinh NVARCHAR(5) NOT NULL,
    NgaySinh DATE NOT NULL,
    MaLop VARCHAR(10) NOT NULL,
    CONSTRAINT FK_SinhVien_Lop FOREIGN KEY (MaLop) REFERENCES Lop(MaLop)
);
GO

INSERT INTO Khoa(MaKhoa, TenKhoa) VALUES
('CNTT', N'Công nghệ thông tin'),
('QTKD', N'Quản trị kinh doanh'),
('KT', N'Kế toán');
GO

INSERT INTO Lop(MaLop, TenLop, MaKhoa) VALUES
('DHTH01', N'Đại học tin học 01', 'CNTT'),
('DHTH02', N'Đại học tin học 02', 'CNTT'),
('DHQT01', N'Đại học quản trị 01', 'QTKD'),
('DHKT01', N'Đại học kế toán 01', 'KT');
GO

INSERT INTO MonHoc(MaMon, TenMon, SoTinChi) VALUES
('CSDL', N'Cơ sở dữ liệu', 3),
('WPF', N'Lập trình WPF', 3),
('CTDL', N'Cấu trúc dữ liệu', 4),
('KTLT', N'Kỹ thuật lập trình', 3);
GO

INSERT INTO SinhVien(MaSV, HoTen, GioiTinh, NgaySinh, MaLop) VALUES
('SV001', N'Trịnh Hữu Kiên Quốc', N'Nam', '2000-04-12', 'DHTH01'),
('SV002', N'Nguyễn Thị Hồng', N'Nữ', '2001-07-20', 'DHTH02'),
('SV003', N'Lê Minh Anh', N'Nam', '2002-11-05', 'DHQT01');
GO
