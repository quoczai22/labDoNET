using MVVM_Bai5.ViewModels;
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
using MVVM_Bai5.Models;

namespace MVVM_Bai5.Views
{
    /// <summary>
    /// Interaction logic for StudentView.xaml
    /// </summary>
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
            vm.AddStudent();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            vm.RemoveStudent();
        }

        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {
            vm.Undo();
        }

        private void BtnRedo_Click(object sender, RoutedEventArgs e)
        {
            vm.Redo();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StudentViewModel viewModel = this.DataContext as StudentViewModel;
            if (viewModel != null)
            {
                DataModel.Save("data.json", viewModel.Students);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            vm.EditStudent();
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            StudentViewModel viewModel = this.DataContext as StudentViewModel;
            if (viewModel != null)
            {
                DataModel.Load("data.json", viewModel.Students);
            }
        }
    }
}
