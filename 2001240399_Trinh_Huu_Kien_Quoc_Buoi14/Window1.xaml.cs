using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi14
{
    public partial class Window1 : Window
    {
        private readonly string connStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QLSinhVien;Integrated Security=True;MultipleActiveResultSets=True";
        private string currentTitle;

        public Window1()
        {
            InitializeComponent();
        }

        public Window1(string maLopDuocChon) : this()
        {
            Loaded += (s, e) =>
            {
                if (!string.IsNullOrEmpty(maLopDuocChon))
                {
                    cboLop.SelectedValue = maLopDuocChon;
                    HienThiSinhVienTheoLop(maLopDuocChon);
                }
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadComboBox();
                if (cboLop.Items.Count > 0) cboLop.SelectedIndex = 0;
                if (cboKhoa.Items.Count > 0) cboKhoa.SelectedIndex = 0;
                btnShowReport_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private DataTable LayDuLieu(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private void LoadComboBox()
        {
            DataTable lop = LayDuLieu(@"
SELECT MaLop, MaLop AS TenHienThi
FROM Lop
ORDER BY MaLop");
            cboLop.ItemsSource = lop.DefaultView;

            DataTable khoa = LayDuLieu("SELECT MaKhoa, TenKhoa FROM Khoa ORDER BY MaKhoa");
            cboKhoa.ItemsSource = khoa.DefaultView;
        }

        private string LoaiBaoCao
        {
            get
            {
                ComboBoxItem item = cboLoaiBaoCao.SelectedItem as ComboBoxItem;
                return item == null ? "SinhVienTheoLop" : item.Tag.ToString();
            }
        }

        private void cboLoaiBaoCao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboLop == null || cboKhoa == null) return;
            bool dungKhoa = LoaiBaoCao == "LopTheoKhoa";
            lblKhoa.Visibility = dungKhoa ? Visibility.Visible : Visibility.Collapsed;
            cboKhoa.Visibility = dungKhoa ? Visibility.Visible : Visibility.Collapsed;
            lblLop.Visibility = dungKhoa ? Visibility.Collapsed : Visibility.Visible;
            cboLop.Visibility = dungKhoa ? Visibility.Collapsed : Visibility.Visible;
        }

        private void btnShowReport_Click(object sender, RoutedEventArgs e)
        {
            if (LoaiBaoCao == "SinhVienTheoLop")
            {
                if (cboLop.SelectedValue == null) { MessageBox.Show("Vui lòng chọn lớp!"); return; }
                HienThiSinhVienTheoLop(cboLop.SelectedValue.ToString());
            }
            else if (LoaiBaoCao == "DiemTheoLop")
            {
                if (cboLop.SelectedValue == null) { MessageBox.Show("Vui lòng chọn lớp!"); return; }
                HienThiDiemTheoLop(cboLop.SelectedValue.ToString());
            }
            else
            {
                if (cboKhoa.SelectedValue == null) { MessageBox.Show("Vui lòng chọn khoa!"); return; }
                HienThiLopTheoKhoa(cboKhoa.SelectedValue.ToString());
            }
        }

        private void GanBaoCao(string tieuDe, DataTable dt)
        {
            currentTitle = tieuDe;
            txtTieuDe.Text = tieuDe;
            dgBaoCao.ItemsSource = dt.DefaultView;
            txtTrangThai.Text = "Số dòng: " + dt.Rows.Count;
        }

        private void HienThiSinhVienTheoLop(string maLop)
        {
            DataTable dt = LayDuLieu(@"
SELECT MaSinhVien AS [Mã sinh viên], HoTen AS [Họ tên], GioiTinh AS [Giới tính],
       CONVERT(varchar(10), NgaySinh, 103) AS [Ngày sinh], MaLop AS [Mã lớp]
FROM SinhVien
WHERE MaLop = @MaLop
ORDER BY MaSinhVien", new SqlParameter("@MaLop", maLop));
            GanBaoCao("DANH SÁCH SINH VIÊN THEO LỚP " + maLop, dt);
        }

        private void HienThiDiemTheoLop(string maLop)
        {
            DataTable dt = LayDuLieu(@"
SELECT sv.MaLop AS [Mã lớp], k.MaSinhVien AS [Mã sinh viên], sv.HoTen AS [Họ tên],
       k.MaMonHoc AS [Mã môn], mh.TenMonHoc AS [Tên môn], k.NamHoc AS [Năm học],
       k.HocKy AS [Học kỳ], k.Diem AS [Điểm]
FROM KetQua k
INNER JOIN SinhVien sv ON k.MaSinhVien = sv.MaSinhVien
LEFT JOIN MonHoc mh ON k.MaMonHoc = mh.MaMonHoc
WHERE sv.MaLop = @MaLop
ORDER BY k.MaMonHoc, k.NamHoc, k.HocKy, k.MaSinhVien", new SqlParameter("@MaLop", maLop));
            GanBaoCao("BẢNG ĐIỂM THEO LỚP " + maLop, dt);
        }

        private void HienThiLopTheoKhoa(string maKhoa)
        {
            DataTable dt = LayDuLieu(@"
SELECT k.MaKhoa AS [Mã khoa], k.TenKhoa AS [Tên khoa], l.MaLop AS [Mã lớp]
FROM Lop l
INNER JOIN Khoa k ON l.MaKhoa = k.MaKhoa
WHERE l.MaKhoa = @MaKhoa
ORDER BY l.MaLop", new SqlParameter("@MaKhoa", maKhoa));
            GanBaoCao("DANH SÁCH LỚP THEO KHOA " + maKhoa, dt);
        }

        private void btnXemIn_Click(object sender, RoutedEventArgs e)
        {
            if (dgBaoCao.ItemsSource == null) return;
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                FlowDocument doc = new FlowDocument(new Paragraph(new Run(currentTitle ?? "BÁO CÁO")) { FontSize = 18, FontWeight = FontWeights.Bold });
                doc.Blocks.Add(new Paragraph(new Run("Dữ liệu đang hiển thị trên bảng báo cáo.")));
                printDialog.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, currentTitle ?? "Báo cáo");
            }
        }

        private void btnDong_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
