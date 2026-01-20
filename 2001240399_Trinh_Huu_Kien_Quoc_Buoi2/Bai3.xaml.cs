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
    /// Interaction logic for Bai3.xaml
    /// </summary>
    public partial class Bai3 : Window
    {
        public Bai3()
        {
            InitializeComponent();
        }

        private void seat_click(object sender, RoutedEventArgs e)
        {
            Button bt= sender as Button;
            int gia =int.Parse(bt.Tag.ToString());
            if (bt.Background == Brushes.Gold)
            {
                MessageBox.Show("Ghe nay da duoc ban");
            }
            if (bt.Background == Brushes.LemonChiffon)
            {
                bt.Background = Brushes.DeepSkyBlue;
            }
            else
            {
                bt.Background = Brushes.LemonChiffon;
            }
        }

        private void chon(object sender, RoutedEventArgs e)
        {
            int tongtien = 0;
            foreach (Button btn in SeatGrid.Children)
            {
                if (btn.Background == Brushes.DeepSkyBlue)
                {
                    btn.Background = Brushes.Gold;
                    tongtien += int.Parse(btn.Tag.ToString());
                }
            }
            txt_tongtien.Text = tongtien.ToString();
        }

        private void huy(object sender, RoutedEventArgs e)
        { 
            foreach (Button btn in SeatGrid.Children)
            {
                btn.Background = Brushes.LemonChiffon;
                txt_tongtien.Clear();
            }
        }

        private void thoat(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
