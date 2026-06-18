using BTVN.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BTVN.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string _currentPage = "Employee";
        public bool IsEmployeeVisible => _currentPage == "Employee";
        public bool IsAccountVisible => _currentPage == "Account";
        public bool IsStudentVisible => _currentPage == "Student";

        public ObservableCollection<EmployeeModel> Employees { get; set; }
        public ObservableCollection<AccountModel> Accounts { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public ObservableCollection<StudentClass> Classes { get; set; }

        private EmployeeModel? _selectedEmployee;
        public EmployeeModel? SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string EmployeeId { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeAddress { get; set; } = string.Empty;
        public string EmployeePosition { get; set; } = string.Empty;

        private AccountModel? _selectedAccount;
        public AccountModel? SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
                if (value != null)
                {
                    AccountNumber = value.AccountNumber;
                    CustomerName = value.CustomerName;
                    AccountAddress = value.Address;
                    SelectedCity = value.City;
                    Balance = value.Balance;
                    OnAccountInputChanged();
                }
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string AccountNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string AccountAddress { get; set; } = string.Empty;
        public string? SelectedCity { get; set; }
        public decimal Balance { get; set; }

        public decimal TotalBalance => Accounts.Sum(x => x.Balance);

        private StudentClass? _selectedClass;
        public StudentClass? SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged(nameof(SelectedClass));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private Student? _selectedStudent;
        public Student? SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
                if (value != null)
                {
                    StudentId = value.StudentId;
                    StudentName = value.FullName;
                    StudentAddress = value.Address;
                    OnStudentInputChanged();
                }
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string StudentAddress { get; set; } = string.Empty;
        public bool IsAddClassMode { get; set; }
        public string NewClassName { get; set; } = string.Empty;

        public ICommand ShowEmployeeCommand { get; set; }
        public ICommand ShowAccountCommand { get; set; }
        public ICommand ShowStudentCommand { get; set; }

        public ICommand AddEmployeeCommand { get; set; }
        public ICommand SaveEmployeeCommand { get; set; }
        public ICommand EditEmployeeCommand { get; set; }
        public ICommand DeleteEmployeeCommand { get; set; }

        public ICommand AddAccountCommand { get; set; }
        public ICommand SaveAccountCommand { get; set; }
        public ICommand EditAccountCommand { get; set; }
        public ICommand DeleteAccountCommand { get; set; }

        public ICommand AddClassCommand { get; set; }
        public ICommand SaveStudentCommand { get; set; }
        public ICommand DeleteStudentCommand { get; set; }

        public MainViewModel()
        {
            Employees = new ObservableCollection<EmployeeModel>();
            Accounts = new ObservableCollection<AccountModel>();
            Cities = new ObservableCollection<string> { "HCM", "HN", "Đà Nẵng", "Cần Thơ" };
            Classes = new ObservableCollection<StudentClass>();

            LoadSampleData();

            ShowEmployeeCommand = new RelayCommand(_ => ShowPage("Employee"));
            ShowAccountCommand = new RelayCommand(_ => ShowPage("Account"));
            ShowStudentCommand = new RelayCommand(_ => ShowPage("Student"));

            AddEmployeeCommand = new RelayCommand(_ => ClearEmployeeInput());
            SaveEmployeeCommand = new RelayCommand(_ => SaveEmployee());
            EditEmployeeCommand = new RelayCommand(_ => LoadEmployeeInput(), _ => SelectedEmployee != null);
            DeleteEmployeeCommand = new RelayCommand(_ => DeleteEmployee(), _ => SelectedEmployee != null);

            AddAccountCommand = new RelayCommand(_ => ClearAccountInput());
            SaveAccountCommand = new RelayCommand(_ => SaveAccount());
            EditAccountCommand = new RelayCommand(_ => SaveAccount(), _ => SelectedAccount != null);
            DeleteAccountCommand = new RelayCommand(_ => DeleteAccount(), _ => SelectedAccount != null);

            AddClassCommand = new RelayCommand(_ => AddClass(), _ => IsAddClassMode && !string.IsNullOrWhiteSpace(NewClassName));
            SaveStudentCommand = new RelayCommand(_ => SaveStudent(), _ => SelectedClass != null);
            DeleteStudentCommand = new RelayCommand(_ => DeleteStudent(), _ => SelectedStudent != null);
        }

        public void NotifyEmployeeInputChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        public void OnAccountInputChanged()
        {
            OnPropertyChanged(nameof(AccountNumber));
            OnPropertyChanged(nameof(CustomerName));
            OnPropertyChanged(nameof(AccountAddress));
            OnPropertyChanged(nameof(SelectedCity));
            OnPropertyChanged(nameof(Balance));
        }

        public void OnStudentInputChanged()
        {
            OnPropertyChanged(nameof(StudentId));
            OnPropertyChanged(nameof(StudentName));
            OnPropertyChanged(nameof(StudentAddress));
            OnPropertyChanged(nameof(IsAddClassMode));
            OnPropertyChanged(nameof(NewClassName));
            CommandManager.InvalidateRequerySuggested();
        }

        public void SelectStudentTreeItem(object? item)
        {
            if (item is StudentClass studentClass)
            {
                SelectedClass = studentClass;
                SelectedStudent = null;
            }
            else if (item is Student student)
            {
                SelectedStudent = student;
                SelectedClass = Classes.FirstOrDefault(c => c.Students.Contains(student));
            }
        }

        private void LoadSampleData()
        {
            Employees.Add(new EmployeeModel { STT = 1, EmployeeId = "NV001", FullName = "Nguyễn Văn An", Address = "Q1", Position = "Quản lý" });
            Employees.Add(new EmployeeModel { STT = 2, EmployeeId = "NV002", FullName = "Trần Thị Bình", Address = "Q3", Position = "Nhân viên" });

            Accounts.Add(new AccountModel { STT = 1, AccountNumber = "001", CustomerName = "Nguyễn Văn A", Address = "Q1", City = "HCM", Balance = 1000000 });

            Classes.Add(new StudentClass
            {
                ClassName = "05DHTH1",
                Students =
                {
                    new Student { StudentId = "001", FullName = "Lương Minh Châu", Address = "Q12" },
                    new Student { StudentId = "002", FullName = "Nguyễn Minh Đạt", Address = "Q1" }
                }
            });
            Classes.Add(new StudentClass
            {
                ClassName = "05DHTH2",
                Students = { new Student { StudentId = "003", FullName = "Nguyễn Trí Đức", Address = "Q5" } }
            });
            Classes.Add(new StudentClass { ClassName = "05DHTH3" });
            Classes.Add(new StudentClass { ClassName = "05DHTH4" });
        }

        private void ShowPage(string page)
        {
            _currentPage = page;
            OnPropertyChanged(nameof(IsEmployeeVisible));
            OnPropertyChanged(nameof(IsAccountVisible));
            OnPropertyChanged(nameof(IsStudentVisible));
        }

        private void SaveEmployee()
        {
            EmployeeId = EmployeeId.Trim();
            EmployeeName = EmployeeName.Trim();
            EmployeeAddress = EmployeeAddress.Trim();
            EmployeePosition = EmployeePosition.Trim();

            if (string.IsNullOrWhiteSpace(EmployeeId) || string.IsNullOrWhiteSpace(EmployeeName))
            {
                MessageBox.Show("Vui lòng nhập mã và họ tên nhân viên.");
                return;
            }

            bool duplicate = Employees.Any(x => x.EmployeeId.Equals(EmployeeId, StringComparison.OrdinalIgnoreCase) && x != SelectedEmployee);
            if (duplicate)
            {
                MessageBox.Show("Mã nhân viên đã tồn tại.");
                return;
            }

            if (SelectedEmployee == null)
            {
                Employees.Add(new EmployeeModel
                {
                    STT = Employees.Count + 1,
                    EmployeeId = EmployeeId.ToUpper(),
                    FullName = EmployeeName,
                    Address = EmployeeAddress,
                    Position = EmployeePosition
                });
            }
            else
            {
                SelectedEmployee.EmployeeId = EmployeeId.ToUpper();
                SelectedEmployee.FullName = EmployeeName;
                SelectedEmployee.Address = EmployeeAddress;
                SelectedEmployee.Position = EmployeePosition;
                OnPropertyChanged(nameof(Employees));
            }

            ClearEmployeeInput();
        }

        private void LoadEmployeeInput()
        {
            if (SelectedEmployee == null)
                return;

            EmployeeId = SelectedEmployee.EmployeeId;
            EmployeeName = SelectedEmployee.FullName;
            EmployeeAddress = SelectedEmployee.Address;
            EmployeePosition = SelectedEmployee.Position;
            OnPropertyChanged(nameof(EmployeeId));
            OnPropertyChanged(nameof(EmployeeName));
            OnPropertyChanged(nameof(EmployeeAddress));
            OnPropertyChanged(nameof(EmployeePosition));
        }

        private void DeleteEmployee()
        {
            if (SelectedEmployee == null)
                return;

            Employees.Remove(SelectedEmployee);
            UpdateEmployeeStt();
            ClearEmployeeInput();
        }

        private void ClearEmployeeInput()
        {
            SelectedEmployee = null;
            EmployeeId = string.Empty;
            EmployeeName = string.Empty;
            EmployeeAddress = string.Empty;
            EmployeePosition = string.Empty;
            OnPropertyChanged(nameof(EmployeeId));
            OnPropertyChanged(nameof(EmployeeName));
            OnPropertyChanged(nameof(EmployeeAddress));
            OnPropertyChanged(nameof(EmployeePosition));
        }

        private void UpdateEmployeeStt()
        {
            for (int i = 0; i < Employees.Count; i++)
                Employees[i].STT = i + 1;
        }

        private void SaveAccount()
        {
            AccountNumber = AccountNumber.Trim();
            CustomerName = CustomerName.Trim();
            AccountAddress = AccountAddress.Trim();

            if (string.IsNullOrWhiteSpace(AccountNumber) || string.IsNullOrWhiteSpace(CustomerName) || string.IsNullOrWhiteSpace(SelectedCity))
            {
                MessageBox.Show("Vui lòng nhập số tài khoản, tên khách hàng và thành phố.");
                return;
            }

            if (Balance < 0)
            {
                MessageBox.Show("Số tiền không được âm.");
                return;
            }

            bool duplicate = Accounts.Any(x => x.AccountNumber.Equals(AccountNumber, StringComparison.OrdinalIgnoreCase) && x != SelectedAccount);
            if (duplicate)
            {
                MessageBox.Show("Số tài khoản đã tồn tại.");
                return;
            }

            if (SelectedAccount == null)
            {
                Accounts.Add(new AccountModel
                {
                    STT = Accounts.Count + 1,
                    AccountNumber = AccountNumber,
                    CustomerName = CustomerName,
                    Address = AccountAddress,
                    City = SelectedCity,
                    Balance = Balance
                });
            }
            else
            {
                SelectedAccount.AccountNumber = AccountNumber;
                SelectedAccount.CustomerName = CustomerName;
                SelectedAccount.Address = AccountAddress;
                SelectedAccount.City = SelectedCity;
                SelectedAccount.Balance = Balance;
                OnPropertyChanged(nameof(Accounts));
            }

            OnPropertyChanged(nameof(TotalBalance));
            ClearAccountInput();
        }

        private void DeleteAccount()
        {
            if (SelectedAccount == null)
                return;

            Accounts.Remove(SelectedAccount);
            UpdateAccountStt();
            OnPropertyChanged(nameof(TotalBalance));
            ClearAccountInput();
        }

        private void ClearAccountInput()
        {
            SelectedAccount = null;
            AccountNumber = string.Empty;
            CustomerName = string.Empty;
            AccountAddress = string.Empty;
            SelectedCity = null;
            Balance = 0;
            OnAccountInputChanged();
        }

        private void UpdateAccountStt()
        {
            for (int i = 0; i < Accounts.Count; i++)
                Accounts[i].STT = i + 1;
        }

        private void AddClass()
        {
            string className = NewClassName.Trim();
            bool duplicate = Classes.Any(x => x.ClassName.Equals(className, StringComparison.OrdinalIgnoreCase));
            if (duplicate)
            {
                MessageBox.Show("Lớp đã tồn tại.");
                return;
            }

            StudentClass studentClass = new StudentClass { ClassName = className.ToUpper() };
            Classes.Add(studentClass);
            SelectedClass = studentClass;
            NewClassName = string.Empty;
            OnStudentInputChanged();
        }

        private void SaveStudent()
        {
            if (SelectedClass == null)
            {
                MessageBox.Show("Vui lòng chọn lớp.");
                return;
            }

            StudentId = StudentId.Trim();
            StudentName = StudentName.Trim();
            StudentAddress = StudentAddress.Trim();

            if (string.IsNullOrWhiteSpace(StudentId) || string.IsNullOrWhiteSpace(StudentName))
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên và họ tên.");
                return;
            }

            bool duplicate = Classes.SelectMany(c => c.Students)
                .Any(x => x.StudentId.Equals(StudentId, StringComparison.OrdinalIgnoreCase) && x != SelectedStudent);
            if (duplicate)
            {
                MessageBox.Show("Mã sinh viên đã tồn tại.");
                return;
            }

            if (SelectedStudent == null)
            {
                Student student = new Student
                {
                    StudentId = StudentId.ToUpper(),
                    FullName = StudentName,
                    Address = StudentAddress
                };
                SelectedClass.Students.Add(student);
                SelectedStudent = student;
            }
            else
            {
                SelectedStudent.StudentId = StudentId.ToUpper();
                SelectedStudent.FullName = StudentName;
                SelectedStudent.Address = StudentAddress;
                OnPropertyChanged(nameof(Classes));
            }
        }

        private void DeleteStudent()
        {
            if (SelectedStudent == null)
                return;

            StudentClass? ownerClass = Classes.FirstOrDefault(c => c.Students.Contains(SelectedStudent));
            ownerClass?.Students.Remove(SelectedStudent);
            SelectedStudent = null;
            StudentId = string.Empty;
            StudentName = string.Empty;
            StudentAddress = string.Empty;
            OnStudentInputChanged();
        }
    }
}
