namespace Lab11_ValidationNavigation.Models
{
    public class Khoa
    {
        public string MaKhoa { get; set; }
        public string TenKhoa { get; set; }
    }

    public class Lop
    {
        public string MaLop { get; set; }
        public string TenLop { get; set; }
        public string MaKhoa { get; set; }
    }

    public class MonHoc
    {
        public string MaMon { get; set; }
        public string TenMon { get; set; }
        public int SoTinChi { get; set; }
    }

    public class SinhVien
    {
        public string MaSV { get; set; }
        public string HoTen { get; set; }
        public string MaLop { get; set; }
        public int Tuoi { get; set; }
    }
}
