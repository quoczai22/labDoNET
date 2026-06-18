using Bai3.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Bai3.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<Faculty> Faculties { get; set; }

        private Faculty? _selectedFaculty;
        public Faculty? SelectedFaculty
        {
            get { return _selectedFaculty; }
            set
            {
                _selectedFaculty = value;
                OnPropertyChanged(nameof(SelectedFaculty));
                OnPropertyChanged(nameof(ClassesOfSelectedFaculty));
                if (SelectedFaculty != null && SelectedClass != null && !SelectedFaculty.Classes.Contains(SelectedClass))
                    SelectedClass = null;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private StudentClass? _selectedClass;
        public StudentClass? SelectedClass
        {
            get { return _selectedClass; }
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
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
                LoadSelectedStudentInfo();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ObservableCollection<StudentClass> ClassesOfSelectedFaculty
        {
            get
            {
                if (SelectedFaculty == null)
                    return new ObservableCollection<StudentClass>();
                return SelectedFaculty.Classes;
            }
        }

        private string _newFacultyName;
        public string NewFacultyName
        {
            get { return _newFacultyName; }
            set
            {
                _newFacultyName = value;
                OnPropertyChanged(nameof(NewFacultyName));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _newClassName;
        public string NewClassName
        {
            get { return _newClassName; }
            set
            {
                _newClassName = value;
                OnPropertyChanged(nameof(NewClassName));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _studentId;
        public string StudentId
        {
            get { return _studentId; }
            set
            {
                _studentId = value;
                OnPropertyChanged(nameof(StudentId));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _studentName;
        public string StudentName
        {
            get { return _studentName; }
            set
            {
                _studentName = value;
                OnPropertyChanged(nameof(StudentName));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _studentAddress;
        public string StudentAddress
        {
            get { return _studentAddress; }
            set
            {
                _studentAddress = value;
                OnPropertyChanged(nameof(StudentAddress));
            }
        }

        private bool _isMale;
        public bool IsMale
        {
            get { return _isMale; }
            set
            {
                _isMale = value;
                OnPropertyChanged(nameof(IsMale));
                OnPropertyChanged(nameof(IsFemale));
            }
        }

        public bool IsFemale
        {
            get { return !IsMale; }
            set
            {
                IsMale = !value;
                OnPropertyChanged(nameof(IsFemale));
            }
        }

        private bool _isStudying;
        public bool IsStudying
        {
            get { return _isStudying; }
            set
            {
                _isStudying = value;
                OnPropertyChanged(nameof(IsStudying));
            }
        }

        public ICommand AddFacultyCommand { get; set; }
        public ICommand AddClassCommand { get; set; }
        public ICommand AddStudentCommand { get; set; }
        public ICommand DeleteStudentCommand { get; set; }
        public ICommand ClearInputCommand { get; set; }

        public MainViewModel()
        {
            Faculties = new ObservableCollection<Faculty>();
            _newFacultyName = string.Empty;
            _newClassName = string.Empty;
            _studentId = string.Empty;
            _studentName = string.Empty;
            _studentAddress = string.Empty;
            _isMale = true;
            _isStudying = true;

            LoadSampleData();

            AddFacultyCommand = new RelayCommand(AddFaculty, CanAddFaculty);
            AddClassCommand = new RelayCommand(AddClass, CanAddClass);
            AddStudentCommand = new RelayCommand(AddStudent, CanAddStudent);
            DeleteStudentCommand = new RelayCommand(DeleteStudent, CanDeleteStudent);
            ClearInputCommand = new RelayCommand(ClearInput);
        }

        private void LoadSampleData()
        {
            Faculty congNgheThongTin = new Faculty
            {
                FacultyId = "CNTT",
                FacultyName = "Công nghệ thông tin"
            };
            congNgheThongTin.Classes.Add(new StudentClass
            {
                ClassId = "20DTHA1",
                ClassName = "20DTHA1",
                Students =
                {
                    new Student { StudentId = "SV001", FullName = "Nguyễn Văn An", Address = "TP.HCM", IsMale = true, IsStudying = true },
                    new Student { StudentId = "SV002", FullName = "Trần Thị Bình", Address = "Bình Dương", IsMale = false, IsStudying = true }
                }
            });
            congNgheThongTin.Classes.Add(new StudentClass
            {
                ClassId = "21DTHB1",
                ClassName = "21DTHB1",
                Students =
                {
                    new Student { StudentId = "SV003", FullName = "Lê Văn Cường", Address = "Đồng Nai", IsMale = true, IsStudying = true }
                }
            });

            Faculty kinhTe = new Faculty
            {
                FacultyId = "KT",
                FacultyName = "Kinh tế"
            };
            kinhTe.Classes.Add(new StudentClass
            {
                ClassId = "20DKTA1",
                ClassName = "20DKTA1",
                Students =
                {
                    new Student { StudentId = "SV004", FullName = "Phạm Thị Dung", Address = "Long An", IsMale = false, IsStudying = false }
                }
            });

            Faculties.Add(congNgheThongTin);
            Faculties.Add(kinhTe);

            SelectedFaculty = Faculties.FirstOrDefault();
            SelectedClass = SelectedFaculty?.Classes.FirstOrDefault();
        }

        private void AddFaculty(object parameter)
        {
            string facultyName = NewFacultyName.Trim();
            if (string.IsNullOrWhiteSpace(facultyName))
            {
                MessageBox.Show("Tên khoa không được rỗng.");
                return;
            }

            bool isDuplicate = Faculties.Any(x => x.FacultyName.Equals(facultyName, StringComparison.OrdinalIgnoreCase));
            if (isDuplicate)
            {
                MessageBox.Show("Khoa này đã tồn tại.");
                return;
            }

            Faculty faculty = new Faculty
            {
                FacultyId = GenerateFacultyId(facultyName),
                FacultyName = facultyName
            };

            Faculties.Add(faculty);
            SelectedFaculty = faculty;
            NewFacultyName = string.Empty;
            MessageBox.Show("Thêm khoa thành công.");
        }

        private bool CanAddFaculty(object parameter)
        {
            return !string.IsNullOrWhiteSpace(NewFacultyName);
        }

        private void AddClass(object parameter)
        {
            if (SelectedFaculty == null)
            {
                MessageBox.Show("Vui lòng chọn khoa trước khi thêm lớp.");
                return;
            }

            string className = NewClassName.Trim();
            if (string.IsNullOrWhiteSpace(className))
            {
                MessageBox.Show("Tên lớp không được rỗng.");
                return;
            }

            bool isDuplicate = SelectedFaculty.Classes.Any(x => x.ClassName.Equals(className, StringComparison.OrdinalIgnoreCase));
            if (isDuplicate)
            {
                MessageBox.Show("Lớp này đã tồn tại trong khoa.");
                return;
            }

            StudentClass studentClass = new StudentClass
            {
                ClassId = className.ToUpper(),
                ClassName = className
            };

            SelectedFaculty.Classes.Add(studentClass);
            SelectedClass = studentClass;
            NewClassName = string.Empty;
            MessageBox.Show("Thêm lớp thành công.");
        }

        private bool CanAddClass(object parameter)
        {
            return SelectedFaculty != null && !string.IsNullOrWhiteSpace(NewClassName);
        }

        private void AddStudent(object parameter)
        {
            if (SelectedClass == null)
            {
                MessageBox.Show("Vui lòng chọn lớp trước khi thêm sinh viên.");
                return;
            }

            string studentId = StudentId.Trim();
            string studentName = StudentName.Trim();

            if (string.IsNullOrWhiteSpace(studentId))
            {
                MessageBox.Show("Mã sinh viên không được rỗng.");
                return;
            }

            if (string.IsNullOrWhiteSpace(studentName))
            {
                MessageBox.Show("Họ tên sinh viên không được rỗng.");
                return;
            }

            bool isDuplicate = Faculties
                .SelectMany(f => f.Classes)
                .SelectMany(c => c.Students)
                .Any(s => s.StudentId.Equals(studentId, StringComparison.OrdinalIgnoreCase));

            if (isDuplicate)
            {
                MessageBox.Show("Mã sinh viên đã tồn tại.");
                return;
            }

            Student student = new Student
            {
                StudentId = studentId.ToUpper(),
                FullName = studentName,
                Address = StudentAddress.Trim(),
                IsMale = IsMale,
                IsStudying = IsStudying
            };

            SelectedClass.Students.Add(student);
            SelectedStudent = student;
            ClearStudentInput();
            MessageBox.Show("Thêm sinh viên thành công.");
        }

        private bool CanAddStudent(object parameter)
        {
            return SelectedClass != null
                   && !string.IsNullOrWhiteSpace(StudentId)
                   && !string.IsNullOrWhiteSpace(StudentName);
        }

        private void DeleteStudent(object parameter)
        {
            if (SelectedStudent == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa trên TreeView.");
                return;
            }

            StudentClass? ownerClass = Faculties
                .SelectMany(f => f.Classes)
                .FirstOrDefault(c => c.Students.Contains(SelectedStudent));

            if (ownerClass == null)
            {
                MessageBox.Show("Không tìm thấy lớp của sinh viên đang chọn.");
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Bạn có chắc muốn xóa sinh viên \"{SelectedStudent.FullName}\"?",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ownerClass.Students.Remove(SelectedStudent);
                SelectedStudent = null;
                ClearStudentInput();
                MessageBox.Show("Xóa sinh viên thành công.");
            }
        }

        private bool CanDeleteStudent(object parameter)
        {
            return SelectedStudent != null;
        }

        public void SelectTreeItem(object? item)
        {
            if (item is Faculty faculty)
            {
                SelectedFaculty = faculty;
                SelectedClass = null;
                SelectedStudent = null;
            }
            else if (item is StudentClass studentClass)
            {
                SelectedClass = studentClass;
                SelectedFaculty = Faculties.FirstOrDefault(f => f.Classes.Contains(studentClass));
                SelectedStudent = null;
            }
            else if (item is Student student)
            {
                SelectedStudent = student;
                SelectedClass = Faculties.SelectMany(f => f.Classes).FirstOrDefault(c => c.Students.Contains(student));
                SelectedFaculty = Faculties.FirstOrDefault(f => SelectedClass != null && f.Classes.Contains(SelectedClass));
            }
        }

        private void LoadSelectedStudentInfo()
        {
            if (SelectedStudent == null)
                return;

            StudentId = SelectedStudent.StudentId;
            StudentName = SelectedStudent.FullName;
            StudentAddress = SelectedStudent.Address;
            IsMale = SelectedStudent.IsMale;
            IsStudying = SelectedStudent.IsStudying;
        }

        private void ClearInput(object parameter)
        {
            ClearStudentInput();
            SelectedStudent = null;
        }

        private void ClearStudentInput()
        {
            StudentId = string.Empty;
            StudentName = string.Empty;
            StudentAddress = string.Empty;
            IsMale = true;
            IsStudying = true;
        }

        private string GenerateFacultyId(string facultyName)
        {
            string normalized = new string(facultyName
                .Where(char.IsLetterOrDigit)
                .Take(4)
                .ToArray())
                .ToUpper();

            if (string.IsNullOrWhiteSpace(normalized))
                normalized = "KHOA";

            string id = normalized;
            int index = 1;
            while (Faculties.Any(f => f.FacultyId.Equals(id, StringComparison.OrdinalIgnoreCase)))
            {
                id = normalized + index;
                index++;
            }

            return id;
        }
    }
}
