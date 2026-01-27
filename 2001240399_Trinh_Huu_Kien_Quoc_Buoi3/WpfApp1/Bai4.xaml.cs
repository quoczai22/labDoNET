using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class Bai4 : Window
    {
        public Bai4()
        {
            InitializeComponent();
        }

        private void BtnHienThongTin_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Họ tên: {txtHoTen.Text}");
            sb.AppendLine($"Ngày sinh: {dtpNgaySinh.SelectedDate:dd/MM/yyyy}");
            sb.AppendLine($"Giới tính: {(radNam.IsChecked == true ? "Nam" : "Nữ")}");
            sb.AppendLine($"Quốc tịch: {(cboQuocTich.SelectedItem as ComboBoxItem)?.Content}");
            sb.AppendLine($"Nghề nghiệp: {txtNgheNghiep.Text}");

            sb.Append("Sở thích: ");
            if (chkDocSach.IsChecked == true) sb.Append("Đọc sách, ");
            if (chkDuLich.IsChecked == true) sb.Append("Du lịch, ");
            if (chkNgheNhac.IsChecked == true) sb.Append("Nghe nhạc, ");
            if (chkTheThao.IsChecked == true) sb.Append("Thể thao, ");
            if (chkGame.IsChecked == true) sb.Append("Chơi game");

            sb.AppendLine("\nKỹ năng:");
            foreach (var item in lstKyNang.SelectedItems)
            {
                sb.AppendLine("- " + (item as ListBoxItem)?.Content);
            }

            MessageBox.Show(sb.ToString(), "Thông tin cá nhân");
        }

        private void BtnThoat_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
