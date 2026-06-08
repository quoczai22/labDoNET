using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using _2001240399_Trinh_Huu_Kien_Quoc_KTL2.Models;

namespace _2001240399_Trinh_Huu_Kien_Quoc_KTL2.ViewModels
{
    public class Bai1ViewModel : BaseViewModel
    {
        QL_KaraokeEntities1 db = new QL_KaraokeEntities1();

        ObservableCollection<PHONG> _danhSachPhong;
        public ObservableCollection<PHONG> DanhSachPhong
        {
            get => _danhSachPhong;
            set { _danhSachPhong = value; OnPropertyChanged(nameof(DanhSachPhong)); }
        }

        ObservableCollection<LOAIPHONG> _danhSachTang;
        public ObservableCollection<LOAIPHONG> DanhSachTang
        {
            get => _danhSachTang;
            set
            {
                _danhSachTang = value;
                OnPropertyChanged(nameof(DanhSachTang));
            }
        }

        string _maPhong;
        public string MaPhong
        {
            get => _maPhong;
            set
            {
                _maPhong = value;
                OnPropertyChanged(nameof(MaPhong));
            }
        }
        string _tenPhong;
        public string TenPhong
        {
            get => _tenPhong;
            set
            {
                _tenPhong = value;
                OnPropertyChanged(nameof(TenPhong));
            }
        }

        decimal _giaPhong;
        public decimal GiaPhong
        {
            get => _giaPhong;
            set
            {
                _giaPhong = value;
                OnPropertyChanged(nameof(GiaPhong));
            }
        }

        int _sucChuaToiDa;
        public int SucChuaToiDa
        {
            get => _sucChuaToiDa;
            set
            {
                _sucChuaToiDa = value;
                OnPropertyChanged(nameof(SucChuaToiDa));
            }
        }
        bool _loaiPhongQuat;
        public bool LoaiPhongQuat
        {
            get => _loaiPhongQuat;
            set
            {
                _loaiPhongQuat = value;
                OnPropertyChanged(nameof(LoaiPhongQuat));
                if (value) LoaiPhongMayLanh = false;
            }
        }
        LOAIPHONG _selectedTang;
        public LOAIPHONG SelectedTang
        {
            get => _selectedTang;
            set
            {
                _selectedTang = value;
                OnPropertyChanged(nameof(SelectedTang));
            }
        }
    
        
        PHONG _selectedPhong;
        public PHONG SelectedPhong
        {
            get => _selectedPhong;
            set
            {
                _selectedPhong = value;
                OnPropertyChanged(nameof(SelectedPhong));
                if (SelectedPhong != null)
                {
                    MaPhong = SelectedPhong.MaPhong;
                    TenPhong = SelectedPhong.TenPhong;
                    GiaPhong = SelectedPhong.GiaPhong ?? 0;
                    SucChuaToiDa = SelectedPhong.SucChua ?? 0;
                    LoaiPhongQuat =SelectedPhong.KieuPhong == 1; // Giả sử KieuPhong == 1 là quạt
                    LoaiPhongMayLanh = SelectedPhong.KieuPhong == 2;
                    SelectedTang = DanhSachTang.FirstOrDefault(x => x.MaNhom == _selectedPhong.MaNhom);
                }
            }
        }

        private bool _loaiPhongMayLanh;
        public bool LoaiPhongMayLanh
        {
            get => _loaiPhongMayLanh;
            set
            {
                _loaiPhongMayLanh = value;
                OnPropertyChanged(nameof(LoaiPhongMayLanh));
                if (value) LoaiPhongQuat = false; // Bỏ tick phòng quạt
            }
        }

        public ICommand ThemCommand {  get; set; }
        public ICommand XoaCommand { get; set; }
        public ICommand SuaCommand { get; set; }
        public ICommand LuuCommand { get; set; }

        public Bai1ViewModel()
        {
            LoadData();
            ThemCommand = new RelayCommand(Them, CanThem);
            XoaCommand = new RelayCommand(Xoa, CanXoa);
            SuaCommand = new RelayCommand(Sua, Cansua);
            LuuCommand = new RelayCommand(Luu, Canluu);
        }

