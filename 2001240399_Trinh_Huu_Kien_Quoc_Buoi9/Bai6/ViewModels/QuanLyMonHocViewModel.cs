using Bai6.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Bai6.ViewModels
{
    public class QuanLyMonHocViewModel : BaseViewModel
    {
        QLSinhVienEntities db = new QLSinhVienEntities();

        public ObservableCollection<MonHoc> DS_MonHoc { get; set; }
        public List<string> DS_NamHoc { get; set; } = new List<string> { "2024-2025", "2025-2026", "2026-2027" };
        public List<int> DS_HocKy { get; set; } = new List<int> { 1, 2, 3 };

        private ObservableCollection<KetQua> _dsKetQua;
        public ObservableCollection<KetQua> DS_KetQua
        {
            get => _dsKetQua;
            set
            {
                _dsKetQua = value;
                OnPropertyChanged(nameof(DS_KetQua));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _selectedMonHoc;
        public string SelectedMonHoc
        {
            get { return _selectedMonHoc; }
            set { _selectedMonHoc = value; OnPropertyChanged(nameof(SelectedMonHoc)); CommandManager.InvalidateRequerySuggested(); }
        }

        private string _selectedNamHoc;
        public string SelectedNamHoc
        {
            get { return _selectedNamHoc; }
            set { _selectedNamHoc = value; OnPropertyChanged(nameof(SelectedNamHoc)); CommandManager.InvalidateRequerySuggested(); }
        }

        private int? _selectedHocKy;
        public int? SelectedHocKy
        {
            get { return _selectedHocKy; }
            set { _selectedHocKy = value; OnPropertyChanged(nameof(SelectedHocKy)); CommandManager.InvalidateRequerySuggested(); }
        }

        public RelayCommand LoadCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }

        public QuanLyMonHocViewModel()
        {
            LoadCommand = new RelayCommand(Load, CanLoad);
            SaveCommand = new RelayCommand(Save, CanSave);
            LoadData();
        }

        void LoadData()
        {
            var dsMon = db.MonHocs.OrderBy(m => m.MaMonHoc).ToList();
            DS_MonHoc = new ObservableCollection<MonHoc>(dsMon);
            OnPropertyChanged(nameof(DS_MonHoc));

            if (dsMon.Count > 0) SelectedMonHoc = dsMon[0].MaMonHoc;
            SelectedNamHoc = DS_NamHoc[0];
            SelectedHocKy = DS_HocKy[0];

            Load(null);
        }

        void Load(object p)
        {
            if (!CanLoad(p))
            {
                MessageBox.Show("Vui lòng chọn đầy đủ Môn học, Năm học và Học kỳ trước khi tải!");
                return;
            }

            string maMon = SelectedMonHoc.Trim();
            string namHoc = SelectedNamHoc.Trim();
            int hocKy = SelectedHocKy.Value;

            var dsSinhVien = db.SinhViens
                .AsNoTracking()
                .OrderBy(s => s.MaSinhVien)
                .ToList();

            var dsKetQuaDaCo = db.KetQuas
                .AsNoTracking()
                .Where(k => k.MaMonHoc == maMon && k.NamHoc == namHoc && k.HocKy == hocKy)
                .ToList();

            var dsDiem = new ObservableCollection<KetQua>();
            foreach (var sv in dsSinhVien)
            {
                var kq = dsKetQuaDaCo.FirstOrDefault(k => k.MaSinhVien == sv.MaSinhVien);
                if (kq == null)
                {
                    kq = new KetQua
                    {
                        MaSinhVien = sv.MaSinhVien,
                        MaMonHoc = maMon,
                        NamHoc = namHoc,
                        HocKy = hocKy,
                        Diem = null
                    };
                }
                kq.SinhVien = sv;
                dsDiem.Add(kq);
            }

            DS_KetQua = dsDiem;
        }

        bool CanLoad(object p)
        {
            return !string.IsNullOrWhiteSpace(SelectedMonHoc) &&
                   !string.IsNullOrWhiteSpace(SelectedNamHoc) &&
                   SelectedHocKy != null;
        }

        void Save(object p)
        {
            if (DS_KetQua == null || DS_KetQua.Count == 0)
            {
                MessageBox.Show("Vui lòng tải danh sách sinh viên trước khi lưu điểm!");
                return;
            }

            try
            {
                foreach (var item in DS_KetQua)
                {
                    var kq = db.KetQuas.FirstOrDefault(x => x.MaSinhVien == item.MaSinhVien &&
                                                            x.MaMonHoc == item.MaMonHoc &&
                                                            x.NamHoc == item.NamHoc &&
                                                            x.HocKy == item.HocKy);
                    if (kq == null)
                    {
                        kq = new KetQua
                        {
                            MaSinhVien = item.MaSinhVien,
                            MaMonHoc = item.MaMonHoc,
                            NamHoc = item.NamHoc,
                            HocKy = item.HocKy,
                            Diem = item.Diem
                        };
                        db.KetQuas.Add(kq);
                    }
                    else
                    {
                        kq.Diem = item.Diem;
                    }
                }

                db.SaveChanges();
                MessageBox.Show("Lưu điểm thành công!");
                Load(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống khi lưu dữ liệu: " + ex.Message);
            }
        }

        bool CanSave(object p) { return DS_KetQua != null && DS_KetQua.Count > 0; }
    }
}
