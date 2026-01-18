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
    /// Interaction logic for Btvn.xaml
    /// </summary>
    public partial class Btvn : Window
    {
        private string[,] people = new string[10, 3];
        private int count = 0;

        public Btvn()
        {
            InitializeComponent();
        }

        private void BtnHienThi_Click(object sender, RoutedEventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            string tuoi = txtTuoi.Text.Trim();
            string ghiChu = txtGhiChu.Text.Trim();

            if (string.IsNullOrWhiteSpace(hoTen) ||
                string.IsNullOrWhiteSpace(tuoi) ||
                string.IsNullOrWhiteSpace(ghiChu))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }

            if (!int.TryParse(tuoi, out int tuoiSo))
            {
                MessageBox.Show("Tuổi phải là số");
                return;
            }

            if (count >= people.GetLength(0))
            {
                MessageBox.Show("Danh sách đã đầy");
                return;
            }

            people[count, 0] = hoTen;
            people[count, 1] = tuoiSo.ToString();
            people[count, 2] = ghiChu;
            count++;

            lstDanhSach.Items.Clear();
            for (int i = 0; i < count; i++)
            {
                lstDanhSach.Items.Add(
                    $"{i + 1}.Ho va ten {people[i, 0]} Tuoi {people[i, 1]} Ghi chu {people[i, 2]}"
                );
            }

            txtHoTen.Clear();
            txtTuoi.Clear();
            txtGhiChu.Clear();
            txtHoTen.Focus();

            
        }

        private void BtnThongTin_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Thông tin cá nhân");
        }

        private void BtnCaiDat_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Cài đặt");
        }

        private void BtnGioiThieu_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Giới thiệu");
        }
    }
}

