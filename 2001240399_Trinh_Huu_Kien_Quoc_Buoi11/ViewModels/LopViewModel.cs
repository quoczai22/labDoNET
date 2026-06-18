using _2001240399_TrinhHuuKienQuoc_Buoi11.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace _2001240399_TrinhHuuKienQuoc_Buoi11.ViewModels
{
    public class LopViewModel : BaseViewModel
    {
        private QLSinhVien_Buoi11Entities db = new QLSinhVien_Buoi11Entities();
        private bool isAdding;
        private bool isEditing;

        public ObservableCollection<Lop> DS_Lop { get; set; }
        public ObservableCollection<Khoa> DS_Khoa { get; set; }
        public RelayCommand ThemCommand { get; set; }
        public RelayCommand SuaCommand { get; set; }
        public RelayCommand XoaCommand { get; set; }
        public RelayCommand LuuCommand { get; set; }
        public RelayCommand HuyCommand { get; set; }

        private Lop _SelectedLop;
        public Lop SelectedLop
        {
            get => _SelectedLop;
            set
            {
                _SelectedLop = value;
                OnPropertyChanged(nameof(SelectedLop));
                if (SelectedLop != null && !isAdding && !isEditing)
                {
                    MaLop = SelectedLop.MaLop;
                    TenLop = SelectedLop.TenLop;
                    MaKhoa = SelectedLop.MaKhoa;
                }
            }
        }

        private string _MaLop;
        public string MaLop { get => _MaLop; set { _MaLop = value; OnPropertyChanged(nameof(MaLop)); } }
        private string _TenLop;
        public string TenLop { get => _TenLop; set { _TenLop = value; OnPropertyChanged(nameof(TenLop)); } }
        private string _MaKhoa;
        public string MaKhoa { get => _MaKhoa; set { _MaKhoa = value; OnPropertyChanged(nameof(MaKhoa)); } }

        public LopViewModel()
        {
            ThemCommand = new RelayCommand(o => ExecuteThem());
            SuaCommand = new RelayCommand(o => ExecuteSua(), o => SelectedLop != null && !isAdding && !isEditing);
            XoaCommand = new RelayCommand(o => ExecuteXoa(), o => SelectedLop != null && !isAdding && !isEditing);
            LuuCommand = new RelayCommand(o => ExecuteLuu(), o => isAdding || isEditing);
            HuyCommand = new RelayCommand(o => ExecuteHuy(), o => isAdding || isEditing);
            LoadData();
        }

        private void LoadData()
        {
            DS_Lop = new ObservableCollection<Lop>(db.Lops.ToList());
            DS_Khoa = new ObservableCollection<Khoa>(db.Khoas.ToList());
            OnPropertyChanged(nameof(DS_Lop));
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
            if (SelectedLop == null) return;
            isAdding = false;
            isEditing = true;
        }

        private void ExecuteXoa()
        {
            if (SelectedLop == null) return;
            if (MessageBox.Show("Bạn chắc chắn muốn xóa lớp này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            try
            {
                var lopDelete = db.Lops.Find(SelectedLop.MaLop);
                if (lopDelete != null)
                {
                    db.Lops.Remove(lopDelete);
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
            if (string.IsNullOrWhiteSpace(MaLop) || string.IsNullOrWhiteSpace(TenLop) || string.IsNullOrWhiteSpace(MaKhoa))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin lớp!");
                return;
            }
            try
            {
                if (isAdding)
                {
                    if (db.Lops.Any(l => l.MaLop == MaLop.Trim()))
                    {
                        MessageBox.Show("Mã lớp đã tồn tại!");
                        return;
                    }
                    db.Lops.Add(new Lop { MaLop = MaLop.Trim(), TenLop = TenLop.Trim(), MaKhoa = MaKhoa });
                }
                else if (isEditing && SelectedLop != null)
                {
                    var lopUp = db.Lops.Find(SelectedLop.MaLop);
                    if (lopUp != null)
                    {
                        lopUp.TenLop = TenLop.Trim();
                        lopUp.MaKhoa = MaKhoa;
                    }
                }
                db.SaveChanges();
                MessageBox.Show("Lưu dữ liệu lớp thành công!");
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
            SelectedLop = null;
            ClearForm();
        }

        private void ClearForm()
        {
            MaLop = string.Empty;
            TenLop = string.Empty;
            MaKhoa = null;
        }
    }
}
