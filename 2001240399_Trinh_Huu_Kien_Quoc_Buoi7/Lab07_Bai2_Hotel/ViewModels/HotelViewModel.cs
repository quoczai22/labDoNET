using Lab07_Bai2_Hotel.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Lab07_Bai2_Hotel.ViewModels
{
    public class HotelViewModel : BaseViewModel
    {
        private ObservableCollection<HotelBill> _bills;
        public ObservableCollection<HotelBill> Bills
        {
            get { return _bills; }
            set
            {
                _bills = value;
                OnPropertyChanged(nameof(Bills));
            }
        }

        private ICollectionView _billsView;
        public ICollectionView BillsView
        {
            get { return _billsView; }
            set
            {
                _billsView = value;
                OnPropertyChanged(nameof(BillsView));
            }
        }

        private HotelBill _selectedBill;
        public HotelBill SelectedBill
        {
            get { return _selectedBill; }
            set
            {
                _selectedBill = value;
                OnPropertyChanged(nameof(SelectedBill));
            }
        }

        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        private int _days;
        public int Days
        {
            get { return _days; }
            set
            {
                _days = value;
                OnPropertyChanged(nameof(Days));
            }
        }

        private string _roomType;
        public string RoomType
        {
            get { return _roomType; }
            set
            {
                _roomType = value;
                OnPropertyChanged(nameof(RoomType));
            }
        }

        private bool _hasTivi;
        public bool HasTivi
        {
            get { return _hasTivi; }
            set
            {
                _hasTivi = value;
                OnPropertyChanged(nameof(HasTivi));
            }
        }

        private bool _hasInternet;
        public bool HasInternet
        {
            get { return _hasInternet; }
            set
            {
                _hasInternet = value;
                OnPropertyChanged(nameof(HasInternet));
            }
        }

        private bool _hasHotWater;
        public bool HasHotWater
        {
            get { return _hasHotWater; }
            set
            {
                _hasHotWater = value;
                OnPropertyChanged(nameof(HasHotWater));
            }
        }

        private bool _karaoke;
        public bool Karaoke
        {
            get { return _karaoke; }
            set
            {
                _karaoke = value;
                OnPropertyChanged(nameof(Karaoke));
            }
        }

        private bool _breakfast;
        public bool Breakfast
        {
            get { return _breakfast; }
            set
            {
                _breakfast = value;
                OnPropertyChanged(nameof(Breakfast));
            }
        }

        public ObservableCollection<string> RoomTypes { get; set; }

        public int TotalCheckoutCount => Bills.Count;
        public decimal TotalRevenue => Bills.Sum(x => x.TotalAmount);

        public RelayCommand CalculateCommand { get; set; }
        public RelayCommand ResetCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand LoadCommand { get; set; }

        public HotelViewModel()
        {
            Bills = new ObservableCollection<HotelBill>();
            BillsView = CollectionViewSource.GetDefaultView(Bills);

            RoomTypes = new ObservableCollection<string>
            {
                "Phòng đơn",
                "Phòng đôi",
                "Phòng ba"
            };

            RoomType = "Phòng đơn";

            CalculateCommand = new RelayCommand(CalculateBill);
            ResetCommand = new RelayCommand(x => ResetForm());
            DeleteCommand = new RelayCommand(x => RemoveBill(), x => SelectedBill != null);
        }

        private decimal GetRoomPrice()
        {
            if (RoomType == "Phòng đơn") return 300000;
            if (RoomType == "Phòng đôi") return 350000;
            return 400000;
        }

        public void CalculateBill(object obj)
        {
            if (string.IsNullOrWhiteSpace(FullName))
            {
                MessageBox.Show("Họ tên không được rỗng!");
                return;
            }

            if (Days <= 0)
            {
                MessageBox.Show("Số ngày ở phải lớn hơn 0!");
                return;
            }

            decimal roomCost = GetRoomPrice() * Days;

            int amenity = 0;
            if (HasTivi) amenity++;
            if (HasInternet) amenity++;
            if (HasHotWater) amenity++;

            decimal amenityCost = amenity * 10000;

            decimal serviceCost = 0;
            if (Karaoke) serviceCost += 50000;
            if (Breakfast) serviceCost += 15000 * Days;

            decimal total = roomCost + amenityCost + serviceCost;

            Bills.Add(new HotelBill
            {
                FullName = FullName,
                Address = Address,
                Days = Days,
                RoomType = RoomType,
                HasTivi = HasTivi,
                HasInternet = HasInternet,
                HasHotWater = HasHotWater,
                Karaoke = Karaoke,
                Breakfast = Breakfast,
                TotalAmount = total
            });

            RefreshStatistics();
            MessageBox.Show("Tổng tiền thanh toán: " + total.ToString("N0"));
            ResetForm();
        }

        public void RemoveBill()
        {
            if (SelectedBill != null)
            {
                Bills.Remove(SelectedBill);
                SelectedBill = null;
                RefreshStatistics();
            }
        }

        public void ResetForm()
        {
            FullName = string.Empty;
            Address = string.Empty;
            Days = 0;
            RoomType = "Phòng đơn";
            HasTivi = false;
            HasInternet = false;
            HasHotWater = false;
            Karaoke = false;
            Breakfast = false;
        }

        public void RefreshStatistics()
        {
            BillsView = CollectionViewSource.GetDefaultView(Bills);
            OnPropertyChanged(nameof(Bills));
            OnPropertyChanged(nameof(BillsView));
            OnPropertyChanged(nameof(TotalCheckoutCount));
            OnPropertyChanged(nameof(TotalRevenue));
        }
    }
}
