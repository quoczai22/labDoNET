using System.Collections.ObjectModel;

namespace Bai3.Models
{
    public class Faculty
    {
        public string FacultyId { get; set; }
        public string FacultyName { get; set; }
        public ObservableCollection<StudentClass> Classes { get; set; }

        public Faculty()
        {
            FacultyId = string.Empty;
            FacultyName = string.Empty;
            Classes = new ObservableCollection<StudentClass>();
        }
    }
}
