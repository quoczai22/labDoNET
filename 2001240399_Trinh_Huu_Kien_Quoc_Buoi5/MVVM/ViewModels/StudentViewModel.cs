using System.Collections.ObjectModel;
using System.ComponentModel;
using MVVM.Models;

namespace MVVM.ViewModels
{
    public class StudentViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Student> Students { get; }

        public StudentViewModel()
        {
            Students = new ObservableCollection<Student>
            {
                new Student { Name = "Nguyễn Văn A", Age = 20 },
                new Student { Name = "Trần Thị B", Age = 22 },
                new Student { Name = "Lê Văn C", Age = 19 }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}