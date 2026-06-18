using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using Lab11_ValidationNavigation.Data;
using Lab11_ValidationNavigation.Models;

namespace Lab11_ValidationNavigation.ViewModels
{
    public class SinhVienViewModel : BaseViewModel
    {
        private SinhVien _selectedSinhVien;

        private ObservableCollection<SinhVien> _dsSinhVien;
        private ObservableCollection<Lop> _dsLop;

        public ObservableCollection<SinhVien> DS_SinhVien { get { return _dsSinhVien; } set { _dsSinhVien = value; OnPropertyChanged(); } }
        public ObservableCollection<Lop> DS_Lop { get { return _dsLop; } set { _dsLop = value; OnPropertyChanged(); } }
        public SinhVienInputViewModel NewSinhVien { get; } = new SinhVienInputViewModel();

        public SinhVien SelectedSinhVien
        {
            get { return _selectedSinhVien; }
            set
            {
                _selectedSinhVien = value;
                OnPropertyChanged();
                if (value != null)
                {
                    NewSinhVien.IsEdit = true;
                    NewSinhVien.OldMaSV = value.MaSV;
                    NewSinhVien.MaSV = value.MaSV;
                    NewSinhVien.HoTen = value.HoTen;
                    NewSinhVien.MaLop = value.MaLop;
                    NewSinhVien.Tuoi = value.Tuoi.ToString();
                }
            }
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        public SinhVienViewModel()
        {
            AddCommand = new RelayCommand(o => Add());
            UpdateCommand = new RelayCommand(o => Update(), o => SelectedSinhVien != null);
            DeleteCommand = new RelayCommand(o => Delete(), o => SelectedSinhVien != null);
            ClearCommand = new RelayCommand(o => Clear());
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                DS_SinhVien = SqlData.LoadSinhViens();
                DS_Lop = SqlData.LoadLops();
            }
            catch (SqlException ex)
            {
                DS_SinhVien = new ObservableCollection<SinhVien>();
                DS_Lop = new ObservableCollection<Lop>();
                MessageBox.Show("Chua ket noi duoc CSDL QLSinhVien_Buoi11. Hay chay file CSDL_Buoi11.sql truoc.\n" + ex.Message);
            }
            SelectedSinhVien = DS_SinhVien.FirstOrDefault();
        }

        private void Add()
        {
            NewSinhVien.IsEdit = false;
            if (!NewSinhVien.IsValid)
            {
                MessageBox.Show("Du lieu sinh vien khong hop le.");
                return;
            }

            var sinhVien = new SinhVien
            {
                MaSV = NewSinhVien.MaSV,
                HoTen = NewSinhVien.HoTen,
                MaLop = NewSinhVien.MaLop,
                Tuoi = int.Parse(NewSinhVien.Tuoi)
            };
            SqlData.AddSinhVien(sinhVien);
            LoadData();
            SelectedSinhVien = DS_SinhVien.FirstOrDefault(s => s.MaSV == sinhVien.MaSV);
        }

        private void Update()
        {
            NewSinhVien.IsEdit = true;
            NewSinhVien.OldMaSV = SelectedSinhVien.MaSV;
            if (!NewSinhVien.IsValid)
            {
                MessageBox.Show("Du lieu cap nhat sinh vien khong hop le.");
                return;
            }

            var oldMaSV = SelectedSinhVien.MaSV;
            var sinhVien = new SinhVien
            {
                MaSV = NewSinhVien.MaSV,
                HoTen = NewSinhVien.HoTen,
                MaLop = NewSinhVien.MaLop,
                Tuoi = int.Parse(NewSinhVien.Tuoi)
            };
            SqlData.UpdateSinhVien(oldMaSV, sinhVien);
            LoadData();
            SelectedSinhVien = DS_SinhVien.FirstOrDefault(s => s.MaSV == sinhVien.MaSV);
        }

        private void Delete()
        {
            SqlData.DeleteSinhVien(SelectedSinhVien.MaSV);
            LoadData();
        }

        private void Clear()
        {
            NewSinhVien.IsEdit = false;
            NewSinhVien.OldMaSV = null;
            NewSinhVien.MaSV = "";
            NewSinhVien.HoTen = "";
            NewSinhVien.MaLop = DS_Lop.FirstOrDefault()?.MaLop;
            NewSinhVien.Tuoi = "";
        }
    }
}
