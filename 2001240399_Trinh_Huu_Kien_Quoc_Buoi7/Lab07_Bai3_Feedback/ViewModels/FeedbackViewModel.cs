using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Lab07_Bai3_Feedback.Models;

namespace Lab07_Bai3_Feedback.ViewModels
{
    public class FeedbackViewModel : BaseViewModel
    {
        private string _customerName;
        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                _customerName = value;
                OnPropertyChanged(nameof(CustomerName));
            }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        private string _suggestionText;
        public string SuggestionText
        {
            get { return _suggestionText; }
            set
            {
                _suggestionText = value;
                OnPropertyChanged(nameof(SuggestionText));
            }
        }

        public ObservableCollection<string> Answers { get; set; }

        private string _selectedServiceAnswer;
        public string SelectedServiceAnswer
        {
            get { return _selectedServiceAnswer; }
            set
            {
                _selectedServiceAnswer = value;
                OnPropertyChanged(nameof(SelectedServiceAnswer));
            }
        }

        private string _selectedProductAnswer;
        public string SelectedProductAnswer
        {
            get { return _selectedProductAnswer; }
            set
            {
                _selectedProductAnswer = value;
                OnPropertyChanged(nameof(SelectedProductAnswer));
            }
        }

        private string _selectedAttitudeAnswer;
        public string SelectedAttitudeAnswer
        {
            get { return _selectedAttitudeAnswer; }
            set
            {
                _selectedAttitudeAnswer = value;
                OnPropertyChanged(nameof(SelectedAttitudeAnswer));
            }
        }

        public ObservableCollection<Suggestion> Suggestions { get; set; }
        public ObservableCollection<FeedbackResponse> Responses { get; set; }

        public RelayCommand SendSuggestionCommand { get; set; }
        public RelayCommand SaveResponseCommand { get; set; }
        public RelayCommand ResetCommand { get; set; }
        public RelayCommand SaveSuggestionFileCommand { get; set; }
        public RelayCommand LoadSuggestionFileCommand { get; set; }
        public RelayCommand SaveResponseFileCommand { get; set; }
        public RelayCommand LoadResponseFileCommand { get; set; }

        public FeedbackViewModel()
        {
            Suggestions = new ObservableCollection<Suggestion>();
            Responses = new ObservableCollection<FeedbackResponse>();

            Answers = new ObservableCollection<string>
            {
                "Rất hài lòng",
                "Hài lòng",
                "Bình thường",
                "Không hài lòng"
            };

            SendSuggestionCommand = new RelayCommand(SendSuggestion);
            SaveResponseCommand = new RelayCommand(SaveResponse);
            ResetCommand = new RelayCommand(x => ResetForm());
        }

        private bool ValidateCustomer()
        {
            if (string.IsNullOrWhiteSpace(CustomerName))
            {
                MessageBox.Show("Tên khách hàng không được rỗng!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Phone))
            {
                MessageBox.Show("Số điện thoại không được rỗng!");
                return false;
            }

            return true;
        }

        public void SendSuggestion(object obj)
        {
            if (!ValidateCustomer()) return;

            if (string.IsNullOrWhiteSpace(SuggestionText))
            {
                MessageBox.Show("Nội dung góp ý không được rỗng!");
                return;
            }

            Suggestions.Add(new Suggestion
            {
                CustomerName = CustomerName,
                Phone = Phone,
                Content = SuggestionText
            });

            MessageBox.Show("Đã gửi góp ý!");
            SuggestionText = string.Empty;
            RefreshAll();
        }

        public void SaveResponse(object obj)
        {
            if (!ValidateCustomer()) return;

            Responses.Add(new FeedbackResponse
            {
                CustomerName = CustomerName,
                Phone = Phone,
                ServiceAnswer = string.IsNullOrWhiteSpace(SelectedServiceAnswer) ? "Chưa trả lời" : SelectedServiceAnswer,
                ProductAnswer = string.IsNullOrWhiteSpace(SelectedProductAnswer) ? "Chưa trả lời" : SelectedProductAnswer,
                AttitudeAnswer = string.IsNullOrWhiteSpace(SelectedAttitudeAnswer) ? "Chưa trả lời" : SelectedAttitudeAnswer
            });

            MessageBox.Show("Đã lưu phản hồi!");
            RefreshAll();
        }

        public void ResetForm()
        {
            CustomerName = string.Empty;
            Phone = string.Empty;
            SuggestionText = string.Empty;
            SelectedServiceAnswer = null;
            SelectedProductAnswer = null;
            SelectedAttitudeAnswer = null;
            RefreshAll();
        }

        public void RefreshAll()
        {
            OnPropertyChanged(nameof(CustomerName));
            OnPropertyChanged(nameof(Phone));
            OnPropertyChanged(nameof(SuggestionText));
            OnPropertyChanged(nameof(Suggestions));
            OnPropertyChanged(nameof(Responses));
            OnPropertyChanged(nameof(SelectedServiceAnswer));
            OnPropertyChanged(nameof(SelectedProductAnswer));
            OnPropertyChanged(nameof(SelectedAttitudeAnswer));
        }
    }

}
