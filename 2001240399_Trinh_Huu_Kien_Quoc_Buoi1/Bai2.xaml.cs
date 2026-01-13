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
using System.Globalization;
using System.Xml.Linq;

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi1
{
    /// <summary>
    /// Interaction logic for Bai2.xaml
    /// </summary>
    public partial class Bai2 : Window
    {
        public Bai2()
        {
            InitializeComponent();
        }

        private void btn_click( object sender, RoutedEventArgs e )
        {
            string fullName = txtName.Text.Trim(); // Lẩy chuỗi từ TextBox
            if (string.IsNullOrEmpty(fullName) || fullName == "Nhập tên bạn")
                txtGreeting.Text = "Vui Lòng nhập họ và tên!";
            else {
                // Viết hoa chữ đầu mỗi từ
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo; // Lấy văn hóa hiện tại
                string formattedName = textInfo.ToTitleCase(fullName.ToLower());
                txtGreeting.Text = $"Xin chao, {formattedName} ";
            }
        }
    }
}
