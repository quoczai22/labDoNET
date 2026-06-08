using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi8.Models
{
    public class ClassModel
    {
         private string _tenLop; 
        private ObservableCollection<StudentModel> _danhSachSinhVien; 

        public string TenLop
        {
            get => _tenLop;
            set { _tenLop = value; OnPropertyChanged(nameof(TenLop)); }
            
        }

        public ObservableCollection<StudentModel> DanhSachSinhVien
        {
            get => _danhSachSinhVien;
            set { _danhSachSinhVien = value; OnPropertyChanged(nameof(DanhSachSinhVien)); }
            
        }

        public ClassModel()
        {
             DanhSachSinhVien = new ObservableCollection<StudentModel>(); 
        }

         public event PropertyChangedEventHandler PropertyChanged; 
        protected void OnPropertyChanged(string propertyName)
        {
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
        }
    }
}

