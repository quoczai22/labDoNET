using Bai2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Bai2.ViewModels
{
    public class KhoaViewModel: BaseViewModel
    {
        QL_Khoa db = new QL_Khoa();
        public ObservableCollection<Khoa> DS_Khoa { get; set; }
        private Khoa _SelectedKhoa;
        public Khoa SelectedKhoa
        {
            get => _SelectedKhoa;
            set
            {
                _SelectedKhoa = value;
                OnPropertyChanged(nameof(SelectedKhoa));
            }
        }
        public KhoaViewModel()
        {
            LoadData();
        }
        void LoadData()
        {
            DS_Khoa = new ObservableCollection<Khoa>(db.Khoas.ToList());
        }
    }
}
