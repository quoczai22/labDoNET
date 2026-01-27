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
    /// Interaction logic for Bai1.xaml
    /// </summary>
    public partial class Bai1 : Window
    {
        public Bai1()
        {
            InitializeComponent();
        }

        private void Xem(object sender, RoutedEventArgs e)
        {
            String strMess, strHoTen, strTitle, strNN = "";
            strHoTen = txthodem.Text+""+txtten.Text;
            if(rbnam.IsChecked==true)
                strTitle="Mr.";
            else if(rbnu.IsChecked==true)
                strTitle="Ms.";
            else
                strTitle="Mrs.";
           strMess= "Xin chao "+strTitle+" "+ strHoTen+"\n";
            if (nnta.IsChecked == true)
                strNN = "Tieng anh";
            if (nntt.IsChecked == true)
                strNN=(strNN=="")?"Tieng Trung":strNN+",va Tieng Trung";
            if(cbqq.SelectedIndex>=0)
                strMess+= "Quoc tich: "+cbqq.Text+"\n";
            MessageBox.Show(strMess+"Ban biet: "+strNN,"Thong bao");
        }

        private void Nhap(object sender, RoutedEventArgs e)
        {
            txtten.Text = "";
            txthodem.Text = "";
            rbnam.IsChecked =true;
            nnta.IsChecked = false;
            nnta.IsChecked = false;
            nntt.IsChecked= false;
            cbqq.SelectedIndex = 0;
        }
    }
}
