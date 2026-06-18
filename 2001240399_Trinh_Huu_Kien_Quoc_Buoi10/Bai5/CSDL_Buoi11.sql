USE master;
GO

IF DB_ID('QLSinhVien_Buoi11') IS NOT NULL
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

CREATE TABLE Lop
(
    MaLop VARCHAR(10) NOT NULL PRIMARY KEY,
    TenLop NVARCHAR(50) NOT NULL,
    MaKhoa VARCHAR(5) NOT NULL,
    CONSTRAINT FK_Lop_Khoa FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa)
);

CREATE TABLE MonHoc
(
    MaMon VARCHAR(10) NOT NULL PRIMARY KEY,
    TenMon NVARCHAR(50) NOT NULL UNIQUE,
    SoTinChi INT NOT NULL CHECK (SoTinChi BETWEEN 1 AND 10)
);

CREATE TABLE SinhVien
(
    MaSV VARCHAR(10) NOT NULL PRIMARY KEY,
    HoTen NVARCHAR(50) NOT NULL,
    MaLop VARCHAR(10) NOT NULL,
    Tuoi INT NOT NULL CHECK (Tuoi BETWEEN 16 AND 60),
    CONSTRAINT FK_SinhVien_Lop FOREIGN KEY (MaLop) REFERENCES Lop(MaLop)
);
GO

INSERT INTO Khoa(MaKhoa, TenKhoa) VALUES
('CNTT', N'Cong nghe thong tin'),
('QTKD', N'Quan tri kinh doanh'),
('KT', N'Ke toan');

INSERT INTO Lop(MaLop, TenLop, MaKhoa) VALUES
('DHTH01', N'Dai hoc Tin hoc 01', 'CNTT'),
('DHTH02', N'Dai hoc Tin hoc 02', 'CNTT'),
('DHQT01', N'Dai hoc Quan tri 01', 'QTKD');

INSERT INTO MonHoc(MaMon, TenMon, SoTinChi) VALUES
('NET', N'Cong nghe .NET', 3),
('CSDL', N'Co so du lieu', 3),
('WPF', N'Lap trinh WPF', 4);

INSERT INTO SinhVien(MaSV, HoTen, MaLop, Tuoi) VALUES
('SV01', N'Nguyen Van A', 'DHTH01', 20),
('SV02', N'Tran Thi B', 'DHTH02', 21);
GO
