using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bai2.ViewModels;

namespace Bai2.Models
{
    public class Bill : BaseViewModel
    {
        private int _stt;
        public int STT
        {
            get { return _stt; }
            set { _stt = value; OnPropertyChanged(nameof(STT)); }
        }

        public string TenKhach { get; set; }
        public string LoaiPhong { get; set; }
        public string TienNghi { get; set; }
        public string DichVu { get; set; }
        public int SoNgay { get; set; }
        public double ThanhTien { get; set; }
    }
}
