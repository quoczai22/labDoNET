using Bai4.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bai4.ViewModels
{
    public class QuanLyMonHocViewModel : BaseViewModel
    {
        private QLSinhVienEntities db = new QLSinhVienEntities();

        private bool isAdding = false;
        private bool isEditing = false;

        private ObservableCollection<MonHoc> _dsMonHoc;
        public ObservableCollection<MonHoc> DS_MonHoc
        {
            get => _dsMonHoc;
            set { _dsMonHoc = value; OnPropertyChanged(nameof(DS_MonHoc)); }
        }

        private MonHoc _selectedMonHoc;
        public MonHoc SelectedMonHoc
        {
            get => _selectedMonHoc;
            set
            {
                _selectedMonHoc = value;
                OnPropertyChanged(nameof(SelectedMonHoc));

                if (SelectedMonHoc != null && !isAdding && !isEditing)
                {
                    MaMon = SelectedMonHoc.MaMonHoc;
                    TenMonHoc = SelectedMonHoc.TenMonHoc;
                    SoTC = SelectedMonHoc.SoTC?.ToString() ?? "0";
                    SelectedTinhChat = SelectedMonHoc.TinhChat;
                }
            }
        }

        private string _maMon;
        public string MaMon { get => _maMon; set { _maMon = value; OnPropertyChanged(nameof(MaMon)); } }

        private string _tenMonHoc;
        public string TenMonHoc { get => _tenMonHoc; set { _tenMonHoc = value; OnPropertyChanged(nameof(TenMonHoc)); } }

        private string _soTC;
        public string SoTC { get => _soTC; set { _soTC = value; OnPropertyChanged(nameof(SoTC)); } }

        private string _selectedTinhChat;
        public string SelectedTinhChat { get => _selectedTinhChat; set { _selectedTinhChat = value; OnPropertyChanged(nameof(SelectedTinhChat)); } }

        public List<string> DS_TinhChat { get; set; } = new List<string> { "Tự chọn", "Bắt buộc" };

        public ICommand ThemCommand { get; set; }
        public ICommand SuaCommand { get; set; }
        public ICommand XoaCommand { get; set; }
        public ICommand LuuCommand { get; set; }
        public ICommand HuyCommand { get; set; }

        public QuanLyMonHocViewModel()
        {
            ThemCommand = new RelayCommand(ExecuteThem, CanExecuteThem);
            SuaCommand = new RelayCommand(ExecuteSua, CanExecuteSua);
            XoaCommand = new RelayCommand(ExecuteXoa, CanExecuteXoa);
            LuuCommand = new RelayCommand(ExecuteLuu, CanExecuteLuu);
            HuyCommand = new RelayCommand(ExecuteHuy, CanExecuteHuy);

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                DS_MonHoc = new ObservableCollection<MonHoc>(db.MonHocs.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh sách dữ liệu môn học: " + ex.Message);
            }
        }

        private void ExecuteThem(object p)
        {
            isAdding = true;
            isEditing = false;

            MaMon = string.Empty;
            TenMonHoc = string.Empty;
            SoTC = string.Empty;
            SelectedTinhChat = DS_TinhChat[0];
        }

        private void ExecuteSua(object p)
        {
            if (SelectedMonHoc == null)
            {
                MessageBox.Show("Vui lòng chọn một môn học từ danh sách bảng dưới trước khi thực hiện chức năng sửa!");
                return;
            }
            isAdding = false;
            isEditing = true;
        }

        private void ExecuteXoa(object p)
        {
            if (SelectedMonHoc == null)
            {
                MessageBox.Show("Vui lòng chọn môn học cần xóa từ bảng hiển thị dữ liệu!");
                return;
            }

            var confirm = MessageBox.Show($"Bạn có thực sự chắc chắn muốn xóa môn học {SelectedMonHoc.TenMonHoc} ra khỏi hệ thống?", "Xác nhận yêu cầu xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    db.MonHocs.Remove(SelectedMonHoc);
                    db.SaveChanges();
                    MessageBox.Show("Xóa thông tin dữ liệu học phần môn học thành công!");
                    LoadData();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xảy ra lỗi hệ thống hoặc bản ghi đã liên kết ràng buộc khóa ngoại với bảng kết quả điểm số sinh viên: " + ex.Message);
                }
            }
        }

        private void ExecuteLuu(object p)
        {
            if (!isAdding && !isEditing)
            {
                MessageBox.Show("Hệ thống hiện tại không nằm trong trạng thái xử lý Thêm mới hoặc Sửa đổi để thực hiện thao tác lưu trữ dữ liệu!");
                return;
            }

            if (string.IsNullOrWhiteSpace(MaMon) || string.IsNullOrWhiteSpace(TenMonHoc) || string.IsNullOrWhiteSpace(SoTC))
            {
                MessageBox.Show("Vui lòng nhập điền thông tin đầy đủ vào toàn bộ các ô yêu cầu!");
                return;
            }

            if (!int.TryParse(SoTC, out int soTinChi) || soTinChi <= 0)
            {
                MessageBox.Show("Số tín chỉ của học phần nhập vào bắt buộc phải là định dạng kiểu số nguyên dương!");
                return;
            }

            try
            {
                if (isAdding)
                {
                    string trimmedMa = MaMon.Trim();
                    if (db.MonHocs.Any(m => m.MaMonHoc.Trim() == trimmedMa))
                    {
                        MessageBox.Show("Mã học phần môn học này đã tồn tại sẵn trong cơ sở dữ liệu hệ thống!");
                        return;
                    }

                    var newMon = new MonHoc
                    {
                        MaMonHoc = trimmedMa,
                        TenMonHoc = TenMonHoc.Trim(),
                        SoTC = soTinChi,
                        TinhChat = SelectedTinhChat
                    };

                    db.MonHocs.Add(newMon);
                }
                else if (isEditing)
                {
                    var editMon = db.MonHocs.FirstOrDefault(m => m.MaMonHoc == SelectedMonHoc.MaMonHoc);
                    if (editMon != null)
                    {
                        editMon.TenMonHoc = TenMonHoc.Trim();
                        editMon.SoTC = soTinChi;
                        editMon.TinhChat = SelectedTinhChat;
                    }
                }

                db.SaveChanges();
                MessageBox.Show("Lưu trữ thông tin môn học vào hệ thống thành công!");

                isAdding = false;
                isEditing = false;
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu khi lưu trữ: " + ex.Message);
            }
        }

        private void ExecuteHuy(object p)
        {
            isAdding = false;
            isEditing = false;
            ClearForm();
            SelectedMonHoc = null;
        }

        private void ClearForm()
        {
            MaMon = string.Empty;
            TenMonHoc = string.Empty;
            SoTC = string.Empty;
            SelectedTinhChat = null;
        }

        bool CanExecuteThem(object p)
        {
            return !isAdding && !isEditing;
        }

        bool CanExecuteSua(object p)
        {
            return SelectedMonHoc != null && !isAdding && !isEditing;
        }

        bool CanExecuteXoa(object p)
        {
            return SelectedMonHoc != null && !isAdding && !isEditing;
        }

        bool CanExecuteLuu(object p)
        {
            return isAdding || isEditing;
        }

        bool CanExecuteHuy(object p)
        {
            return isAdding || isEditing;

        }
    }
}
