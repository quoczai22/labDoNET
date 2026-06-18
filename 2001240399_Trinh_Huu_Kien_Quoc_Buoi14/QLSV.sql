-- Tạo cơ sở dữ liệu
CREATE DATABASE QuanLySinhVien;
GO
USE QuanLySinhVien;
GO

-- 1. Tạo bảng Khoa
CREATE TABLE Khoa (
    MaKhoa VARCHAR(20) PRIMARY KEY,
    TenKhoa NVARCHAR(100) NOT NULL
);
GO

-- 2. Tạo bảng Lop (Đã thêm cột TenLop)
CREATE TABLE Lop (
    MaLop VARCHAR(20) PRIMARY KEY,
    TenLop NVARCHAR(100) NOT NULL,
    MaKhoa VARCHAR(20),
    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa)
);
GO

-- 3. Tạo bảng SinhVien
CREATE TABLE SinhVien (
    MaSinhVien VARCHAR(20) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(10),
    NgaySinh DATE,
    MaLop VARCHAR(20),
    FOREIGN KEY (MaLop) REFERENCES Lop(MaLop)
);
GO

-- 4. Tạo bảng MonHoc
CREATE TABLE MonHoc (
    MaMonHoc VARCHAR(20) PRIMARY KEY,
    TenMonHoc NVARCHAR(100) NOT NULL,
    SoTC INT,
    TinhChat NVARCHAR(50)
);
GO

-- 5. Tạo bảng KetQua
CREATE TABLE KetQua (
    MaSinhVien VARCHAR(20),
    MaMonHoc VARCHAR(20),
    NamHoc INT,
    HocKy INT,
    Diem FLOAT,
    PRIMARY KEY (MaSinhVien, MaMonHoc, NamHoc, HocKy),
    FOREIGN KEY (MaSinhVien) REFERENCES SinhVien(MaSinhVien),
    FOREIGN KEY (MaMonHoc) REFERENCES MonHoc(MaMonHoc)
);
GO

-- Thêm dữ liệu vào bảng Khoa
INSERT INTO Khoa (MaKhoa, TenKhoa) 
VALUES 
    ('CNTT', N'Công nghệ thông tin'),
    ('KT', N'Kế toán'),
    ('QTKD', N'Quản trị kinh doanh');
GO

-- Thêm dữ liệu vào bảng Lop (Bao gồm TenLop)
INSERT INTO Lop (MaLop, TenLop, MaKhoa) 
VALUES 
    ('10DHTH', N'Đại học Tin học khóa 10', 'CNTT'),
    ('11DHKTMP', N'Đại học Kỹ thuật Phần mềm khóa 11', 'CNTT'),
    ('10DHKT', N'Đại học Kế toán khóa 10', 'KT');
GO

-- Thêm dữ liệu vào bảng SinhVien
INSERT INTO SinhVien (MaSinhVien, HoTen, GioiTinh, NgaySinh, MaLop) 
VALUES 
    ('SV001', N'Nguyễn Văn A', N'Nam', '2004-05-12', '10DHTH'),
    ('SV002', N'Trần Thị B', N'Nữ', '2004-10-21', '10DHTH'),
    ('SV003', N'Lê Văn C', N'Nam', '2005-01-15', '10DHKT');
GO

-- Thêm dữ liệu vào bảng MonHoc
INSERT INTO MonHoc (MaMonHoc, TenMonHoc, SoTC, TinhChat) 
VALUES 
    ('CSDL', N'Cơ sở dữ liệu', 3, N'Bắt buộc'),
    ('OOP', N'Lập trình hướng đối tượng', 3, N'Bắt buộc'),
    ('NLT', N'Nguyên lý thống kê', 2, N'Tự chọn');
GO

-- Thêm dữ liệu vào bảng KetQua
INSERT INTO KetQua (MaSinhVien, MaMonHoc, NamHoc, HocKy, Diem) 
VALUES 
    ('SV001', 'CSDL', 2024, 1, 8.5),
    ('SV001', 'OOP', 2024, 1, 7.0),
    ('SV002', 'CSDL', 2024, 1, 9.0),
    ('SV003', 'NLT', 2024, 2, 8.0);
GO