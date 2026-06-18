using _2001240399_TrinhHuuKienQuoc_Buoi11.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace _2001240399_TrinhHuuKienQuoc_Buoi11.ViewModels
{
    public class MonHocViewModel : BaseViewModel, IDataErrorInfo
    {
        private QLSinhVien_Buoi11Entities db = new QLSinhVien_Buoi11Entities();
        private bool isAdding;
        private bool isEditing;

        public ObservableCollection<MonHoc> DS_MonHoc { get; set; }
        public RelayCommand ThemCommand { get; set; }
        public RelayCommand SuaCommand { get; set; }
        public RelayCommand XoaCommand { get; set; }
        public RelayCommand LuuCommand { get; set; }
        public RelayCommand HuyCommand { get; set; }

        private MonHoc _SelectedMonHoc;
        public MonHoc SelectedMonHoc
        {
            get => _SelectedMonHoc;
            set
            {
                _SelectedMonHoc = value;
                OnPropertyChanged(nameof(SelectedMonHoc));
                if (SelectedMonHoc != null && !isAdding && !isEditing)
                {
                    MaMon = SelectedMonHoc.MaMon;
                    TenMon = SelectedMonHoc.TenMon;
                    SoTinChi = SelectedMonHoc.SoTinChi.ToString();
                }
            }
        }

        private string _MaMon;
        public string MaMon { get => _MaMon; set { _MaMon = value; OnPropertyChanged(nameof(MaMon)); RefreshValid(); } }
        private string _TenMon;
        public string TenMon { get => _TenMon; set { _TenMon = value; OnPropertyChanged(nameof(TenMon)); RefreshValid(); } }
        private string _SoTinChi;
        public string SoTinChi { get => _SoTinChi; set { _SoTinChi = value; OnPropertyChanged(nameof(SoTinChi)); RefreshValid(); } }

        public bool IsValid => string.IsNullOrEmpty(this[nameof(MaMon)]) && string.IsNullOrEmpty(this[nameof(TenMon)]) && string.IsNullOrEmpty(this[nameof(SoTinChi)]);
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(MaMon))
                {
                    if (string.IsNullOrWhiteSpace(MaMon)) return "Mã môn không được để trống";
                    if (MaMon.Trim().Length > 10) return "Mã môn tối đa 10 ký tự";
                    if (isAdding && db.MonHocs.Any(m => m.MaMon == MaMon.Trim())) return "Mã môn đã tồn tại";
                }
                if (columnName == nameof(TenMon))
                {
                    if (string.IsNullOrWhiteSpace(TenMon)) return "Tên môn không được để trống";
                    if (TenMon.Trim().Length > 50) return "Tên môn tối đa 50 ký tự";
                    if (db.MonHocs.Any(m => m.TenMon == TenMon.Trim() && (isAdding || m.MaMon != SelectedMonHoc.MaMon))) return "Tên môn đã tồn tại";
                }
                if (columnName == nameof(SoTinChi))
                {
                    if (string.IsNullOrWhiteSpace(SoTinChi)) return "Số tín chỉ không được để trống";
                    int soTC;
                    if (!int.TryParse(SoTinChi, out soTC)) return "Số tín chỉ phải là số nguyên";
                    if (soTC <= 0 || soTC > 10) return "Số tín chỉ phải từ 1 đến 10";
                }
                return null;
            }
        }

        public MonHocViewModel()
        {
            ThemCommand = new RelayCommand(o => ExecuteThem());
            SuaCommand = new RelayCommand(o => ExecuteSua(), o => SelectedMonHoc != null && !isAdding && !isEditing);
            XoaCommand = new RelayCommand(o => ExecuteXoa(), o => SelectedMonHoc != null && !isAdding && !isEditing);
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
            DS_MonHoc = new ObservableCollection<MonHoc>(db.MonHocs.ToList());
            OnPropertyChanged(nameof(DS_MonHoc));
        }

        private void ExecuteThem()
        {
            isAdding = true;
            isEditing = false;
            ClearForm();
        }

        private void ExecuteSua()
        {
            if (SelectedMonHoc == null) return;
            isAdding = false;
            isEditing = true;
            RefreshValid();
        }

        private void ExecuteXoa()
        {
            if (SelectedMonHoc == null) return;
            if (MessageBox.Show("Bạn chắc chắn muốn xóa môn học này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            try
            {
                var monDelete = db.MonHocs.Find(SelectedMonHoc.MaMon);
                if (monDelete != null)
                {
                    db.MonHocs.Remove(monDelete);
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
            int soTC = int.Parse(SoTinChi);
            try
            {
                if (isAdding)
                {
                    db.MonHocs.Add(new MonHoc { MaMon = MaMon.Trim(), TenMon = TenMon.Trim(), SoTinChi = soTC });
                }
                else if (isEditing && SelectedMonHoc != null)
                {
                    var monUp = db.MonHocs.Find(SelectedMonHoc.MaMon);
                    if (monUp != null)
                    {
                        monUp.TenMon = TenMon.Trim();
                        monUp.SoTinChi = soTC;
                    }
                }
                db.SaveChanges();
                MessageBox.Show("Lưu dữ liệu môn học thành công!");
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
            SelectedMonHoc = null;
            ClearForm();
        }

        private void ClearForm()
        {
            MaMon = string.Empty;
            TenMon = string.Empty;
            SoTinChi = string.Empty;
        }
    }
}
