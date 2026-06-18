using _2001240399_Trinh_Huu_Kien_Quoc_KT2L2.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace _2001240399_Trinh_Huu_Kien_Quoc_KT2L2.ViewModels
{
    public class Bai2ViewModel : BaseViewModel
    {
        QuanLyKhachSanEntities db = new QuanLyKhachSanEntities();

        ObservableCollection<LOAIPHONG> _danhSachLoaiPhong;
        public ObservableCollection<LOAIPHONG> DanhSachLoaiPhong
        {
            get { return _danhSachLoaiPhong; }
            set { _danhSachLoaiPhong = value; OnPropertyChanged(nameof(DanhSachLoaiPhong)); }
        }

        LOAIPHONG _selectedLoaiPhong;
        public LOAIPHONG SelectedLoaiPhong
        {
            get { return _selectedLoaiPhong; }
            set
            {
                _selectedLoaiPhong = value;
                OnPropertyChanged(nameof(SelectedLoaiPhong));
            }
        }

        int? _sucChuaTimKiem;
        public int? SucChuaTimKiem
        {
            get { return _sucChuaTimKiem; }
            set
            {
                _sucChuaTimKiem = value;
                OnPropertyChanged(nameof(SucChuaTimKiem));
            }
        }

        ObservableCollection<PHONG> _ketQuaTimKiem;
        public ObservableCollection<PHONG> KetQuaTimKiem
        {
            get { return _ketQuaTimKiem; }
            set { _ketQuaTimKiem = value; OnPropertyChanged(nameof(KetQuaTimKiem)); }
        }

        PHONG _selectedPhong;
        public PHONG SelectedPhong
        {
            get { return _selectedPhong; }
            set
            {
                _selectedPhong = value;
                OnPropertyChanged(nameof(SelectedPhong));
                CapNhatChiTietPhong();
            }
        }

        string _chiTietTenPhong;
        public string ChiTietTenPhong
        {
            get { return _chiTietTenPhong; }
            set { _chiTietTenPhong = value; OnPropertyChanged(nameof(ChiTietTenPhong)); }
        }

        string _chiTietSucChua;
        public string ChiTietSucChua
        {
            get { return _chiTietSucChua; }
            set { _chiTietSucChua = value; OnPropertyChanged(nameof(ChiTietSucChua)); }
        }

        string _chiTietGiaPhong;
        public string ChiTietGiaPhong
        {
            get { return _chiTietGiaPhong; }
            set { _chiTietGiaPhong = value; OnPropertyChanged(nameof(ChiTietGiaPhong)); }
        }

        string _chiTietLoaiPhong;
        public string ChiTietLoaiPhong
        {
            get { return _chiTietLoaiPhong; }
            set { _chiTietLoaiPhong = value; OnPropertyChanged(nameof(ChiTietLoaiPhong)); }
        }

        string _chiTietTinhTrang;
        public string ChiTietTinhTrang
        {
            get { return _chiTietTinhTrang; }
            set { _chiTietTinhTrang = value; OnPropertyChanged(nameof(ChiTietTinhTrang)); }
        }

        public ICommand TimKiemCommand { get; set; }

        public Bai2ViewModel()
        {
            DanhSachLoaiPhong = new ObservableCollection<LOAIPHONG>(db.LOAIPHONGs.ToList());
            KetQuaTimKiem = new ObservableCollection<PHONG>();
            TimKiemCommand = new RelayCommand(TimKiem, p => SelectedLoaiPhong != null && SucChuaTimKiem != null && SucChuaTimKiem > 0);
        }

        void TimKiem(object p)
        {
            if (SelectedLoaiPhong == null || SucChuaTimKiem == null)
            {
                MessageBox.Show("Vui lòng chọn loại phòng và nhập sức chứa.");
                return;
            }

            if (SucChuaTimKiem <= 0)
            {
                MessageBox.Show("Sức chứa tìm kiếm phải lớn hơn 0.");
                return;
            }

            var query = db.PHONGs.Where(x =>
                x.MaLoai == SelectedLoaiPhong.MaLoai &&
                x.SucChua >= SucChuaTimKiem.Value);

            KetQuaTimKiem = new ObservableCollection<PHONG>(query.ToList());
            SelectedPhong = KetQuaTimKiem.FirstOrDefault();

            if (KetQuaTimKiem.Count == 0)
            {
                MessageBox.Show("Không tìm thấy phòng phù hợp.");
            }
        }

        void CapNhatChiTietPhong()
        {
            if (SelectedPhong == null)
            {
                ChiTietTenPhong = string.Empty;
                ChiTietSucChua = string.Empty;
                ChiTietGiaPhong = string.Empty;
                ChiTietLoaiPhong = string.Empty;
                ChiTietTinhTrang = string.Empty;
                return;
            }

            ChiTietTenPhong = SelectedPhong.TenPhong;
            ChiTietSucChua = SelectedPhong.SucChua.ToString();
            ChiTietGiaPhong = SelectedPhong.GiaPhong.ToString("N0");
            ChiTietLoaiPhong = SelectedPhong.LOAIPHONG != null ? SelectedPhong.LOAIPHONG.TenLoai : SelectedLoaiPhong?.TenLoai;
            ChiTietTinhTrang = SelectedPhong.TinhTrang == 0 ? "Phòng trống" : "Khách đang nhận phòng";
        }
    }
}
