using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for Bai5.xaml
    /// </summary>
    public partial class Bai5 : Window
    {
        public Bai5()
        {
            InitializeComponent();
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e) {
            TextBox tb = sender as TextBox;
            if (string.IsNullOrWhiteSpace(tb.Text))
                MessageBox.Show($"{tb.Name} không được để trống!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void Button_Click(object sender, RoutedEventArgs e) {
            string info = $"Họ tên: {txtHoTen.Text}\nTuổi: {txtTuoi.Text} \nGhi chu: {txtGhiChu.Text} ";
        MessageBox. Show(info, "Thông tin đã nhập", MessageBoxButton. OK, MessageBoxImage. Information);
        }
    }
}
