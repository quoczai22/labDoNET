using Bai2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bai2.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        // Danh sách nạp dữ liệu (ComboBox, ListBox, CheckBox)
        public ObservableCollection<RoomType> DanhSachLoaiPhong { get; set; }
        public ObservableCollection<ServiceType> DanhSachTienNghi { get; set; }
        public ObservableCollection<ServiceType> DanhSachDichVu { get; set; }

        // Danh sách DataGrid
        public ObservableCollection<Bill> DanhSachHoaDon { get; set; }

        // Thuộc tính Binding Input
        private string _tenKhachHang;
        public string TenKhachHang
        {
            get { return _tenKhachHang; }
            set { _tenKhachHang = value; OnPropertyChanged(nameof(TenKhachHang)); }
        }

        private string _soNgayLuuTruText;
        public string SoNgayLuuTruText
        {
            get { return _soNgayLuuTruText; }
            set { _soNgayLuuTruText = value; OnPropertyChanged(nameof(SoNgayLuuTruText)); }
        }

        private RoomType _selectedRoomType;
        public RoomType SelectedRoomType
        {
            get { return _selectedRoomType; }
            set { _selectedRoomType = value; OnPropertyChanged(nameof(SelectedRoomType)); }
        }

        private Bill _selectedBill;
        public Bill SelectedBill
        {
            get { return _selectedBill; }
            set { _selectedBill = value; OnPropertyChanged(nameof(SelectedBill)); }
        }

        // Thuộc tính Binding Thống kê
        private int _tongLuotKhach;
        public int TongLuotKhach
        {
            get { return _tongLuotKhach; }
            set { _tongLuotKhach = value; OnPropertyChanged(nameof(TongLuotKhach)); }
        }

        private double _tongDoanhThu;
        public double TongDoanhThu
        {
            get { return _tongDoanhThu; }
            set { _tongDoanhThu = value; OnPropertyChanged(nameof(TongDoanhThu)); }
        }

        // Commands
        public ICommand ThanhToanCommand { get; set; }
        public ICommand XoaDongCommand { get; set; }
        public ICommand NhapMoiCommand { get; set; }
        public ICommand ThoatCommand { get; set; }

        public MainViewModel()
        {
            DanhSachHoaDon = new ObservableCollection<Bill>();

            KhoiTaoDuLieu();
            ThietLapGiaTriMacDinh();

            ThanhToanCommand = new RelayCommand(ThanhToan);
            XoaDongCommand = new RelayCommand(XoaDong);
            NhapMoiCommand = new RelayCommand(NhapMoi);
            ThoatCommand = new RelayCommand(Thoat);
        }

        private void KhoiTaoDuLieu()
        {
            DanhSachLoaiPhong = new ObservableCollection<RoomType>
            {
                new RoomType { Ten = "Phòng đơn", Gia = 300000 },
                new RoomType { Ten = "Phòng đôi", Gia = 350000 },
                new RoomType { Ten = "Phòng VIP", Gia = 500000 }
            };

            DanhSachTienNghi = new ObservableCollection<ServiceType>
            {
                new ServiceType { Ten = "Mỗi tiện nghi", Gia = 10000 }
                // Nếu có thêm tiện nghi cụ thể, có thể add thêm tại đây
            };

            DanhSachDichVu = new ObservableCollection<ServiceType>
            {
                new ServiceType { Ten = "Karaoke", Gia = 50000 },
                new ServiceType { Ten = "Dịch vụ làm đẹp", Gia = 100000 }
            };
        }

        private void ThietLapGiaTriMacDinh()
        {
            TenKhachHang = string.Empty;
            SoNgayLuuTruText = string.Empty;
            if (DanhSachLoaiPhong.Any())
                SelectedRoomType = DanhSachLoaiPhong.First();

            foreach (var tn in DanhSachTienNghi) tn.IsSelected = false;
            foreach (var dv in DanhSachDichVu) dv.IsSelected = false;
        }

        private void ThanhToan(object obj)
        {
            // Kiểm tra dữ liệu
            if (string.IsNullOrWhiteSpace(TenKhachHang))
            {
                MessageBox.Show("Tên khách không được để trống.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(SoNgayLuuTruText, out int soNgay) || soNgay <= 0)
            {
                MessageBox.Show("Số ngày lưu trú phải là số và > 0.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedRoomType == null)
            {
                MessageBox.Show("Vui lòng chọn loại phòng.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Tính tiền
            double giaPhong = SelectedRoomType.Gia;
            double tongTienTienNghi = DanhSachTienNghi.Where(x => x.IsSelected).Sum(x => x.Gia);
            double tongTienDichVu = DanhSachDichVu.Where(x => x.IsSelected).Sum(x => x.Gia);

            double tongTien = (giaPhong + tongTienTienNghi + tongTienDichVu) * soNgay;

            // Tạo chuỗi Tiện nghi & Dịch vụ
            string tienNghi = string.Join(", ", DanhSachTienNghi.Where(x => x.IsSelected).Select(x => x.Ten));
            string dichVu = string.Join(", ", DanhSachDichVu.Where(x => x.IsSelected).Select(x => x.Ten));

            // Thêm dữ liệu vào lưới
            Bill newBill = new Bill
            {
                STT = DanhSachHoaDon.Count + 1,
                TenKhach = TenKhachHang,
                LoaiPhong = SelectedRoomType.Ten,
                TienNghi = string.IsNullOrEmpty(tienNghi) ? "Không" : tienNghi,
                DichVu = string.IsNullOrEmpty(dichVu) ? "Không" : dichVu,
                SoNgay = soNgay,
                ThanhTien = tongTien
            };

            DanhSachHoaDon.Add(newBill);
            CapNhatThongKe();
        }

        private void XoaDong(object obj)
        {
            if (SelectedBill != null)
            {
                DanhSachHoaDon.Remove(SelectedBill);

                // Cập nhật lại STT
                for (int i = 0; i < DanhSachHoaDon.Count; i++)
                {
                    DanhSachHoaDon[i].STT = i + 1;
                }

                CapNhatThongKe();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void NhapMoi(object obj)
        {
            ThietLapGiaTriMacDinh();
        }

        private void CapNhatThongKe()
        {
            TongLuotKhach = DanhSachHoaDon.Count;
            TongDoanhThu = DanhSachHoaDon.Sum(x => x.ThanhTien);
        }

        private void Thoat(object obj)
        {
            if (obj is Window window)
            {
                window.Close();
            }
        }
    }
}
