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
-- Thêm dữ liệu bảng MonHoc
INSERT INTO MonHoc (MaMonHoc, TenMonHoc, SoTC, TinhChat) VALUES
('CSDL', N'Cơ sở dữ liệu', 3, N'Bắt buộc'),
('WPF', N'Lập trình WPF', 3, N'Bắt buộc'),
('CTDL', N'Cấu trúc dữ liệu', 4, N'Bắt buộc'),
('AVCB', N'Anh văn căn bản', 2, N'Tự chọn');

-- Thêm dữ liệu bảng KetQua
INSERT INTO KetQua (MaSinhVien, MaMonHoc, NamHoc, HocKy, Diem) VALUES
('SV001', 'AVCB', '2024-2025', 1, 8.5),
('SV002', 'AVCB', '2024-2025', 1, 7.5),
('SV003', 'AVCB', '2024-2025', 1, 9.0),
('SV004', 'AVCB', '2024-2025', 1, 6.5),
('SV005', 'AVCB', '2024-2025', 1, 8.0),
('SV006', 'AVCB', '2024-2025', 1, 7.0),
('SV007', 'AVCB', '2024-2025', 1, 8.2),
('SV008', 'AVCB', '2024-2025', 1, 7.8),
('SV009', 'AVCB', '2024-2025', 1, 6.8),
('SV010', 'AVCB', '2024-2025', 1, 8.7),
('SV001', 'CSDL', '2024-2025', 1, 8.0),
('SV002', 'CSDL', '2024-2025', 1, 7.2),
('SV003', 'CSDL', '2024-2025', 1, 8.9),
('SV004', 'CSDL', '2024-2025', 1, 6.9),
('SV005', 'CSDL', '2024-2025', 1, 7.6),
('SV006', 'CSDL', '2024-2025', 1, 8.1),
('SV007', 'CSDL', '2024-2025', 1, 7.4),
('SV008', 'CSDL', '2024-2025', 1, 8.3),
('SV009', 'CSDL', '2024-2025', 1, 7.1),
('SV010', 'CSDL', '2024-2025', 1, 8.6);
GO
-- Bổ sung thêm dữ liệu test cho MonHoc
IF NOT EXISTS (SELECT 1 FROM MonHoc WHERE MaMonHoc = 'LTCB') INSERT INTO MonHoc (MaMonHoc, TenMonHoc, SoTC, TinhChat) VALUES ('LTCB', N'Lập trình căn bản', 4, N'Bắt buộc');
IF NOT EXISTS (SELECT 1 FROM MonHoc WHERE MaMonHoc = 'LTNC') INSERT INTO MonHoc (MaMonHoc, TenMonHoc, SoTC, TinhChat) VALUES ('LTNC', N'Lập trình nâng cao', 4, N'Bắt buộc');
IF NOT EXISTS (SELECT 1 FROM MonHoc WHERE MaMonHoc = 'MMT') INSERT INTO MonHoc (MaMonHoc, TenMonHoc, SoTC, TinhChat) VALUES ('MMT', N'Mạng máy tính', 3, N'Bắt buộc');
IF NOT EXISTS (SELECT 1 FROM MonHoc WHERE MaMonHoc = 'PTTK') INSERT INTO MonHoc (MaMonHoc, TenMonHoc, SoTC, TinhChat) VALUES ('PTTK', N'Phân tích thiết kế hệ thống', 3, N'Bắt buộc');
IF NOT EXISTS (SELECT 1 FROM MonHoc WHERE MaMonHoc = 'TRR') INSERT INTO MonHoc (MaMonHoc, TenMonHoc, SoTC, TinhChat) VALUES ('TRR', N'Toán rời rạc', 3, N'Bắt buộc');
IF NOT EXISTS (SELECT 1 FROM MonHoc WHERE MaMonHoc = 'KNS') INSERT INTO MonHoc (MaMonHoc, TenMonHoc, SoTC, TinhChat) VALUES ('KNS', N'Kỹ năng mềm', 2, N'Tự chọn');
GO

