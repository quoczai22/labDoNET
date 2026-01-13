using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Xml.Linq;

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi1
{
    /// <summary>
    /// Interaction logic for Bai3.xaml
    /// </summary>
    public partial class Bai3 : Window
    {
        public Bai3()
        {
            InitializeComponent();
        }


        private void btn_Gui(object sender, RoutedEventArgs e)
        {
            string fullName = txthovaten.Text.Trim(); 
            string ageText =txttuoi.Text.Trim();
            txtshowten.Text = $"Họ tên: {fullName} ";
            showage.Text = $"Tuổi: {ageText} ";
        }
    }
}
