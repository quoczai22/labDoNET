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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Bai3.xaml
    /// </summary>
    public partial class Bai3 : Window
    {
        public Bai3()
        {
            InitializeComponent();
            cboBan.ItemsSource = new List<string> { "Bàn 1", "Bàn 2", "Bàn 3", "Bàn 4" };
            cboMon.ItemsSource = new List<string> { "Phở", "Bún bò", "Mì", "Trà sữa", "Cơm tấm" };
        }
        private void ThemMon_Click(object sender, RoutedEventArgs e)
        {
            if(cboMon.SelectedItem==null || cboBan.SelectedItem==null)
            {
                MessageBox.Show("Vui lòng chọn món và bàn!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            lstOrderItems.Items.Add(cboMon.SelectedItem.ToString());

            txtThongTin.Text = $"Khach hang: {txtThongTinKhach.Text}\n";
            txtSDT.Text = $"So dien thoai: {txtSDTKhach.Text}\n";
            txtBan.Text = $"Ban: {cboBan.SelectedItem.ToString()}\n";
        }

        private void Xoa_Click(object sender, RoutedEventArgs e)
        {
            if (lstOrderItems.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn món để xóa!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                lstOrderItems.Items.Remove(lstOrderItems.SelectedItem);
            }
        }

        private void DatMon_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtThongTinKhach.Text)||string.IsNullOrWhiteSpace(txtSDTKhach.Text)||cboBan.SelectedItem==null||lstOrderItems.Items.Count==0)
            {
                MessageBox.Show("Vui lòng nhập tên món mới!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Đặt món thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
