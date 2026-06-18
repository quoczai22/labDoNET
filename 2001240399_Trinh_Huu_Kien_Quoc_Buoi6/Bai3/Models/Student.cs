namespace Bai3.Models
{
    public class Student
    {
        public string StudentId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public bool IsMale { get; set; }
        public bool IsStudying { get; set; }

        public string GenderName
        {
            get { return IsMale ? "Nam" : "Nữ"; }
        }

        public string StatusName
        {
            get { return IsStudying ? "Đang học" : "Nghỉ học"; }
        }

        public Student()
        {
            StudentId = string.Empty;
            FullName = string.Empty;
            Address = string.Empty;
            IsStudying = true;
        }
    }
}
