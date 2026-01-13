using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi1
{
    /// <summary>
    /// Interaction logic for Bai4.xaml
    /// </summary>
    public partial class Bai4 : Window
    {
        private string[,] people = new string[5, 2];
        private int count = 0;
        public Bai4()
        {
            InitializeComponent();
        }
        private void btn_click(object sender, RoutedEventArgs e)
        {
            string name =txtten.Text.Trim();
            string age =txttuoi.Text.Trim();
            if (string.IsNullOrEmpty(name)  || string.IsNullOrEmpty(age)){
                showkq.Text = "Vui long hap day du ho ten";
                return;
            }
            if (!int.TryParse(age, out int ageNum))
            {
                showkq.Text = "Tuoi phai là so !";
                return;
            }
            if (count >= people.GetLength(0))
            {
                showkq.Text = "Mảng đầy, không thể nhập thêm!";
                return;
            }

            people[count, 0] = name;
            people[count, 1] = age.ToString();
            count++;

            showkq.Text = "Danh sách:\n";
            for (int i = 0; i < count; i++)
            {
                showkq.Text += $"{i + 1}. {people[i, 0]} - {people[i, 1]} tuổi\n";
            }

            txtten.Clear();
            txttuoi.Clear();
            txtten.Focus();
        }
    }
}


