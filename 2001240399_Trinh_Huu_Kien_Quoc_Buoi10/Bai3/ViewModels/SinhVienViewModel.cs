using Bai3.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Bai3.ViewModels
{
    public class SinhVienViewModel : BaseViewModel
    {
        QLCapNhatDiemEntities db = new QLCapNhatDiemEntities();

        public ObservableCollection<MonHoc> DS_MonHoc { get; set; }
        public List<string> DS_NamHoc { get; set; } = new List<string> { "2023-2024", "2024-2025", "2025-2026" };
        public List<int> DS_HocKy { get; set; } = new List<int> { 1, 2, 3 };

        private ObservableCollection<KetQua> _dsKetQua = new ObservableCollection<KetQua>();
        public ObservableCollection<KetQua> DS_KetQua
        {
            get => _dsKetQua;
            set { _dsKetQua = value; OnPropertyChanged(nameof(DS_KetQua)); }
        }

        private KetQua _selectedSinhVien;
        public KetQua SelectedSinhVien
        {
            get => _selectedSinhVien;
            set
            {
                _selectedSinhVien = value;
                OnPropertyChanged(nameof(SelectedSinhVien));
                if (SelectedSinhVien != null)
                {
                    NewDiem = SelectedSinhVien.SinhVien.KetQuas.FirstOrDefault()?.Diem;
                }
            }
        }

        private string _selectedMaMonHoc;
        public string SelectedMaMonHoc
        {
            get => _selectedMaMonHoc;
            set
            {
                _selectedMaMonHoc = value;
                OnPropertyChanged(nameof(SelectedMaMonHoc));
            }
        }

        private string _selectedNamHoc;
        public string SelectedNamHoc
        {
            get => _selectedNamHoc;
            set
            {
                _selectedNamHoc = value;
                OnPropertyChanged(nameof(SelectedNamHoc));
            }
        }

        private int _selectedHocKy;
        public int SelectedHocKy
        {
            get => _selectedHocKy;
            set
            {
                _selectedHocKy = value;
                OnPropertyChanged(nameof(SelectedHocKy));
            }
        }

        private double? _newDiem;
        public double? NewDiem
        {
            get => _newDiem;
            set { _newDiem = value; OnPropertyChanged(nameof(NewDiem)); }
        }

        string _masv;
        public string MaSV
        {
            get => _masv;
            set { _masv = value; OnPropertyChanged(nameof(MaSV)); }
        }

        private int _tenSinhVien;
        public int TenSinhVien
        {
            get => _tenSinhVien;
            set { _tenSinhVien = value; OnPropertyChanged(nameof(TenSinhVien)); }
        }
        string _maLop;
        public string MaLop { 
            get => _maLop;
            set { _maLop = value; OnPropertyChanged(nameof(MaLop)); }
        }

        string _diemSV;
        public string DiemSV { 
            get => _diemSV;
            set { _diemSV = value; OnPropertyChanged(nameof(DiemSV)); }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand InputCommand { get; set; }

        public SinhVienViewModel()
        {
            SaveCommand = new RelayCommand(Save, CanSave);
            LoadCommand = new RelayCommand(LoadDataList, CanLoadDataList);
            InputCommand = new RelayCommand(Input, CanInput);

            LoadDefaultComboBoxData();

            if (DS_MonHoc != null && DS_MonHoc.Count > 0) SelectedMaMonHoc = DS_MonHoc[0].MaMonHoc;
            SelectedNamHoc = DS_NamHoc[1];
            SelectedHocKy = DS_HocKy[0];
        }
        bool CanSave(object p)
        {
            return SelectedSinhVien != null && NewDiem.HasValue && NewDiem >= 0 && NewDiem <= 10;
        }
        
        bool CanLoadDataList(object p)
        {
            return !string.IsNullOrEmpty(SelectedMaMonHoc) && !string.IsNullOrEmpty(SelectedNamHoc) && SelectedHocKy > 0;
        }

        bool CanInput(object p)
        {
            return SelectedSinhVien != null && NewDiem.HasValue && NewDiem >= 0 && NewDiem <= 10;
        }
        void LoadDefaultComboBoxData()
        {
            try
            {
                DS_MonHoc = new ObservableCollection<MonHoc>(db.MonHocs.ToList());
                OnPropertyChanged(nameof(DS_MonHoc));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Thông báo lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void LoadDataList(object p)
        {
            if (string.IsNullOrEmpty(SelectedMaMonHoc) || string.IsNullOrEmpty(SelectedNamHoc) || SelectedHocKy == 0)
            {
                return;
            }

            DS_KetQua.Clear();

            var listSinhVien = db.SinhViens.ToList();
            var danhSachMoi = new ObservableCollection<KetQua>();

            foreach (var sv in listSinhVien)
            {
                var ketQuaDb = db.KetQuas.FirstOrDefault(k =>
                    k.MaSinhVien == sv.MaSinhVien &&
                    k.MaMonHoc.Trim() == SelectedMaMonHoc.Trim() &&
                    k.NamHoc.Trim() == SelectedNamHoc.Trim() &&
                    k.HocKy == SelectedHocKy);

                danhSachMoi.Add(new KetQua
                {
                    MaSinhVien = sv.MaSinhVien,
                    SinhVien = sv,
                    MaMonHoc = SelectedMaMonHoc,
                    NamHoc = SelectedNamHoc,
                    HocKy = SelectedHocKy,
                });
            }

            DS_KetQua = danhSachMoi;
        }

        private void Input(object p)
        {
            if (SelectedSinhVien == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng sinh viên trên bảng trước!");
                return;
            }

            if (!NewDiem.HasValue || NewDiem < 0 || NewDiem > 10)
            {
                MessageBox.Show("Điểm nhập vào phải nằm trong khoảng từ 0 đến 10!");
                return;
            }

            var ketQua = db.KetQuas.FirstOrDefault(kq =>
                kq.MaSinhVien == SelectedSinhVien.MaSinhVien &&
                kq.MaMonHoc.Trim() == SelectedMaMonHoc.Trim() &&
                kq.NamHoc.Trim() == SelectedNamHoc.Trim() &&
                kq.HocKy == SelectedHocKy);

            if (ketQua == null)
            {
                ketQua = new KetQua
                {
                    MaSinhVien = SelectedSinhVien.MaSinhVien,
                    MaMonHoc = SelectedMaMonHoc,
                    NamHoc = SelectedNamHoc,
                    HocKy = SelectedHocKy
                };
                db.KetQuas.Add(ketQua);
            }

            ketQua.Diem = NewDiem;
            db.SaveChanges();

            MessageBox.Show("Cập nhật điểm thành công!");
        }

        private void Save(object p)
        {
            try
            {
                db.SaveChanges();
                MessageBox.Show("Lưu điểm thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }
    }

}