        void LoadData()
        {
            DanhSachPhong = new ObservableCollection<PHONG> ( db.PHONGs.ToList() );
            DanhSachTang = new ObservableCollection<LOAIPHONG>(db.LOAIPHONGs.ToList());
        }
        bool CanThem(object p)
        {
            return true;
        }
        void Them(object p)
        {
            if (string.IsNullOrWhiteSpace(MaPhong) || string.IsNullOrWhiteSpace(TenPhong) || SelectedTang == null)
            {
                System.Windows.MessageBox.Show("Vui lòng nhập đầy đủ thông tin: Mã phòng, Tên phòng và Chọn tầng!",
                    "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }
            bool isExisted = db.PHONGs.Any(x => x.MaPhong == MaPhong);
            if (isExisted)
            {
                System.Windows.MessageBox.Show("Mã phòng này đã tồn tại! Vui lòng nhập mã khác.",
                    "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            PHONG phongmoi = new PHONG()
            {
                MaPhong = MaPhong,
                TenPhong = TenPhong,
                GiaPhong = GiaPhong,
                SucChua = SucChuaToiDa,
                KieuPhong = LoaiPhongQuat ? 1 : 2,
                MaNhom = SelectedTang.MaNhom
            };
            try
            {
                db.PHONGs.Add(phongmoi);
                db.SaveChanges();
                phongmoi.LOAIPHONG = SelectedTang;
                DanhSachPhong.Add(phongmoi);
                System.Windows.MessageBox.Show("Thêm phòng mới thành công!",
            "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                LamSachFrom();
            }
            catch(Exception ex) 
            {
                System.Windows.MessageBox.Show("Lỗi khi lưu vào cơ sở dữ liệu: " + ex.Message,
            "Lỗi", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
        void LamSachFrom()
        {
            MaPhong = string.Empty;
            TenPhong = string.Empty;
            GiaPhong = 0;
            SucChuaToiDa = 0;
            LoaiPhongQuat = true;
            SelectedTang = null;
        }

        bool CanXoa(object p)
        {
            return true;
        }
        
        void Xoa(object p)
        {
            if (SelectedPhong == null)
            {
                System.Windows.MessageBox.Show("Vui lòng chọn một phòng từ danh sách để xóa!",
                    "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }
            var result = System.Windows.MessageBox.Show($"Bạn có chắc chắn muốn xóa phòng '{SelectedPhong.TenPhong}' không?",
            "Xác nhận xóa", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                try
                {
                    var phongxoa = db.PHONGs.SingleOrDefault(x => x.MaPhong == SelectedPhong.MaPhong);
                    if (phongxoa != null)
                    {
                        db.PHONGs.Remove(phongxoa);
                        db.SaveChanges();
                        DanhSachPhong.Remove(SelectedPhong);
                        System.Windows.MessageBox.Show("Xóa phòng thành công!",
                        "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                        LamSachFrom();
                    }
                }catch (Exception ex) {
                    System.Windows.MessageBox.Show("Không thể xóa phòng này! Có thể phòng đang có dữ liệu đặt phòng liên quan.\n\nChi tiết lỗi: " + ex.Message,
                "Lỗi", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }  
        }

        bool Cansua(object p)
        {
            return true;
        }

        void Sua(object p)
        {
            if (SelectedPhong == null)
            {
                System.Windows.MessageBox.Show("Vui lòng chọn một phòng từ danh sách để sửa!",
                    "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(TenPhong) || SelectedTang == null)
            {
                System.Windows.MessageBox.Show("Vui lòng nhập đầy đủ Tên phòng và Chọn tầng!",
                    "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }
            try
            {
                var phongsua = db.PHONGs.SingleOrDefault(x => x.MaPhong == SelectedPhong.MaPhong);
                if (phongsua != null)
                {
                    phongsua.TenPhong = TenPhong;
                    phongsua.GiaPhong = GiaPhong;
                    phongsua.SucChua = SucChuaToiDa;
                    phongsua.KieuPhong = LoaiPhongQuat ? 1 : 2;
                    phongsua.MaNhom = SelectedTang.MaNhom;
                    db.SaveChanges();
                    phongsua.LOAIPHONG = SelectedTang;
                    int index = DanhSachPhong.IndexOf(SelectedPhong);
                    if (index != -1)
                    {
                        DanhSachPhong.RemoveAt(index);
                        DanhSachPhong.Insert(index, phongsua);
                        SelectedPhong = phongsua;
                    }
                    System.Windows.MessageBox.Show("Cập nhật thông tin phòng thành công!",
                "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Lỗi khi cập nhật cơ sở dữ liệu: " + ex.Message,
            "Lỗi", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
        bool Canluu(object p)
        {
            return true;
        }

        void Luu(object p)
        {
            try
            {
                db.SaveChanges();
                System.Windows.MessageBox.Show("Lưu dữ liệu thành công!",
                    "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message,
                    "Lỗi", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
