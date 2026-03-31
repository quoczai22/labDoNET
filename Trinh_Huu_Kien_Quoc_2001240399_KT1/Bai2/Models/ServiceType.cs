using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bai2.ViewModels;

namespace Bai2.Models
{
    public class ServiceType : BaseViewModel
    {
        public string Ten { get; set; }
        public double Gia { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }
}
