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

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi8.Views
{
    /// <summary>
    /// Interaction logic for QuanLySinhVienView.xaml
    /// </summary>
    public partial class QuanLySinhVienView : Window
    {
        public QuanLySinhVienView()
        {
            InitializeComponent();
            var vm = new ViewModels.MainViewModel();
            var ucLop = new UC_QuanLyLop { DataContext = vm };
            var ucSV = new UC_DanhSachSinhVien { DataContext = vm };
            TabQuanLyLop.Content = new UC_QuanLyLop();
            TabDanhSachSV.Content = new UC_DanhSachSinhVien();
        }
    }
}
