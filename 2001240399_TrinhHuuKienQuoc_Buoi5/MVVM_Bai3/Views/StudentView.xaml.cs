using MVVM_Bai3.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MVVM_Bai3.Views
{
    /// <summary>
    /// Interaction logic for StudentView.xaml
    /// </summary>
    public partial class StudentView : Window
    {
        private StudentViewModel_Ob vm;
        public StudentView()
        {
            InitializeComponent();
            // Use the same ViewModel instance that XAML created as DataContext.
            vm = DataContext as StudentViewModel_Ob;
            if (vm == null)
            {
                // Fallback: create a new instance and assign it to DataContext
                vm = new StudentViewModel_Ob();
                DataContext = vm;
            }
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        { 
            vm.AddStudent();
        }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            vm.DeleteStudent();
        }
        private void BtnSort_Click(object sender, RoutedEventArgs e) 
        {
            vm.ToggleSortByAge(); 
        }
    }
}

