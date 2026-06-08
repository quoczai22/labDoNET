using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BT2.Models;
using BT2.Views;

namespace BT2.ViewModels
{
    public class Class1:BaseViewModel
    {
       
        // ── Thông tin nhân viên đang đăng nhập ───────────────────────────────
        private NHANVIEN _currentNhanVien;

   
        public string GreetingMessage
            => _currentNhanVien != null
                ? $"Xin chào {_currentNhanVien.TenNV}"
                : "Xin chào Chưa đăng nhập";


        public bool IsQuanLy
            => _currentNhanVien != null && _currentNhanVien.VaiTro == "Quản lý";

        public bool IsNhanVienKho
            => _currentNhanVien != null && _currentNhanVien.VaiTro == "Nhân viên kho";

        public bool IsNhanVienBanHang
            => _currentNhanVien != null && _currentNhanVien.VaiTro == "Nhân viên bán hàng";

      
        public bool CanAccessDanhMuc => IsQuanLy;

        public bool CanAccessLapPhieuNhap => IsQuanLy || IsNhanVienKho;


        public bool CanAccessThongKeNhap => IsQuanLy || IsNhanVienKho;

    
        public bool CanAccessThongKeTonKho => IsQuanLy || IsNhanVienBanHang;

     
        private object _currentSubView;
        public object CurrentSubView
        {
            get => _currentSubView;
            set { _currentSubView = value; OnPropertyChanged(); }
        }

        // ── Commands ──────────────────────────────────────────────────────────
        public RelayCommand LogoutCommand { get; }
        public RelayCommand OpenLapPhieuNhapCommand { get; }

        public Class1(NHANVIEN nv)
        {
            _currentNhanVien = nv;

            LogoutCommand = new RelayCommand(ExecuteLogout);
         
        }


        private void ExecuteLogout(object obj)
        {
            _currentNhanVien = null;
            CurrentSubView = null;

            OnPropertyChanged(nameof(GreetingMessage));
            OnPropertyChanged(nameof(IsQuanLy));
            OnPropertyChanged(nameof(IsNhanVienKho));
            OnPropertyChanged(nameof(IsNhanVienBanHang));
            OnPropertyChanged(nameof(CanAccessDanhMuc));
            OnPropertyChanged(nameof(CanAccessLapPhieuNhap));
            OnPropertyChanged(nameof(CanAccessThongKeNhap));
            OnPropertyChanged(nameof(CanAccessThongKeTonKho));

            // Đóng cửa sổ chính
            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.DataContext == this)
                ?.Close();
        }
    }
}
