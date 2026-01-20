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
    /// Interaction logic for Bai1.xaml
    /// </summary>
    public partial class Bai1 : Window
    {
        public Bai1()
        {
            InitializeComponent();
        }

        private void btn_hienthi_Click(object sender, RoutedEventArgs e)
        {
            int age;
            string s;
            s = "Ho ten la" + txthoten.Text + "\n";
            age = DateTime.Now.Year - Convert.ToInt32(txtns.Text);
            s = s + "Nam sinh: " + txtns.Text + "\n";
            MessageBox.Show(s);
            txthoten.Clear();
            txtns.Clear();
            txthoten.Focus();
        }

        private void txtns_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (!tb.Text.All(char.IsDigit) || string.IsNullOrWhiteSpace(tb.Text))
            {
                lbloi2.Text = $"{tb.Tag} Phai nhap so";
                tb.BorderBrush = Brushes.Red;
            }
            else
            {
                int a = int.Parse(tb.Text);
                if (a > DateTime.Now.Year)
                {
                    lbloi2.Text = $"{tb.Tag} Nam sinh khong hop le";
                    tb.BorderBrush = Brushes.Red;
                }
                else
                {
                    lbloi2.Text = "";
                    tb.BorderBrush = Brushes.Green;
                }
            }
        }

        private void txthoten_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                lbloi1.Text = $"{tb.Tag}Chua nhap ho ten";
                tb.BorderBrush = Brushes.Red;
            }
            else
            {
                lbloi1.Text = "";
                tb.BorderBrush = Brushes.Green;
            }
        }

        private void btn_thoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
