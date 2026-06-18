using System.Collections.ObjectModel;

namespace Bai3.Models
{
    public class StudentClass
    {
        public string ClassId { get; set; }
        public string ClassName { get; set; }
        public ObservableCollection<Student> Students { get; set; }

        public StudentClass()
        {
            ClassId = string.Empty;
            ClassName = string.Empty;
            Students = new ObservableCollection<Student>();
        }
    }
}
