using Bai4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.ObjectModel;

namespace Bai4.ViewModels
{
    public class DanhSachSinhVienViewModel : BaseViewModel
    {
        QLSinhVienEntities db = new QLSinhVienEntities();

        public ObservableCollection<SinhVien> DS_SinhVien { get; set; }
        public ObservableCollection<Lop> DS_Lop { get; set; }
        public List<string> DS_GioiTinh { get; set; }

        SinhVien _selectedSinhVien;
        public SinhVien SelectedSinhVien
        {
            get { return _selectedSinhVien; }
            set
            {
                _selectedSinhVien = value;
                OnPropertyChanged(nameof(SelectedSinhVien));
                if (SelectedSinhVien != null)
                {
                    MaSV = SelectedSinhVien.MaSinhVien;
                    TenSV = SelectedSinhVien.HoTen;
                    NgaySinh = SelectedSinhVien.NgaySinh;
                    GioiTinh = SelectedSinhVien.GioiTinh;
                    SelectedMaLop = SelectedSinhVien.MaLop;
                }
            }
        }

        string _maSV;
        public string MaSV { get { return _maSV; } set { _maSV = value; OnPropertyChanged(nameof(MaSV)); } }
        string _tenSV;
        public string TenSV { get { return _tenSV; } set { _tenSV = value; OnPropertyChanged(nameof(TenSV)); } }
        DateTime? _ngaySinh;
        public DateTime? NgaySinh { get { return _ngaySinh; } set { _ngaySinh = value; OnPropertyChanged(nameof(NgaySinh)); } }
        string _gioiTinh;
        public string GioiTinh { get { return _gioiTinh; } set { _gioiTinh = value; OnPropertyChanged(nameof(GioiTinh)); } }
        string _selectedMaLop;
        public string SelectedMaLop { get { return _selectedMaLop; } set { _selectedMaLop = value; OnPropertyChanged(nameof(SelectedMaLop)); } }

        public RelayCommand AddCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        

        public DanhSachSinhVienViewModel()
        {
            LoadData();
            AddCommand = new RelayCommand(o => PrepareAdd());
            EditCommand = new RelayCommand(o => PrepareEdit(), o => SelectedSinhVien != null);
            DeleteCommand = new RelayCommand(o => Delete(), o => SelectedSinhVien != null);
            SaveCommand = new RelayCommand(o => Save());
            CancelCommand = new RelayCommand(o => Cancel());
        }

        void LoadData()
        {
            DS_SinhVien = new ObservableCollection<SinhVien>(db.SinhViens.ToList());
            OnPropertyChanged(nameof(DS_SinhVien));
            DS_Lop = new ObservableCollection<Lop>(db.Lops.ToList());
            OnPropertyChanged(nameof(DS_Lop));
            DS_GioiTinh = new List<string> { "Nam", "Nữ" };
            OnPropertyChanged(nameof(DS_GioiTinh));
        }

        void PrepareAdd()
        {
            SelectedSinhVien = null;
            MaSV = string.Empty;
            TenSV = string.Empty;
            NgaySinh = null;
            GioiTinh = null;
            SelectedMaLop = null;
        }

        void PrepareEdit()
        {
            if (SelectedSinhVien != null)
            {
                MaSV = SelectedSinhVien.MaSinhVien;
                TenSV = SelectedSinhVien.HoTen;
                NgaySinh = SelectedSinhVien.NgaySinh;
                GioiTinh = SelectedSinhVien.GioiTinh;
                SelectedMaLop = SelectedSinhVien.MaLop;
            }
        }

        void Delete()
        {
            if (SelectedSinhVien != null)
            {
                db.SinhViens.Remove(SelectedSinhVien);
                db.SaveChanges();
                LoadData();
            }
        }

        void Save()
        {
            if (SelectedSinhVien == null)
            {
                var newSinhVien = new SinhVien
                {
                    MaSinhVien = MaSV,
                    HoTen = TenSV,
                    NgaySinh = NgaySinh,
                    GioiTinh = GioiTinh,
                    MaLop = SelectedMaLop
                };
                db.SinhViens.Add(newSinhVien);
            }
            else
            {
                var existingSinhVien = db.SinhViens.Find(SelectedSinhVien.MaSinhVien);
                if (existingSinhVien != null)
                {
                    existingSinhVien.HoTen = TenSV;
                    existingSinhVien.NgaySinh = NgaySinh;
                    existingSinhVien.GioiTinh = GioiTinh;
                    existingSinhVien.MaLop = SelectedMaLop;
                }
            }
            db.SaveChanges();
            LoadData();
        }

        void Cancel()
        {
            if (SelectedSinhVien != null)
            {
                MaSV = SelectedSinhVien.MaSinhVien;
                TenSV = SelectedSinhVien.HoTen;
                NgaySinh = SelectedSinhVien.NgaySinh;
                GioiTinh = SelectedSinhVien.GioiTinh;
                SelectedMaLop = SelectedSinhVien.MaLop;
            }
            else
            {
                MaSV = string.Empty;
                TenSV = string.Empty;
                NgaySinh = null;
                GioiTinh = null;
                SelectedMaLop = null;
            }
        }
    }
}
