USE master;
GO
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'QLSinhVien')
    DROP DATABASE QLSinhVien;
GO
CREATE DATABASE QLSinhVien;
GO
USE QLSinhVien;
GO

-- 1. Bảng Khoa
CREATE TABLE Khoa (
    MaKhoa VARCHAR(20) PRIMARY KEY,
    TenKhoa NVARCHAR(100) NOT NULL
);

-- 2. Bảng Lop
CREATE TABLE Lop (
    MaLop VARCHAR(20) PRIMARY KEY,
    MaKhoa VARCHAR(20),
    CONSTRAINT FK_Lop_Khoa FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa)
);

-- 3. Bảng SinhVien
CREATE TABLE SinhVien (
    MaSinhVien VARCHAR(20) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(10),
    NgaySinh DATE,
    MaLop VARCHAR(20),
    CONSTRAINT FK_SV_Lop FOREIGN KEY (MaLop) REFERENCES Lop(MaLop)
);

-- 4. Bảng MonHoc
CREATE TABLE MonHoc (
    MaMonHoc VARCHAR(20) PRIMARY KEY,
    TenMonHoc NVARCHAR(100) NOT NULL,
    SoTC INT,
    TinhChat NVARCHAR(50)
);

-- 5. Bảng KetQua (Khóa chính gồm 4 cột)
CREATE TABLE KetQua (
    MaSinhVien VARCHAR(20) NOT NULL,
    MaMonHoc VARCHAR(20) NOT NULL,
    NamHoc VARCHAR(20) NOT NULL,
    HocKy INT NOT NULL,
    Diem FLOAT,
    PRIMARY KEY (MaSinhVien, MaMonHoc, NamHoc, HocKy),
    CONSTRAINT FK_KQ_SV FOREIGN KEY (MaSinhVien) REFERENCES SinhVien(MaSinhVien),
    CONSTRAINT FK_KQ_MH FOREIGN KEY (MaMonHoc) REFERENCES MonHoc(MaMonHoc)
);
GO
USE QLSinhVien;
GO

-- Thêm Khoa
INSERT INTO Khoa VALUES ('CNTT', N'Công nghệ thông tin');
INSERT INTO Khoa VALUES ('KT', N'Kế toán');
INSERT INTO Khoa VALUES ('NN', N'Ngoại ngữ');

-- Thêm Lớp
INSERT INTO Lop VALUES ('20CNTT01', 'CNTT');
INSERT INTO Lop VALUES ('20CNTT02', 'CNTT');
INSERT INTO Lop VALUES ('21KT01', 'KT');

-- Thêm Sinh Viên
INSERT INTO SinhVien VALUES ('2001240399', N'Trịnh Hữu Kiến Quốc', N'Nam', '2006-04-17', '20CNTT01');
INSERT INTO SinhVien VALUES ('2001240001', N'Nguyễn Văn A', N'Nam', '2005-01-20', '20CNTT01');
INSERT INTO SinhVien VALUES ('2001240002', N'Trần Thị B', N'Nữ', '2005-05-15', '21KT01');

-- Thêm Môn Học
INSERT INTO MonHoc VALUES ('CSDL', N'Cơ sở dữ liệu', 3, N'Bắt buộc');
INSERT INTO MonHoc VALUES ('WPF', N'Lập trình Windows', 4, N'Tự chọn');
INSERT INTO MonHoc VALUES ('TA1', N'Tiếng Anh 1', 2, N'Bắt buộc');

-- Thêm Kết Quả
INSERT INTO KetQua VALUES ('2001240399', 'CSDL', '2025-2026', 1, 8.5);
INSERT INTO KetQua VALUES ('2001240399', 'WPF', '2025-2026', 1, 9.0);
INSERT INTO KetQua VALUES ('2001240001', 'CSDL', '2025-2026', 1, 7.0);
GO