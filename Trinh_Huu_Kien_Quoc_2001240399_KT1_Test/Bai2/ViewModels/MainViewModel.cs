using Bai2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Bai2.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly KhoDuLieu _khoDuLieu;

        public ObservableCollection<LoaiSanPham> DanhSachLoai { get; set; }
        public ObservableCollection<string> DanhSachBan { get; set; }

        private LoaiSanPham _selectedLoai;
        public LoaiSanPham SelectedLoai
        {
            get => _selectedLoai;
            set
            {
                _selectedLoai = value;
                OnPropertyChanged(nameof(SelectedLoai));
                FilterSanPham();
            }
        }

        private ObservableCollection<SanPham> _danhSachSanPhamFiltered;
        public ObservableCollection<SanPham> DanhSachSanPhamFiltered
        {
            get => _danhSachSanPhamFiltered;
            set
            {
                _danhSachSanPhamFiltered = value;
                OnPropertyChanged(nameof(DanhSachSanPhamFiltered));
            }
        }

        private SanPham _selectedSanPham;
        public SanPham SelectedSanPham
        {
            get => _selectedSanPham;
            set
            {
                _selectedSanPham = value;
                OnPropertyChanged(nameof(SelectedSanPham));
            }
        }

        private string _soLuongText;
        public string SoLuongText
        {
            get => _soLuongText;
            set
            {
                _soLuongText = value;
                OnPropertyChanged(nameof(SoLuongText));
            }
        }

        public ObservableCollection<ChiTietHoaDon> DanhSachChiTietHoaDon { get; set; }

        private double _tongTienHienTai;
        public double TongTienHienTai
        {
            get => _tongTienHienTai;
            set
            {
                _tongTienHienTai = value;
                OnPropertyChanged(nameof(TongTienHienTai));
            }
        }

        private string _hoTen;
        public string HoTen
        {
            get => _hoTen;
            set
            {
                _hoTen = value;
                OnPropertyChanged(nameof(HoTen));
            }
        }

        private string _soDienThoai;
        public string SoDienThoai
        {
            get => _soDienThoai;
            set
            {
                _soDienThoai = value;
                OnPropertyChanged(nameof(SoDienThoai));
            }
        }

        private string _selectedBan;
        public string SelectedBan
        {
            get => _selectedBan;
            set
            {
                _selectedBan = value;
                OnPropertyChanged(nameof(SelectedBan));
            }
        }

        public ICommand ThemCommand { get; set; }
        public ICommand TinhTienCommand { get; set; }

        public MainViewModel()
        {
            _khoDuLieu = new KhoDuLieu();
            DanhSachLoai = new ObservableCollection<LoaiSanPham>(_khoDuLieu.DanhSachLoaiSanPham);
            DanhSachSanPhamFiltered = new ObservableCollection<SanPham>();
            DanhSachChiTietHoaDon = new ObservableCollection<ChiTietHoaDon>();
            DanhSachBan = new ObservableCollection<string>
            {
                "Bàn 1 - Tầng trệt",
                "Bàn 2 - Tầng trệt",
                "Bàn 3 - Tầng trệt",
                "Bàn 1 - Tầng 1",
                "Bàn 2 - Tầng 1",
                "Bàn 3 - Tầng 1",
                "Bàn 4 - Tầng 1"
            };

            ThemCommand = new RelayCommand(ThemChiTiet);
            TinhTienCommand = new RelayCommand(TinhTien);
        }

        private void FilterSanPham()
        {
            if (SelectedLoai != null)
            {
                var filtered = _khoDuLieu.DanhSachSanPham
                    .Where(x => x.MaLoai == SelectedLoai.MaLoai)
                    .ToList();
                DanhSachSanPhamFiltered = new ObservableCollection<SanPham>(filtered);
            }
            else
            {
                DanhSachSanPhamFiltered = new ObservableCollection<SanPham>();
            }
        }

        private void ThemChiTiet(object obj)
        {
            if (SelectedSanPham == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm.");
                return;
            }

            if (int.TryParse(SoLuongText, out int soLuong) && soLuong > 0)
            {
                var chiTiet = new ChiTietHoaDon
                {
                    MaSanPham = SelectedSanPham.MaSanPham,
                    TenSanPham = SelectedSanPham.TenSanPham,
                    DonGia = SelectedSanPham.DonGia,
                    SoLuong = soLuong
                };

                DanhSachChiTietHoaDon.Add(chiTiet);
                SoLuongText = string.Empty;
                CapNhatTongTien();
            }
            else
            {
                MessageBox.Show("Số lượng không hợp lệ.");
            }
        }

        private void CapNhatTongTien()
        {
            TongTienHienTai = DanhSachChiTietHoaDon.Sum(x => x.ThanhTien());
        }

        private void TinhTien(object obj)
        {
            if (string.IsNullOrWhiteSpace(HoTen))
            {
                MessageBox.Show("Vui lòng nhập họ tên khách hàng.");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedBan))
            {
                MessageBox.Show("Vui lòng chọn vị trí bàn.");
                return;
            }

            if (DanhSachChiTietHoaDon.Count == 0)
            {
                MessageBox.Show("Hóa đơn chưa có sản phẩm nào.");
                return;
            }

            HoaDon hd = new HoaDon
            {
                TenKhachHang = HoTen,
                DienThoai = SoDienThoai,
                TenBan = SelectedBan,
                DanhSachChiTiet = DanhSachChiTietHoaDon.ToList()
            };

            MessageBox.Show(hd.ToString(), "Thông tin hóa đơn");
        }
    }
}
