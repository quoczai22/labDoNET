using _2001240399_Trinh_Huu_Kien_Quoc_KTL2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _2001240399_Trinh_Huu_Kien_Quoc_KTL2.ViewModels
{
    public class Bai3ViewModel:BaseViewModel
    {
        QL_KaraokeEntities1 db = new QL_KaraokeEntities1();

        public ObservableCollection<PHONG> DanhSachPhong { get; set; }
        public ObservableCollection<ChiTietPhuThuModel> DanhSachChiTietPhuThu { get; set; }
        public ObservableCollection<KHACHHANG> DanhSachKhachHang { get; set; }
        public ObservableCollection<PHUTHU> DanhSachPhuThu { get; set; }

        public string NgayDatPhong { get; set; }=DateTime.Now.ToString("dd/MM/yyyy");

        PHONG _selectedPhong;
        public PHONG SelectedPhong
        {
            get { return _selectedPhong; }
            set
            {
                _selectedPhong = value;
                OnPropertyChanged(nameof(SelectedPhong));
                if (_selectedPhong != null)
                {
                    GiaPhongDTO = _selectedPhong.GiaPhong.ToString();
                    SucChuaDTO = _selectedPhong.SucChua.ToString();
                }
                else
                {
                    GiaPhongDTO = string.Empty;
                    SucChuaDTO = string.Empty;
                }
                TinhTongTien();
                CommandManager.InvalidateRequerySuggested();
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

        KHACHHANG _selectedKhachHang;
        public KHACHHANG SelectedKhachHang
        {
            get { return _selectedKhachHang; }
            set
            {
                _selectedKhachHang = value;
                OnPropertyChanged(nameof(SelectedKhachHang));
                CommandManager.InvalidateRequerySuggested();
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
                CommandManager.InvalidateRequerySuggested();
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
                CommandManager.InvalidateRequerySuggested();
            }
        }

        PHUTHU _selectedPhuThu;
        public PHUTHU SelectedPhuThu
        {
            get { return _selectedPhuThu; } 
            set
            {
                _selectedPhuThu = value;
                OnPropertyChanged(nameof(SelectedPhuThu));
                if(SelectedPhuThu != null)
                {
                    GiaPhuThuDTO = SelectedPhuThu.GiaPT.ToString();
                }
                CommandManager.InvalidateRequerySuggested();
            }
        }

        string _giaPhuThuDTO;
        public string GiaPhuThuDTO
        {
            get { return _giaPhuThuDTO; }
            set { _giaPhuThuDTO = value; OnPropertyChanged(nameof(GiaPhuThuDTO)); }
        }

        int _soLuongDTO;
        public int SsoLuongDTO
        {
            get { return _soLuongDTO; }
            set
            {
                _soLuongDTO = value;
                OnPropertyChanged(nameof(SsoLuongDTO));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        decimal tongTienTamTinh;
        public decimal TongTienTamTinh
        {
            get
            {
                return tongTienTamTinh;
            }
            set { tongTienTamTinh = value; OnPropertyChanged(nameof(TongTienTamTinh)); }
        }

        public ICommand ThemPhuThuCommand { get; set; }
        public ICommand DatPhongCommand { get; set; }

        public Bai3ViewModel()
        { 
            LoadData();
            ThemPhuThuCommand = new RelayCommand(ThemPhuThu, CanThemPhuThu);
            DatPhongCommand = new RelayCommand(DatPhong, CanDatPhong);
        }

        bool CanThemPhuThu(object p)
        {
            return SelectedPhong != null && SelectedPhuThu != null && SsoLuongDTO > 0;
        }
        bool CanDatPhong(object p)
        {
            return SelectedPhong != null && SelectedKhachHang != null && !string.IsNullOrEmpty(GioVaoDTO) && !string.IsNullOrEmpty(GioRaDTO);
        }

        void ThemPhuThu(object p)
        {
            var ctPhuThu = new ChiTietPhuThuModel()
            {
                MaPhuThu = SelectedPhuThu.MaPhuThu,
                TenPhuThu = SelectedPhuThu.TenPhuThu,
                GiaPhuThu = SelectedPhuThu.GiaPT ?? 0,
                SoLuong = SsoLuongDTO,
                ThanhTien = (SelectedPhuThu.GiaPT ?? 0) * SsoLuongDTO
            };
            DanhSachChiTietPhuThu.Add(ctPhuThu);
            TinhTongTien();
        }

        void DatPhong(object p)
        {
            try
            {
                // 1. Ghép ngày đặt và giờ vào/ra thành DateTime hoàn chỉnh
                DateTime ngayDat = DateTime.ParseExact(NgayDatPhong, "dd/MM/yyyy", null);
                if (!TimeSpan.TryParse(GioVaoDTO, out TimeSpan gioVao) || !TimeSpan.TryParse(GioRaDTO, out TimeSpan gioRa))
                {
                    System.Windows.MessageBox.Show("Giờ vào/giờ ra không hợp lệ. Vui lòng nhập theo dạng HH:mm, ví dụ 19:30.");
                    return;
                }

                if (gioRa <= gioVao)
                {
                    System.Windows.MessageBox.Show("Giờ ra phải lớn hơn giờ vào.");
                    return;
                }

                DateTime thoiGianVao = ngayDat.Add(gioVao);
                DateTime thoiGianRa = ngayDat.Add(gioRa);

                bool biTrungLich = db.DATPHONGs.Any(x =>
                    x.MaPh == SelectedPhong.MaPhong &&
                    x.NgayDat < thoiGianRa &&
                    x.NgayTra > thoiGianVao);

                if (biTrungLich)
                {
                    System.Windows.MessageBox.Show("Phòng này đã được đặt trong khoảng thời gian đã chọn.");
                    return;
                }

                // 2. Tạo phiếu đặt phòng
                var datPhong = new DATPHONG
                {
                    MaKH = SelectedKhachHang.MaKhachHang,
                    MaPh = SelectedPhong.MaPhong,
                    NgayDat = thoiGianVao,
                    NgayTra = thoiGianRa
                };

                db.DATPHONGs.Add(datPhong);
                db.SaveChanges(); // Lưu phiếu đặt để CSDL sinh ra MaDatPhong tự động 

                // 3. Lưu danh sách phụ thu vào bảng CHITIETDATPHONG
                foreach (var item in DanhSachChiTietPhuThu)
                {
                    var ct = new CHITIETDATPHONG
                    {
                        MaDP = datPhong.MaDatPhong, // Lấy ID vừa được sinh ra
                        MaPT = item.MaPhuThu,
                        SL = item.SoLuong
                    };
                    db.CHITIETDATPHONGs.Add(ct);
                }

                if (DanhSachChiTietPhuThu.Count > 0)
                {
                    db.SaveChanges();
                }

                System.Windows.MessageBox.Show("Đặt phòng thành công!");

                // Reset lại giỏ hàng sau khi đặt xong
                DanhSachChiTietPhuThu.Clear();
                TinhTongTien();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        void LoadData()
        {
            DanhSachPhong = new ObservableCollection<PHONG>(db.PHONGs.ToList());
            DanhSachKhachHang = new ObservableCollection<KHACHHANG>(db.KHACHHANGs.ToList());
            DanhSachPhuThu = new ObservableCollection<PHUTHU>(db.PHUTHUs.ToList());
            DanhSachChiTietPhuThu = new ObservableCollection<ChiTietPhuThuModel>();
        }

        void TinhTongTien()
        {
            try
            {
                decimal tongTienPhuThu = DanhSachChiTietPhuThu.Sum(c => c.ThanhTien ?? 0);
                decimal tienHat = 0;

                // Tính tiền hát theo giờ = (giờ ra - giờ vào) * giá phòng 
                if (!string.IsNullOrWhiteSpace(GioVaoDTO) && !string.IsNullOrWhiteSpace(GioRaDTO) && SelectedPhong != null)
                {
                    if (!TimeSpan.TryParse(GioVaoDTO, out TimeSpan thoiGianVao) || !TimeSpan.TryParse(GioRaDTO, out TimeSpan thoiGianRa))
                    {
                        return;
                    }

                    double soGio = (thoiGianRa - thoiGianVao).TotalHours;

                    if (soGio > 0)
                    {
                        tienHat = (decimal)soGio * (SelectedPhong.GiaPhong ?? 0);
                    }
                }

                TongTienTamTinh = tongTienPhuThu + tienHat;
            }
            catch
            {
                // Bỏ qua lỗi nếu người dùng nhập giờ chưa đúng định dạng
            }
        }
    }
}
