using Bai7.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;
using System.Windows;
using System.Windows.Input;

namespace Bai7.ViewModels
{
    public class BangDiemSinhVienViewModel : BaseViewModel
    {
        QLSinhVienEntities db = new QLSinhVienEntities();

        // 1. Nguồn dữ liệu bộ lọc ComboBox
        public List<string> DS_NamHoc { get; set; } = new List<string> { "2024-2025", "2025-2026", "2026-2027" };
        public List<int> DS_HocKy { get; set; } = new List<int> { 1, 2, 3 };

        // 2. Dữ liệu Thông tin sinh viên
        private string _maSV;
        public string MaSV
        {
            get => _maSV;
            set { _maSV = value; OnPropertyChanged(nameof(MaSV)); }
        }

        private string _hoTenHienThi;
        public string HoTenHienThi { get => _hoTenHienThi; set { _hoTenHienThi = value; OnPropertyChanged(nameof(HoTenHienThi)); } }

        private string _lopHienThi;
        public string LopHienThi { get => _lopHienThi; set { _lopHienThi = value; OnPropertyChanged(nameof(LopHienThi)); } }

        // 3. Dữ liệu Bộ lọc chọn
        private string _selectedNamHoc;
        public string SelectedNamHoc { get => _selectedNamHoc; set { _selectedNamHoc = value; OnPropertyChanged(nameof(SelectedNamHoc)); } }

        private int? _selectedHocKy;
        public int? SelectedHocKy { get => _selectedHocKy; set { _selectedHocKy = value; OnPropertyChanged(nameof(SelectedHocKy)); } }

        // 4. Danh sách hiển thị DataGrid & Thống kê
        private ObservableCollection<DiemChiTietModel> _dsKetQua = new ObservableCollection<DiemChiTietModel>();
        public ObservableCollection<DiemChiTietModel> DS_KetQua { get => _dsKetQua; set { _dsKetQua = value; OnPropertyChanged(nameof(DS_KetQua)); } }

        private int _tongTinChi;
        public int TongTinChi { get => _tongTinChi; set { _tongTinChi = value; OnPropertyChanged(nameof(TongTinChi)); } }

        private double _gpa;
        public double GPA { get => _gpa; set { _gpa = value; OnPropertyChanged(nameof(GPA)); } }

        private string _xepLoai;
        public string XepLoai { get => _xepLoai; set { _xepLoai = value; OnPropertyChanged(nameof(XepLoai)); } }

        // 5. Hệ thống lệnh lệnh điều khiển (Commands)
        public ICommand TimKiemCommand { get; set; }
        public ICommand XemDiemCommand { get; set; }

        public BangDiemSinhVienViewModel()
        {
            SelectedNamHoc = DS_NamHoc[0]; // Mặc định 2024-2025 giống ảnh sếp gửi
            SelectedHocKy = 1;

            TimKiemCommand = new RelayCommand(TimSinhVien, CanTimSinhVien);
            XemDiemCommand = new RelayCommand(XemDiem, CanXemDiem);
        }

        private bool CanTimSinhVien(object parameter)
        {
            // Sửa lỗi: Nút Tìm sáng lên ngay khi có chữ trong ô Mã SV
            return !string.IsNullOrWhiteSpace(MaSV);
        }

        private void TimSinhVien(object parameter)
        {
            string maSV = MaSV.Trim();
            var sv = db.SinhViens.FirstOrDefault(s => s.MaSinhVien == maSV);

            if (sv != null)
            {
                HoTenHienThi = sv.HoTen;
                LopHienThi = sv.MaLop;

                DS_KetQua.Clear();
                ResetThongKe();
            }
            else
            {
                MessageBox.Show("Không tìm thấy sinh viên có mã: " + maSV, "Thông báo");
                HoTenHienThi = "";
                LopHienThi = "";
            }
        }

        private void XemDiem(object parameter)
        {
            if (string.IsNullOrWhiteSpace(MaSV)) return;

            var sv = db.SinhViens.FirstOrDefault(s => s.MaSinhVien == MaSV.Trim());
            if (sv == null)
            {
                MessageBox.Show("Vui lòng tìm kiếm sinh viên hợp lệ trước khi xem điểm!");
                return;
            }

            if (string.IsNullOrEmpty(SelectedNamHoc) || SelectedHocKy == null)
            {
                MessageBox.Show("Vui lòng chọn năm học và học kỳ.");
                return;
            }

            // Thực hiện nạp dữ liệu liên kết bảng qua phương thức .Include() nâng cao
            var ketQua = db.KetQuas.Include(k => k.MonHoc)
                                   .Where(kq => kq.MaSinhVien == sv.MaSinhVien &&
                                                kq.NamHoc.Trim() == SelectedNamHoc.Trim() &&
                                                kq.HocKy == SelectedHocKy)
                                   .ToList();

            var danhSachUI = new ObservableCollection<DiemChiTietModel>();
            int stt = 1;
            int tongTC = 0;
            double tongDiemHeSo = 0;

            foreach (var kq in ketQua)
            {
                int soTC = kq.MonHoc.SoTC ?? 0;
                string diemChu = "";

                if (kq.Diem.HasValue)
                {
                    diemChu = QuyDoiDiemChu(kq.Diem.Value);
                    tongTC += soTC;
                    tongDiemHeSo += (kq.Diem.Value * soTC);
                }

                danhSachUI.Add(new DiemChiTietModel
                {
                    STT = stt++,
                    MaMonHoc = kq.MaMonHoc,
                    TenMonHoc = kq.MonHoc.TenMonHoc,
                    SoTC = soTC,
                    DiemSo = kq.Diem,
                    DiemChu = diemChu
                });
            }

            DS_KetQua = danhSachUI;
            TongTinChi = tongTC;

            if (tongTC > 0)
            {
                GPA = tongDiemHeSo / tongTC;
                XepLoai = XepLoaiHocLuc(GPA);
            }
            else
            {
                ResetThongKe();
            }
        }

        private bool CanXemDiem(object parameter)
        {
            return true;
        }

        private void ResetThongKe()
        {
            TongTinChi = 0;
            GPA = 0;
            XepLoai = "";
        }

        private string QuyDoiDiemChu(double diem)
        {
            if (diem >= 8.5) return "A";
            if (diem >= 7.0) return "B";
            if (diem >= 5.5) return "C";
            if (diem >= 4.0) return "D";
            return "F";
        }

        private string XepLoaiHocLuc(double gpa)
        {
            if (gpa >= 9.0) return "Xuất sắc";
            if (gpa >= 8.0) return "Giỏi";
            if (gpa >= 7.0) return "Khá";
            if (gpa >= 5.0) return "Trung bình";
            return "Yếu";
        }
    }
}