using MVVM_Bai2.ViewModels;
using System;
using System.Windows;
using System.Xml.Linq;

namespace MVVM_Bai2.Views
{
    public partial class StudentView : Window
    {
        StudentViewModel vm;

        public StudentView()
        {
            InitializeComponent();
            vm = new StudentViewModel();
            this.DataContext = vm;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = txtName.Text;
                int age = int.Parse(txtAge.Text);

                vm.AddStudent(name, age);

                txtName.Clear();
                txtAge.Clear();
            }
            catch
            {
                MessageBox.Show("Dữ liệu không hợp lệ!");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vm.DeleteStudent();
            }
            catch
            {
                MessageBox.Show("Không thể xóa!");
            }
        }
    }
}
