using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MVVM_Bai5.Models;

namespace MVVM_Bai5.ViewModels
{
    public class StudentViewModel : BaseViewModel
    {
        public int StudentCountMale => Students.Count(s=>s.Gender=="Nam");
        public int StudentCountFemale => Students.Count(s=> s.Gender == "Nữ");

        private ObservableCollection<Student> _students;
        
        private Stack<ObservableCollection<Student>>_undoStack = new Stack<ObservableCollection<Student>>();
        
        private Stack<ObservableCollection<Student>> _redoStack = new Stack<ObservableCollection<Student>>();
        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }
        private ICollectionView _studentsView;
        public ICollectionView StudentsView
        {
            get { return _studentsView; }
            set
            {
                _studentsView = value;
                OnPropertyChanged(nameof(StudentsView));
            }
        }

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
            }
        }

        public StudentViewModel()
        {
            Students = new ObservableCollection<Student>
            {
                new Student { Name = "Quốc", Age = 19 , Gender = "Nam", City = "Hồ Chí Minh"},
                new Student { Name = "Vy", Age = 20, Gender = "Nữ", City = "Hồ Chí Minh"},
            };
            StudentsView = CollectionViewSource.GetDefaultView(Students);
            StudentsView.Filter = StudentFilter;
        }

        private string _newname;
        public string NewName
        {
            get { return _newname; }
            set
            {
                _newname = value;
                OnPropertyChanged(nameof(NewName));
            }
        }

        private int _newage;
        public int NewAge
        {
            get { return _newage; }
            set
            {
                _newage = value;
                OnPropertyChanged(nameof(NewAge));
            }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                StudentsView.Refresh();
            }
        }

        private string _selectedCity;
        public string SelectedCity
        {
            get => _selectedCity;
            set
            {
                _selectedCity = value;
                OnPropertyChanged(nameof(SelectedCity));
            }
        }

        private bool _isMale;
        public bool IsMale
        {
            get => _isMale;
            set
            {
                _isMale = value;
                OnPropertyChanged(nameof(IsMale));
            }
        }

        private bool _isFemale;
        public bool IsFemale
        {
            get => _isFemale;
            set
            {
                _isFemale = value;
                OnPropertyChanged(nameof(IsFemale));
            }
        }

        public void AddStudent()
        {
            if (string.IsNullOrWhiteSpace(NewName) || NewAge <= 0)
            {
                MessageBox.Show("Dữ liệu không hợp lệ!");
                return;
            }

            string gender = IsMale ? "Nam" : "Nữ";

            _undoStack.Push(new ObservableCollection<Student>(Students));

            Students.Add(new Student { Name = NewName, Age = NewAge, Gender = gender, City = SelectedCity });

            NewName = string.Empty;
            NewAge = 0;
            IsMale = false;
            IsFemale = false;
            SelectedCity = null;

            OnPropertyChanged(nameof(NewName));
            OnPropertyChanged(nameof(NewAge));
            OnPropertyChanged(nameof(IsMale));
            OnPropertyChanged(nameof(IsFemale));
            OnPropertyChanged(nameof(SelectedCity));

            OnPropertyChanged(nameof(StudentCountMale));
            OnPropertyChanged(nameof(StudentCountFemale));
        }

        public void RemoveStudent()
        {
            if (SelectedStudent != null)
            {
                Students.Remove(SelectedStudent);
                SelectedStudent = null;
            }
            OnPropertyChanged(nameof(StudentCountMale));
            OnPropertyChanged(nameof(StudentCountFemale));
        }
        public void EditStudent()
        {
            if (SelectedStudent != null)
            {
                if (string.IsNullOrWhiteSpace(NewName) || NewAge <= 0 || string.IsNullOrWhiteSpace(SelectedCity))
                {
                    MessageBox.Show("Dữ liệu không hợp lệ!");
                    return;
                }

                // Cập nhật thông tin từ ô nhập vào đối tượng SelectedStudent
                SelectedStudent.Name = NewName;
                SelectedStudent.Age = NewAge;
                SelectedStudent.City = SelectedCity;
                SelectedStudent.Gender = IsMale ? "Nam" : "Nữ";

                // Thông báo cho UI cập nhật lại
                OnPropertyChanged(nameof(SelectedStudent));
                OnPropertyChanged(nameof(StudentCountMale));
                OnPropertyChanged(nameof(StudentCountFemale));
                OnPropertyChanged(nameof(NewName));
                OnPropertyChanged(nameof(NewAge));
                OnPropertyChanged(nameof(SelectedCity));
                OnPropertyChanged(nameof(IsMale));
                OnPropertyChanged(nameof(IsFemale));
            }
        }


        public void Undo()
        {
            if (_undoStack.Count > 0)
            {
                _redoStack.Push(new ObservableCollection<Student>(Students));
                Students = _undoStack.Pop();
                OnPropertyChanged(nameof(Students));
                OnPropertyChanged(nameof(StudentCountMale));
                OnPropertyChanged(nameof(StudentCountFemale));
                OnPropertyChanged(nameof(NewName));
                OnPropertyChanged(nameof(NewAge));
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                _undoStack.Push(new ObservableCollection<Student>(Students));
                Students = _redoStack.Pop();
                OnPropertyChanged(nameof(Students));
                OnPropertyChanged(nameof(StudentCountMale));
                OnPropertyChanged(nameof(StudentCountFemale));
                OnPropertyChanged(nameof(NewName));
                OnPropertyChanged(nameof(NewAge));
            }
        }

        private bool StudentFilter(object obj)
        {
            if (string.IsNullOrEmpty(SearchText))
                return true;
            var student = obj as Student;
            if (student == null)
            {
                return false;
            }
            return student.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
        }

    }
}
