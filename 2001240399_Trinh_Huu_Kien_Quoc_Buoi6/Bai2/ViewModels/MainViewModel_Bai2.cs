using Bai2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bai2.ViewModels
{
     public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<Department> Departments { get; set; }

        private Department _selectedDepartment;
        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                _selectedDepartment = value;
                OnPropertyChanged(nameof(SelectedDepartment));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _newDepartmentName;
        public string NewDepartmentName
        {
            get => _newDepartmentName;
            set
            {
                _newDepartmentName = value;
                OnPropertyChanged(nameof(NewDepartmentName));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _employeeId;
        public string EmployeeId
        {
            get => _employeeId;
            set
            {
                _employeeId = value;
                OnPropertyChanged(nameof(EmployeeId));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _employeeName;
        public string EmployeeName
        {
            get => _employeeName;
            set
            {
                _employeeName = value;
                OnPropertyChanged(nameof(EmployeeName));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _employeeAddress;
        public string EmployeeAddress
        {
            get => _employeeAddress;
            set
            {
                _employeeAddress = value;
                OnPropertyChanged(nameof(EmployeeAddress));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand AddEmployeeCommand { get; set; }
        public ICommand AddDepartmentCommand { get; set; }
        public ICommand RemoveDepartmentCommand { get; set; }

        public MainViewModel()
        {
            Departments = new ObservableCollection<Department>();

            LoadSampleData();

            AddEmployeeCommand = new RelayCommand(AddEmployeeExecute, CanAddEmployeeExecute);
            AddDepartmentCommand = new RelayCommand(AddDepartmentExecute, CanAddDepartmentExecute);
            RemoveDepartmentCommand = new RelayCommand(RemoveDepartmentExecute, CanRemoveDepartmentExecute);
        }

        private void LoadSampleData()
        {
            Departments.Add(new Department
            {
                Name = "Tổ chức cán bộ",
                Employees = new ObservableCollection<Employee>
        {
            new Employee { EmployeeId = "NV01", FullName = "Nguyễn Văn An", Address = "TP.HCM" },
            new Employee { EmployeeId = "NV02", FullName = "Trần Thị Bình", Address = "Đồng Nai" }
        }
            });

            Departments.Add(new Department
            {
                Name = "Tổ chức hành chính",
                Employees = new ObservableCollection<Employee>
        {
            new Employee { EmployeeId = "NV03", FullName = "Lê Minh Cường", Address = "Bình Dương" },
            new Employee { EmployeeId = "NV04", FullName = "Phạm Thu Dung", Address = "TP.HCM" }
        }
            });

            Departments.Add(new Department
            {
                Name = "Kế hoạch",
                Employees = new ObservableCollection<Employee>
        {
            new Employee { EmployeeId = "NV05", FullName = "Hoàng Quốc Đạt", Address = "Long An" },
            new Employee { EmployeeId = "NV06", FullName = "Võ Thanh Hà", Address = "TP.HCM" }
        }
            });

            Departments.Add(new Department
            {
                Name = "Kế toán",
                Employees = new ObservableCollection<Employee>
        {
            new Employee { EmployeeId = "NV07", FullName = "Đặng Thị Lan", Address = "Tây Ninh" },
            new Employee { EmployeeId = "NV08", FullName = "Bùi Anh Khoa", Address = "TP.HCM" }
        }
            });
        }

        private void AddEmployeeExecute(object parameter)
        {
            if (SelectedDepartment == null)
            {
                MessageBox.Show("Vui lòng chọn phòng ban trước khi thêm nhân viên!");
                return;
            }

            if (string.IsNullOrWhiteSpace(EmployeeId))
            {
                MessageBox.Show("Mã nhân viên không được rỗng.");
                return;
            }

            if (string.IsNullOrWhiteSpace(EmployeeName))
            {
                MessageBox.Show("Tên nhân viên không được rỗng.");
                return;
            }

            bool isDuplicate = SelectedDepartment.Employees.Any(e =>
                e.EmployeeId.Equals(EmployeeId.Trim(), StringComparison.OrdinalIgnoreCase));

            if (isDuplicate)
            {
                MessageBox.Show("Mã nhân viên đã tồn tại trong phòng ban!");
                return;
            }

            Employee emp = new Employee
            {
                EmployeeId = EmployeeId.Trim(),
                FullName = EmployeeName.Trim(),
                Address = EmployeeAddress
            };

            SelectedDepartment.Employees.Add(emp);

            ClearEmployeeInput();
            MessageBox.Show("Thêm nhân viên thành công.");
        }

        private bool CanAddEmployeeExecute(object parameter)
        {
            return SelectedDepartment != null
                   && !string.IsNullOrWhiteSpace(EmployeeId)
                   && !string.IsNullOrWhiteSpace(EmployeeName);
        }

        private void AddDepartmentExecute(object parameter)
        {
            if (string.IsNullOrWhiteSpace(NewDepartmentName))
            {
                MessageBox.Show("Tên phòng ban không được rỗng.");
                return;
            }

            bool isDuplicate = Departments.Any(d =>
                d.Name.Equals(NewDepartmentName.Trim(), StringComparison.OrdinalIgnoreCase));

            if (isDuplicate)
            {
                MessageBox.Show("Tên phòng ban đã tồn tại!");
                return;
            }

            Department dep = new Department
            {
                Name = NewDepartmentName.Trim()
            };

            Departments.Add(dep);
            NewDepartmentName = string.Empty;

            MessageBox.Show("Thêm phòng ban thành công.");
        }

        private bool CanAddDepartmentExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace(NewDepartmentName);
        }

        private void RemoveDepartmentExecute(object parameter)
        {
            if (SelectedDepartment == null)
            {
                MessageBox.Show("Vui lòng chọn phòng ban cần xóa.");
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Bạn có chắc muốn xóa phòng ban \"{SelectedDepartment.Name}\"?",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Departments.Remove(SelectedDepartment);
                SelectedDepartment = null;
                MessageBox.Show("Xóa phòng ban thành công.");
            }
        }

        private bool CanRemoveDepartmentExecute(object parameter)
        {
            return SelectedDepartment != null;
        }

        private void ClearEmployeeInput()
        {
            EmployeeId = string.Empty;
            EmployeeName = string.Empty;
            EmployeeAddress = string.Empty;
        }
    }
}
