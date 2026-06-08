using BT2.Models;
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

namespace BT2.Views
{
    /// <summary>
    /// Interaction logic for DangNhapView.xaml
    /// </summary>
    public partial class DangNhapView : Window
    {
        public DangNhapView()
        {
            InitializeComponent();
            DataContext = new ViewModels.Class1(null);
        }
        public DangNhapView(NHANVIEN nhanvien)
        {
            InitializeComponent();
            DataContext = new ViewModels.Class1(nhanvien);
        }
    }
}
