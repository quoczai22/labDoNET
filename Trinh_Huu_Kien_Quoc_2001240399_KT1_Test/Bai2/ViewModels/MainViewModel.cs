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
        private KhoDuLieu _khoDuLieu;

        public ObservableCollection<LoaiSanPham> DanhSachLoai { get; set; }

        private LoaiSanPham _selectedLoai;
        public LoaiSanPham SelectedLoai
        {
            get { return _selectedLoai; }
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
            get { return _danhSachSanPhamFiltered; }
            set { _danhSachSanPhamFiltered = value; OnPropertyChanged(nameof(DanhSachSanPhamFiltered)); }
        }

        public SanPham SelectedSanPham { get; set; }

        private string _soLuongText;
        public string SoLuongText
        {
            get { return _soLuongText; }
            set { _soLuongText = value; OnPropertyChanged(nameof(SoLuongText)); }
        }

        public ObservableCollection<ChiTietHoaDon> DanhSachChiTietHoaDon { get; set; }

        private double _tongTienHienTai;
        public double TongTienHienTai
        {
            get { return _tongTienHienTai; }
            set { _tongTienHienTai = value; OnPropertyChanged(nameof(TongTienHienTai)); }
        }

        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }

        public ICommand ThemCommand { get; set; }
        public ICommand TinhTienCommand { get; set; }

        public MainViewModel()
        {
            _khoDuLieu = new KhoDuLieu();
            DanhSachLoai = new ObservableCollection<LoaiSanPham>(_khoDuLieu.DanhSachLoaiSanPham);
            DanhSachSanPhamFiltered = new ObservableCollection<SanPham>();
            DanhSachChiTietHoaDon = new ObservableCollection<ChiTietHoaDon>();

            ThemCommand = new RelayCommand(ThemChiTiet);
            TinhTienCommand = new RelayCommand(TinhTien);
        }

        private void FilterSanPham()
        {
            if (SelectedLoai != null)
            {
                var filtered = _khoDuLieu.DanhSachSanPham.Where(x => x.MaLoai == SelectedLoai.MaLoai).ToList();
                DanhSachSanPhamFiltered = new ObservableCollection<SanPham>(filtered);
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
            HoaDon hd = new HoaDon
            {
                TenKhachHang = this.HoTen,
                DienThoai = this.SoDienThoai,
                TenBan = "Bàn đã chọn", // Logic lấy bàn tương tự nếu anh liên kết list vị trí
                DanhSachChiTiet = this.DanhSachChiTietHoaDon.ToList()
            };

            MessageBox.Show(hd.ToString(), "Thông tin hóa đơn");
        }
    }
}
