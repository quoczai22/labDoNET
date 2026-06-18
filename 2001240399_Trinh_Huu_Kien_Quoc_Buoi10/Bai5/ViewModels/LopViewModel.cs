using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using Lab11_ValidationNavigation.Data;
using Lab11_ValidationNavigation.Models;

namespace Lab11_ValidationNavigation.ViewModels
{
    public class LopViewModel : BaseViewModel
    {
        private Lop _selectedLop;
        private string _maLop;
        private string _tenLop;
        private string _maKhoa;
        private bool _isEditing;
        private bool _isAdding;

        private ObservableCollection<Lop> _dsLop;
        private ObservableCollection<Khoa> _dsKhoa;

        public ObservableCollection<Lop> DS_Lop { get { return _dsLop; } set { _dsLop = value; OnPropertyChanged(); } }
        public ObservableCollection<Khoa> DS_Khoa { get { return _dsKhoa; } set { _dsKhoa = value; OnPropertyChanged(); } }

        public Lop SelectedLop
        {
            get { return _selectedLop; }
            set
            {
                _selectedLop = value;
                OnPropertyChanged();
                if (value != null && !IsAdding)
                {
                    MaLop = value.MaLop;
                    TenLop = value.TenLop;
                    MaKhoa = value.MaKhoa;
                }
            }
        }

        public string MaLop { get { return _maLop; } set { _maLop = value; OnPropertyChanged(); } }
        public string TenLop { get { return _tenLop; } set { _tenLop = value; OnPropertyChanged(); } }
        public string MaKhoa { get { return _maKhoa; } set { _maKhoa = value; OnPropertyChanged(); } }
        public bool IsEditing { get { return _isEditing; } set { _isEditing = value; OnPropertyChanged(); } }
        public bool IsAdding { get { return _isAdding; } set { _isAdding = value; OnPropertyChanged(); } }
        public bool IsSaving => IsAdding || IsEditing;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DeleteCommand { get; }

        public LopViewModel()
        {
            AddCommand = new RelayCommand(o => BeginAdd());
            EditCommand = new RelayCommand(o => BeginEdit(), o => SelectedLop != null);
            SaveCommand = new RelayCommand(o => Save(), o => IsSaving);
            CancelCommand = new RelayCommand(o => Cancel(), o => IsSaving);
            DeleteCommand = new RelayCommand(o => Delete(), o => SelectedLop != null);
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                DS_Lop = SqlData.LoadLops();
                DS_Khoa = SqlData.LoadKhoas();
            }
            catch (SqlException ex)
            {
                DS_Lop = new ObservableCollection<Lop>();
                DS_Khoa = new ObservableCollection<Khoa>();
                MessageBox.Show("Chua ket noi duoc CSDL QLSinhVien_Buoi11. Hay chay file CSDL_Buoi11.sql truoc.\n" + ex.Message);
            }
            SelectedLop = DS_Lop.FirstOrDefault();
        }

        private void BeginAdd()
        {
            IsAdding = true;
            IsEditing = false;
            MaLop = "";
            TenLop = "";
            MaKhoa = DS_Khoa.FirstOrDefault()?.MaKhoa;
            OnPropertyChanged(nameof(IsSaving));
        }

        private void BeginEdit()
        {
            IsEditing = true;
            IsAdding = false;
            OnPropertyChanged(nameof(IsSaving));
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(MaLop) || string.IsNullOrWhiteSpace(TenLop) || string.IsNullOrWhiteSpace(MaKhoa))
            {
                MessageBox.Show("Ma lop, ten lop va khoa khong duoc de trong.");
                return;
            }

            if (IsAdding)
            {
                if (SqlData.Exists("Lop", "MaLop", MaLop))
                {
                    MessageBox.Show("Ma lop da ton tai.");
                    return;
                }

                SqlData.AddLop(new Lop { MaLop = MaLop, TenLop = TenLop, MaKhoa = MaKhoa });
                LoadData();
                SelectedLop = DS_Lop.FirstOrDefault(l => l.MaLop == MaLop);
            }
            else if (IsEditing && SelectedLop != null)
            {
                SqlData.UpdateLop(new Lop { MaLop = MaLop, TenLop = TenLop, MaKhoa = MaKhoa });
                LoadData();
                SelectedLop = DS_Lop.FirstOrDefault(l => l.MaLop == MaLop);
            }

            IsAdding = false;
            IsEditing = false;
            OnPropertyChanged(nameof(IsSaving));
        }

        private void Cancel()
        {
            IsAdding = false;
            IsEditing = false;
            OnPropertyChanged(nameof(IsSaving));
            SelectedLop = SelectedLop ?? DS_Lop.FirstOrDefault();
        }

        private void Delete()
        {
            if (SqlData.Exists("SinhVien", "MaLop", SelectedLop.MaLop))
            {
                MessageBox.Show("Khong the xoa lop dang co sinh vien.");
                return;
            }

            SqlData.DeleteLop(SelectedLop.MaLop);
            LoadData();
        }
    }
}
