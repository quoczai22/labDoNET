using Bai6.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Windows;

namespace Bai6.ViewModels
{
    public class QuanLyMonHocViewModel : BaseViewModel
    {
        QLSinhVienEntities db = new QLSinhVienEntities();

        public ObservableCollection<MonHoc> DS_MonHoc { get; set; }
        public List<string> DS_NamHoc { get; set; } = new List<string> { "2024-2025", "2025-2026", "2026-2027" };

        // Đổi thành danh sách kiểu int để đồng bộ với trường HocKy trong Database
        public List<int> DS_HocKy { get; set; } = new List<int> { 1, 2, 3 };

        private ObservableCollection<KetQua> _dsKetQua;
        public ObservableCollection<KetQua> DS_KetQua
        {
            get => _dsKetQua;
            set
            {
                _dsKetQua = value;
                // Kích hoạt thông báo để DataGrid cập nhật giao diện lập tức
                OnPropertyChanged(nameof(DS_KetQua));
            }
        }

        private string _selectedMonHoc;
        public string SelectedMonHoc
        {
            get { return _selectedMonHoc; }
            set { _selectedMonHoc = value; OnPropertyChanged(nameof(SelectedMonHoc)); }
        }

        private string _selectedNamHoc;
        public string SelectedNamHoc
        {
            get { return _selectedNamHoc; }
            set { _selectedNamHoc = value; OnPropertyChanged(nameof(SelectedNamHoc));  }
        }

        private int? _selectedHocKy;
        public int? SelectedHocKy
        {
            get { return _selectedHocKy; }
            set { _selectedHocKy = value; OnPropertyChanged(nameof(SelectedHocKy));  }
        }

        // Khai báo chuẩn tên hai Command liên kết với file XAML
        public RelayCommand LoadCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }

        public QuanLyMonHocViewModel()
        {
            // 1. Khởi tạo các lệnh Nút bấm trước tiên
            LoadCommand = new RelayCommand(Load, CanLoad);
            SaveCommand = new RelayCommand(Save, CanSave);

            // 2. Gọi LoadData sau cùng để nạp dữ liệu lên Form
            LoadData();
        }

        void LoadData()
        {
            // 1. Tải danh sách Môn học đổ vào ComboBox
            var dsMon = db.MonHocs.ToList();
            DS_MonHoc = new ObservableCollection<MonHoc>(dsMon);
            OnPropertyChanged(nameof(DS_MonHoc));

            // 2. GÁN GIÁ TRỊ MẶC ĐỊNH CHO 3 COMBOBOX ĐỂ KHÔNG BỊ TRỐNG
            if (dsMon.Count > 0)
            {
                SelectedMonHoc = dsMon[0].MaMonHoc;
            }
            SelectedNamHoc = DS_NamHoc[0];
            SelectedHocKy = DS_HocKy[0];

            // 3. Tự động gọi hàm Tải danh sách sinh viên ngay khi vừa mở Form
            Load(null);
        }

        void Load(object p)
        {
            // Kiểm tra ràng buộc điều kiện chọn
            if (string.IsNullOrEmpty(SelectedMonHoc) || string.IsNullOrEmpty(SelectedNamHoc) || SelectedHocKy == null)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ Môn học, Năm học và Học kỳ trước khi tải!");
                return;
            }

            // Giải phóng các thực thể thêm tạm chưa lưu khỏi bộ nhớ RAM để tránh lỗi nhân đôi dòng khi bấm nút nhiều lần
            var addedEntries = db.ChangeTracker.Entries<KetQua>()
                                .Where(e => e.State == EntityState.Added).ToList();
            foreach (var entry in addedEntries)
            {
                entry.State = EntityState.Detached;
            }

            var dsSinhVien = db.SinhViens.Include(s => s.KetQuas).ToList();
            var dsDiem = new ObservableCollection<KetQua>();

            foreach (var sv in dsSinhVien)
            {
                // Sử dụng .Trim() để loại bỏ khoảng trắng thừa của kiểu dữ liệu dữ liệu dạng chuỗi cố định (char/nchar) trong SQL
                var kq = db.KetQuas.FirstOrDefault(k => k.MaSinhVien == sv.MaSinhVien &&
                                                        k.MaMonHoc.Trim() == SelectedMonHoc.Trim() &&
                                                        k.NamHoc.Trim() == SelectedNamHoc.Trim() &&
                                                        k.HocKy == SelectedHocKy);

                if (kq == null)
                {
                    kq = new KetQua
                    {
                        MaSinhVien = sv.MaSinhVien,
                        SinhVien = sv,
                        MaMonHoc = SelectedMonHoc,
                        NamHoc = SelectedNamHoc,
                        HocKy = SelectedHocKy.Value
                    };
                    db.KetQuas.Add(kq);
                }
                dsDiem.Add(kq);
            }

            DS_KetQua = dsDiem;
        }

        bool CanLoad(object p) { return true; }

        void Save(object p)
        {
            try
            {
                db.SaveChanges();
                MessageBox.Show("Lưu thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống khi lưu dữ liệu: " + ex.Message);
            }
        }

        bool CanSave(object p) { return DS_KetQua != null && DS_KetQua.Count > 0; }
    }
}