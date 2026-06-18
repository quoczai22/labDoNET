using _2001240399_TrinhHuuKienQuoc_Buoi11.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace _2001240399_TrinhHuuKienQuoc_Buoi11.ViewModels
{
    public class KhoaViewModel : BaseViewModel, IDataErrorInfo
    {
        private QLSinhVien_Buoi11Entities db = new QLSinhVien_Buoi11Entities();
        private bool isAdding;
        private bool isEditing;

        public ObservableCollection<Khoa> DS_Khoa { get; set; }
        public RelayCommand ThemCommand { get; set; }
        public RelayCommand SuaCommand { get; set; }
        public RelayCommand XoaCommand { get; set; }
        public RelayCommand LuuCommand { get; set; }
        public RelayCommand HuyCommand { get; set; }

        private Khoa _SelectedKhoa;
        public Khoa SelectedKhoa
        {
            get => _SelectedKhoa;
            set
            {
                _SelectedKhoa = value;
                OnPropertyChanged(nameof(SelectedKhoa));
                if (SelectedKhoa != null && !isAdding && !isEditing)
                {
                    MaKhoa = SelectedKhoa.MaKhoa;
                    TenKhoa = SelectedKhoa.TenKhoa;
                }
            }
        }

        private string _MaKhoa;
        public string MaKhoa { get => _MaKhoa; set { _MaKhoa = value; OnPropertyChanged(nameof(MaKhoa)); RefreshValid(); } }
        private string _TenKhoa;
        public string TenKhoa { get => _TenKhoa; set { _TenKhoa = value; OnPropertyChanged(nameof(TenKhoa)); RefreshValid(); } }

        public bool IsValid => string.IsNullOrEmpty(this[nameof(MaKhoa)]) && string.IsNullOrEmpty(this[nameof(TenKhoa)]);
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(MaKhoa))
                {
                    if (string.IsNullOrWhiteSpace(MaKhoa)) return "Mã khoa không được để trống";
                    if (MaKhoa.Trim().Length > 5) return "Mã khoa tối đa 5 ký tự";
                    if (isAdding && db.Khoas.Any(k => k.MaKhoa == MaKhoa.Trim())) return "Mã khoa đã tồn tại";
                }
                if (columnName == nameof(TenKhoa))
                {
                    if (string.IsNullOrWhiteSpace(TenKhoa)) return "Tên khoa không được để trống";
                    if (TenKhoa.Trim().Length > 50) return "Tên khoa tối đa 50 ký tự";
                }
                return null;
            }
        }

        public KhoaViewModel()
        {
            ThemCommand = new RelayCommand(o => ExecuteThem());
            SuaCommand = new RelayCommand(o => ExecuteSua(), o => SelectedKhoa != null && !isAdding && !isEditing);
            XoaCommand = new RelayCommand(o => ExecuteXoa(), o => SelectedKhoa != null && !isAdding && !isEditing);
            LuuCommand = new RelayCommand(o => ExecuteLuu(), o => (isAdding || isEditing) && IsValid);
            HuyCommand = new RelayCommand(o => ExecuteHuy(), o => isAdding || isEditing);
            LoadData();
        }

        private void RefreshValid()
        {
            OnPropertyChanged(nameof(IsValid));
            CommandManager.InvalidateRequerySuggested();
        }

        private void LoadData()
        {
            DS_Khoa = new ObservableCollection<Khoa>(db.Khoas.ToList());
            OnPropertyChanged(nameof(DS_Khoa));
        }

        private void ExecuteThem()
        {
            isAdding = true;
            isEditing = false;
            ClearForm();
        }

        private void ExecuteSua()
        {
            if (SelectedKhoa == null) return;
            isAdding = false;
            isEditing = true;
            RefreshValid();
        }

        private void ExecuteXoa()
        {
            if (SelectedKhoa == null) return;
            if (MessageBox.Show("Bạn chắc chắn muốn xóa khoa này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            try
            {
                var khoaDelete = db.Khoas.Find(SelectedKhoa.MaKhoa);
                if (khoaDelete != null)
                {
                    db.Khoas.Remove(khoaDelete);
                    db.SaveChanges();
                    LoadData();
                    ClearForm();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message);
            }
        }

        private void ExecuteLuu()
        {
            if (!IsValid) return;
            try
            {
                if (isAdding)
                {
                    db.Khoas.Add(new Khoa { MaKhoa = MaKhoa.Trim(), TenKhoa = TenKhoa.Trim() });
                }
                else if (isEditing && SelectedKhoa != null)
                {
                    var khoaUp = db.Khoas.Find(SelectedKhoa.MaKhoa);
                    if (khoaUp != null) khoaUp.TenKhoa = TenKhoa.Trim();
                }
                db.SaveChanges();
                MessageBox.Show("Lưu dữ liệu khoa thành công!");
                isAdding = false;
                isEditing = false;
                LoadData();
                ClearForm();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }

        private void ExecuteHuy()
        {
            isAdding = false;
            isEditing = false;
            SelectedKhoa = null;
            ClearForm();
        }

        private void ClearForm()
        {
            MaKhoa = string.Empty;
            TenKhoa = string.Empty;
        }
    }
}
