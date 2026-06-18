using BTVN.ViewModels;
using System.Windows;

namespace BTVN
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (DataContext is MainViewModel viewModel)
                viewModel.SelectStudentTreeItem(e.NewValue);
        }
    }
}
