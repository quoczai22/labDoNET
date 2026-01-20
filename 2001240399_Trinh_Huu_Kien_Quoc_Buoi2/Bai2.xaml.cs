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

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi2
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
        public void ChangeTheme(string themeName)
        {
            var dict=new ResourceDictionary {Source =new Uri($"Themes/{themeName}.xaml",UriKind.Relative)};
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboX.SelectedIndex == 0)
            {
                ChangeTheme("ThemeLight");
            }
            else{
                ChangeTheme("ThemeDark");
            }
        }
    }
}
