using _2001240399_Trinh_Huu_Kien_Quoc_Buoi9.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi9.ViewModels
{
    internal class KhoaViewModel: BaseViewModel
    {
        private QLSinhVienEntities db = new QLSinhVienEntities();
        public ObservableCollection<Khoa> DS_Khoa { get; set; }
        private Khoa _SelectedKhoa;
        public Khoa SelectedKhoa
        {
            get => _SelectedKhoa;
            set
            {
                _SelectedKhoa = value;
                OnPropertyChanged(nameof(SelectedKhoa));
            }
        }
        public KhoaViewModel()
        {
            LoadData();
        }
        void LoadData()
        {
            DS_Khoa = new ObservableCollection<Khoa>(db.Khoas.ToList());
        }
    }
}
