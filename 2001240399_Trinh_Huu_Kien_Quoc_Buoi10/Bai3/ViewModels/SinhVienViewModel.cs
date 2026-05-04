using Bai3.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Bai3.Views;
namespace Bai3.ViewModels
{
    public class SinhVienViewModel : BaseViewModel
    {
        QLSinhVienEntities db = new QLSinhVienEntities();

        public ObservableCollection<SinhVien> DS_SinhVien { get; set; }
        public ObservableCollection<MonHoc> DS_MonHoc { get; set; }
        public List<string> DS_NamHoc { get; set; } = new List<string> { "2023-2024", "2024-2025" };
        public List<int> DS_HocKy { get; set; } = new List<int> { 1, 2, 3 };
        public RelayCommand SaveCommnad { get; set; }
        public RelayCommand LoadCommand { get; set; }
        public RelayCommand InputCommand { get; set; }
        public ObservableCollection<SinhVienViewModel> DS_KetQua { get; set; } = new ObservableCollection<SinhVienViewModel>();

        private SinhVienViewModel _SelectedSinhVien;
        public SinhVienViewModel SelectedSinhVien
        {
            get => _SelectedSinhVien;
            set
            {
                _SelectedSinhVien = value;
                OnPropertyChanged(nameof(SelectedSinhVien));
                if (SelectedSinhVien != null)
                { 
                    DiemSV = SelectedSinhVien.DiemSV;
                }
            }
        }
        string _selectedMaMonHoc;
        public string SelectedMaMonHoc
        {
            get => _selectedMaMonHoc;
            set
            {
                _selectedMaMonHoc = value;
                OnPropertyChanged(nameof(SelectedMaMonHoc));
            }
        }
        string _selectedNamHoc;
        public string SelectedNamHoc
        {
            get => _selectedNamHoc;
            set
            {
                _selectedNamHoc = value;
                OnPropertyChanged(nameof(SelectedNamHoc));
            }
        }

        int _selectedHocKy;
        public int SelectedHocKy
        {
            get => _selectedHocKy;
            set
            { _selectedHocKy = value;
                OnPropertyChanged(nameof(SelectedHocKy));
            }
        }


        private string _maSV;
        public string MaSV
            {
            get => _maSV;
            set
            {
                _maSV = value;
                OnPropertyChanged(nameof(MaSV));
            }
        }
        
        private string _tenSV;
        public string TenSV
        {
            get => _tenSV;
            set
            {
                _tenSV = value;
                OnPropertyChanged(nameof(TenSV));
            }
        }
        string _maLopSV;
        public string MaLopSV
            {
            get => _maLopSV;
            set
            {
                _maLopSV = value;
                OnPropertyChanged(nameof(MaLopSV));
            }
        }
        double _diemSV;
        public double DiemSV
            {
            get => _diemSV;
            set
            {
                _diemSV = value;
                OnPropertyChanged(nameof(DiemSV));
            }
        }

        double? _newDiem;
        public double? NewDiem
        {
            get => _newDiem;
            set
            {
                _newDiem = value;
                OnPropertyChanged(nameof(NewDiem));
            }
        }

        public SinhVienViewModel()
        {
                SaveCommnad = new RelayCommand(o => Save());
                LoadCommand = new RelayCommand(o => LoadDataList());
                InputCommand = new RelayCommand(o => Input());
            LoadData();
        }

        void LoadData()
        {
            try
            {
                DS_SinhVien = new ObservableCollection<SinhVien>(db.SinhViens.ToList());
                OnPropertyChanged(nameof(DS_SinhVien));
                DS_MonHoc = new ObservableCollection<MonHoc>(db.MonHocs.ToList());
                OnPropertyChanged(nameof(DS_MonHoc));
            }
            catch (Exception ex)
            {
                string errorMsg = "LỖI KẾT NỐI ENTITY FRAMEWORK:\n" + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMsg += "\n\nCHI TIẾT LỖI (Inner 1):\n" + ex.InnerException.Message;

                    if (ex.InnerException.InnerException != null)
                    {
                        errorMsg += "\n\nCHI TIẾT LỖI (Inner 2):\n" + ex.InnerException.InnerException.Message;
                    }
                }
                MessageBox.Show(errorMsg, "Lỗi Database", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Input()
        {
            if (SelectedSinhVien != null && NewDiem.HasValue)
            {
                if (NewDiem < 0 || NewDiem > 10)
                {
                    MessageBox.Show("Điểm phải từ 0 đến 10!");
                    return;
                }

                var ketQua = db.KetQuas.FirstOrDefault(kq =>
                    kq.MaSinhVien == SelectedSinhVien.MaSV &&
                    kq.MaMonHoc == SelectedMaMonHoc &&
                    kq.NamHoc == SelectedNamHoc &&
                    kq.HocKy == SelectedHocKy);

                if (ketQua != null)
                {
                    ketQua.Diem = NewDiem; 
                    db.SaveChanges();      

                    SelectedSinhVien.DiemSV = NewDiem.Value; 

                    MessageBox.Show("Cập nhật điểm thành công!");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy kết quả để cập nhật!");
                }
                NewDiem = null;
            }
        }
        
        void Save()
        {
            try
            {
                foreach (var item in DS_KetQua)
                {
                    if (item.DiemSV < 0 || item.DiemSV > 10)
                    {
                        MessageBox.Show($"Điểm của sinh viên {item.MaSV} không hợp lệ. Phải từ 0 đến 10!");
                        return;
                    }

                    var ketQuaDb = db.KetQuas.FirstOrDefault(k =>
                        k.MaSinhVien == item.MaSV &&
                        k.MaMonHoc == SelectedMaMonHoc &&
                        k.NamHoc == SelectedNamHoc &&
                        k.HocKy == SelectedHocKy);

                    if (ketQuaDb != null)
                    {
                        ketQuaDb.Diem = item.DiemSV;
                    }
                }

                db.SaveChanges();
                MessageBox.Show("Lưu thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }
        void LoadDataList()
        {
            var lstSV = db.SinhViens.ToList();
            var lstDiem =new ObservableCollection<KetQua>(db.KetQuas.ToList());
            if (string.IsNullOrEmpty(SelectedMaMonHoc) || string.IsNullOrEmpty(SelectedNamHoc))
            {
                return;
            }
            DS_KetQua.Clear();
            var danhSachKetQua = (from kq in db.KetQuas
                                  join sv in db.SinhViens on kq.MaSinhVien equals sv.MaSinhVien
                                  where kq.MaMonHoc == SelectedMaMonHoc
                                     && kq.NamHoc == SelectedNamHoc
                                     && kq.HocKy == SelectedHocKy
                                  select new SinhVienViewModel
                                  {
                                      MaSV = sv.MaSinhVien,
                                      TenSV = sv.HoTen,
                                      MaLopSV = sv.MaLop,
                                      DiemSV = (double)(kq.Diem ?? 0)
                                  }).ToList();

            foreach (var item in danhSachKetQua)
            {
                DS_KetQua.Add(item);
            }
        }
    }
}