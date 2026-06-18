using System;
using System.Linq;
using _2001240399_Trinh_Huu_Kien_Quoc_KT2L2.Models;
class Check
{
    static void Main()
    {
        using (var db = new QuanLyKhachSanEntities())
        {
            Console.WriteLine(db.PHONGs.Count());
        }
    }
}
