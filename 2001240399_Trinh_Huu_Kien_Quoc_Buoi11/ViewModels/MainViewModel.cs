namespace _2001240399_TrinhHuuKienQuoc_Buoi11.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private object _CurrentViewModel;
        public object CurrentViewModel
        {
            get => _CurrentViewModel;
            set { _CurrentViewModel = value; OnPropertyChanged(nameof(CurrentViewModel)); }
        }

        public RelayCommand ShowKhoaCommand { get; set; }
        public RelayCommand ShowLopCommand { get; set; }
        public RelayCommand ShowMonHocCommand { get; set; }
        public RelayCommand ShowSinhVienCommand { get; set; }

        public MainViewModel()
        {
            ShowKhoaCommand = new RelayCommand(o => CurrentViewModel = new KhoaViewModel());
            ShowLopCommand = new RelayCommand(o => CurrentViewModel = new LopViewModel());
            ShowMonHocCommand = new RelayCommand(o => CurrentViewModel = new MonHocViewModel());
            ShowSinhVienCommand = new RelayCommand(o => CurrentViewModel = new SinhVienViewModel());
            CurrentViewModel = new KhoaViewModel();
        }
    }
}
