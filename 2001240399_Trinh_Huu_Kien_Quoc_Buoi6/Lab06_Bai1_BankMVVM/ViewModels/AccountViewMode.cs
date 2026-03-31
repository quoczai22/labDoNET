using Lab06_Bai1_BankMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Lab06_Bai1_BankMVVM.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        public ObservableCollection<AccountModel> Accounts { get; set; }
        public ObservableCollection<string> Cities { get; set; }

        private AccountModel _selectedAccount;
        public AccountModel SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));

                if (_selectedAccount != null && !IsAdding && !IsEditing)
                {
                    InputAccountNumber = _selectedAccount.AccountNumber;
                    InputCustomerName = _selectedAccount.CustomerName;
                    InputAddress = _selectedAccount.Address;
                    InputCity = _selectedAccount.City;
                    InputBalance = _selectedAccount.Balance;
                }

                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _inputAccountNumber;
        public string InputAccountNumber
        {
            get => _inputAccountNumber;
            set
            {
                _inputAccountNumber = value;
                OnPropertyChanged(nameof(InputAccountNumber));
            }
        }

        private string _inputCustomerName;
        public string InputCustomerName
        {
            get => _inputCustomerName;
            set
            {
                _inputCustomerName = value;
                OnPropertyChanged(nameof(InputCustomerName));
            }
        }

        private string _inputAddress;
        public string InputAddress
        {
            get => _inputAddress;
            set
            {
                _inputAddress = value;
                OnPropertyChanged(nameof(InputAddress));
            }
        }

        private string _inputCity;
        public string InputCity
        {
            get => _inputCity;
            set
            {
                _inputCity = value;
                OnPropertyChanged(nameof(InputCity));
            }
        }

        private decimal _inputBalance;
        public decimal InputBalance
        {
            get => _inputBalance;
            set
            {
                _inputBalance = value;
                OnPropertyChanged(nameof(InputBalance));
            }
        }

        private bool _isAdding;
        public bool IsAdding
        {
            get => _isAdding;
            set
            {
                _isAdding = value;
                OnPropertyChanged(nameof(IsAdding));
                OnPropertyChanged(nameof(AddButtonText));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string AddButtonText => IsAdding ? "Hủy" : "Thêm";

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
                OnPropertyChanged(nameof(EditButtonText));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string EditButtonText => IsEditing ? "Hủy" : "Sửa";

        public decimal TotalBalance => Accounts.Sum(x => x.Balance);

        public ICommand AddCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public AccountViewModel()
        {
            Accounts = new ObservableCollection<AccountModel>();
            Cities = new ObservableCollection<string>();

            Cities.Add("HCM");
            Cities.Add("HN");
            Cities.Add("Đà Nẵng");
            Cities.Add("Cần Thơ");

            LoadSampleData();

            AddCommand = new RelayCommand(AddOrCancel);
            SaveCommand = new RelayCommand(Save, CanSave);
            EditCommand = new RelayCommand(EditOrCancel, CanEditOrDelete);
            DeleteCommand = new RelayCommand(Delete, CanEditOrDelete);
        }

        private void LoadSampleData()
        {
            Accounts.Add(new AccountModel
            {
                STT = 1,
                AccountNumber = "001",
                CustomerName = "Nguyễn Văn A",
                Address = "Q1",
                City = "HCM",
                Balance = 1000000
            });

            Accounts.Add(new AccountModel
            {
                STT = 2,
                AccountNumber = "002",
                CustomerName = "Trần Văn B",
                Address = "Q7",
                City = "HN",
                Balance = 2000000
            });

            Accounts.Add(new AccountModel
            {
                STT = 3,
                AccountNumber = "003",
                CustomerName = "Lê Văn C",
                Address = "Q10",
                City = "Đà Nẵng",
                Balance = 1500000
            });

            OnPropertyChanged(nameof(TotalBalance));
        }

        private void AddOrCancel(object obj)
        {
            if (!IsAdding)
            {
                ClearInput();
                SelectedAccount = null;
                IsAdding = true;
                IsEditing = false;
            }
            else
            {
                ClearInput();
                IsAdding = false;
            }
        }

        private void EditOrCancel(object obj)
        {
            if (!IsEditing)
            {
                if (SelectedAccount == null)
                {
                    MessageBox.Show("Vui lòng chọn tài khoản cần sửa.");
                    return;
                }

                InputAccountNumber = SelectedAccount.AccountNumber;
                InputCustomerName = SelectedAccount.CustomerName;
                InputAddress = SelectedAccount.Address;
                InputCity = SelectedAccount.City;
                InputBalance = SelectedAccount.Balance;

                IsEditing = true;
                IsAdding = false;
            }
            else
            {
                ClearInput();
                IsEditing = false;

                if (SelectedAccount != null)
                {
                    InputAccountNumber = SelectedAccount.AccountNumber;
                    InputCustomerName = SelectedAccount.CustomerName;
                    InputAddress = SelectedAccount.Address;
                    InputCity = SelectedAccount.City;
                    InputBalance = SelectedAccount.Balance;
                }
            }
        }

        private void Save(object obj)
        {
            if (!ValidateInput())
                return;

            if (IsAdding)
            {
                AccountModel acc = new AccountModel
                {
                    STT = Accounts.Count + 1,
                    AccountNumber = InputAccountNumber.Trim(),
                    CustomerName = InputCustomerName.Trim(),
                    Address = InputAddress,
                    City = InputCity,
                    Balance = InputBalance
                };

                Accounts.Add(acc);
                UpdateSTT();

                OnPropertyChanged(nameof(TotalBalance));
                MessageBox.Show("Thêm tài khoản thành công.");

                ClearInput();
                IsAdding = false;
            }
            else if (IsEditing)
            {
                if (SelectedAccount == null)
                {
                    MessageBox.Show("Không có tài khoản để sửa.");
                    return;
                }

                SelectedAccount.AccountNumber = InputAccountNumber.Trim();
                SelectedAccount.CustomerName = InputCustomerName.Trim();
                SelectedAccount.Address = InputAddress;
                SelectedAccount.City = InputCity;
                SelectedAccount.Balance = InputBalance;

                RefreshDataGrid();
                OnPropertyChanged(nameof(TotalBalance));

                MessageBox.Show("Cập nhật tài khoản thành công.");

                ClearInput();
                IsEditing = false;
            }
        }

        private void Delete(object obj)
        {
            if (SelectedAccount == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa.");
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                "Bạn có chắc muốn xóa tài khoản này không?",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Accounts.Remove(SelectedAccount);
                SelectedAccount = null;

                UpdateSTT();
                ClearInput();

                OnPropertyChanged(nameof(TotalBalance));
                MessageBox.Show("Xóa thành công.");
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(InputAccountNumber))
            {
                MessageBox.Show("Số tài khoản không được rỗng.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(InputCustomerName))
            {
                MessageBox.Show("Tên khách hàng không được rỗng.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(InputCity))
            {
                MessageBox.Show("Vui lòng chọn thành phố.");
                return false;
            }

            if (InputBalance < 0)
            {
                MessageBox.Show("Số tiền không được âm.");
                return false;
            }

            bool isDuplicate = false;

            if (IsAdding)
            {
                isDuplicate = Accounts.Any(x => x.AccountNumber == InputAccountNumber.Trim());
            }
            else if (IsEditing && SelectedAccount != null)
            {
                isDuplicate = Accounts.Any(x =>
                    x != SelectedAccount &&
                    x.AccountNumber == InputAccountNumber.Trim());
            }

            if (isDuplicate)
            {
                MessageBox.Show("Số tài khoản đã tồn tại.");
                return false;
            }

            return true;
        }

        private bool CanSave(object obj)
        {
            return IsAdding || IsEditing;
        }

        private bool CanEditOrDelete(object obj)
        {
            return SelectedAccount != null;
        }

        private void ClearInput()
        {
            InputAccountNumber = "";
            InputCustomerName = "";
            InputAddress = "";
            InputCity = null;
            InputBalance = 0;
        }

        private void UpdateSTT()
        {
            for (int i = 0; i < Accounts.Count; i++)
            {
                Accounts[i].STT = i + 1;
            }
        }

        private void RefreshDataGrid()
        {
            var temp = Accounts.ToList();
            Accounts.Clear();

            foreach (var item in temp)
            {
                Accounts.Add(item);
            }
        }
    }
}
