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

namespace Trinh_Huu_Kien_Quoc_2001240399_KT1_Test
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

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder info = new StringBuilder();

            info.AppendLine($"Mã sinh viên: {txtMaSV.Text}");
            info.AppendLine($"Họ tên: {txtHoTen.Text}");

            string gioiTinh = (radNam.IsChecked == true) ? "Nam" : "Nữ";
            info.AppendLine($"Giới tính: {gioiTinh}");

            List<string> soThichList = new List<string>();
            if (chkTheThao.IsChecked == true) soThichList.Add("Thể thao");
            if (chkAmNhac.IsChecked == true) soThichList.Add("Âm nhạc");
            if (chkDuLich.IsChecked == true) soThichList.Add("Du lịch");

            string soThich = soThichList.Count > 0 ? string.Join(", ", soThichList) : "Không có";
            info.AppendLine($"Sở thích: {soThich}");

            string lop = "Chưa chọn";
            if (cboLop.SelectedItem is ComboBoxItem selectedItem)
            {
                lop = selectedItem.Content.ToString();
            }
            info.AppendLine($"Lớp: {lop}");

            List<string> monHocList = new List<string>();
            foreach (ListBoxItem item in lstMonHoc.SelectedItems)
            {
                monHocList.Add(item.Content.ToString());
            }

            string monHoc = monHocList.Count > 0 ? string.Join(", ", monHocList) : "Không có";
            info.AppendLine($"Môn học: {monHoc}");

            MessageBox.Show(info.ToString(), "Thông tin sinh viên", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Anh có chắc chắn muốn đóng cửa sổ không?",
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