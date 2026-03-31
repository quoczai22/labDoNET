using System.Collections.ObjectModel;

namespace Bai2.Models
{
    public class Department
    {
        public string Name { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }

        public Department()
        {
            Employees = new ObservableCollection<Employee>();
        }
    }
}
