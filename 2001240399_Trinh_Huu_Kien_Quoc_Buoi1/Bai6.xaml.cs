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

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi1
{
    /// <summary>
    /// Interaction logic for Bai6.xaml
    /// </summary>
    public partial class Bai6 : Window
    {
        public Bai6()
        {
            InitializeComponent();
        }

        private void btnHienThi_Click(object sender, RoutedEventArgs e)
        {
            string ten= txtHoTen.Text.Trim();
            string tuoi = txtTuoi.Text.Trim();
            string ghichu = txtGhiChu.Text.Trim();

           txtKetQua.Text=$"Ho va ten{txtHoTen.Text}\n"+$"Tuoi {txtTuoi.Text}\n"+$"Ghi chu {txtGhiChu.Text}";

        }
    }
}
