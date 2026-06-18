using Bai3.ViewModels;
using System.Windows;

namespace Bai3.Views
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
                viewModel.SelectTreeItem(e.NewValue);
        }
    }
}
