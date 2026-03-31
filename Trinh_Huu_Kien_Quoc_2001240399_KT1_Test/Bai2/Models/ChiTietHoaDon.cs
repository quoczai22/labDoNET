using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai2.Models
{
    public class ChiTietHoaDon
    {
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }

        public double ThanhTien()
        {
            return DonGia * SoLuong;
        }

        public override string ToString()
        {
            return $"{TenSanPham} - SL: {SoLuong} - Đơn giá: {DonGia} - Thành tiền: {ThanhTien()}";
        }
    }
}
