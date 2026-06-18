using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace _2001240399_Trinh_Huu_Kien_Quoc_Buoi13
{
    public partial class MainWindow : Window
    {
        private readonly string connStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QLSinhVien;Integrated Security=True;MultipleActiveResultSets=True";
        private string currentSql;
        private string currentTitle;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HienThiBaoCao("DANH SÁCH SINH VIÊN", @"
SELECT MaSinhVien AS [Mã sinh viên], HoTen AS [Họ tên], GioiTinh AS [Giới tính],
       CONVERT(varchar(10), NgaySinh, 103) AS [Ngày sinh], MaLop AS [Mã lớp]
FROM SinhVien
ORDER BY MaSinhVien");
        }

        private DataTable LayDuLieu(string sql)
        {
            using (SqlDataAdapter da = new SqlDataAdapter(sql, connStr))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private void HienThiBaoCao(string tieuDe, string sql)
        {
            try
            {
                currentTitle = tieuDe;
                currentSql = sql;
                txtTieuDe.Text = tieuDe;
                DataTable dt = LayDuLieu(sql);
                dgBaoCao.ItemsSource = dt.DefaultView;
                txtTrangThai.Text = "Số dòng: " + dt.Rows.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải báo cáo: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnKhoa_Click(object sender, RoutedEventArgs e)
        {
            HienThiBaoCao("DANH SÁCH KHOA", "SELECT MaKhoa AS [Mã khoa], TenKhoa AS [Tên khoa] FROM Khoa ORDER BY MaKhoa");
        }

        private void btnLop_Click(object sender, RoutedEventArgs e)
        {
            HienThiBaoCao("DANH SÁCH LỚP", @"
SELECT l.MaLop AS [Mã lớp], l.MaKhoa AS [Mã khoa], k.TenKhoa AS [Tên khoa]
FROM Lop l LEFT JOIN Khoa k ON l.MaKhoa = k.MaKhoa
ORDER BY l.MaKhoa, l.MaLop");
        }

        private void btnMonHoc_Click(object sender, RoutedEventArgs e)
        {
            HienThiBaoCao("DANH SÁCH MÔN HỌC", "SELECT MaMonHoc AS [Mã môn], TenMonHoc AS [Tên môn học], SoTC AS [Số tín chỉ], TinhChat AS [Tính chất] FROM MonHoc ORDER BY MaMonHoc");
        }

        private void btnSinhVien_Click(object sender, RoutedEventArgs e)
        {
            HienThiBaoCao("DANH SÁCH SINH VIÊN", @"
SELECT MaSinhVien AS [Mã sinh viên], HoTen AS [Họ tên], GioiTinh AS [Giới tính],
       CONVERT(varchar(10), NgaySinh, 103) AS [Ngày sinh], MaLop AS [Mã lớp]
FROM SinhVien
ORDER BY MaSinhVien");
        }

        private void btnDiem_Click(object sender, RoutedEventArgs e)
        {
            HienThiBaoCao("DANH SÁCH ĐIỂM", @"
SELECT k.MaSinhVien AS [Mã sinh viên], sv.HoTen AS [Họ tên], k.MaMonHoc AS [Mã môn], mh.TenMonHoc AS [Tên môn],
       k.NamHoc AS [Năm học], k.HocKy AS [Học kỳ], k.Diem AS [Điểm]
FROM KetQua k
LEFT JOIN SinhVien sv ON k.MaSinhVien = sv.MaSinhVien
LEFT JOIN MonHoc mh ON k.MaMonHoc = mh.MaMonHoc
ORDER BY k.MaMonHoc, k.NamHoc, k.HocKy, k.MaSinhVien");
        }

        private void btnTaiLai_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(currentSql)) HienThiBaoCao(currentTitle, currentSql);
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
