using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab07_Bai1_Cafe.Models
{
    public class CafeInvoice
    {
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string TableName { get; set; }
        public bool IsStudent { get; set; }
        public List<FoodDrinkItem> Foods { get; set; }
        public List<FoodDrinkItem> Drinks { get; set; }

        public decimal SubTotal
        {
            get
            {
                decimal foodTotal = Foods != null ? Foods.Sum(x => x.Price) : 0;
                decimal drinkTotal = Drinks != null ? Drinks.Sum(x => x.Price) : 0;
                return foodTotal + drinkTotal;
            }
        }

        public decimal Discount
        {
            get { return IsStudent ? SubTotal * 0.2m : 0; }
        }

        public decimal Total
        {
            get { return SubTotal - Discount; }
        }

        public string FoodText
        {
            get
            {
                if (Foods == null || Foods.Count == 0) return "";
                return string.Join(", ", Foods.Select(x => x.Name));
            }
        }

        public string DrinkText
        {
            get
            {
                if (Drinks == null || Drinks.Count == 0) return "";
                return string.Join(", ", Drinks.Select(x => x.Name));
            }
        }
    }

}
