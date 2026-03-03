using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using MVVM_Bai3.Models;
using Student = MVVM_Bai3.Models.StudentModel.Student;

namespace MVVM_Bai3.ViewModels
{
    public class StudentViewModel_Ob : BaseViewModel
    {
        // Danh sách sinh viên
        private ObservableCollection<Student> _students;
        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged("Students");
            }
        }

        private ICollectionView _studentsView;
        public ICollectionView StudentsView
        {
            get { return _studentsView; }
            set
            {
                _studentsView = value;
                OnPropertyChanged("StudentsView");
            }
        }

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged("SelectedStudent");
            }
        }

        private string _newName;
        public string NewName
        {
            get { return _newName; }
            set
            {
                _newName = value;
                OnPropertyChanged("NewName");
            }
        }

        private int _newAge;
        public int NewAge
        {
            get { return _newAge; }
            set
            {
                _newAge = value;
                OnPropertyChanged("NewAge");
            }
        }

        private string _filterText;
        public string FilterText
        {
            get { return _filterText; }
            set
            {
                _filterText = value;
                OnPropertyChanged("FilterText");
                StudentsView.Refresh(); // Cập nhật filter
            }
        }

        public StudentViewModel_Ob()
        {
            Students = new ObservableCollection<Student>
            {
                new Student { Name = "An", Age = 20 },
                new Student { Name = "Bình", Age = 18 },
                new Student { Name = "Chi", Age = 19 },
                new Student { Name = "Châu", Age = 26 }
            };

            StudentsView = CollectionViewSource.GetDefaultView(Students);

            StudentsView.SortDescriptions.Add(new SortDescription("Age", ListSortDirection.Ascending));

            StudentsView.Filter = FilterStudents;
        }

        private bool _isAscending = true;

        public void ToggleSortByAge()
        {
            if (StudentsView == null) return;

            StudentsView.SortDescriptions.Clear();

            if (_isAscending)
                StudentsView.SortDescriptions.Add(new SortDescription("Age", ListSortDirection.Descending));
            else
                StudentsView.SortDescriptions.Add(new SortDescription("Age", ListSortDirection.Ascending));

            _isAscending = !_isAscending;
        }

        private bool FilterStudents(object obj)
        {
            if (string.IsNullOrWhiteSpace(FilterText))
                return true;

            Student student = obj as Student;
            if (student == null) return false;

            return student.Name.IndexOf(FilterText, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public void AddStudent()
        {
            if (string.IsNullOrWhiteSpace(NewName) || NewAge <= 0)
            {
                MessageBox.Show("Dữ liệu không hợp lệ!");
                return;
            }

            Students.Add(new Student { Name = NewName, Age = NewAge });

            NewName = string.Empty;
            NewAge = 0;
        }

        public void DeleteStudent()
        {
            if (SelectedStudent == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên để xóa!");
                return;
            }

            Students.Remove(SelectedStudent);
        }
    }
}
