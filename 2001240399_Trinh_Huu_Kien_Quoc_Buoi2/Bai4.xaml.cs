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

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi2
{
    /// <summary>
    /// Interaction logic for Bai4.xaml
    /// </summary>
    public partial class Bai4 : Window
    {
        public Bai4()
        {
            InitializeComponent();
        }

        private void LoaiPT(object sender, RoutedEventArgs e)
        {
            if (bac1.IsChecked == true)
            {
                a.Visibility = Visibility.Visible;
                bien_a.Visibility = Visibility.Visible;
                b.Visibility = Visibility.Visible;
                bien_b.Visibility = Visibility.Visible;
                c.Visibility = Visibility.Hidden;
                bien_c.Visibility = Visibility.Hidden;
            }
            else
            {
                a.Visibility = Visibility.Visible;
                bien_a.Visibility = Visibility.Visible;
                b.Visibility = Visibility.Visible;
                bien_b.Visibility = Visibility.Visible;
                c.Visibility = Visibility.Visible;
                bien_c.Visibility = Visibility.Visible;
            }
        }

        private void GiaiPT(object sender, RoutedEventArgs e)
        {
            double a,b, c;
            if (!double.TryParse(bien_a.Text, out  a) ||
               !double.TryParse(bien_b.Text, out  b))
            {
                showkq.Text = ("Vui lòng nhập số hợp lệ!");
                return;
            }
            if (bac2.IsChecked == true)
            {
                if (!double.TryParse(bien_c.Text, out c))
                {
                    showkq.Text = ("Vui lòng nhập số hợp lệ!");
                    return;
                }
                if (a == 0)
                {
                    showkq.Text = ("Hệ số 'a' phải khác 0 trong phương trình bậc hai!");
                    return;
                }
                double delta = b * b - 4 * a * c;
                if (delta < 0)
                {
                    showkq.Text = ("Phương trình vô nghiệm!");
                }
                else if (delta == 0)
                {
                    double x = -b / (2 * a);
                    showkq.Text = ($"Phương trình có nghiệm kép x1 = x2 = {x}");
                }
                else
                {
                    double x1 = (-b + Math.Sqrt(delta)) / (2 * a);
                    double x2 = (-b - Math.Sqrt(delta)) / (2 * a);
                    showkq.Text = ($"Phương trình có hai nghiệm phân biệt x1 = {x1} \n x2 = {x2}");
                }
            }
            else
            {
                if (a == 0)
                {
                    if (b == 0)
                    {
                        showkq.Text = ("Phương trình vô số nghiệm!");
                    }
                    else
                    {
                        showkq.Text = ("Phương trình vô nghiệm!");
                    }
                }
                else
                {
                    double x = -b / a;
                    showkq.Text = ($"Phương trình có nghiệm x = {x}");
                }
            }
        }

        private void Thoat(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (bac1.IsChecked != true && bac2.IsChecked != true)
            {
                showkq.Text = ("Vui lòng chọn loại phương trình!");
            }
        }
    }
}
