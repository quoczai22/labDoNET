using Bai5.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Bai5.ViewModels
{
    public class QuanLyMonHocViewModel : BaseViewModel
    {
        QLSinhVienEntities db = new QLSinhVienEntities();
        private bool isAdding = false;
        private bool isEditing = false;

        public ObservableCollection<MonHoc> DS_MonHoc { get; set; }
        public List<string> DS_TinhChat { get; set; } = new List<string> { "Bắt buộc", "Tự chọn" };

        MonHoc _selectedMonHoc;
        public MonHoc SelectedMonHoc
        {
            get { return _selectedMonHoc; }
            set
            {
                _selectedMonHoc = value;
                OnPropertyChanged(nameof(SelectedMonHoc));
                if (SelectedMonHoc != null && !isAdding && !isEditing)
                {
                    MaMH = SelectedMonHoc.MaMonHoc;
                    TenMH = SelectedMonHoc.TenMonHoc;
                    SoTinChi = SelectedMonHoc.SoTC;
                    TinhChat = SelectedMonHoc.TinhChat;
                }
                CommandManager.InvalidateRequerySuggested();
            }
        }

        string _maMH;
        public string MaMH { get { return _maMH; } set { _maMH = value; OnPropertyChanged(nameof(MaMH)); } }

        string _tenMH;
        public string TenMH { get { return _tenMH; } set { _tenMH = value; OnPropertyChanged(nameof(TenMH)); } }

        int? _soTinChi;
        public int? SoTinChi { get { return _soTinChi; } set { _soTinChi = value; OnPropertyChanged(nameof(SoTinChi)); } }

        string _tinhChat;
        public string TinhChat { get { return _tinhChat; } set { _tinhChat = value; OnPropertyChanged(nameof(TinhChat)); } }

        private bool _isReadOnlyMaMon;
        public bool IsReadOnlyMaMon { get { return _isReadOnlyMaMon; } set { _isReadOnlyMaMon = value; OnPropertyChanged(nameof(IsReadOnlyMaMon)); } }

        private bool _isReadOnlyTenMon;
        public bool IsReadOnlyTenMon { get { return _isReadOnlyTenMon; } set { _isReadOnlyTenMon = value; OnPropertyChanged(nameof(IsReadOnlyTenMon)); } }

        public RelayCommand AddCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public QuanLyMonHocViewModel()
        {
            LoadData();
            AddCommand = new RelayCommand(Add, CanAdd);
            EditCommand = new RelayCommand(Edit, CanEdit);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel, CanCancel);
        }

        void LoadData()
        {
            DS_MonHoc = new ObservableCollection<MonHoc>(db.MonHocs.ToList());
            OnPropertyChanged(nameof(DS_MonHoc));
        }

        bool CanAdd(object p) { return !isAdding && !isEditing; }
        public bool CanEdit(object p) { return SelectedMonHoc != null && !isAdding && !isEditing; }
        public bool CanDelete(object p) { return SelectedMonHoc != null && !isAdding && !isEditing; }
        public bool CanSave(object p) { return isAdding || isEditing; }
        public bool CanCancel(object p) { return isAdding || isEditing; }

        void Add(object p)
        {
            isAdding = true;
            isEditing = false;
            SelectedMonHoc = null;
            IsReadOnlyMaMon = false;
            IsReadOnlyTenMon = false;
            ClearForm();
            CommandManager.InvalidateRequerySuggested();
        }

        void Edit(object p)
        {
            if (SelectedMonHoc == null)
            {
                MessageBox.Show("Vui lòng chọn môn học cần sửa!");
                return;
            }

            isAdding = false;
            isEditing = true;
            IsReadOnlyMaMon = true;
            IsReadOnlyTenMon = false;
            MaMH = SelectedMonHoc.MaMonHoc;
            TenMH = SelectedMonHoc.TenMonHoc;
            SoTinChi = SelectedMonHoc.SoTC;
            TinhChat = SelectedMonHoc.TinhChat;
            CommandManager.InvalidateRequerySuggested();
        }

        void Delete(object p)
        {
            if (SelectedMonHoc == null)
            {
                MessageBox.Show("Vui lòng chọn môn học cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn chắc chắn muốn xóa môn học này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;

            try
            {
                var monHoc = db.MonHocs.Find(SelectedMonHoc.MaMonHoc);
                if (monHoc != null)
                {
                    db.MonHocs.Remove(monHoc);
                    db.SaveChanges();
                    LoadData();
                    ClearForm();
                    SelectedMonHoc = null;
                    MessageBox.Show("Xóa môn học thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message);
            }
        }

        void Save(object p)
        {
            if (!isAdding && !isEditing)
            {
                MessageBox.Show("Vui lòng bấm Thêm hoặc Sửa trước khi Lưu!");
                return;
            }

            if (string.IsNullOrWhiteSpace(MaMH) || string.IsNullOrWhiteSpace(TenMH) || SoTinChi == null || string.IsNullOrWhiteSpace(TinhChat))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin môn học!");
                return;
            }

            try
            {
                if (isAdding)
                {
                    if (db.MonHocs.Any(m => m.MaMonHoc == MaMH.Trim()))
                    {
                        MessageBox.Show("Mã môn học đã tồn tại!");
                        return;
                    }

                    var newMonHoc = new MonHoc
                    {
                        MaMonHoc = MaMH.Trim(),
                        TenMonHoc = TenMH.Trim(),
                        SoTC = SoTinChi,
                        TinhChat = TinhChat
                    };
                    db.MonHocs.Add(newMonHoc);
                }
                else if (isEditing)
                {
                    var monHoc = db.MonHocs.Find(SelectedMonHoc.MaMonHoc);
                    if (monHoc != null)
                    {
                        monHoc.TenMonHoc = TenMH.Trim();
                        monHoc.SoTC = SoTinChi;
                        monHoc.TinhChat = TinhChat;
                    }
                }

                db.SaveChanges();
                MessageBox.Show("Lưu dữ liệu thành công!");
                isAdding = false;
                isEditing = false;
                IsReadOnlyMaMon = false;
                IsReadOnlyTenMon = false;
                LoadData();
                ClearForm();
                SelectedMonHoc = null;
                CommandManager.InvalidateRequerySuggested();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                MessageBox.Show("Lỗi ràng buộc DB:\n" + string.Join("\n", errorMessages));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }

        void Cancel(object p)
        {
            isAdding = false;
            isEditing = false;
            IsReadOnlyMaMon = false;
            IsReadOnlyTenMon = false;
            SelectedMonHoc = null;
            ClearForm();
            CommandManager.InvalidateRequerySuggested();
        }

        void ClearForm()
        {
            MaMH = string.Empty;
            TenMH = string.Empty;
            TinhChat = null;
            SoTinChi = null;
        }
    }
}
