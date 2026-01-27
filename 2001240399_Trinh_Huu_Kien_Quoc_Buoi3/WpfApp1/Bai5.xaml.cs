using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class Bai5 : Window
    {
        public Bai5()
        {
            InitializeComponent();
        }

        private void btn_them_giayto_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtgiayto.Text))
            {
                lstgiayto.Items.Add(txtgiayto.Text);
                txtgiayto.Clear();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập giấy tờ.");
            }
        }

        private void btn_them(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs()) return;

            string gioiTinh = rbnnam.IsChecked == true ? "Nam" : "Nữ";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Mã nhân viên: " + txtma.Text);
            sb.AppendLine("Họ tên: " + txtten.Text);
            sb.AppendLine("Giới tính: " + gioiTinh);
            sb.AppendLine("Ngày sinh: " + dtpns.Text);
            sb.AppendLine("Phòng ban: " + cbpb.Text);
            sb.AppendLine("Ghi chú: " + txtnote.Text);
            sb.AppendLine("Giấy tờ nộp kèm:");

            foreach (var item in lstgiayto.Items)
            {
                if (item is ListBoxItem lbi)
                    sb.AppendLine("- " + lbi.Content.ToString());
                else
                    sb.AppendLine("- " + item.ToString());
            }


            MessageBox.Show(sb.ToString(), "Thông tin nhân viên");
        }

        private void btn_xoa(object sender, RoutedEventArgs e)
        {
            txtma.Clear();
            txtten.Clear();
            txtnote.Clear();
            txtgiayto.Clear();

            rbnnam.IsChecked = false;
            rbnnu.IsChecked = false;

            dtpns.SelectedDate = null;
            cbpb.SelectedIndex = 0;

            lstgiayto.Items.Clear();
        }

        private void btn_thoat(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtma.Text))
            {
                MessageBox.Show("Mã không được để trống.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtten.Text))
            {
                MessageBox.Show("Họ tên không được để trống.");
                return false;
            }

            if (rbnnam.IsChecked == false && rbnnu.IsChecked == false)
            {
                MessageBox.Show("Vui lòng chọn giới tính.");
                return false;
            }

            if (dtpns.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày sinh.");
                return false;
            }

            if (cbpb.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn phòng ban.");
                return false;
            }

            if (lstgiayto.Items.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một giấy tờ.");
                return false;
            }

            return true;
        }
    }
}
