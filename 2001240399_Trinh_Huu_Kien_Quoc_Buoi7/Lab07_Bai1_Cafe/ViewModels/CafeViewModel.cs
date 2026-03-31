using Lab07_Bai1_Cafe.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Lab07_Bai1_Cafe.ViewModels
{
    public class CafeViewModel : BaseViewModel
    {
        public int TotalCustomerCount => Invoices.Count;
        public decimal TotalRevenue => Invoices.Sum(x => x.Total);

        private ObservableCollection<CafeInvoice> _invoices;
        public ObservableCollection<CafeInvoice> Invoices
        {
            get { return _invoices; }
            set
            {
                _invoices = value;
                OnPropertyChanged(nameof(Invoices));
            }
        }

        private ICollectionView _invoicesView;
        public ICollectionView InvoicesView
        {
            get { return _invoicesView; }
            set
            {
                _invoicesView = value;
                OnPropertyChanged(nameof(InvoicesView));
            }
        }

        private CafeInvoice _selectedInvoice;
        public CafeInvoice SelectedInvoice
        {
            get { return _selectedInvoice; }
            set
            {
                _selectedInvoice = value;
                OnPropertyChanged(nameof(SelectedInvoice));
            }
        }

        public ObservableCollection<string> Tables { get; set; }
        public ObservableCollection<FoodDrinkItem> FoodList { get; set; }
        public ObservableCollection<FoodDrinkItem> DrinkList { get; set; }

        private string _customerName;
        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                _customerName = value;
                OnPropertyChanged(nameof(CustomerName));
            }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        private string _selectedTable;
        public string SelectedTable
        {
            get { return _selectedTable; }
            set
            {
                _selectedTable = value;
                OnPropertyChanged(nameof(SelectedTable));
            }
        }

        private bool _isStudent;
        public bool IsStudent
        {
            get { return _isStudent; }
            set
            {
                _isStudent = value;
                OnPropertyChanged(nameof(IsStudent));
            }
        }

        private bool _food1;
        public bool Food1 { get { return _food1; } set { _food1 = value; OnPropertyChanged(nameof(Food1)); } }

        private bool _food2;
        public bool Food2 { get { return _food2; } set { _food2 = value; OnPropertyChanged(nameof(Food2)); } }

        private bool _food3;
        public bool Food3 { get { return _food3; } set { _food3 = value; OnPropertyChanged(nameof(Food3)); } }

        private bool _food4;
        public bool Food4 { get { return _food4; } set { _food4 = value; OnPropertyChanged(nameof(Food4)); } }

        private bool _food5;
        public bool Food5 { get { return _food5; } set { _food5 = value; OnPropertyChanged(nameof(Food5)); } }

        private bool _drink1;
        public bool Drink1 { get { return _drink1; } set { _drink1 = value; OnPropertyChanged(nameof(Drink1)); } }

        private bool _drink2;
        public bool Drink2 { get { return _drink2; } set { _drink2 = value; OnPropertyChanged(nameof(Drink2)); } }

        private bool _drink3;
        public bool Drink3 { get { return _drink3; } set { _drink3 = value; OnPropertyChanged(nameof(Drink3)); } }

        private bool _drink4;
        public bool Drink4 { get { return _drink4; } set { _drink4 = value; OnPropertyChanged(nameof(Drink4)); } }

        private bool _drink5;
        public bool Drink5 { get { return _drink5; } set { _drink5 = value; OnPropertyChanged(nameof(Drink5)); } }

        public RelayCommand CreateCommand { get; set; }
        public RelayCommand CheckoutCommand { get; set; }
        public RelayCommand ResetCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand LoadCommand { get; set; }

        public CafeViewModel()
        {
            Invoices = new ObservableCollection<CafeInvoice>();

            Tables = new ObservableCollection<string>
            {
                "Bàn 1",
                "Bàn 2",
                "Bàn 3",
                "Bàn 4"
            };

            FoodList = new ObservableCollection<FoodDrinkItem>
            {
                new FoodDrinkItem { Name = "Bánh mì trứng", Price = 15000, Type = "Food" },
                new FoodDrinkItem { Name = "Bánh mì cá", Price = 15000, Type = "Food" },
                new FoodDrinkItem { Name = "Mì tôm trứng", Price = 20000, Type = "Food" },
                new FoodDrinkItem { Name = "Mì xào bò", Price = 30000, Type = "Food" },
                new FoodDrinkItem { Name = "Mì cay", Price = 50000, Type = "Food" }
            };

            DrinkList = new ObservableCollection<FoodDrinkItem>
            {
                new FoodDrinkItem { Name = "Cafe đen", Price = 20000, Type = "Drink" },
                new FoodDrinkItem { Name = "Cafe đá", Price = 25000, Type = "Drink" },
                new FoodDrinkItem { Name = "Cafe sữa", Price = 25000, Type = "Drink" },
                new FoodDrinkItem { Name = "Cafe sữa đá", Price = 30000, Type = "Drink" },
                new FoodDrinkItem { Name = "Cafe kem", Price = 35000, Type = "Drink" }
            };

            InvoicesView = CollectionViewSource.GetDefaultView(Invoices);

            CreateCommand = new RelayCommand(CreateInvoice);
            CheckoutCommand = new RelayCommand(Checkout);
            ResetCommand = new RelayCommand(x => ResetForm());
            DeleteCommand = new RelayCommand(x => RemoveInvoice(), x => SelectedInvoice != null);
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(CustomerName))
            {
                MessageBox.Show("Tên khách hàng không được rỗng!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Phone))
            {
                MessageBox.Show("Số điện thoại không được rỗng!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(SelectedTable))
            {
                MessageBox.Show("Phải chọn 1 bàn!");
                return false;
            }

            bool hasFood = Food1 || Food2 || Food3 || Food4 || Food5;
            bool hasDrink = Drink1 || Drink2 || Drink3 || Drink4 || Drink5;

            if (!hasFood && !hasDrink)
            {
                MessageBox.Show("Phải chọn ít nhất 1 món ăn hoặc 1 nước uống!");
                return false;
            }

            return true;
        }

        private List<FoodDrinkItem> GetSelectedFoods()
        {
            List<FoodDrinkItem> foods = new List<FoodDrinkItem>();

            if (Food1) foods.Add(FoodList[0]);
            if (Food2) foods.Add(FoodList[1]);
            if (Food3) foods.Add(FoodList[2]);
            if (Food4) foods.Add(FoodList[3]);
            if (Food5) foods.Add(FoodList[4]);

            return foods;
        }

        private List<FoodDrinkItem> GetSelectedDrinks()
        {
            List<FoodDrinkItem> drinks = new List<FoodDrinkItem>();

            if (Drink1) drinks.Add(DrinkList[0]);
            if (Drink2) drinks.Add(DrinkList[1]);
            if (Drink3) drinks.Add(DrinkList[2]);
            if (Drink4) drinks.Add(DrinkList[3]);
            if (Drink5) drinks.Add(DrinkList[4]);

            return drinks;
        }

        public void CreateInvoice(object obj)
        {
            if (!ValidateInput())
                return;

            CafeInvoice invoice = new CafeInvoice
            {
                CustomerName = CustomerName,
                Phone = Phone,
                TableName = SelectedTable,
                IsStudent = IsStudent,
                Foods = GetSelectedFoods(),
                Drinks = GetSelectedDrinks()
            };

            MessageBox.Show(
                "Tên khách hàng: " + invoice.CustomerName +
                "\nSố điện thoại: " + invoice.Phone +
                "\nBàn: " + invoice.TableName +
                "\nThức ăn: " + invoice.FoodText +
                "\nNước uống: " + invoice.DrinkText +
                "\nTạm tính: " + invoice.SubTotal.ToString("N0") +
                "\nGiảm giá: " + invoice.Discount.ToString("N0") +
                "\nTổng tiền: " + invoice.Total.ToString("N0"),
                "Thông tin hóa đơn"
            );
        }

        public void Checkout(object obj)
        {
            if (!ValidateInput())
                return;

            CafeInvoice invoice = new CafeInvoice
            {
                CustomerName = CustomerName,
                Phone = Phone,
                TableName = SelectedTable,
                IsStudent = IsStudent,
                Foods = GetSelectedFoods(),
                Drinks = GetSelectedDrinks()
            };

            Invoices.Add(invoice);
            RefreshStatistics();

            MessageBox.Show("Thanh toán thành công!");
            ResetForm();
        }

        public void RemoveInvoice()
        {
            if (SelectedInvoice != null)
            {
                Invoices.Remove(SelectedInvoice);
                SelectedInvoice = null;
                RefreshStatistics();
            }
        }

        public void ResetForm()
        {
            CustomerName = string.Empty;
            Phone = string.Empty;
            SelectedTable = null;
            IsStudent = false;

            Food1 = false;
            Food2 = false;
            Food3 = false;
            Food4 = false;
            Food5 = false;

            Drink1 = false;
            Drink2 = false;
            Drink3 = false;
            Drink4 = false;
            Drink5 = false;
        }

        public void RefreshStatistics()
        {
            InvoicesView = CollectionViewSource.GetDefaultView(Invoices);
            OnPropertyChanged(nameof(Invoices));
            OnPropertyChanged(nameof(InvoicesView));
            OnPropertyChanged(nameof(TotalCustomerCount));
            OnPropertyChanged(nameof(TotalRevenue));
        }
    }

}
