using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BTVN.ViewModels
{
    public class MainViewModel : BaseViewModel
    {// 1. Lưu trữ thông tin nhân viên đăng nhập
        private NHANVIEN _currentNhanVien;
        public NHANVIEN CurrentNhanVien
        {
            get => _currentNhanVien;
            set
            {
                _currentNhanVien = value;
                OnPropertyChanged(nameof(CurrentNhanVien));
                OnPropertyChanged(nameof(GreetingMessage));

                // Gọi hàm phân quyền mỗi khi có nhân viên mới
                PhanQuyen();
            }
        }

        // Lời chào hiển thị trên giao diện
        public string GreetingMessage => CurrentNhanVien != null ? $"Xin chào {CurrentNhanVien.TenNV}" : "Xin chào Chưa đăng nhập";

        // 2. Các cờ Binding điều khiển IsEnabled trên View
        private bool _isQuanLy;
        public bool IsQuanLy
        {
            get => _isQuanLy;
            set { _isQuanLy = value; OnPropertyChanged(nameof(IsQuanLy)); }
        }

        private bool _canAccessDanhMuc;
        public bool CanAccessDanhMuc
        {
            get => _canAccessDanhMuc;
            set { _canAccessDanhMuc = value; OnPropertyChanged(nameof(CanAccessDanhMuc)); }
        }

        private bool _canAccessLapPhieuNhap;
        public bool CanAccessLapPhieuNhap
        {
            get => _canAccessLapPhieuNhap;
            set { _canAccessLapPhieuNhap = value; OnPropertyChanged(nameof(CanAccessLapPhieuNhap)); }
        }

        private bool _canAccessThongKeTonKho;
        public bool CanAccessThongKeTonKho
        {
            get => _canAccessThongKeTonKho;
            set { _canAccessThongKeTonKho = value; OnPropertyChanged(nameof(CanAccessThongKeTonKho)); }
        }

        private bool _canAccessThongKeNhap;
        public bool CanAccessThongKeNhap
        {
            get => _canAccessThongKeNhap;
            set { _canAccessThongKeNhap = value; OnPropertyChanged(nameof(CanAccessThongKeNhap)); }
        }

        // 3. Khai báo các Command
        public ICommand LogoutCommand { get; set; }
        public ICommand OpenLapPhieuNhapCommand { get; set; }

        // Constructor nhận vào User đang đăng nhập
        public MainViewModel(NHANVIEN nhanVien)
        {
            CurrentNhanVien = nhanVien;

            // Khởi tạo Commands
            LogoutCommand = new RelayCommand(DangXuat);
            OpenLapPhieuNhapCommand = new RelayCommand(MoFormPhieuNhap);
        }

        // 4. Logic phân quyền (RBAC)
        private void PhanQuyen()
        {
            if (CurrentNhanVien == null)
            {
                // Vô hiệu hóa toàn bộ nếu chưa đăng nhập
                IsQuanLy = CanAccessDanhMuc = CanAccessLapPhieuNhap = CanAccessThongKeTonKho = CanAccessThongKeNhap = false;
                return;
            }

            string vaiTro = CurrentNhanVien.VaiTro;

            // Quản lý: Có toàn quyền
            IsQuanLy = vaiTro == "Quản lý";
            CanAccessDanhMuc = IsQuanLy;

            // Nhân viên kho: Được lập phiếu nhập, thống kê nhập
            CanAccessLapPhieuNhap = IsQuanLy || vaiTro == "Nhân viên kho";
            CanAccessThongKeNhap = IsQuanLy || vaiTro == "Nhân viên kho";

            // Nhân viên bán hàng: Được xem thống kê tồn kho
            CanAccessThongKeTonKho = IsQuanLy || vaiTro == "Nhân viên bán hàng";
        }

        // 5. Xử lý Đăng xuất
        private void DangXuat(object parameter)
        {
            CurrentNhanVien = null; // Xóa session

            // Mở lại màn hình đăng nhập (Giả định tên form là frmDangNhap)
            Views.frmDangNhap frmLogin = new Views.frmDangNhap();
            frmLogin.Show();

            // Đóng màn hình chính hiện tại
            foreach (Window window in Application.Current.Windows)
            {
                if (window is Views.frmMain)
                {
                    window.Close();
                    break;
                }
            }
        }

        // 6. Xử lý Mở các Form con
        private void MoFormPhieuNhap(object parameter)
        {
            // Mở cửa sổ Phiếu nhập (Giả định tên form là frmPhieuNhap)
            Views.frmPhieuNhap frmPN = new Views.frmPhieuNhap();
            frmPN.ShowDialog();
        }
    }
}
