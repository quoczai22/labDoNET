using _2001240399_Trinh_Huu_Kien_Quoc_Buoi10.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi10.ViewModels
{
    public class KhoaViewModel : BaseViewModel
    {
        QL_KhoaEntities db = new QL_KhoaEntities();

        public ObservableCollection<Khoa> DS_Khoa { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand UpdateCommand { get; set; }

        private Khoa _SelectedKhoa;
        public Khoa SelectedKhoa
        {
            get => _SelectedKhoa;
            set
            {
                _SelectedKhoa = value;
                OnPropertyChanged(nameof(SelectedKhoa));
                if (SelectedKhoa != null)
                {
                    MaKhoa = SelectedKhoa.MaKhoa;
                    TenKhoa = SelectedKhoa.TenKhoa;
                }
            }
        }

        private string _MaKhoa;
        public string MaKhoa
            {
            get => _MaKhoa;
            set
            {
                _MaKhoa = value;
                OnPropertyChanged(nameof(MaKhoa));
            }
        }
        
        private string _TenKhoa;
        public string TenKhoa
        {
            get => _TenKhoa;
            set
            {
                _TenKhoa = value;
                OnPropertyChanged(nameof(TenKhoa));
            }
        }

        public KhoaViewModel()
        {

            AddCommand = new RelayCommand(o => Add());
            DeleteCommand = new RelayCommand(o => Delete(), o => SelectedKhoa != null);
            UpdateCommand = new RelayCommand(o => Update(), o => SelectedKhoa != null);
            LoadData();
        }

        void LoadData()
        {
            DS_Khoa = new ObservableCollection<Khoa>(db.Khoas.ToList());
            OnPropertyChanged(nameof(DS_Khoa));
        }

        private void Add()
        {
            if (string.IsNullOrWhiteSpace(MaKhoa) || string.IsNullOrWhiteSpace(TenKhoa)) return;
            var newKhoa = new Khoa
            {
                MaKhoa = MaKhoa,
                TenKhoa = TenKhoa,
            };
            db.Khoas.Add(newKhoa);
            db.SaveChanges();
            LoadData();
            MaKhoa=string.Empty;
            TenKhoa=string.Empty;
        }

        private void Delete()
        {
            if (SelectedKhoa == null) return;
            var KhoaDelete=db.Khoas.Find(SelectedKhoa.MaKhoa);
            if (KhoaDelete != null)
            {
                if (MessageBox.Show("Bạn chắc chắn muốn xóa khoa này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
                try
                {
                    db.Khoas.Remove(KhoaDelete);
                    db.SaveChanges();
                    LoadData();
                    MaKhoa = string.Empty;
                    TenKhoa = string.Empty;
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message);
                }
            }
        }
        private void Update()
        {
            try
            {
                if (SelectedKhoa == null) return;
                if (SelectedKhoa == null || string.IsNullOrEmpty(TenKhoa)|| string.IsNullOrEmpty(MaKhoa)) return;
                var khoaUp = db.Khoas.Find(SelectedKhoa.MaKhoa);
                if (khoaUp != null)
                {
                    khoaUp.MaKhoa = MaKhoa;
                    khoaUp.TenKhoa = TenKhoa;
                    db.SaveChanges();
                    LoadData();
                    MaKhoa = string.Empty;
                    TenKhoa=string.Empty;
                    MessageBox.Show("Cập nhật thành công!");
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
            }
        }
    }
}