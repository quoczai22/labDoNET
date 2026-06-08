using _2001240399_Trinh_Huu_Kien_Quoc_KT2L2.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace _2001240399_Trinh_Huu_Kien_Quoc_KT2L2.ViewModels
{
    public class Bai1ViewModel : BaseViewModel
    {
        QuanLyKhachSanEntities db = new QuanLyKhachSanEntities();

        ObservableCollection<PHONG> _danhSachPhong;
        public ObservableCollection<PHONG> DanhSachPhong
        {
            get { return _danhSachPhong; }
            set { _danhSachPhong = value; OnPropertyChanged(nameof(DanhSachPhong)); }
        }

        ObservableCollection<LOAIPHONG> _danhSachLoaiPhong;
        public ObservableCollection<LOAIPHONG> DanhSachLoaiPhong
        {
            get { return _danhSachLoaiPhong; }
            set { _danhSachLoaiPhong = value; OnPropertyChanged(nameof(DanhSachLoaiPhong)); }
        }

        string _maPhong;
        public string MaPhong
        {
            get { return _maPhong; }
            set { _maPhong = value; OnPropertyChanged(nameof(MaPhong)); }
        }

        string _tenPhong;
        public string TenPhong
        {
            get { return _tenPhong; }
            set { _tenPhong = value; OnPropertyChanged(nameof(TenPhong)); }
        }

        int _sucChua;
        public int SucChua
        {
            get { return _sucChua; }
            set { _sucChua = value; OnPropertyChanged(nameof(SucChua)); }
        }

        decimal _giaPhong;
        public decimal GiaPhong
        {
            get { return _giaPhong; }
            set { _giaPhong = value; OnPropertyChanged(nameof(GiaPhong)); }
        }

        bool _phongTrong = true;
        public bool PhongTrong
        {
            get { return _phongTrong; }
            set
            {
                _phongTrong = value;
                OnPropertyChanged(nameof(PhongTrong));
                if (value) PhongDangNhan = false;
            }
        }

        bool _phongDangNhan;
        public bool PhongDangNhan
        {
            get { return _phongDangNhan; }
            set
            {
                _phongDangNhan = value;
                OnPropertyChanged(nameof(PhongDangNhan));
                if (value) PhongTrong = false;
            }
        }

        LOAIPHONG _selectedLoaiPhong;
        public LOAIPHONG SelectedLoaiPhong
        {
            get { return _selectedLoaiPhong; }
            set
            {
                _selectedLoaiPhong = value;
                OnPropertyChanged(nameof(SelectedLoaiPhong));
                if (value != null && GiaPhong == 0)
                {
                    GiaPhong = value.GiaLoai;
                }
            }
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
                    MaPhong = value.MaPhong;
                    TenPhong = value.TenPhong;
                    SucChua = value.SucChua;
                    GiaPhong = value.GiaPhong;
                    PhongTrong = value.TinhTrang == 0;
                    PhongDangNhan = value.TinhTrang == 1;
                    SelectedLoaiPhong = DanhSachLoaiPhong.FirstOrDefault(x => x.MaLoai == value.MaLoai);
                }
            }
        }

        public ICommand ThemCommand { get; set; }
        public ICommand XoaCommand { get; set; }
        public ICommand SuaCommand { get; set; }
        public ICommand LamMoiCommand { get; set; }

        public Bai1ViewModel()
        {
            LoadData();
            ThemCommand = new RelayCommand(Them, CanThem);
            XoaCommand = new RelayCommand(Xoa, p => SelectedPhong != null);
            SuaCommand = new RelayCommand(Sua, p => SelectedPhong != null);
            LamMoiCommand = new RelayCommand(p => LamSachForm());
        }

        void LoadData()
        {
            DanhSachLoaiPhong = new ObservableCollection<LOAIPHONG>(db.LOAIPHONGs.ToList());
            DanhSachPhong = new ObservableCollection<PHONG>(db.PHONGs.ToList());
        }

        bool CanThem(object p)
        {
            return true;
        }

        void Them(object p)
        {
            if (string.IsNullOrWhiteSpace(MaPhong) || string.IsNullOrWhiteSpace(TenPhong) || SelectedLoaiPhong == null)
            {
                MessageBox.Show("Vui lòng nhập mã phòng, tên phòng và chọn loại phòng.");
                return;
            }

            if (SucChua <= 0 || GiaPhong <= 0)
            {
                MessageBox.Show("Sức chứa và giá phòng phải lớn hơn 0.");
                return;
            }

            if (db.PHONGs.Any(x => x.MaPhong == MaPhong))
            {
                MessageBox.Show("Mã phòng đã tồn tại.");
                return;
            }

            var phong = new PHONG
            {
                MaPhong = MaPhong,
                TenPhong = TenPhong,
                SucChua = SucChua,
                GiaPhong = GiaPhong,
                MaLoai = SelectedLoaiPhong.MaLoai,
                TinhTrang = PhongTrong ? 0 : 1
            };

            try
            {
                db.PHONGs.Add(phong);
                db.SaveChanges();
                phong.LOAIPHONG = SelectedLoaiPhong;
                DanhSachPhong.Add(phong);
                MessageBox.Show("Thêm phòng thành công.");
                LamSachForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm phòng: " + ex.Message);
            }
        }

        void Xoa(object p)
        {
            if (SelectedPhong == null)
            {
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa phòng này không?", "Xác nhận", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                var phong = db.PHONGs.SingleOrDefault(x => x.MaPhong == SelectedPhong.MaPhong);
                if (phong != null)
                {
                    db.PHONGs.Remove(phong);
                    db.SaveChanges();
                    DanhSachPhong.Remove(SelectedPhong);
                    MessageBox.Show("Xóa phòng thành công.");
                    LamSachForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa phòng vì có dữ liệu liên quan: " + ex.Message);
            }
        }

        void Sua(object p)
        {
            if (SelectedPhong == null || SelectedLoaiPhong == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(TenPhong) || SucChua <= 0 || GiaPhong <= 0)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin hợp lệ.");
                return;
            }

            try
            {
                var phong = db.PHONGs.SingleOrDefault(x => x.MaPhong == SelectedPhong.MaPhong);
                if (phong != null)
                {
                    phong.TenPhong = TenPhong;
                    phong.SucChua = SucChua;
                    phong.GiaPhong = GiaPhong;
                    phong.MaLoai = SelectedLoaiPhong.MaLoai;
                    phong.TinhTrang = PhongTrong ? 0 : 1;
                    db.SaveChanges();
                    phong.LOAIPHONG = SelectedLoaiPhong;

                    int index = DanhSachPhong.IndexOf(SelectedPhong);
                    if (index >= 0)
                    {
                        DanhSachPhong.RemoveAt(index);
                        DanhSachPhong.Insert(index, phong);
                        SelectedPhong = phong;
                    }
                    MessageBox.Show("Cập nhật phòng thành công.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật phòng: " + ex.Message);
            }
        }

        void LamSachForm()
        {
            SelectedPhong = null;
            MaPhong = string.Empty;
            TenPhong = string.Empty;
            SucChua = 0;
            GiaPhong = 0;
            SelectedLoaiPhong = null;
            PhongTrong = true;
        }
    }
}
