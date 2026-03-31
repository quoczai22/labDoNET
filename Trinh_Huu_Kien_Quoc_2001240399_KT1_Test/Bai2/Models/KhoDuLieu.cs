using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai2.Models
{
    public class KhoDuLieu
    {
        public List<LoaiSanPham> DanhSachLoaiSanPham { get; set; }
        public List<SanPham> DanhSachSanPham { get; set; }

        public KhoDuLieu()
        {
            DanhSachLoaiSanPham = new List<LoaiSanPham>();
            DanhSachSanPham = new List<SanPham>();
            KhoiTaoDuLieu();
        }

        private void KhoiTaoDuLieu()
        {
            // Dữ liệu Loại sản phẩm
            DanhSachLoaiSanPham.Add(new LoaiSanPham { MaLoai = "N1", TenLoai = "Cafe" });
            DanhSachLoaiSanPham.Add(new LoaiSanPham { MaLoai = "N2", TenLoai = "Trà" });
            DanhSachLoaiSanPham.Add(new LoaiSanPham { MaLoai = "N3", TenLoai = "Sinh tố" });

            // Dữ liệu Sản phẩm
            DanhSachSanPham.Add(new SanPham { MaSanPham = "1", TenSanPham = "Café đá", DonGia = 23000, MaLoai = "N1" });
            DanhSachSanPham.Add(new SanPham { MaSanPham = "2", TenSanPham = "Café sữa", DonGia = 28000, MaLoai = "N1" });
            DanhSachSanPham.Add(new SanPham { MaSanPham = "3", TenSanPham = "Bạc xỉu", DonGia = 30000, MaLoai = "N1" });
            DanhSachSanPham.Add(new SanPham { MaSanPham = "4", TenSanPham = "Trà sữa Oolong", DonGia = 35000, MaLoai = "N2" });
            DanhSachSanPham.Add(new SanPham { MaSanPham = "5", TenSanPham = "Trà lài", DonGia = 32000, MaLoai = "N2" });
            DanhSachSanPham.Add(new SanPham { MaSanPham = "6", TenSanPham = "Trà hoa cúc", DonGia = 34000, MaLoai = "N2" });
            DanhSachSanPham.Add(new SanPham { MaSanPham = "7", TenSanPham = "Sinh tố dâu", DonGia = 40000, MaLoai = "N3" });
        }
    }
}
