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

namespace _2001240399_Trinh_Huu_Kien_Quoc_Bai3.Views
{
    /// <summary>
    /// Interaction logic for LopKhoaView.xaml
    /// </summary>
    public partial class LopKhoaView : Window
    {
        public LopKhoaView()
        {
            InitializeComponent();
            DataContext = new ViewModels.QuanLyLopKhoaViewModel();
        }
    }
}
