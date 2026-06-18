using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using Lab11_ValidationNavigation.Data;
using Lab11_ValidationNavigation.Models;

namespace Lab11_ValidationNavigation.ViewModels
{
    public class MonHocViewModel : BaseViewModel
    {
        private MonHoc _selectedMonHoc;

        private ObservableCollection<MonHoc> _dsMonHoc;

        public ObservableCollection<MonHoc> DS_MonHoc
        {
            get { return _dsMonHoc; }
            set { _dsMonHoc = value; OnPropertyChanged(); }
        }
        public MonHocInputViewModel NewMonHoc { get; } = new MonHocInputViewModel();

        public MonHoc SelectedMonHoc
        {
            get { return _selectedMonHoc; }
            set
            {
                _selectedMonHoc = value;
                OnPropertyChanged();
                if (value != null)
                {
                    NewMonHoc.IsEdit = true;
                    NewMonHoc.OldMaMon = value.MaMon;
                    NewMonHoc.MaMon = value.MaMon;
                    NewMonHoc.TenMon = value.TenMon;
                    NewMonHoc.SoTinChi = value.SoTinChi.ToString();
                }
            }
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        public MonHocViewModel()
        {
            AddCommand = new RelayCommand(o => Add());
            UpdateCommand = new RelayCommand(o => Update(), o => SelectedMonHoc != null);
            DeleteCommand = new RelayCommand(o => Delete(), o => SelectedMonHoc != null);
            ClearCommand = new RelayCommand(o => Clear());
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                DS_MonHoc = SqlData.LoadMonHocs();
            }
            catch (SqlException ex)
            {
                DS_MonHoc = new ObservableCollection<MonHoc>();
                MessageBox.Show("Chua ket noi duoc CSDL QLSinhVien_Buoi11. Hay chay file CSDL_Buoi11.sql truoc.\n" + ex.Message);
            }
            SelectedMonHoc = DS_MonHoc.FirstOrDefault();
        }

        private void Add()
        {
            NewMonHoc.IsEdit = false;
            if (!NewMonHoc.IsValid)
            {
                MessageBox.Show("Du lieu mon hoc khong hop le.");
                return;
            }

            SqlData.AddMonHoc(new MonHoc { MaMon = NewMonHoc.MaMon, TenMon = NewMonHoc.TenMon, SoTinChi = int.Parse(NewMonHoc.SoTinChi) });
            LoadData();
            SelectedMonHoc = DS_MonHoc.FirstOrDefault(m => m.MaMon == NewMonHoc.MaMon);
        }

        private void Update()
        {
            NewMonHoc.IsEdit = true;
            NewMonHoc.OldMaMon = SelectedMonHoc.MaMon;
            if (!NewMonHoc.IsValid)
            {
                MessageBox.Show("Du lieu cap nhat mon hoc khong hop le.");
                return;
            }

            var oldMaMon = SelectedMonHoc.MaMon;
            SqlData.UpdateMonHoc(oldMaMon, new MonHoc { MaMon = NewMonHoc.MaMon, TenMon = NewMonHoc.TenMon, SoTinChi = int.Parse(NewMonHoc.SoTinChi) });
            LoadData();
            SelectedMonHoc = DS_MonHoc.FirstOrDefault(m => m.MaMon == NewMonHoc.MaMon);
        }

        private void Delete()
        {
            SqlData.DeleteMonHoc(SelectedMonHoc.MaMon);
            LoadData();
        }

        private void Clear()
        {
            NewMonHoc.IsEdit = false;
            NewMonHoc.OldMaMon = null;
            NewMonHoc.MaMon = "";
            NewMonHoc.TenMon = "";
            NewMonHoc.SoTinChi = "";
        }
    }
}
