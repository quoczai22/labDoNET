using System.Windows;

namespace _2001240399_Trinh_Huu_Kien_Quoc_KT2L2.Views
{
    public partial class Bai3 : Window
    {
        public Bai3()
        {
            InitializeComponent();
            DataContext = new ViewModels.Bai3ViewModel();
        }
    }
}
