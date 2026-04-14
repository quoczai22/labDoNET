using _2001240399_Trinh_Huu_Kien_Quoc_Buoi10.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi10.ViewModels
{
    public class KhoaViewModel : BaseViewModel
    {
        QLSinhVienEntities1 db = new QLSinhVienEntities1();

        public ObservableCollection<Khoa> DS_Khoa { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand UpdateCommand { get; set; }

        public Khoa NewKhoa { get; set; }

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
                    NewKhoa = new Khoa()
                    {
                        MaKhoa = _SelectedKhoa.MaKhoa,
                        TenKhoa = _SelectedKhoa.TenKhoa
                    };
                    OnPropertyChanged(nameof(NewKhoa));
                }
            }
        }

        public KhoaViewModel()
        {
            NewKhoa = new Khoa();

            AddCommand = new RelayCommand(o => Add());
            DeleteCommand = new RelayCommand(o => Delete(), o => SelectedKhoa != null);
            UpdateCommand = new RelayCommand(o => Update(), o => SelectedKhoa != null);

            LoadData();
        }

        void LoadData()
        {
            DS_Khoa = new ObservableCollection<Khoa>(db.Khoas.ToList());
            OnPropertyChanged(nameof(DS_Khoa));
            SelectedKhoa = DS_Khoa.FirstOrDefault();
        }

        private void Add()
        {
            var newKhoa = new Khoa
            {
                MaKhoa = NewKhoa.MaKhoa,
                TenKhoa = NewKhoa.TenKhoa
            };

            DS_Khoa.Add(newKhoa);
            db.Khoas.Add(newKhoa);
            SelectedKhoa = newKhoa;

            db.SaveChanges();

            NewKhoa.MaKhoa = "";
            NewKhoa.TenKhoa = "";
            OnPropertyChanged(nameof(NewKhoa));
        }

        private void Delete()
        {
            if (SelectedKhoa == null)
                return;

            if (MessageBox.Show("Bạn chắc chắn muốn xóa khoa này?",
                                "Xác nhận",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            db.Khoas.Remove(SelectedKhoa);
            DS_Khoa.Remove(SelectedKhoa);
            db.SaveChanges();
            SelectedKhoa = DS_Khoa.FirstOrDefault();
        }

        private void Update()
        {
            try
            {
                if (SelectedKhoa == null) return;

                if (SelectedKhoa.MaKhoa != NewKhoa.MaKhoa)
                {
                    MessageBox.Show("Không được phép thay đổi Mã khoa (Khóa chính). Vui lòng chỉ cập nhật Tên khoa.");
                    NewKhoa.MaKhoa = SelectedKhoa.MaKhoa;
                    OnPropertyChanged(nameof(NewKhoa));
                    return;
                }
                // GÁN NGƯỢC: NewKhoa -> SelectedKhoa
                SelectedKhoa.MaKhoa = NewKhoa.MaKhoa;
                SelectedKhoa.TenKhoa = NewKhoa.TenKhoa;
                db.SaveChanges();
                LoadData();
                MessageBox.Show("Cập nhật thành công!");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
            }
        }
    }
}