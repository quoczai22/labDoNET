using System.Windows;
using System.Windows.Controls;

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi4
{
    public partial class Bai2 : Window
    {
        public Bai2()
        {
            InitializeComponent();

            AddPhongBan("Giam doc", "BGD");
            AddPhongBan("Ke toan", "KT");
            AddPhongBan("Ke hoach", "PKH");
        }

        void AddPhongBan(string ten, string ma)
        {
            TreeViewItem pb = new TreeViewItem
            {
                Header = $"{ten}-{ma}"
            };

            treePhongBan.Items.Add(pb);
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            AddPhongBan(txtMaPhong.Text, txtTenPhong.Text);
        }

        private void tvPhongBan_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treePhongBan.SelectedItem is TreeViewItem item)
            {
                string[] pb = item.Header.ToString().Split('-');

                if (pb.Length == 2)
                {
                    lblMaPhong.Text = pb[0];
                    lblTenPhong.Text = pb[1];
                }
            }
        }

        private void BtnThoat_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}