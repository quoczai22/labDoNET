using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BTVN.Models;

namespace BTVN.ViewModels
{
    public class PhieuNhapViewModel : BaseViewModel
    {
        QLHangHoaEntities db = new QLHangHoaEntities();

        string _maPN;

        public string NgayNhap { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");

        public ObservableCollection<NHACUNGCAP> DanhSachNhacc { get; set; }

        private NHACUNGCAP _selectedNCC;
        public NHACUNGCAP SelectedNCC
        {
            get => _selectedNCC;
            set { _selectedNCC = value; OnPropertyChanged(nameof(SelectedNCC)); }
        }

        public ObservableCollection<SANPHAM> DanhSachSP { get; set; }

        private SANPHAM _selectedSP;
        public SANPHAM SelectedSP
        {
            get => _selectedSP;
            set { _selectedSP = value; OnPropertyChanged(nameof(SelectedSP)); }
        }

        int _soLuong;
        public int SoLuong { get => _soLuong; set { _soLuong = value; OnPropertyChanged(nameof(SoLuong)); } }

        decimal _donGia;
        public decimal DonGia { get => _donGia; set { _donGia = value; OnPropertyChanged(nameof(DonGia)); } }

        decimal _tongTien;
        public decimal TongTien { get => _tongTien; set { _tongTien = value; OnPropertyChanged(nameof(TongTien)); } }

        public ObservableCollection<ChiTietPhieuNhapModel> DanhSachChiTiet { get; set; }

        private ChiTietPhieuNhapModel _selectedChiTiet;
        public ChiTietPhieuNhapModel SelectedChiTiet
        {
            get => _selectedChiTiet;
            set { _selectedChiTiet = value; OnPropertyChanged(nameof(SelectedChiTiet)); }
        }

        private Visibility _btnTaoPhieuVisibility = Visibility.Visible;
        public Visibility BtnTaoPhieuVisibility
        {
            get => _btnTaoPhieuVisibility;
            set { _btnTaoPhieuVisibility = value; OnPropertyChanged(nameof(BtnTaoPhieuVisibility)); }
        }

        private bool _isNccEnabled = true;
        public bool IsNccEnabled
        {
            get => _isNccEnabled;
            set { _isNccEnabled = value; OnPropertyChanged(nameof(IsNccEnabled)); }
        }

        public ICommand TaoPhieuCommand { get; set; }
        public ICommand ThemCommand { get; set; }
        public ICommand XoaCommand { get; set; }
        public ICommand LuuCommand { get; set; }
        public ICommand HuyCommand { get; set; }

        public PhieuNhapViewModel()
        {
            LoadData();
            TaoPhieuCommand = new RelayCommand(TaoPhieu, CanTaoPhieu);
            ThemCommand = new RelayCommand(ThemChiTiet, CanThemChiTiet);
            XoaCommand = new RelayCommand(XoaChiTiet, CanXoaChiTiet);
            LuuCommand = new RelayCommand(LuuPhieu, CanLuuPhieu);
            HuyCommand = new RelayCommand(HuyPhieu, CanHuyPhieu);
        }

        void LoadData()
        {
            DanhSachNhacc = new ObservableCollection<NHACUNGCAP>(db.NHACUNGCAPs.ToList());
            DanhSachSP = new ObservableCollection<SANPHAM>(db.SANPHAMs.ToList());
            DanhSachChiTiet = new ObservableCollection<ChiTietPhieuNhapModel>();
        }

        bool CanTaoPhieu(object p)
        {
            return SelectedNCC != null && IsNccEnabled;
        }

        bool CanThemChiTiet(object p)
        {
            return !IsNccEnabled && SelectedSP != null && SoLuong > 0 && DonGia > 0;
        }

        bool CanXoaChiTiet(object p)
        {
            return SelectedChiTiet != null;
        }

        bool CanLuuPhieu(object p)
        {
            return !IsNccEnabled && DanhSachChiTiet != null && DanhSachChiTiet.Count > 0;
        }

        bool CanHuyPhieu(object p)
        {
            return true;
        }

        void TaoPhieu(object p)
        {
            var maxId = db.PHIEUNHAPs.Select(x => x.MAPHIEUNHAP).Max();
            int nextNum = 1;
            if (!string.IsNullOrEmpty(maxId) && maxId.StartsWith("PN"))
            {
                int.TryParse(maxId.Substring(2), out nextNum);
                nextNum++;
            }
            _maPN = "PN" + nextNum.ToString("D6");

            BtnTaoPhieuVisibility = Visibility.Collapsed;
            IsNccEnabled = false;
        }

        void ThemChiTiet(object p)
        {
            var item = new ChiTietPhieuNhapModel
            {
                MaSanPham = SelectedSP.MASANPHAM,
                TenSanPham = SelectedSP.TENSANPHAM,
                SoLuong = SoLuong,
                DonGia = DonGia,
                ThanhTien = SoLuong * DonGia
            };

            DanhSachChiTiet.Add(item);

            TongTien = DanhSachChiTiet.Sum(x => x.ThanhTien);

            SoLuong = 0;
            DonGia = 0;
        }

        void XoaChiTiet(object p)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa chi tiết này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DanhSachChiTiet.Remove(SelectedChiTiet);
                TongTien = DanhSachChiTiet.Sum(x => x.ThanhTien);
            }
        }

        void LuuPhieu(object p)
        {
            try
            {
                var pn = new PHIEUNHAP
                {
                    MAPHIEUNHAP = _maPN,
                    MANCC = SelectedNCC.MANCC,
                    NGAYNHAP = DateTime.Now,
                    THANHTIEN = TongTien,
                    MANV = "NV01"
                };
                db.PHIEUNHAPs.Add(pn);

                foreach (var item in DanhSachChiTiet)
                {
                    var ct = new CHITIETPHIEUNHAP
                    {
                        MAPHIEUNHAP = _maPN,
                        MASANPHAM = item.MaSanPham,
                        DONGIA = item.DonGia,
                        SOLUONG = item.SoLuong
                    };
                    db.CHITIETPHIEUNHAPs.Add(ct);

                    var tonKho = db.TONKHO_NGAY.FirstOrDefault(x => x.MASANPHAM == item.MaSanPham);
                    if (tonKho != null)
                    {
                        tonKho.SOLUONGTON += item.SoLuong;
                    }
                    else
                    {
                        db.TONKHO_NGAY.Add(new TONKHO_NGAY { MASANPHAM = item.MaSanPham, SOLUONGTON = item.SoLuong });
                    }
                }

                db.SaveChanges();
                MessageBox.Show("Lưu phiếu nhập thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                HuyPhieu(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void HuyPhieu(object p)
        {
            DanhSachChiTiet.Clear();
            SelectedNCC = null;
            SelectedSP = null;
            SoLuong = 0;
            DonGia = 0;
            TongTien = 0;
            _maPN = null;

            BtnTaoPhieuVisibility = Visibility.Visible;
            IsNccEnabled = true;
        }
    }
}
