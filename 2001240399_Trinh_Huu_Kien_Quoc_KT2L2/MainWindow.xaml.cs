using System.Windows;

namespace _2001240399_Trinh_Huu_Kien_Quoc_KT2L2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenBai1_Click(object sender, RoutedEventArgs e)
        {
            new Views.Bai1().Show();
        }

        private void OpenBai2_Click(object sender, RoutedEventArgs e)
        {
            new Views.TimKiemView().Show();
        }

        private void OpenBai3_Click(object sender, RoutedEventArgs e)
        {
            new Views.Bai3().Show();
        }
    }
}
