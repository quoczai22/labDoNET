using BTVN.Views;
using BTVN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BTVN.ViewModels
{
    public class Class2
    {
        public class HeThongViewModel : BaseViewModel
        {
            QLHangHoaEntities db = new QLHangHoaEntities();

            string _email;
            public string Email
            {
                get { return _email; }
                set
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }

            string _password;
            public string Password
            {
                get { return _password; }
                set
                {
                    _password = value; OnPropertyChanged(nameof(Password));
                }
            }

            public RelayCommand LoginCommand { get; set; }
            private bool CanLogin(object p)
            {
                return true;
            }
            public void Login(object p)
            {
                // Lấy mật khẩu an toàn từ PasswordBox truyền qua tham số p
                var passwordBox = p as System.Windows.Controls.PasswordBox;
                if (passwordBox != null)
                {
                    Password = passwordBox.Password;
                }

                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!");
                    return;
                }

                try
                {
                    // Thực hiện tìm kiếm tài khoản (Đảm bảo tên bảng là NHANVIENs hoặc NHANVIEN tùy theo DB của bạn)
                    var acc = db.NHANVIENs.FirstOrDefault(nv => nv.Email == Email && nv.MatKhau == Password);
                    if (acc != null)
                    {
                        // Mở Form Main, đóng Form Đăng nhập
                        var mainView = new frmMain (acc);
                        mainView.Show();

                        var loginWindow = Application.Current.Windows
                            .OfType<Window>()
                            .FirstOrDefault(w => w.DataContext == this);
                        loginWindow?.Close();

                    }
                    else
                    {
                        MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message);
                }
            }
            public HeThongViewModel()
            {
                LoginCommand = new RelayCommand(Login, CanLogin);
            }
        }
    }
}
