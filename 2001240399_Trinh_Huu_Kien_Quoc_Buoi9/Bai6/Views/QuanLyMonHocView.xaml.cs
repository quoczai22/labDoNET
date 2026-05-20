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

namespace Bai6.Views
{
    /// <summary>
    /// Interaction logic for QuanLyMonHocView.xaml
    /// </summary>
    public partial class QuanLyMonHocView : Window
    {
        public QuanLyMonHocView()
        {
            InitializeComponent();
            DataContext = new ViewModels.QuanLyMonHocViewModel();
        }
    }
}