-- Bổ sung thêm dữ liệu test cho KetQua
MERGE KetQua AS t
USING (VALUES
('SV001', 'WPF', '2024-2025', 1, 9.0), ('SV002', 'WPF', '2024-2025', 1, 8.2), ('SV003', 'WPF', '2024-2025', 1, 7.8), ('SV004', 'WPF', '2024-2025', 1, 6.4), ('SV005', 'WPF', '2024-2025', 1, 8.6),
('SV006', 'WPF', '2024-2025', 1, 7.1), ('SV007', 'WPF', '2024-2025', 1, 8.0), ('SV008', 'WPF', '2024-2025', 1, 7.7), ('SV009', 'WPF', '2024-2025', 1, 6.9), ('SV010', 'WPF', '2024-2025', 1, 8.4),
('SV001', 'CTDL', '2024-2025', 2, 7.5), ('SV002', 'CTDL', '2024-2025', 2, 8.1), ('SV003', 'CTDL', '2024-2025', 2, 8.8), ('SV004', 'CTDL', '2024-2025', 2, 6.2), ('SV005', 'CTDL', '2024-2025', 2, 7.9),
('SV006', 'CTDL', '2024-2025', 2, 8.3), ('SV007', 'CTDL', '2024-2025', 2, 7.0), ('SV008', 'CTDL', '2024-2025', 2, 8.5), ('SV009', 'CTDL', '2024-2025', 2, 7.6), ('SV010', 'CTDL', '2024-2025', 2, 9.1),
('SV001', 'LTCB', '2025-2026', 1, 8.4), ('SV002', 'LTCB', '2025-2026', 1, 7.6), ('SV003', 'LTCB', '2025-2026', 1, 8.9), ('SV004', 'LTCB', '2025-2026', 1, 7.2), ('SV005', 'LTCB', '2025-2026', 1, 8.0),
('SV006', 'LTCB', '2025-2026', 1, 6.8), ('SV007', 'LTCB', '2025-2026', 1, 7.9), ('SV008', 'LTCB', '2025-2026', 1, 8.7), ('SV009', 'LTCB', '2025-2026', 1, 6.5), ('SV010', 'LTCB', '2025-2026', 1, 8.2),
('SV001', 'MMT', '2025-2026', 2, 7.8), ('SV002', 'MMT', '2025-2026', 2, 8.0), ('SV003', 'MMT', '2025-2026', 2, 7.4), ('SV004', 'MMT', '2025-2026', 2, 6.9), ('SV005', 'MMT', '2025-2026', 2, 8.5),
('SV006', 'MMT', '2025-2026', 2, 7.3), ('SV007', 'MMT', '2025-2026', 2, 8.1), ('SV008', 'MMT', '2025-2026', 2, 7.9), ('SV009', 'MMT', '2025-2026', 2, 6.7), ('SV010', 'MMT', '2025-2026', 2, 8.8),
('SV001', 'KNS', '2026-2027', 1, 9.2), ('SV002', 'KNS', '2026-2027', 1, 8.9), ('SV003', 'KNS', '2026-2027', 1, 9.0), ('SV004', 'KNS', '2026-2027', 1, 8.1), ('SV005', 'KNS', '2026-2027', 1, 8.7),
('SV006', 'KNS', '2026-2027', 1, 7.8), ('SV007', 'KNS', '2026-2027', 1, 8.4), ('SV008', 'KNS', '2026-2027', 1, 9.1), ('SV009', 'KNS', '2026-2027', 1, 8.3), ('SV010', 'KNS', '2026-2027', 1, 8.6)
) AS s(MaSinhVien, MaMonHoc, NamHoc, HocKy, Diem)
ON t.MaSinhVien = s.MaSinhVien AND t.MaMonHoc = s.MaMonHoc AND t.NamHoc = s.NamHoc AND t.HocKy = s.HocKy
WHEN MATCHED THEN UPDATE SET Diem = s.Diem
WHEN NOT MATCHED THEN INSERT (MaSinhVien, MaMonHoc, NamHoc, HocKy, Diem) VALUES (s.MaSinhVien, s.MaMonHoc, s.NamHoc, s.HocKy, s.Diem);
GO

