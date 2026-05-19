using _2001240399_Trinh_Huu_Kien_Quoc_Bai3.Models;
using _2001240399_Trinh_Huu_Kien_Quoc_Bai3.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2001240399_Trinh_Huu_Kien_Quoc_Bai3.ViewModels
{
    public class QuanLyLopKhoaViewModel: BaseViewModel
    {
        QLSinhVienEntities db = new QLSinhVienEntities();
        public ObservableCollection<Khoa> DS_Khoa { get; set; }
        public ObservableCollection<Lop> DS_Lop { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand UpdateCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

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
                }
            }
        }

        private Lop _selectedLop;
        public Lop SelectedLop
        {
            get => _selectedLop;
            set
            {
                _selectedLop = value;
                OnPropertyChanged(nameof(SelectedLop));
                if (SelectedLop != null)
                {
                    {
                        MaLop = SelectedLop.MaLop;
                    }
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

        private string _maLop;
        public string MaLop
        {
            get => _maLop;
            set
            {
                _maLop = value;
                OnPropertyChanged(nameof(MaLop));
            }
        }

        bool _isMaLopEnabled = false;
        public bool IsMaLopEnabled
        {
            get => _isMaLopEnabled;
            set
            {
                _isMaLopEnabled = value;
                OnPropertyChanged(nameof(IsMaLopEnabled));
            }
        }

        bool _isActionEnabled = true;
        public bool IsActionEnabled
        {
            get => _isActionEnabled;
            set
            {
                _isActionEnabled = value;
                OnPropertyChanged(nameof(IsActionEnabled));
            }
        }

        bool _isSaveCancelEnabled = false;
        public bool IsSaveCancelEnabled
        {
            get => _isSaveCancelEnabled;
            set
            {
                _isSaveCancelEnabled = value;
                OnPropertyChanged(nameof(IsSaveCancelEnabled));
            }
        }

        bool isAddingMode = false;

        public QuanLyLopKhoaViewModel()
        {

            AddCommand = new RelayCommand(o => PrepareAdd());
            DeleteCommand = new RelayCommand(o => Delete(), o => SelectedLop != null);
            UpdateCommand = new RelayCommand(o => PrepareEdit(), o => SelectedLop != null);
            SaveCommand = new RelayCommand(o => Save());
            CancelCommand = new RelayCommand(o => Cancel());
            LoadData();
        }

        void LoadData()
        {
            DS_Khoa = new ObservableCollection<Khoa>(db.Khoas.ToList());
            OnPropertyChanged(nameof(DS_Khoa));
            DS_Lop = new ObservableCollection<Lop>(db.Lops.ToList());
            OnPropertyChanged(nameof(DS_Lop));
        }

        void PrepareAdd()
        {
            isAddingMode = true;
            MaLop = string.Empty;
            MaKhoa = null;

            IsMaLopEnabled = true;
            IsActionEnabled = false;
            IsSaveCancelEnabled = true;
        }
        private void Delete()
        {
            if (SelectedLop == null) return;
            bool hasStudents = db.Lops.Any(sv => sv.MaLop == SelectedLop.MaLop);
            if (hasStudents)
            {
                if (MessageBox.Show("Bạn chắc chắn muốn xóa lớp này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
                try
                {
                    var LopDelete = db.Lops.Find(SelectedLop?.MaLop);
                    if (LopDelete != null)
                    {
                        db.Lops.Remove(LopDelete);
                        db.SaveChanges();
                        LoadData();
                        ResetForm();
                        MessageBox.Show("Xóa thành công!");
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message);
                }
            }
        }

        void ResetForm()
        {
            MaLop = string.Empty;
            MaKhoa = null;
            IsMaLopEnabled = false;
            IsActionEnabled = true;
            IsSaveCancelEnabled = false;
            isAddingMode = false;
        }

        private void PrepareEdit()
        {
            if (SelectedLop == null) return;
            bool hasStudents = db.Lops.Any(sv => sv.MaLop == SelectedLop.MaLop);
            isAddingMode = false;
            IsMaLopEnabled = false;
            IsActionEnabled = false;
            IsSaveCancelEnabled = true;
        }

        private void Save()
        {
            if (isAddingMode)
            {
                if (db.Lops.Any(l => l.MaLop == MaLop))
                {
                    MessageBox.Show("Mã lớp này đã tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newLop = new Lop { MaLop = this.MaLop, MaKhoa = this.MaKhoa };
                db.Lops.Add(newLop);
                MessageBox.Show("Thêm lớp thành công!");
            }
            else
            {
                var lopToUpdate = db.Lops.Find(SelectedLop?.MaLop);
                if (lopToUpdate != null)
                {
                    lopToUpdate.MaKhoa = this.MaKhoa;
                    MessageBox.Show("Cập nhật lớp thành công!");
                }
            }
            db.SaveChanges();
            LoadData();
            ResetForm();
        }

        void Cancel()
        {
            if (SelectedLop != null)
            {
                MaLop = SelectedLop.MaLop;
                MaKhoa = SelectedLop.MaKhoa;
            }
            ResetForm();
        }
    }
}
