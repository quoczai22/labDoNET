using MVVM_Bai4.Models;
using System.Windows;
using MVVM_Bai4.ModelViews;

namespace MVVM_Bai4.Views
{
    public partial class StudentView : Window
    {
        private StudentViewModel vm;

        public StudentView()
        {
            InitializeComponent();
            vm = DataContext as StudentViewModel;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            vm?.AddStudent();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            vm?.RemoveStudent();
        }
    }
}
