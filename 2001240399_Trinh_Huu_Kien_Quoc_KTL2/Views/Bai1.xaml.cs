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
using _2001240399_Trinh_Huu_Kien_Quoc_KTL2.ViewModels;

namespace _2001240399_Trinh_Huu_Kien_Quoc_KTL2.Views
{
    /// <summary>
    /// Interaction logic for Bai1.xaml
    /// </summary>
    public partial class Bai1 : Window
    {
        public Bai1()
        {
            InitializeComponent();
            DataContext = new ViewModels.Bai1ViewModel();
        }
    }
}
