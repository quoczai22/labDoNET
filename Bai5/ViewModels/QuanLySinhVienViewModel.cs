using Bai5.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Data.Entity;

namespace Bai5.ViewModels
{
    public class QuanLySinhVienViewModel : BaseViewModel
    {
        private QLSinhVienEntities db = new QLSinhVienEntities();

        private bool isAdding = false;
        private bool isEditing = false;

        private ObservableCollection<SinhVien> _dsSinhVien;
        public ObservableCollection<SinhVien> DS_SinhVien
        {
            get => _dsSinhVien;
            set { _dsSinhVien = value; OnPropertyChanged(nameof(DS_SinhVien)); }
        }

        private ObservableCollection<Lop> _dsLop;
        public ObservableCollection<Lop> DS_Lop
        {
            get => _dsLop;
            set { _dsLop = value; OnPropertyChanged(nameof(DS_Lop)); }
        } 

        private string _maSV;
        public string MaSV { get => _maSV; set { _maSV = value; OnPropertyChanged(nameof(MaSV)); } }

        private string _hoTen;
        public string HoTen { get => _hoTen; set { _hoTen = value; OnPropertyChanged(nameof(HoTen)); } }

        private DateTime? _ngaySinh;
        public DateTime? NgaySinh { get => _ngaySinh; set { _ngaySinh = value; OnPropertyChanged(nameof(NgaySinh)); } }

        private string _selectedMaLop;
        public string SelectedMaLop { get => _selectedMaLop; set { _selectedMaLop = value; OnPropertyChanged(nameof(SelectedMaLop)); } }

        private string _gioiTinh = "Nam"; // Mặc định là Nam
        public string GioiTinh
        {
            get => _gioiTinh;
            set
            {
                _gioiTinh = value;
                OnPropertyChanged(nameof(GioiTinh));
                OnPropertyChanged(nameof(IsNam));
                OnPropertyChanged(nameof(IsNu));
            }
        }
        public bool IsNam
        {
            get => GioiTinh == "Nam";
            set { if (value) GioiTinh = "Nam"; }
        }
        public bool IsNu
        {
            get => GioiTinh == "Nữ";
            set { if (value) GioiTinh = "Nữ"; }
        }

        private SinhVien _selectedSinhVien;
        public SinhVien SelectedSinhVien
        {
            get => _selectedSinhVien;
            set
            {
                _selectedSinhVien = value;
                OnPropertyChanged(nameof(SelectedSinhVien));

                if (_selectedSinhVien != null && !isAdding && !isEditing)
                {
                    MaSV = _selectedSinhVien.MaSinhVien;
                    HoTen = _selectedSinhVien.HoTen;
                    GioiTinh = _selectedSinhVien.GioiTinh;
                    NgaySinh = _selectedSinhVien.NgaySinh;
                    SelectedMaLop = _selectedSinhVien.MaLop;
                }
            }
        }

        public ICommand ThemCommand { get; set; }
        public ICommand SuaCommand { get; set; }
        public ICommand XoaCommand { get; set; }
        public ICommand LuuCommand { get; set; }
        public ICommand HuyCommand { get; set; }

        public QuanLySinhVienViewModel()
        {
            ThemCommand = new RelayCommand(o => ExecuteThem());
            SuaCommand = new RelayCommand(o => ExecuteSua());
            XoaCommand = new RelayCommand(o => ExecuteXoa());
            LuuCommand = new RelayCommand(o => ExecuteLuu());
            HuyCommand = new RelayCommand(o => ExecuteHuy());

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                DS_SinhVien = new ObservableCollection<SinhVien>(db.SinhViens.ToList());
                DS_Lop = new ObservableCollection<Lop>(db.Lops.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message);
            }
        }

        private void ExecuteThem()
        {
            isAdding = true;
            isEditing = false;
            ClearForm();
        }

        private void ExecuteSua()
        {
            if (SelectedSinhVien == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần sửa từ danh sách!");
                return;
            }
            isAdding = false;
            isEditing = true;
        }

        private void ExecuteXoa()
        {
            if (SelectedSinhVien == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa!");
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa sinh viên {SelectedSinhVien.HoTen}?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    db.SinhViens.Remove(SelectedSinhVien);
                    db.SaveChanges();
                    MessageBox.Show("Xóa thành công!");
                    LoadData();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể xóa do sinh viên này đã có điểm trong hệ thống!\nChi tiết: " + ex.Message);
                }
            }
        }

        private void ExecuteLuu()
        {
            if (!isAdding && !isEditing)
            {
                MessageBox.Show("Vui lòng nhấn Thêm hoặc Sửa trước khi Lưu!");
                return;
            }

            if (string.IsNullOrWhiteSpace(MaSV) || string.IsNullOrWhiteSpace(HoTen) || string.IsNullOrWhiteSpace(SelectedMaLop))
            {
                MessageBox.Show("Vui lòng điền đầy đủ Mã SV, Họ tên và chọn Lớp!");
                return;
            }

            try
            {
                if (isAdding)
                {
                    if (db.SinhViens.Any(s => s.MaSinhVien == MaSV.Trim()))
                    {
                        MessageBox.Show("Mã sinh viên đã tồn tại!");
                        return;
                    }

                    var newSV = new SinhVien
                    {
                        MaSinhVien = MaSV.Trim(),
                        HoTen = HoTen.Trim(),
                        GioiTinh = this.GioiTinh,
                        NgaySinh = this.NgaySinh,
                        MaLop = SelectedMaLop
                    };
                    db.SinhViens.Add(newSV);
                }
                else if (isEditing)
                {
                    var editSV = db.SinhViens.FirstOrDefault(s => s.MaSinhVien == SelectedSinhVien.MaSinhVien);
                    if (editSV != null)
                    {
                        editSV.HoTen = HoTen.Trim();
                        editSV.GioiTinh = this.GioiTinh;
                        editSV.NgaySinh = this.NgaySinh;
                        editSV.MaLop = SelectedMaLop;
                    }
                }

                db.SaveChanges();
                MessageBox.Show("Lưu dữ liệu thành công!");

                isAdding = false;
                isEditing = false;
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message);
            }
        }

        private void ExecuteHuy()
        {
            isAdding = false;
            isEditing = false;
            ClearForm();
            SelectedSinhVien = null;
        }

        private void ClearForm()
        {
            MaSV = "";
            HoTen = "";
            GioiTinh = "Nam";
            NgaySinh = DateTime.Now;
            if (DS_Lop != null && DS_Lop.Count > 0) SelectedMaLop = DS_Lop[0].MaLop;
        }
    }
}