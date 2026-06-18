using _2001240399_Trinh_Huu_Kien_Quoc_KT2L2.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace _2001240399_Trinh_Huu_Kien_Quoc_KT2L2.ViewModels
{
    public class Bai3ViewModel : BaseViewModel
    {
        QuanLyKhachSanEntities db = new QuanLyKhachSanEntities();
        int? _maDatPhongHienTai;

        public ObservableCollection<PHONG> DanhSachPhong { get; set; }
        public ObservableCollection<KHACHHANG> DanhSachKhachHang { get; set; }
        public ObservableCollection<DICHVU> DanhSachDichVu { get; set; }
        public ObservableCollection<ChiTietDichVuModel> DanhSachChiTietDichVu { get; set; }

        string _ngayDatPhong = DateTime.Now.ToString("dd/MM/yyyy");
        public string NgayDatPhong
        {
            get { return _ngayDatPhong; }
            set { _ngayDatPhong = value; OnPropertyChanged(nameof(NgayDatPhong)); ResetPhieuHienTai(); }
        }

        PHONG _selectedPhong;
        public PHONG SelectedPhong
        {
            get { return _selectedPhong; }
            set
            {
                _selectedPhong = value;
                OnPropertyChanged(nameof(SelectedPhong));
                if (value != null)
                {
                    GiaPhongDTO = value.GiaPhong.ToString("N0");
                    SucChuaDTO = value.SucChua.ToString();
                    TinhTrangDTO = value.TinhTrang == 0 ? "Phòng trống" : "Khách đang nhận phòng";
                }
                else
                {
                    GiaPhongDTO = string.Empty;
                    SucChuaDTO = string.Empty;
                    TinhTrangDTO = string.Empty;
                }
                TinhTongTien();
                ResetPhieuHienTai();
            }
        }

        KHACHHANG _selectedKhachHang;
        public KHACHHANG SelectedKhachHang
        {
            get { return _selectedKhachHang; }
            set
            {
                _selectedKhachHang = value;
                OnPropertyChanged(nameof(SelectedKhachHang));
                ResetPhieuHienTai();
            }
        }

        string _gioVaoDTO;
        public string GioVaoDTO
        {
            get { return _gioVaoDTO; }
            set
            {
                _gioVaoDTO = value;
                OnPropertyChanged(nameof(GioVaoDTO));
                TinhTongTien();
                ResetPhieuHienTai();
            }
        }

        string _gioRaDTO;
        public string GioRaDTO
        {
            get { return _gioRaDTO; }
            set
            {
                _gioRaDTO = value;
                OnPropertyChanged(nameof(GioRaDTO));
                TinhTongTien();
                ResetPhieuHienTai();
            }
        }

        DICHVU _selectedDichVu;
        public DICHVU SelectedDichVu
        {
            get { return _selectedDichVu; }
            set
            {
                _selectedDichVu = value;
                OnPropertyChanged(nameof(SelectedDichVu));
                GiaDichVuDTO = value != null ? value.GiaDV.ToString("N0") : string.Empty;
            }
        }

        int _soLuongDTO;
        public int SoLuongDTO
        {
            get { return _soLuongDTO; }
            set
            {
                _soLuongDTO = value;
                OnPropertyChanged(nameof(SoLuongDTO));
            }
        }

        string _giaPhongDTO;
        public string GiaPhongDTO
        {
            get { return _giaPhongDTO; }
            set { _giaPhongDTO = value; OnPropertyChanged(nameof(GiaPhongDTO)); }
        }

        string _sucChuaDTO;
        public string SucChuaDTO
        {
            get { return _sucChuaDTO; }
            set { _sucChuaDTO = value; OnPropertyChanged(nameof(SucChuaDTO)); }
        }

        string _tinhTrangDTO;
        public string TinhTrangDTO
        {
            get { return _tinhTrangDTO; }
            set { _tinhTrangDTO = value; OnPropertyChanged(nameof(TinhTrangDTO)); }
        }

        string _giaDichVuDTO;
        public string GiaDichVuDTO
        {
            get { return _giaDichVuDTO; }
            set { _giaDichVuDTO = value; OnPropertyChanged(nameof(GiaDichVuDTO)); }
        }

        decimal _tongTienTamTinh;
        public decimal TongTienTamTinh
        {
            get { return _tongTienTamTinh; }
            set { _tongTienTamTinh = value; OnPropertyChanged(nameof(TongTienTamTinh)); }
        }

        public ICommand ThemDichVuCommand { get; set; }
        public ICommand DatPhongCommand { get; set; }

        public Bai3ViewModel()
        {
            LoadData();
            ThemDichVuCommand = new RelayCommand(ThemDichVu, p => SelectedDichVu != null && SoLuongDTO > 0 && (SelectedPhong != null || _maDatPhongHienTai != null));
            DatPhongCommand = new RelayCommand(DatPhong, p => SelectedPhong != null && SelectedKhachHang != null && !string.IsNullOrWhiteSpace(GioVaoDTO) && !string.IsNullOrWhiteSpace(GioRaDTO));
        }

        void LoadData()
        {
            DanhSachPhong = new ObservableCollection<PHONG>(db.PHONGs.ToList());
            DanhSachKhachHang = new ObservableCollection<KHACHHANG>(db.KHACHHANGs.ToList());
            DanhSachDichVu = new ObservableCollection<DICHVU>(db.DICHVUs.ToList());
            DanhSachChiTietDichVu = new ObservableCollection<ChiTietDichVuModel>();
        }

        void ThemDichVu(object p)
        {
            if (SelectedDichVu == null)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ.");
                return;
            }

            if (SoLuongDTO <= 0)
            {
                MessageBox.Show("Số lượng dịch vụ phải lớn hơn 0.");
                return;
            }

            if (SelectedPhong == null && _maDatPhongHienTai == null)
            {
                MessageBox.Show("Vui lòng chọn phòng trước khi thêm dịch vụ.");
                return;
            }

            var chiTiet = new ChiTietDichVuModel
            {
                MaDV = SelectedDichVu.MaDV,
                TenDV = SelectedDichVu.TenDV,
                GiaDV = SelectedDichVu.GiaDV,
                SoLuong = SoLuongDTO,
                ThanhTien = SelectedDichVu.GiaDV * SoLuongDTO
            };

            if (_maDatPhongHienTai != null)
            {
                try
                {
                    db.CHITIETDATPHONGs.Add(new CHITIETDATPHONG
                    {
                        MaDatPhong = _maDatPhongHienTai.Value,
                        MaDV = chiTiet.MaDV,
                        SoLuong = chiTiet.SoLuong
                    });
                    db.SaveChanges();
                    DanhSachChiTietDichVu.Add(chiTiet);
                    TinhTongTien();
                    MessageBox.Show("Đã thêm dịch vụ vào phiếu đặt phòng hiện tại.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi thêm dịch vụ: " + ex.Message);
                }
                return;
            }

            DanhSachChiTietDichVu.Add(chiTiet);
            TinhTongTien();
        }

        void DatPhong(object p)
        {
            try
            {
                if (SelectedPhong == null || SelectedKhachHang == null)
                {
                    MessageBox.Show("Vui lòng chọn phòng và khách hàng.");
                    return;
                }

                if (SelectedPhong.TinhTrang == 1)
                {
                    MessageBox.Show("Phòng này đang có khách nhận phòng, vui lòng chọn phòng khác.");
                    return;
                }

                if (!DateTime.TryParseExact(NgayDatPhong, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime ngayDat))
                {
                    MessageBox.Show("Ngày đặt không hợp lệ. Vui lòng nhập dạng dd/MM/yyyy.");
                    return;
                }

                if (!TimeSpan.TryParse(GioVaoDTO, out TimeSpan gioVao) || !TimeSpan.TryParse(GioRaDTO, out TimeSpan gioRa))
                {
                    MessageBox.Show("Giờ vào/giờ ra không hợp lệ. Ví dụ: 14:00.");
                    return;
                }

                if (gioRa <= gioVao)
                {
                    MessageBox.Show("Giờ ra phải lớn hơn giờ vào.");
                    return;
                }

                bool trungLich = db.DATPHONGs.Any(x =>
                    x.MaPh == SelectedPhong.MaPhong &&
                    x.NgayDat == ngayDat &&
                    x.GioVao < gioRa &&
                    x.GioRa > gioVao);

                if (trungLich)
                {
                    MessageBox.Show("Phòng này đã được đặt trong khoảng thời gian đã chọn.");
                    return;
                }

                var datPhong = new DATPHONG
                {
                    MaPh = SelectedPhong.MaPhong,
                    MaKH = SelectedKhachHang.MaKH,
                    NgayDat = ngayDat,
                    NgayTra = ngayDat,
                    GioVao = gioVao,
                    GioRa = gioRa
                };

                db.DATPHONGs.Add(datPhong);
                SelectedPhong.TinhTrang = 1;
                db.SaveChanges();

                foreach (var item in DanhSachChiTietDichVu)
                {
                    db.CHITIETDATPHONGs.Add(new CHITIETDATPHONG
                    {
                        MaDatPhong = datPhong.MaDatPhong,
                        MaDV = item.MaDV,
                        SoLuong = item.SoLuong
                    });
                }

                if (DanhSachChiTietDichVu.Count > 0)
                {
                    db.SaveChanges();
                }

                _maDatPhongHienTai = datPhong.MaDatPhong;
                TinhTrangDTO = "Khách đang nhận phòng";
                MessageBox.Show("Đặt phòng thành công. Bạn có thể thêm dịch vụ tiếp cho phiếu đặt này.");
                TinhTongTien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đặt phòng: " + ex.Message);
            }
        }

        void TinhTongTien()
        {
            decimal tienPhong = 0;
            if (SelectedPhong != null && TimeSpan.TryParse(GioVaoDTO, out TimeSpan gioVao) && TimeSpan.TryParse(GioRaDTO, out TimeSpan gioRa))
            {
                double soGio = (gioRa - gioVao).TotalHours;
                if (soGio > 0)
                {
                    tienPhong = (decimal)soGio * SelectedPhong.GiaPhong;
                }
            }

            decimal tienDichVu = DanhSachChiTietDichVu != null ? DanhSachChiTietDichVu.Sum(x => x.ThanhTien) : 0;
            TongTienTamTinh = tienPhong + tienDichVu;
        }

        void ResetPhieuHienTai()
        {
            if (_maDatPhongHienTai != null)
            {
                DanhSachChiTietDichVu?.Clear();
            }
            _maDatPhongHienTai = null;
            TinhTongTien();
        }
    }
}
