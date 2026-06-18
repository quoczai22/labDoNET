using System.ComponentModel;
using Lab11_ValidationNavigation.Data;

namespace Lab11_ValidationNavigation.ViewModels
{
    public class KhoaInputViewModel : BaseViewModel, IDataErrorInfo
    {
        private string _maKhoa;
        private string _tenKhoa;

        public bool IsEdit { get; set; }
        public string OldMaKhoa { get; set; }

        public string MaKhoa
        {
            get { return _maKhoa; }
            set { _maKhoa = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsValid)); }
        }

        public string TenKhoa
        {
            get { return _tenKhoa; }
            set { _tenKhoa = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsValid)); }
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(MaKhoa))
                {
                    if (string.IsNullOrWhiteSpace(MaKhoa)) return "Ma khoa khong duoc de trong";
                    if (MaKhoa.Length > 5) return "Ma khoa toi da 5 ky tu";
                    if (!IsEdit && SqlData.Exists("Khoa", "MaKhoa", MaKhoa)) return "Ma khoa da ton tai";
                }

                if (columnName == nameof(TenKhoa))
                {
                    if (string.IsNullOrWhiteSpace(TenKhoa)) return "Ten khoa khong duoc de trong";
                    if (TenKhoa.Length > 50) return "Ten khoa toi da 50 ky tu";
                }

                return null;
            }
        }

        public bool IsValid =>
            string.IsNullOrEmpty(this[nameof(MaKhoa)]) &&
            string.IsNullOrEmpty(this[nameof(TenKhoa)]);
    }

    public class MonHocInputViewModel : BaseViewModel, IDataErrorInfo
    {
        private string _maMon;
        private string _tenMon;
        private string _soTinChi;

        public bool IsEdit { get; set; }
        public string OldMaMon { get; set; }

        public string MaMon
        {
            get { return _maMon; }
            set { _maMon = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsValid)); }
        }

        public string TenMon
        {
            get { return _tenMon; }
            set { _tenMon = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsValid)); }
        }

        public string SoTinChi
        {
            get { return _soTinChi; }
            set { _soTinChi = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsValid)); }
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(MaMon))
                {
                    if (string.IsNullOrWhiteSpace(MaMon)) return "Ma mon khong duoc de trong";
                    if (MaMon.Length > 10) return "Ma mon toi da 10 ky tu";
                    if (!IsEdit && SqlData.Exists("MonHoc", "MaMon", MaMon)) return "Ma mon da ton tai";
                }

                if (columnName == nameof(TenMon))
                {
                    if (string.IsNullOrWhiteSpace(TenMon)) return "Ten mon khong duoc de trong";
                    if (TenMon.Length > 50) return "Ten mon toi da 50 ky tu";
                    if (SqlData.Exists("MonHoc", "TenMon", TenMon, IsEdit ? "MaMon" : null, OldMaMon)) return "Ten mon phai duy nhat";
                }

                if (columnName == nameof(SoTinChi))
                {
                    if (string.IsNullOrWhiteSpace(SoTinChi)) return "So tin chi khong duoc de trong";
                    if (!int.TryParse(SoTinChi, out int value)) return "So tin chi phai la so nguyen";
                    if (value < 1 || value > 10) return "So tin chi tu 1 den 10";
                }

                return null;
            }
        }

        public bool IsValid =>
            string.IsNullOrEmpty(this[nameof(MaMon)]) &&
            string.IsNullOrEmpty(this[nameof(TenMon)]) &&
            string.IsNullOrEmpty(this[nameof(SoTinChi)]);
    }

    public class SinhVienInputViewModel : BaseViewModel, IDataErrorInfo
    {
        private string _maSV;
        private string _hoTen;
        private string _maLop;
        private string _tuoi;

        public bool IsEdit { get; set; }
        public string OldMaSV { get; set; }

        public string MaSV
        {
            get { return _maSV; }
            set { _maSV = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsValid)); }
        }

        public string HoTen
        {
            get { return _hoTen; }
            set { _hoTen = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsValid)); }
        }

        public string MaLop
        {
            get { return _maLop; }
            set { _maLop = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsValid)); }
        }

        public string Tuoi
        {
            get { return _tuoi; }
            set { _tuoi = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsValid)); }
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(MaSV))
                {
                    if (string.IsNullOrWhiteSpace(MaSV)) return "Ma sinh vien khong duoc de trong";
                    if (MaSV.Length > 10) return "Ma sinh vien toi da 10 ky tu";
                    if (!IsEdit && SqlData.Exists("SinhVien", "MaSV", MaSV)) return "Ma sinh vien da ton tai";
                }

                if (columnName == nameof(HoTen))
                {
                    if (string.IsNullOrWhiteSpace(HoTen)) return "Ho ten khong duoc de trong";
                    if (HoTen.Length > 50) return "Ho ten toi da 50 ky tu";
                }

                if (columnName == nameof(MaLop) && string.IsNullOrWhiteSpace(MaLop)) return "Phai chon lop";

                if (columnName == nameof(Tuoi))
                {
                    if (string.IsNullOrWhiteSpace(Tuoi)) return "Tuoi khong duoc de trong";
                    if (!int.TryParse(Tuoi, out int value)) return "Tuoi phai la so nguyen";
                    if (value < 16 || value > 60) return "Tuoi tu 16 den 60";
                }

                return null;
            }
        }

        public bool IsValid =>
            string.IsNullOrEmpty(this[nameof(MaSV)]) &&
            string.IsNullOrEmpty(this[nameof(HoTen)]) &&
            string.IsNullOrEmpty(this[nameof(MaLop)]) &&
            string.IsNullOrEmpty(this[nameof(Tuoi)]);
    }
}
