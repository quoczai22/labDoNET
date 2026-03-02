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

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi4
{
    public partial class Bai4 : Window
    {
        public Bai4()
        {
            InitializeComponent();
            AddPhongBan("Giám đốc");
            AddPhongBan("Kế toán");
            AddPhongBan("Kế hoạch");
        }

        void AddPhongBan(string ten)
        {
            TreeViewItem pb = new TreeViewItem { Header = ten };
            treePhongBan.Items.Add(pb);
        }

        private void BtnThemPhongBan_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTenPhong.Text))
                AddPhongBan(txtTenPhong.Text);
        }

        private void BtnXoaPhongBan_Click(object sender, RoutedEventArgs e)
        {
            if (treePhongBan.SelectedItem is TreeViewItem item && item.Parent is TreeView)
            {
                if (item.Items.Count == 0)
                    treePhongBan.Items.Remove(item);
                else
                    MessageBox.Show("Không thể xóa phòng ban còn nhân viên");
            }
        }

        private void BtnThemNhanVien_Click(object sender, RoutedEventArgs e)
        {
            if (treePhongBan.SelectedItem is TreeViewItem pb && pb.Parent is TreeView)
            {
                TreeViewItem nv = new TreeViewItem
                {
                    Header = $"{txtHoTen.Text}-{txtMaNV.Text}-{txtDiaChi.Text}-{txtSDT.Text}"
                };
                pb.Items.Add(nv);
                lblPhongBan.Text = pb.Header.ToString();
            }
        }

        private void BtnSuaNhanVien_Click(object sender, RoutedEventArgs e)
        {
            if (treePhongBan.SelectedItem is TreeViewItem nv && nv.Parent is TreeViewItem)
            {
                nv.Header = $"{txtHoTen.Text}-{txtMaNV.Text}-{txtDiaChi.Text}-{txtSDT.Text}";
            }
        }

        private void BtnXoaNhanVien_Click(object sender, RoutedEventArgs e)
        {
            if (treePhongBan.SelectedItem is TreeViewItem nv && nv.Parent is TreeViewItem pb)
            {
                pb.Items.Remove(nv);
            }
        }

        private void treePhongBan_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treePhongBan.SelectedItem is TreeViewItem item)
            {
                string[] parts = item.Header.ToString().Split('-');
                if (parts.Length == 4)
                {
                    txtHoTen.Text = parts[0];
                    txtMaNV.Text = parts[1];
                    txtDiaChi.Text = parts[2];
                    txtSDT.Text = parts[3];
                }
                else
                {
                    lblPhongBan.Text = item.Header.ToString();
                }
            }
        }

        private void BtnThoat_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
