    using MVVM_Bai2.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MVVM_Bai2.ViewModels
{
    public class StudentViewModel : BaseViewModel
    {
        // Danh sách sinh viên
        private List<Student> _students;
        public List<Student> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged("Students");
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

        public StudentViewModel()
        {
            Students = new List<Student>
            {
                new Student { Name = "An", Age = 20 },
                new Student { Name = "Bình", Age = 18 }
            };
        }

        public void AddStudent(string name, int age)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new Exception("Tên sinh viên không được để trống!");
                }
                if (age <= 0)
                {
                    throw new Exception("Tuổi sinh viên phải lớn hơn 0!");
                }

                List<Student> newList = new List<Student>(Students);
                newList.Add(new Student { Name = name, Age = age });
                Students = newList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sinh viên: " + ex.Message);
            }
        }

        public void DeleteStudent()
        {
            try
            {
                if (SelectedStudent == null)
                {
                    throw new Exception("Vui lòng chọn sinh viên để xóa!");
                }

                List<Student> newList = new List<Student>(Students);
                newList.Remove(SelectedStudent);
                Students = newList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sinh viên: " + ex.Message);
            }
        }
    }
}
