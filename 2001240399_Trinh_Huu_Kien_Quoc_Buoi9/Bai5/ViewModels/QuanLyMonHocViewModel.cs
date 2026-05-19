using Bai5.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bai5.Views;

namespace Bai5.ViewModels
{
    public class QuanLyMonHocViewModel : BaseViewModel
    {
        QLSinhVienEntities db = new QLSinhVienEntities();
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
                if (SelectedMonHoc != null)
                {
                    MaMH = SelectedMonHoc.MaMonHoc;
                    TenMH = SelectedMonHoc.TenMonHoc;
                    SoTinChi =SelectedMonHoc.SoTC;
                    TinhChat = SelectedMonHoc.TinhChat;
                }
            }
        }
        string _maMH;
        public string MaMH
        {
            get { return _maMH; }
            set
            {
                _maMH = value;
                OnPropertyChanged(nameof(MaMH));
            }
        }
        string _tenMH;
        public string TenMH
        {
            get { return _tenMH; }
            set
            {
                _tenMH = value;
                OnPropertyChanged(nameof(TenMH));
            }
        }
        int? _soTinChi;
        public int? SoTinChi
        {
            get { return _soTinChi; }
            set
            {
                _soTinChi = value;
                OnPropertyChanged(nameof(SoTinChi));
            }
        }
        string _tinhChat;
        public string TinhChat
        {
            get { return _tinhChat; }
            set
            {
                _tinhChat = value;
                OnPropertyChanged(nameof(TinhChat));
            }
        }
        private bool _isReadOnlyMaMon;
        public bool IsReadOnlyMaMon
        {
            get { return _isReadOnlyMaMon; }
            set { _isReadOnlyMaMon = value; OnPropertyChanged(nameof(IsReadOnlyMaMon)); }
        }

        private bool _isReadOnlyTenMon;
        public bool IsReadOnlyTenMon
        {
            get { return _isReadOnlyTenMon; }
            set { _isReadOnlyTenMon = value; OnPropertyChanged(nameof(IsReadOnlyTenMon)); }
        }

        public RelayCommand AddCommand { get; set; }
        public RelayCommand DeleteCommand { get;set; }
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
        }

        bool CanAdd(object p)  { return true; }
        public bool CanEdit(object p) { return true; }
        public bool CanDelete(object p) { return true; }
        public bool CanSave(object p) { return true; }
        public bool CanCancel(object p)  { return true; }

        void Add(object p)
        {
             var newMonHoc = new MonHoc
            {
                MaMonHoc = MaMH,
                TenMonHoc = TenMH,
                SoTC = SoTinChi,
                TinhChat = TinhChat
            };
            db.MonHocs.Add(newMonHoc);
            try
            {
                SelectedMonHoc = null;
                MaMH = string.Empty;
                TenMH = string.Empty;
                TinhChat = string.Empty;
                SoTinChi = null;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        MessageBox.Show($"Lỗi cột: {validationError.PropertyName} - {validationError.ErrorMessage}");
                    }
                }
            }
            MaMH = TenMH = TinhChat = string.Empty;
            SoTinChi = null;
        }

        void Edit(object p)
        {
            if (SelectedMonHoc != null)
            {
                var monHoc = db.MonHocs.Find(SelectedMonHoc.MaMonHoc);
                if (monHoc != null)
                {
                    monHoc.TenMonHoc = TenMH;
                    monHoc.SoTC = SoTinChi;
                    monHoc.TinhChat = TinhChat;
                    db.SaveChanges();
                    LoadData();
                }
            }
        }

        void Delete(object p)
        {
            if (SelectedMonHoc != null)
            {
                var monHoc = db.MonHocs.Find(SelectedMonHoc.MaMonHoc);
                if (monHoc != null)
                {
                    db.MonHocs.Remove(monHoc);
                    db.SaveChanges();
                    DS_MonHoc.Remove(SelectedMonHoc);
                }
            }
        }

        void Save(object p)
        {
            // 1. Chặn ngay từ đầu nếu TextBox trống (khỏi sợ lỗi null từ EF)
            if (string.IsNullOrWhiteSpace(MaMH) || string.IsNullOrWhiteSpace(TenMH))
            {
                MessageBox.Show("Mã môn và Tên môn không được để trống!");
                return;
            }

            try
            {
                // 2. Tìm xem mã này đã có trong DB chưa
                var monHoc = db.MonHocs.Find(MaMH.Trim());

                if (monHoc == null)
                {
                    // NẾU CHƯA CÓ -> THÊM MỚI
                    monHoc = new MonHoc
                    {
                        MaMonHoc = MaMH.Trim(),
                        TenMonHoc = TenMH.Trim(),
                        SoTC = SoTinChi,
                        TinhChat = TinhChat
                    };
                    db.MonHocs.Add(monHoc);
                }
                else
                {
                    // NẾU ĐÃ CÓ -> CẬP NHẬT (SỬA)
                    monHoc.TenMonHoc = TenMH.Trim();
                    monHoc.SoTC = SoTinChi;
                    monHoc.TinhChat = TinhChat;
                }

                // 3. Đẩy xuống DB
                db.SaveChanges();
                MessageBox.Show("Lưu dữ liệu thành công!");

                // Nạp lại danh sách lên DataGrid
                LoadData();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Bắt lỗi Validation nếu còn
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);
                MessageBox.Show("Lỗi rành buộc DB:\n" + string.Join("\n", errorMessages));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }


        void Cancel(object p)
        {
            MaMH = TenMH = TinhChat = string.Empty;
            SoTinChi = null;
            SelectedMonHoc = null;
        }
    }
}
