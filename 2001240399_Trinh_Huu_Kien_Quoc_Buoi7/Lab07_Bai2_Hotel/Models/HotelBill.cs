using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab07_Bai2_Hotel.Models
{
    public class HotelBill
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public int Days { get; set; }
        public string RoomType { get; set; }
        public bool HasTivi { get; set; }
        public bool HasInternet { get; set; }
        public bool HasHotWater { get; set; }
        public bool Karaoke { get; set; }
        public bool Breakfast { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
