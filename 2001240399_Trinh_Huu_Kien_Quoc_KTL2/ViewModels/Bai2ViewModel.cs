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
    public class Bai2ViewModel :BaseViewModel
    {
        QL_KaraokeEntities1 db=new QL_KaraokeEntities1();

        ObservableCollection<LOAIPHONG> _danhSachTang;
        public ObservableCollection<LOAIPHONG> DanhSachTang
        {
            get { return _danhSachTang; }
            set { _danhSachTang = value; OnPropertyChanged(nameof(DanhSachTang)); }
        }
        
        LOAIPHONG _selectedTang;
        public LOAIPHONG SelectedTang
        {
            get { return _selectedTang; }
            set { _selectedTang = value; OnPropertyChanged(nameof(SelectedTang)); }
        }

        int? _timKiemSucChua;
        public int? TimKiemSucChua
        {
            get { return _timKiemSucChua; }
            set { _timKiemSucChua = value; OnPropertyChanged(nameof(TimKiemSucChua)); }
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
            set { _selectedPhong = value; OnPropertyChanged(nameof(SelectedPhong)); CapNhatChiTietPhong(); }
        }

        string _chiTietPhong;
        public string ChiTietPhong
        {
            get { return _chiTietPhong; }
            set { _chiTietPhong = value; OnPropertyChanged(nameof(ChiTietPhong)); }
        }

        string _chiTietSucChua;
        public string ChiTietSucChua
        {
            get { return _chiTietSucChua; }
            set { _chiTietSucChua = value; OnPropertyChanged(nameof(ChiTietSucChua)); }
        }

        string _chiTietGia;
        public string ChiTietGia
        {
            get { return _chiTietGia; }
            set { _chiTietGia = value; OnPropertyChanged(nameof(ChiTietGia)); }
        }

        string _chiTietKieuPhong;
        public string ChiTietKieuPhong
        {
            get { return _chiTietKieuPhong; }
            set { _chiTietKieuPhong = value; OnPropertyChanged(nameof(ChiTietKieuPhong)); }
        }

        string _chiTietTinhTrang;
        public string CHiTietTinhTrang
        {
            get { return _chiTietTinhTrang; }
            set { _chiTietTinhTrang = value; OnPropertyChanged(nameof(CHiTietTinhTrang)); }
        }

        public ICommand TimKiemCommand { get; set; }

        public Bai2ViewModel()
        {
            DanhSachTang = new ObservableCollection<LOAIPHONG>(db.LOAIPHONGs.ToList());
            TimKiemCommand = new RelayCommand(TimKiem,CanTimKiem);
        }
        void TimKiem(object p)
        {
            if (SelectedTang == null || TimKiemSucChua == null)
            {
                System.Windows.MessageBox.Show("Vui lòng chọn tầng và nhập sức chứa cần tìm.");
                return;
            }

            if (TimKiemSucChua <= 0)
            {
                System.Windows.MessageBox.Show("Sức chứa tìm kiếm phải lớn hơn 0.");
                return;
            }

            var query = db.PHONGs.Where(x => x.MaNhom == SelectedTang.MaNhom && x.SucChua >= TimKiemSucChua);
            KetQuaTimKiem = new ObservableCollection<PHONG>(query.ToList());
            SelectedPhong = KetQuaTimKiem.FirstOrDefault();

            if (KetQuaTimKiem.Count == 0)
            {
                CapNhatChiTietPhong();
                System.Windows.MessageBox.Show("Không tìm thấy phòng phù hợp.");
            }
        }

        bool CanTimKiem( object p)
        {
            return SelectedTang != null && TimKiemSucChua != null && TimKiemSucChua > 0;
        }

        void CapNhatChiTietPhong()
        {
         if(SelectedPhong==null)
            {
                ChiTietPhong = string.Empty;
                ChiTietSucChua = string.Empty;
                ChiTietGia = string.Empty;
                ChiTietKieuPhong = string.Empty;
                CHiTietTinhTrang = string.Empty;
            return;
            }
            ChiTietPhong = SelectedPhong.TenPhong;
            ChiTietSucChua=SelectedPhong.SucChua.ToString();
            ChiTietGia= SelectedPhong.GiaPhong.ToString();

            ChiTietKieuPhong = SelectedPhong.KieuPhong== 1 ? "Phòng quạt" : "Phòng máy lạnh"; ;

            DateTime now=DateTime.Now;
            
            bool isOccupied = db.DATPHONGs.Any(h => h.MaPh == SelectedPhong.MaPhong && h.NgayDat <= now && h.NgayTra >= now);
            CHiTietTinhTrang = isOccupied ? "Đã có người đặt" : "Còn trống";
        }
    }
}
