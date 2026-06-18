using System.Collections.ObjectModel;

namespace Lab11_ValidationNavigation.Models
{
    public static class LabData
    {
        public static ObservableCollection<Khoa> Khoas { get; } = new ObservableCollection<Khoa>
        {
            new Khoa { MaKhoa = "CNTT", TenKhoa = "Cong nghe thong tin" },
            new Khoa { MaKhoa = "QTKD", TenKhoa = "Quan tri kinh doanh" }
        };

        public static ObservableCollection<Lop> Lops { get; } = new ObservableCollection<Lop>
        {
            new Lop { MaLop = "DHTH01", TenLop = "Dai hoc Tin hoc 01", MaKhoa = "CNTT" },
            new Lop { MaLop = "DHTH02", TenLop = "Dai hoc Tin hoc 02", MaKhoa = "CNTT" }
        };

        public static ObservableCollection<MonHoc> MonHocs { get; } = new ObservableCollection<MonHoc>
        {
            new MonHoc { MaMon = "NET", TenMon = "Cong nghe .NET", SoTinChi = 3 },
            new MonHoc { MaMon = "CSDL", TenMon = "Co so du lieu", SoTinChi = 3 }
        };

        public static ObservableCollection<SinhVien> SinhViens { get; } = new ObservableCollection<SinhVien>
        {
            new SinhVien { MaSV = "SV01", HoTen = "Nguyen Van A", MaLop = "DHTH01", Tuoi = 20 },
            new SinhVien { MaSV = "SV02", HoTen = "Tran Thi B", MaLop = "DHTH02", Tuoi = 21 }
        };
    }
}
