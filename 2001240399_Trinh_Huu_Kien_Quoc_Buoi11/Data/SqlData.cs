using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using Lab11_ValidationNavigation.Models;

namespace Lab11_ValidationNavigation.Data
{
    public static class SqlData
    {
        public const string ConnectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QLSinhVien_Buoi11;Integrated Security=True;TrustServerCertificate=True";

        public static ObservableCollection<Khoa> LoadKhoas()
        {
            var list = new ObservableCollection<Khoa>();
            using var connection = OpenConnection();
            using var command = new SqlCommand("SELECT MaKhoa, TenKhoa FROM Khoa ORDER BY MaKhoa", connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Khoa
                {
                    MaKhoa = reader.GetString(0),
                    TenKhoa = reader.GetString(1)
                });
            }
            return list;
        }

        public static ObservableCollection<Lop> LoadLops()
        {
            var list = new ObservableCollection<Lop>();
            using var connection = OpenConnection();
            using var command = new SqlCommand("SELECT MaLop, TenLop, MaKhoa FROM Lop ORDER BY MaLop", connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Lop
                {
                    MaLop = reader.GetString(0),
                    TenLop = reader.GetString(1),
                    MaKhoa = reader.GetString(2)
                });
            }
            return list;
        }

        public static ObservableCollection<MonHoc> LoadMonHocs()
        {
            var list = new ObservableCollection<MonHoc>();
            using var connection = OpenConnection();
            using var command = new SqlCommand("SELECT MaMon, TenMon, SoTinChi FROM MonHoc ORDER BY MaMon", connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new MonHoc
                {
                    MaMon = reader.GetString(0),
                    TenMon = reader.GetString(1),
                    SoTinChi = reader.GetInt32(2)
                });
            }
            return list;
        }

        public static ObservableCollection<SinhVien> LoadSinhViens()
        {
            var list = new ObservableCollection<SinhVien>();
            using var connection = OpenConnection();
            using var command = new SqlCommand("SELECT MaSV, HoTen, MaLop, Tuoi FROM SinhVien ORDER BY MaSV", connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new SinhVien
                {
                    MaSV = reader.GetString(0),
                    HoTen = reader.GetString(1),
                    MaLop = reader.GetString(2),
                    Tuoi = reader.GetInt32(3)
                });
            }
            return list;
        }

        public static bool Exists(string table, string column, string value, string exceptColumn = null, string exceptValue = null)
        {
            try
            {
                using var connection = OpenConnection();
                var sql = $"SELECT COUNT(*) FROM {table} WHERE {column} = @value";
                if (!string.IsNullOrWhiteSpace(exceptColumn))
                {
                    sql += $" AND {exceptColumn} <> @exceptValue";
                }

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@value", value ?? "");
                if (!string.IsNullOrWhiteSpace(exceptColumn))
                {
                    command.Parameters.AddWithValue("@exceptValue", exceptValue ?? "");
                }

                return (int)command.ExecuteScalar() > 0;
            }
            catch
            {
                return false;
            }
        }

        public static void AddKhoa(Khoa khoa)
        {
            Execute("INSERT INTO Khoa(MaKhoa, TenKhoa) VALUES(@MaKhoa, @TenKhoa)",
                p =>
                {
                    p.AddWithValue("@MaKhoa", khoa.MaKhoa);
                    p.AddWithValue("@TenKhoa", khoa.TenKhoa);
                });
        }

        public static void UpdateKhoa(string oldMaKhoa, Khoa khoa)
        {
            Execute("UPDATE Khoa SET MaKhoa=@MaKhoa, TenKhoa=@TenKhoa WHERE MaKhoa=@OldMaKhoa",
                p =>
                {
                    p.AddWithValue("@MaKhoa", khoa.MaKhoa);
                    p.AddWithValue("@TenKhoa", khoa.TenKhoa);
                    p.AddWithValue("@OldMaKhoa", oldMaKhoa);
                });
        }

        public static void DeleteKhoa(string maKhoa)
        {
            Execute("DELETE FROM Khoa WHERE MaKhoa=@MaKhoa", p => p.AddWithValue("@MaKhoa", maKhoa));
        }

        public static void AddLop(Lop lop)
        {
            Execute("INSERT INTO Lop(MaLop, TenLop, MaKhoa) VALUES(@MaLop, @TenLop, @MaKhoa)",
                p =>
                {
                    p.AddWithValue("@MaLop", lop.MaLop);
                    p.AddWithValue("@TenLop", lop.TenLop);
                    p.AddWithValue("@MaKhoa", lop.MaKhoa);
                });
        }

        public static void UpdateLop(Lop lop)
        {
            Execute("UPDATE Lop SET TenLop=@TenLop, MaKhoa=@MaKhoa WHERE MaLop=@MaLop",
                p =>
                {
                    p.AddWithValue("@MaLop", lop.MaLop);
                    p.AddWithValue("@TenLop", lop.TenLop);
                    p.AddWithValue("@MaKhoa", lop.MaKhoa);
                });
        }

        public static void DeleteLop(string maLop)
        {
            Execute("DELETE FROM Lop WHERE MaLop=@MaLop", p => p.AddWithValue("@MaLop", maLop));
        }

        public static void AddMonHoc(MonHoc monHoc)
        {
            Execute("INSERT INTO MonHoc(MaMon, TenMon, SoTinChi) VALUES(@MaMon, @TenMon, @SoTinChi)",
                p =>
                {
                    p.AddWithValue("@MaMon", monHoc.MaMon);
                    p.AddWithValue("@TenMon", monHoc.TenMon);
                    p.AddWithValue("@SoTinChi", monHoc.SoTinChi);
                });
        }

        public static void UpdateMonHoc(string oldMaMon, MonHoc monHoc)
        {
            Execute("UPDATE MonHoc SET MaMon=@MaMon, TenMon=@TenMon, SoTinChi=@SoTinChi WHERE MaMon=@OldMaMon",
                p =>
                {
                    p.AddWithValue("@MaMon", monHoc.MaMon);
                    p.AddWithValue("@TenMon", monHoc.TenMon);
                    p.AddWithValue("@SoTinChi", monHoc.SoTinChi);
                    p.AddWithValue("@OldMaMon", oldMaMon);
                });
        }

        public static void DeleteMonHoc(string maMon)
        {
            Execute("DELETE FROM MonHoc WHERE MaMon=@MaMon", p => p.AddWithValue("@MaMon", maMon));
        }

        public static void AddSinhVien(SinhVien sinhVien)
        {
            Execute("INSERT INTO SinhVien(MaSV, HoTen, MaLop, Tuoi) VALUES(@MaSV, @HoTen, @MaLop, @Tuoi)",
                p =>
                {
                    p.AddWithValue("@MaSV", sinhVien.MaSV);
                    p.AddWithValue("@HoTen", sinhVien.HoTen);
                    p.AddWithValue("@MaLop", sinhVien.MaLop);
                    p.AddWithValue("@Tuoi", sinhVien.Tuoi);
                });
        }

        public static void UpdateSinhVien(string oldMaSV, SinhVien sinhVien)
        {
            Execute("UPDATE SinhVien SET MaSV=@MaSV, HoTen=@HoTen, MaLop=@MaLop, Tuoi=@Tuoi WHERE MaSV=@OldMaSV",
                p =>
                {
                    p.AddWithValue("@MaSV", sinhVien.MaSV);
                    p.AddWithValue("@HoTen", sinhVien.HoTen);
                    p.AddWithValue("@MaLop", sinhVien.MaLop);
                    p.AddWithValue("@Tuoi", sinhVien.Tuoi);
                    p.AddWithValue("@OldMaSV", oldMaSV);
                });
        }

        public static void DeleteSinhVien(string maSV)
        {
            Execute("DELETE FROM SinhVien WHERE MaSV=@MaSV", p => p.AddWithValue("@MaSV", maSV));
        }

        private static SqlConnection OpenConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        private static void Execute(string sql, Action<SqlParameterCollection> addParameters)
        {
            using var connection = OpenConnection();
            using var command = new SqlCommand(sql, connection);
            addParameters(command.Parameters);
            command.ExecuteNonQuery();
        }
    }
}
