using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using Lab11_ValidationNavigation.Data;
using Lab11_ValidationNavigation.Models;

namespace Lab11_ValidationNavigation.ViewModels
{
    public class KhoaViewModel : BaseViewModel
    {
        private Khoa _selectedKhoa;

        private ObservableCollection<Khoa> _dsKhoa;

        public ObservableCollection<Khoa> DS_Khoa
        {
            get { return _dsKhoa; }
            set { _dsKhoa = value; OnPropertyChanged(); }
        }
        public KhoaInputViewModel NewKhoa { get; } = new KhoaInputViewModel();

        public Khoa SelectedKhoa
        {
            get { return _selectedKhoa; }
            set
            {
                _selectedKhoa = value;
                OnPropertyChanged();
                if (value != null)
                {
                    NewKhoa.IsEdit = true;
                    NewKhoa.OldMaKhoa = value.MaKhoa;
                    NewKhoa.MaKhoa = value.MaKhoa;
                    NewKhoa.TenKhoa = value.TenKhoa;
                }
            }
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        public KhoaViewModel()
        {
            AddCommand = new RelayCommand(o => Add());
            UpdateCommand = new RelayCommand(o => Update(), o => SelectedKhoa != null);
            DeleteCommand = new RelayCommand(o => Delete(), o => SelectedKhoa != null);
            ClearCommand = new RelayCommand(o => Clear());
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                DS_Khoa = SqlData.LoadKhoas();
            }
            catch (SqlException ex)
            {
                DS_Khoa = new ObservableCollection<Khoa>();
                MessageBox.Show("Chua ket noi duoc CSDL QLSinhVien_Buoi11. Hay chay file CSDL_Buoi11.sql truoc.\n" + ex.Message);
            }
            SelectedKhoa = DS_Khoa.FirstOrDefault();
        }

        private void Add()
        {
            NewKhoa.IsEdit = false;
            if (!NewKhoa.IsValid)
            {
                MessageBox.Show("Du lieu khong hop le. Vui long kiem tra lai.");
                return;
            }

            SqlData.AddKhoa(new Khoa { MaKhoa = NewKhoa.MaKhoa, TenKhoa = NewKhoa.TenKhoa });
            LoadData();
            SelectedKhoa = DS_Khoa.FirstOrDefault(k => k.MaKhoa == NewKhoa.MaKhoa);
            MessageBox.Show("Them khoa thanh cong!");
        }

        private void Update()
        {
            NewKhoa.IsEdit = true;
            NewKhoa.OldMaKhoa = SelectedKhoa.MaKhoa;
            if (!NewKhoa.IsValid)
            {
                MessageBox.Show("Du lieu cap nhat khong hop le.");
                return;
            }

            if (NewKhoa.MaKhoa != SelectedKhoa.MaKhoa &&
                SqlData.Exists("Khoa", "MaKhoa", NewKhoa.MaKhoa))
            {
                MessageBox.Show("Ma khoa da ton tai!");
                return;
            }

            var oldMaKhoa = SelectedKhoa.MaKhoa;
            SqlData.UpdateKhoa(oldMaKhoa, new Khoa { MaKhoa = NewKhoa.MaKhoa, TenKhoa = NewKhoa.TenKhoa });
            LoadData();
            SelectedKhoa = DS_Khoa.FirstOrDefault(k => k.MaKhoa == NewKhoa.MaKhoa);
            MessageBox.Show("Cap nhat thanh cong!");
        }

        private void Delete()
        {
            if (SqlData.Exists("Lop", "MaKhoa", SelectedKhoa.MaKhoa))
            {
                MessageBox.Show("Khong the xoa khoa dang co lop.");
                return;
            }

            SqlData.DeleteKhoa(SelectedKhoa.MaKhoa);
            LoadData();
        }

        private void Clear()
        {
            NewKhoa.IsEdit = false;
            NewKhoa.OldMaKhoa = null;
            NewKhoa.MaKhoa = "";
            NewKhoa.TenKhoa = "";
        }
    }
}
