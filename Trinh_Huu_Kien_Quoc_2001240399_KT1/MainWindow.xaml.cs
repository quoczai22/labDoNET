using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Trinh_Huu_Kien_Quoc_2001240399_KT1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        // Xử lý sự kiện cho nút "Xem thông tin"
        private void btnXemThongTin_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder info = new StringBuilder();

            info.AppendLine($"Họ và tên: {txtHoTen.Text}");
            info.AppendLine($"Nghề nghiệp: {txtNgheNghiep.Text}");

            string gioiTinh = (radNam.IsChecked == true) ? "Nam" : "Nữ";
            info.AppendLine($"Giới tính: {gioiTinh}");

            string ngaySinh = dpNgaySinh.SelectedDate.HasValue
                ? dpNgaySinh.SelectedDate.Value.ToString("dd/MM/yyyy")
                : "Chưa chọn";
            info.AppendLine($"Ngày sinh: {ngaySinh}");

            string quocTich = "";
            if (cboQuocTich.SelectedItem is ComboBoxItem selectedItem)
            {
                quocTich = selectedItem.Content.ToString();
            }
            info.AppendLine($"Quốc tịch: {quocTich}");

            List<string> soThichList = new List<string>();
            if (chkDocSach.IsChecked == true) soThichList.Add("Đọc sách");
            if (chkNgheNhac.IsChecked == true) soThichList.Add("Nghe nhạc");
            if (chkTheThao.IsChecked == true) soThichList.Add("Thể thao");
            if (chkDuLich.IsChecked == true) soThichList.Add("Du lịch");

            string soThich = soThichList.Count > 0 ? string.Join(", ", soThichList) : "Không có";
            info.AppendLine($"Sở thích: {soThich}");

            List<string> kyNangList = new List<string>();
            foreach (ListBoxItem item in lstKyNang.SelectedItems)
            {
                kyNangList.Add(item.Content.ToString());
            }

            string kyNang = kyNangList.Count > 0 ? string.Join(", ", kyNangList) : "Không có";
            info.AppendLine($"Kỹ năng: {kyNang}");

            MessageBox.Show(info.ToString(), "Thông tin đã nhập", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Anh có chắc chắn muốn thoát ứng dụng không?",
                                                      "Xác nhận thoát",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}