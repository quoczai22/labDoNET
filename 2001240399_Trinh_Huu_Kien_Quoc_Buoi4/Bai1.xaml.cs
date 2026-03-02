using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Bai1 : Window
    {
        public Bai1()
        {
            InitializeComponent();
        }
        private void MenuNhanVien_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            MainContent.Children.Add(new UserControl_ucNhanVien());
        }

        private void MenuPhongBan_Click(object sender,
        RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            MainContent.Children.Add(new ucPhongBan());
        }
        private void BtnThoat_Click(object sender,
        RoutedEventArgs e)
        {
            Close();
        }
    }
}