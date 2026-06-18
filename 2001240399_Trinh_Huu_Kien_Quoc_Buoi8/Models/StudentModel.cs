using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi8.Models
{
    public class StudentModel
    {
        private string _maSV;
        private string _hoTen;
        private bool _gioiTinh = true; 
        private string _thanhPho;
        private string _diaChi;
        private string _tenLop;

        public string MaSV { get => _maSV; set { _maSV = value; OnPropertyChanged(nameof(MaSV)); } }
        
        public string HoTen { get => _hoTen; set { _hoTen = value; OnPropertyChanged(nameof(HoTen)); } }

        public bool GioiTinh
        {
            
            get => _gioiTinh; 
            set
            {
                _gioiTinh = value;
                OnPropertyChanged(nameof(GioiTinh));
                OnPropertyChanged(nameof(GioiTinhText)); 
            }
        }

        public bool GioiTinhNu
        {
            get => !GioiTinh;
            set
            {
                GioiTinh = !value;
                OnPropertyChanged(nameof(GioiTinhNu));
            }
        }

        public string GioiTinhText => GioiTinh ? "Nam" : "Nữ"; 

        public string ThanhPho { get => _thanhPho; set { _thanhPho = value; OnPropertyChanged(nameof(ThanhPho)); } }
        public string DiaChi { get => _diaChi; set { _diaChi = value; OnPropertyChanged(nameof(DiaChi)); } }
        public string TenLop { get => _tenLop; set { _tenLop = value; OnPropertyChanged(nameof(TenLop)); } }

         public event PropertyChangedEventHandler PropertyChanged; 
        protected void OnPropertyChanged(string propertyName)
        {
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
        }
    }
}
