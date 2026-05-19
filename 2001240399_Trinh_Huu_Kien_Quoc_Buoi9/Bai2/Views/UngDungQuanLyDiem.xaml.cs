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
namespace Bai2.Views
{
    /// <summary>
    /// Interaction logic for UngDungQuanLyDiem.xaml
    /// </summary>
    public partial class UngDungQuanLyDiem : Window
    {
        public UngDungQuanLyDiem()
        {
            InitializeComponent();
            
        }

        void MenuKhoa_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UC_DanhMuc();
        }

        void MenuThoat_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
