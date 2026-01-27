using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1
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

        private bool ValidateInputs()
        {
            bool valid = true;
            Brush normalBrush = Brushes.White;
            Brush errorBrush = Brushes.LightPink;

            // Kiểm tra họ và tên
            if (string.IsNullOrEmpty(txtHoTen.Text))
            {
                txtHoTen.Background = errorBrush;
                valid = false;
            }
            else
            {
                txtHoTen.Background = normalBrush;
            }

            // Kiểm tra nghề nghiệp
            if (string.IsNullOrEmpty(txtNgheNghiep.Text))
            {
                txtNgheNghiep.Background = errorBrush;
                valid = false;
            }
            else
            {
                txtNgheNghiep.Background = normalBrush;
            }

            // Kiểm tra ngày sinh
            if (!dtpNgaySinh.SelectedDate.HasValue)
            {
                dtpNgaySinh.Background = errorBrush;
                valid = false;
            }
            else
            {
                dtpNgaySinh.Background = normalBrush;
            }

            // Kiểm tra quốc tịch
            if (cmbQuocTich.SelectedItem == null)
            {
                cmbQuocTich.Background = errorBrush;
                valid = false;
            }
            else
            {
                cmbQuocTich.Background = normalBrush;
            }

            if (!valid)
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);

            return valid;
        }

        private void BtnXemThongTin_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs()) return;

            // Lấy thông tin từ các control
            string hoTen = txtHoTen.Text.Trim();
            string ngheNghiep = txtNgheNghiep.Text.Trim();
            string gioiTinh = (radNam.IsChecked == true) ? "Nam" : (radNu.IsChecked == true) ? "Nữ" : "Chưa chọn";
            string ngaySinh = dtpNgaySinh.SelectedDate?.ToString("dd/MM/yyyy") ?? "Chưa chọn";
            string quocTich = (cmbQuocTich.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Chưa chọn";

            // Sở thích
            var soThich = new StringBuilder();
            if (chkTheThao.IsChecked == true)
                soThich.Append("Thể thao, ");
            if (chkNgheNhac.IsChecked == true)
                soThich.Append("Âm nhạc, ");
            if (chkDuLich.IsChecked == true)
                soThich.Append("Du lịch, ");
            if (chkDocSach.IsChecked == true)
                soThich.Append("Đọc sách, ");
            string soThichStr = soThich.Length > 0 ? soThich.ToString().TrimEnd(',', ' ') : "Không có sở thích";

            // Kỹ năng
            var kyNang = lstKyNang.SelectedItems.Cast<ListBoxItem>().Select(i => i.Content.ToString());
            string kyNangStr = kyNang.Any() ? string.Join(", ", kyNang) : "Chưa chọn";

            // Ghi chú
            string ghiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? "Không có" : txtGhiChu.Text.Trim();

            // Hiển thị thông tin lên UI
            lblHoTen.Text = hoTen;
            lblGioiTinh.Text = gioiTinh;
            lblQuocTich.Text = quocTich;
            lblNgheNghiep.Text = ngheNghiep;
            lblSoThich.Text = soThichStr;
            lblKyNang.Text = kyNangStr;
            lblGhiChu.Text = ghiChu;

            // Chuyển sang tab Xem thông tin
            tabControl.SelectedIndex = 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
