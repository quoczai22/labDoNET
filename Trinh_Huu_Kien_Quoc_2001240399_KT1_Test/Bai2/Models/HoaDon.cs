using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai2.Models
{
    public class HoaDon
    {
        public string TenKhachHang { get; set; }
        public string DienThoai { get; set; }
        public string TenBan { get; set; }
        public List<ChiTietHoaDon> DanhSachChiTiet { get; set; }

        public HoaDon()
        {
            DanhSachChiTiet = new List<ChiTietHoaDon>();
        }

        public double TongTien()
        {
            return DanhSachChiTiet.Sum(x => x.ThanhTien());
        }

        public override string ToString()
        {
            string details = string.Join("\n", DanhSachChiTiet.Select(x => x.ToString()));
            return $"Khách hàng: {TenKhachHang}\nSĐT: {DienThoai}\nBàn: {TenBan}\n\nChi tiết:\n{details}\n\nTổng cộng: {TongTien()}";
        }
    }
}
