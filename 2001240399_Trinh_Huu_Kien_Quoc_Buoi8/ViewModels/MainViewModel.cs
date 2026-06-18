using _2001240399_Trinh_Huu_Kien_Quoc_Buoi8.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Win32;
using System.Collections.Generic;
using _2001240399_Trinh_Huu_Kien_Quoc_Buoi8.Views;

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi8.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ClassModel> DanhSachLop { get; set; }
        public ObservableCollection<StudentModel> DanhSachSinhVien { get; set; }
        public ObservableCollection<string> DanhSachThanhPho { get; set; }
        public ObservableCollection<ClassModel> DanhSachLopLoc { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        ClassModel _lopDangChon;
        public ClassModel LopDangChon
        {
            get => _lopDangChon;
            set
            {
                _lopDangChon = value;
                OnPropertyChanged(nameof(LopDangChon));
            }
        }

        StudentModel _selectedStudentTree;
        public StudentModel SelectedStudentTree
        {
            get => _selectedStudentTree;
            set
            {
                _selectedStudentTree = value;
                if (value != null)
                {

                    SinhVienDangChon = new StudentModel
                    {
                        MaSV = value.MaSV,
                        HoTen = value.HoTen,
                        GioiTinh = value.GioiTinh,
                        ThanhPho = value.ThanhPho,
                        DiaChi = value.DiaChi,
                        TenLop = value.TenLop
                    };
                    LopDangChon = DanhSachLop.FirstOrDefault(x => x.TenLop == value.TenLop);
                }
                OnPropertyChanged(nameof(SelectedStudentTree));
            }
        }

        StudentModel _sinhVienDangChon;
        public StudentModel SinhVienDangChon
        {
            get => _sinhVienDangChon;
            set
            {
                _sinhVienDangChon = value;
                OnPropertyChanged(nameof(SinhVienDangChon));
            }
        }

        private ICollectionView _sinhVienView;
        public ICollectionView SinhVienView
        {
            get => _sinhVienView;
            set
            {
                _sinhVienView = value;
                OnPropertyChanged(nameof(SinhVienView));
            }
        }

        string _tuKhoaTimKiem;
        public string TuKhoaTimKiem
        {
            get => _tuKhoaTimKiem;
            set
            {
                _tuKhoaTimKiem = value;
                OnPropertyChanged(nameof(TuKhoaTimKiem));
                SinhVienView?.Refresh();
            }
        }
        ClassModel _lopLocDangChon;
        public ClassModel LopLocDangChon
        {
            get => _lopLocDangChon;
            set
            {
                _lopLocDangChon = value;
                OnPropertyChanged(nameof(LopLocDangChon));
                SinhVienView?.Refresh();
            }
        }

        bool _isThemLop;
        public bool IsThemLop
        {
            get => _isThemLop;
            set
            {
                _isThemLop = value;
                OnPropertyChanged(nameof(IsThemLop));
            }
        }

        string _tenLopMoi;
        public string TenLopMoi
        {
            get => _tenLopMoi;
            set
            {
                _tenLopMoi = value;
                OnPropertyChanged(nameof(TenLopMoi));

            }
        }
        private StudentModel _selectedStudentGrid;
        public StudentModel SelectedStudentGrid
        {
            get => _selectedStudentGrid;
            set
            {
                _selectedStudentGrid = value;
                OnPropertyChanged(nameof(SelectedStudentGrid));
            }
        }
        bool _gioiTinh; // true = Nam, false = Nữ
        public bool GioiTinh
        {
            get => _gioiTinh;
            set
            {
                _gioiTinh = value;
                OnPropertyChanged(nameof(GioiTinh));
                OnPropertyChanged(nameof(GioiTinhNu)); // notify cả Nữ
            }
        }

        public bool GioiTinhNu
        {
            get => !_gioiTinh;
            set => GioiTinh = !value;
        }

        public ICommand ThemLopCommand { get; set; }

        public ICommand ThemSVCommand { get; set; }

        public ICommand XoaSVCommand { get; set; }

        public ICommand CapNhatSVCommand { get; set; }

        public ICommand XuatFileCommand { get; set; }

        public MainViewModel()
        {
            DanhSachSinhVien = new ObservableCollection<StudentModel>();

            DanhSachThanhPho = new ObservableCollection<string> { "Hà Nội", "TP.HCM", "Đà Nẵng", "Cần Thơ" };

            SinhVienDangChon = new StudentModel();

            DanhSachLop = new ObservableCollection<ClassModel>();

            DanhSachLopLoc = new ObservableCollection<ClassModel> { new ClassModel { TenLop = "All" } };
            LoadSampleData();
 
            ThemLopCommand = new RelayCommand(ThemLop);
            ThemSVCommand = new RelayCommand(ThemSinhVien);
            XoaSVCommand = new RelayCommand(XoaSinhVien);
            CapNhatSVCommand = new RelayCommand(CapNhatSinhVien);
            XuatFileCommand = new RelayCommand(XuatFile);

            SinhVienView = CollectionViewSource.GetDefaultView(DanhSachSinhVien);
            SinhVienView.Filter = FilterSinhVien;
        }

        private void LoadSampleData()
        {
            var lop1 = new ClassModel { TenLop = "05DHTH1" };
            var lop2 = new ClassModel { TenLop = "05DHTH2" };
            var lop3 = new ClassModel { TenLop = "05DHTH3" };
            var lop4 = new ClassModel { TenLop = "05DHTH4" };

            DanhSachLop.Add(lop1);
            DanhSachLop.Add(lop2);
            DanhSachLop.Add(lop3);
            DanhSachLop.Add(lop4);

            foreach (var lop in DanhSachLop)
                DanhSachLopLoc.Add(lop);

            AddSampleStudent(lop1, "001", "Lương Minh Châu", true, "TP.HCM", "Q12");
            AddSampleStudent(lop1, "002", "Nguyễn Minh Đạt", true, "TP.HCM", "Q1");
            AddSampleStudent(lop2, "003", "Nguyễn Trí Đức", true, "Đà Nẵng", "Q5");

            LopDangChon = lop1;
            LopLocDangChon = DanhSachLopLoc.FirstOrDefault();
        }

        private void AddSampleStudent(ClassModel lop, string maSV, string hoTen, bool gioiTinh, string thanhPho, string diaChi)
        {
            var sinhVien = new StudentModel
            {
                MaSV = maSV,
                HoTen = hoTen,
                GioiTinh = gioiTinh,
                ThanhPho = thanhPho,
                DiaChi = diaChi,
                TenLop = lop.TenLop
            };

            DanhSachSinhVien.Add(sinhVien);
            lop.DanhSachSinhVien.Add(sinhVien);
        }

        private bool FilterSinhVien(object obj)

        {
            if (obj == null)
                return false;
            StudentModel sv = obj as StudentModel;
            if (sv == null)
                return false;
            bool dungLop;
            if (_lopLocDangChon == null || _lopLocDangChon.TenLop == "All")
            {
                dungLop = true;

            }
            else
            {
                dungLop = sv.TenLop == _lopLocDangChon.TenLop;
            }
            bool dungTen;
            if (string.IsNullOrWhiteSpace(TuKhoaTimKiem))
            {
                dungTen = true;
            }
            else
            {
                dungTen = sv.HoTen.ToLower().Contains(TuKhoaTimKiem.ToLower());
            }
            return dungTen && dungLop;
        }

        void ThemLop(object p)
        {
            if (string.IsNullOrWhiteSpace(TenLopMoi))
            {
                MessageBox.Show("Tên lớp không được để trống");
                return;
            }
            if (DanhSachLop.Any(l => l.TenLop == TenLopMoi))
            {
                MessageBox.Show("Lớp đã tồn tại");
                return;
            }
            ClassModel lop = new ClassModel();
            lop.TenLop = TenLopMoi;
            DanhSachLop.Add(lop);
            LopDangChon = lop;
            TenLopMoi = string.Empty;
            DanhSachLopLoc.Add(lop);

        }

        void ThemSinhVien(object p)
        {
            if (LopDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn lớp trước khi thêm sinh viên");
                return;
            }
            if (string.IsNullOrWhiteSpace(SinhVienDangChon.HoTen))
            {
                MessageBox.Show("Họ tên không được để trống");
                return;
            }

            var svMoi = new StudentModel
            {
                MaSV = SinhVienDangChon.MaSV,
                HoTen = SinhVienDangChon.HoTen,
                GioiTinh = SinhVienDangChon.GioiTinh,
                ThanhPho = SinhVienDangChon.ThanhPho,
                DiaChi = SinhVienDangChon.DiaChi,
                TenLop = LopDangChon.TenLop
            };
            DanhSachSinhVien.Add(svMoi);
            LopDangChon.DanhSachSinhVien.Add(svMoi);
            SinhVienDangChon = new StudentModel();
        }

        void CapNhatSinhVien(object p)
        {
            if (SelectedStudentTree == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên để cập nhật");
                return;
            }
            var svCapNhat = DanhSachSinhVien.FirstOrDefault(sv => sv.MaSV == SelectedStudentTree.MaSV);
            if (svCapNhat != null)
            {
                svCapNhat.HoTen = SinhVienDangChon.HoTen;
                svCapNhat.GioiTinh = SinhVienDangChon.GioiTinh;
                svCapNhat.ThanhPho = SinhVienDangChon.ThanhPho;
                svCapNhat.DiaChi = SinhVienDangChon.DiaChi;
                if (LopDangChon != null && LopDangChon.TenLop != svCapNhat.TenLop)
                {
                    var lopCu = DanhSachLop.FirstOrDefault(l => l.TenLop == svCapNhat.TenLop);
                    lopCu?.DanhSachSinhVien.Remove(svCapNhat);
                    svCapNhat.TenLop = LopDangChon.TenLop;
                    LopDangChon.DanhSachSinhVien.Add(svCapNhat);
                }
                SinhVienView.Refresh();
                MessageBox.Show("Cập nhật sinh viên thành công");
                SinhVienDangChon = new StudentModel();
            }
        }

        void XoaSinhVien(object p)
        {
            if (SelectedStudentTree == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên để xóa");
                return;
            }
            var svXoa = DanhSachSinhVien.FirstOrDefault(sv => sv.MaSV == SelectedStudentTree.MaSV);
            if (svXoa != null)
            {
                DanhSachSinhVien.Remove(svXoa);
                var lop = DanhSachLop.FirstOrDefault(l => l.TenLop == svXoa.TenLop);
                lop?.DanhSachSinhVien.Remove(svXoa);
                SinhVienDangChon = new StudentModel();
            }
        }

        void XuatFile(object p)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                FileName = "DanhSachSinhVien.csv"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                    {
                        sw.WriteLine("MaSV,HoTen,GioiTinh,ThanhPho,DiaChi,TenLop");
                        foreach (var sv in DanhSachSinhVien)
                        {
                            sw.WriteLine($"{sv.MaSV},{sv.HoTen},{sv.GioiTinhText},{sv.ThanhPho},{sv.DiaChi},{sv.TenLop}");
                        }
                    }
                    MessageBox.Show("Xuất file thành công");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xuất file: {ex.Message}");
                }
            }
        }
    }
}
