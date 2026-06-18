using System.Collections.ObjectModel;

namespace BTVN.Models
{
    public class StudentClass
    {
        public string ClassName { get; set; } = string.Empty;
        public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();
    }
}
