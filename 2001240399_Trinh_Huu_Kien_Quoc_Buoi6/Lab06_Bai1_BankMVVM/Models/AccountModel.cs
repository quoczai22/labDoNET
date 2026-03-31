using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab06_Bai1_BankMVVM.Models
{
    public class AccountModel
    {
        public int STT { get; set; }
        public string AccountNumber { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public decimal Balance { get; set; }
    }
}
