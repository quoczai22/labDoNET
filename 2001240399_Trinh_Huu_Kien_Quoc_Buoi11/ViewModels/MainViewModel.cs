using System.Windows.Input;

namespace Lab11_ValidationNavigation.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private object _currentViewModel;

        public object CurrentViewModel
        {
            get { return _currentViewModel; }
            set { _currentViewModel = value; OnPropertyChanged(); }
        }

        public ICommand OpenKhoaCommand { get; }
        public ICommand OpenLopCommand { get; }
        public ICommand OpenMonHocCommand { get; }
        public ICommand OpenSinhVienCommand { get; }

        public MainViewModel()
        {
            OpenKhoaCommand = new RelayCommand(o => CurrentViewModel = new KhoaViewModel());
            OpenLopCommand = new RelayCommand(o => CurrentViewModel = new LopViewModel());
            OpenMonHocCommand = new RelayCommand(o => CurrentViewModel = new MonHocViewModel());
            OpenSinhVienCommand = new RelayCommand(o => CurrentViewModel = new SinhVienViewModel());
            CurrentViewModel = new KhoaViewModel();
        }
    }
}
