using _2001240399_TrinhHuuKienQuoc_Buoi11.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace _2001240399_TrinhHuuKienQuoc_Buoi11.ViewModels
{
    public class SinhVienViewModel : BaseViewModel, IDataErrorInfo
    {
        private QLSinhVien_Buoi11Entities db = new QLSinhVien_Buoi11Entities();
        private bool isAdding;
        private bool isEditing;

        public ObservableCollection<SinhVien> DS_SinhVien { get; set; }
        public ObservableCollection<Lop> DS_Lop { get; set; }
        public List<string> DS_GioiTinh { get; set; } = new List<string> { "Nam", "Nữ" };

        public RelayCommand ThemCommand { get; set; }
        public RelayCommand SuaCommand { get; set; }
        public RelayCommand XoaCommand { get; set; }
        public RelayCommand LuuCommand { get; set; }
        public RelayCommand HuyCommand { get; set; }

        public bool IsMaSVReadOnly => isEditing;

        private SinhVien _SelectedSinhVien;
        public SinhVien SelectedSinhVien
        {
            get => _SelectedSinhVien;
            set
            {
                _SelectedSinhVien = value;
                OnPropertyChanged(nameof(SelectedSinhVien));
                if (SelectedSinhVien != null && !isAdding && !isEditing)
                {
                    MaSV = SelectedSinhVien.MaSV;
                    HoTen = SelectedSinhVien.HoTen;
                    GioiTinh = SelectedSinhVien.GioiTinh;
                    NgaySinh = SelectedSinhVien.NgaySinh;
                    MaLop = SelectedSinhVien.MaLop;
                }
            }
        }

        private string _MaSV;
        public string MaSV { get => _MaSV; set { _MaSV = value; OnPropertyChanged(nameof(MaSV)); RefreshValid(); } }
        private string _HoTen;
        public string HoTen { get => _HoTen; set { _HoTen = value; OnPropertyChanged(nameof(HoTen)); RefreshValid(); } }
        private string _GioiTinh;
        public string GioiTinh { get => _GioiTinh; set { _GioiTinh = value; OnPropertyChanged(nameof(GioiTinh)); RefreshValid(); } }
        private DateTime? _NgaySinh;
        public DateTime? NgaySinh { get => _NgaySinh; set { _NgaySinh = value; OnPropertyChanged(nameof(NgaySinh)); RefreshValid(); } }
        private string _MaLop;
        public string MaLop { get => _MaLop; set { _MaLop = value; OnPropertyChanged(nameof(MaLop)); RefreshValid(); } }

        public bool IsValid => string.IsNullOrEmpty(this[nameof(MaSV)]) && string.IsNullOrEmpty(this[nameof(HoTen)]) && string.IsNullOrEmpty(this[nameof(GioiTinh)]) && string.IsNullOrEmpty(this[nameof(NgaySinh)]) && string.IsNullOrEmpty(this[nameof(MaLop)]);
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(MaSV))
                {
                    if (string.IsNullOrWhiteSpace(MaSV)) return "Mã sinh viên không được để trống";
                    if (MaSV.Trim().Length > 10) return "Mã sinh viên tối đa 10 ký tự";
                    if (isAdding && db.SinhViens.Any(s => s.MaSV == MaSV.Trim())) return "Mã sinh viên đã tồn tại";
                    if (isEditing && SelectedSinhVien != null && MaSV.Trim() != SelectedSinhVien.MaSV) return "Không được sửa mã sinh viên";
                }
                if (columnName == nameof(HoTen))
                {
                    if (string.IsNullOrWhiteSpace(HoTen)) return "Họ tên không được để trống";
                    if (HoTen.Trim().Length > 50) return "Họ tên tối đa 50 ký tự";
                }
                if (columnName == nameof(GioiTinh))
                {
                    if (string.IsNullOrWhiteSpace(GioiTinh)) return "Vui lòng chọn giới tính";
                }
                if (columnName == nameof(NgaySinh))
                {
                    if (NgaySinh == null) return "Vui lòng chọn ngày sinh";
                    if (TinhTuoi(NgaySinh.Value) < 18) return "Sinh viên phải đủ 18 tuổi";
                }
                if (columnName == nameof(MaLop))
                {
                    if (string.IsNullOrWhiteSpace(MaLop)) return "Vui lòng chọn lớp";
                }
                return null;
            }
        }

        public SinhVienViewModel()
        {
            ThemCommand = new RelayCommand(o => ExecuteThem());
            SuaCommand = new RelayCommand(o => ExecuteSua(), o => SelectedSinhVien != null && !isAdding && !isEditing);
            XoaCommand = new RelayCommand(o => ExecuteXoa(), o => SelectedSinhVien != null && !isAdding && !isEditing);
            LuuCommand = new RelayCommand(o => ExecuteLuu(), o => (isAdding || isEditing) && IsValid);
            HuyCommand = new RelayCommand(o => ExecuteHuy(), o => isAdding || isEditing);
            LoadData();
        }

        private void RefreshValid()
        {
            OnPropertyChanged(nameof(IsValid));
            CommandManager.InvalidateRequerySuggested();
        }

        private int TinhTuoi(DateTime ngaySinh)
        {
            var today = DateTime.Today;
            int tuoi = today.Year - ngaySinh.Year;
            if (ngaySinh.Date > today.AddYears(-tuoi)) tuoi--;
            return tuoi;
        }

        private void LoadData()
        {
            DS_SinhVien = new ObservableCollection<SinhVien>(db.SinhViens.ToList());
            DS_Lop = new ObservableCollection<Lop>(db.Lops.ToList());
            OnPropertyChanged(nameof(DS_SinhVien));
            OnPropertyChanged(nameof(DS_Lop));
        }

        private void ExecuteThem()
        {
            isAdding = true;
            isEditing = false;
            OnPropertyChanged(nameof(IsMaSVReadOnly));
            ClearForm();
        }

        private void ExecuteSua()
        {
            if (SelectedSinhVien == null) return;
            isAdding = false;
            isEditing = true;
            OnPropertyChanged(nameof(IsMaSVReadOnly));
            RefreshValid();
        }

        private void ExecuteXoa()
        {
            if (SelectedSinhVien == null) return;
            if (MessageBox.Show("Bạn chắc chắn muốn xóa sinh viên này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            try
            {
                var svDelete = db.SinhViens.Find(SelectedSinhVien.MaSV);
                if (svDelete != null)
                {
                    db.SinhViens.Remove(svDelete);
                    db.SaveChanges();
                    LoadData();
                    ClearForm();
                }
            }
            catch (Exception ex)
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
                    db.SinhViens.Add(new SinhVien { MaSV = MaSV.Trim(), HoTen = HoTen.Trim(), GioiTinh = GioiTinh, NgaySinh = NgaySinh.Value, MaLop = MaLop });
                }
                else if (isEditing && SelectedSinhVien != null)
                {
                    var svUp = db.SinhViens.Find(SelectedSinhVien.MaSV);
                    if (svUp != null)
                    {
                        svUp.HoTen = HoTen.Trim();
                        svUp.GioiTinh = GioiTinh;
                        svUp.NgaySinh = NgaySinh.Value;
                        svUp.MaLop = MaLop;
                    }
                }
                db.SaveChanges();
                MessageBox.Show("Lưu dữ liệu sinh viên thành công!");
                isAdding = false;
                isEditing = false;
                OnPropertyChanged(nameof(IsMaSVReadOnly));
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }

        private void ExecuteHuy()
        {
            isAdding = false;
            isEditing = false;
            OnPropertyChanged(nameof(IsMaSVReadOnly));
            SelectedSinhVien = null;
            ClearForm();
        }

        private void ClearForm()
        {
            MaSV = string.Empty;
            HoTen = string.Empty;
            GioiTinh = null;
            NgaySinh = null;
            MaLop = null;
        }
    }
}
