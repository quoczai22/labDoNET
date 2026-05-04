-- 1. Tạo cơ sở dữ liệu
CREATE DATABASE QLSinhVien;
GO

-- 2. Sử dụng cơ sở dữ liệu vừa tạo
USE QLSinhVien;
GO

-- 3. Tạo bảng Khoa
CREATE TABLE Khoa (
    MaKhoa VARCHAR(20) PRIMARY KEY,
    TenKhoa NVARCHAR(100) NOT NULL
);

-- 4. Tạo bảng MonHoc
CREATE TABLE MonHoc (
    MaMonHoc VARCHAR(20) PRIMARY KEY,
    TenMonHoc NVARCHAR(100) NOT NULL,
    SoTC INT,
    TinhChat NVARCHAR(50)
);

-- 5. Tạo bảng Lop (Bảng con của Khoa)
CREATE TABLE Lop (
    MaLop VARCHAR(20) PRIMARY KEY,
    MaKhoa VARCHAR(20),
    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa)
);

-- 6. Tạo bảng SinhVien (Bảng con của Lop)
CREATE TABLE SinhVien (
    MaSinhVien VARCHAR(20) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(10),
    NgaySinh DATE,
    MaLop VARCHAR(20),
    FOREIGN KEY (MaLop) REFERENCES Lop(MaLop)
);

-- 7. Tạo bảng KetQua (Bảng con của SinhVien và MonHoc)
CREATE TABLE KetQua (
    MaSinhVien VARCHAR(20),
    MaMonHoc VARCHAR(20),
    NamHoc VARCHAR(20),
    HocKy INT,
    Diem FLOAT,
    PRIMARY KEY (MaSinhVien, MaMonHoc, NamHoc, HocKy),
    FOREIGN KEY (MaSinhVien) REFERENCES SinhVien(MaSinhVien),
    FOREIGN KEY (MaMonHoc) REFERENCES MonHoc(MaMonHoc)
);
GO

-- 8. THÊM DỮ LIỆU MẪU --

-- Thêm dữ liệu bảng Khoa
INSERT INTO Khoa (MaKhoa, TenKhoa) VALUES
('CNTT', N'Công nghệ thông tin'),
('SH', N'Công nghệ sinh học'),
('TP', N'Công nghệ thực phẩm');

-- Thêm dữ liệu bảng Lop
INSERT INTO Lop (MaLop, MaKhoa) VALUES
('18CDTH1', 'CNTT'),
('19DTH21', 'CNTT'),
('20SH01', 'SH'),
('21TPK', 'TP');

-- Thêm dữ liệu bảng SinhVien (Dựa theo ảnh báo cáo)
INSERT INTO SinhVien (MaSinhVien, HoTen, GioiTinh, NgaySinh, MaLop) VALUES
('SV001', N'Nguyễn Văn An', N'Nam', '2000-01-15', '18CDTH1'),
('SV002', N'Trần Thị Bích', N'Nữ', '2000-03-22', '18CDTH1'),
('SV003', N'Lê Văn Cường', N'Nam', '2001-05-10', '19DTH21'),
('SV004', N'Phạm Hồng Duyên', N'Nữ', '2001-07-18', '19DTH21'),
('SV005', N'Hoàng Minh Đức', N'Nam', '2002-09-30', '20SH01'),
('SV006', N'Vũ Thị Hà', N'Nữ', '2002-11-05', '20SH01'),
('SV007', N'Đỗ Văn Em', N'Nam', '2003-02-14', '21TPK'),
('SV008', N'Ngô Thị Lan', N'Nữ', '2003-04-25', '21TPK'),
('SV009', N'Bùi Văn Khánh', N'Nam', '2000-06-12', '18CDTH1'),
('SV010', N'Mai Thị Ngọc', N'Nữ', '2001-08-20', '19DTH21');
GO