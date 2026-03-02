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

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi4
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
        private void MenuSinhVien_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            MainContent.Children.Add(new ucSinhVien());
        }
        private void MenuLopHoc_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            MainContent.Children.Add(new ucLopHoc());
        }
        private void BtnThoat_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void BtnThem_Click(object sender, RoutedEventArgs e)
        { 
            MessageBox.Show("Thêm dữ liệu mới");
        }
        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sửa dữ liệu");
        }
        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Xóa dữ liệu");
        }
    }
}
