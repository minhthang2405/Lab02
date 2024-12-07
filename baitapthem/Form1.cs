using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using baitapthem.EntitiesDb;

namespace baitapthem
{
    public partial class frm_banvechieuphim : Form
    {
        private QuanLyRapModel db = new QuanLyRapModel();
        private Dictionary<int, Button> seatButtons = new Dictionary<int, Button>();
        private Dictionary<int, int> giaGhe = new Dictionary<int, int>
        {
            { 1, 30000 }, { 2, 30000 }, { 3, 30000 }, { 4, 30000 }, { 5, 30000 },
            { 6, 40000 }, { 7, 40000 }, { 8, 40000 }, { 9, 40000 }, { 10, 40000 },
            { 11, 50000 }, { 12, 50000 }, { 13, 50000 }, { 14, 50000 }, { 15, 50000 },
            { 16, 80000 }, { 17, 80000 }, { 18, 80000 }, { 19, 80000 }, { 20, 80000 }
        };
        private List<int> gheDangChon = new List<int>();
        private List<int> gheDaBan = new List<int>();
        private int maKhachHangCounter = 1;

        public frm_banvechieuphim()
        {
            InitializeComponent();
            InitializeSeatButtons();
            LoadDataFromDatabase();
            dataGridView_KhachHang.SelectionChanged += DataGridView_KhachHang_SelectionChanged;
            UpdateGenderCounts(); // Add this line
        }

        private void InitializeSeatButtons()
        {
            foreach (Control control in groupBoxSeats.Controls)
            {
                if (control is Button button)
                {
                    int seatNumber;
                    if (int.TryParse(button.Text, out seatNumber))
                    {
                        button.BackColor = Color.White;
                        button.Tag = seatNumber;
                        button.Click += btnChooseASeat;
                        seatButtons[seatNumber] = button;
                    }
                }
            }
        }

        private void LoadDataFromDatabase()
        {
            var khachHangs = db.KhachHang.Include("HoaDon").Include("Ghe").Include("KhuVuc").ToList();
            foreach (var kh in khachHangs)
            {
                if (kh.Ghe.Any()) // Check if the customer has bought any seats
                {
                    var tongTien = kh.Ghe.Sum(g => giaGhe[g.SoGhe]);
                    var hoaDon = kh.HoaDon.FirstOrDefault();
                    var ngayMua = hoaDon?.NgayMua.ToString("dd/MM/yyyy") ?? "N/A"; // Get the real purchase date
                    var gioMua = hoaDon?.GioMua.ToString(@"hh\:mm\:ss") ?? "N/A"; // Get the real purchase time
                    var tenKhuVuc = kh.KhuVuc?.TenKhuVuc ?? "N/A"; // Get the KhuVuc name

                    dataGridView_KhachHang.Rows.Add(kh.MaKhachHang, kh.TenKhachHang, $"{ngayMua} {gioMua}", tongTien, string.Join(", ", kh.Ghe.Select(g => g.SoGhe)), tenKhuVuc, kh.GioiTinh);
                    foreach (var ghe in kh.Ghe)
                    {
                        gheDaBan.Add(ghe.SoGhe);
                        seatButtons[ghe.SoGhe].BackColor = Color.Yellow;
                    }
                }
            }
            UpdateGenderCounts(); // Add this line
        }
        private void UpdateGenderCounts()
        {
            int totalNam = 0;
            int totalNu = 0;

            foreach (DataGridViewRow row in dataGridView_KhachHang.Rows)
            {
                if (row.Cells["GioiTinh"].Value?.ToString() == "Nam")
                {
                    totalNam++;
                }
                else if (row.Cells["GioiTinh"].Value?.ToString() == "Nữ") // Ensure the string matches exactly
                {
                    totalNu++;
                }
            }

            total_Nam.Text = totalNam.ToString();
            total_Nu.Text = totalNu.ToString();
        }







        private void btnChooseASeat(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                int seatNumber = (int)btn.Tag;

                if (btn.BackColor == Color.White)
                {
                    btn.BackColor = Color.Blue;
                    gheDangChon.Add(seatNumber);
                }
                else if (btn.BackColor == Color.Blue)
                {
                    btn.BackColor = Color.White;
                    gheDangChon.Remove(seatNumber);
                }
                else if (btn.BackColor == Color.Yellow)
                {
                    MessageBox.Show("Ghế đã được bán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnChon_Click(object sender, EventArgs e)
        {
            if (gheDangChon.Count == 0)
            {
                MessageBox.Show("Không có ghế nào được chọn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenKH.Text) || string.IsNullOrWhiteSpace(txtSDT.Text) ||
                string.IsNullOrWhiteSpace(cmb_Khuvuc.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin khách hàng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int totalAmount = 0;
            foreach (int seatNumber in gheDangChon)
            {
                seatButtons[seatNumber].BackColor = Color.Yellow;
                gheDaBan.Add(seatNumber);
                totalAmount += giaGhe[seatNumber];
            }

            string gheDaChon = string.Join(", ", gheDangChon);
            string ngayMua = DateTime.Now.ToString("dd/MM/yyyy");
            string gioMua = DateTime.Now.ToString("HH:mm:ss");

            txtThanhTien.Text = $"{totalAmount} VND";

            var khachHang = new EntitiesDb.KhachHang
            {
                MaKhachHang = maKhachHangCounter,
                TenKhachHang = txtTenKH.Text,
                SDT = txtSDT.Text,
                MaKhuVuc = GetKhuVucIdByName(cmb_Khuvuc.Text),
            };

            var hoaDon = new EntitiesDb.HoaDon
            {
                MaKhachHang = maKhachHangCounter,
                NgayMua = DateTime.Now.Date,
                GioMua = DateTime.Now.TimeOfDay,
                TongTien = totalAmount
            };

            try
            {
                db.KhachHang.Add(khachHang);
                db.HoaDon.Add(hoaDon);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving to database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dataGridView_KhachHang.Rows.Add(maKhachHangCounter, txtTenKH.Text, $"{ngayMua} {gioMua}", txtThanhTien.Text, gheDaChon, cmb_Khuvuc.Text);

            maKhachHangCounter++;
            gheDangChon.Clear();
        }




        private int GetKhuVucIdByName(string tenKhuVuc)
        {
            var khuVuc = db.KhuVuc.FirstOrDefault(kv => kv.TenKhuVuc == tenKhuVuc);
            return khuVuc?.MaKhuVuc ?? 0; // Return 0 if not found
        }


        private void btnHuy_Click(object sender, EventArgs e)
        {
            foreach (int seatNumber in gheDangChon)
            {
                seatButtons[seatNumber].BackColor = Color.White;
            }

            gheDangChon.Clear();
            txtThanhTien.Text = "0 VND";
        }

        private void btnKetthuc_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frm_banvechieuphim_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                e.Cancel = true;
        }

        private void DataGridView_KhachHang_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView_KhachHang.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView_KhachHang.SelectedRows[0];
                txtTenKH.Text = selectedRow.Cells["TenKhachHang"].Value?.ToString() ?? string.Empty;
                txtSDT.Text = selectedRow.Cells["SDT"].Value?.ToString() ?? string.Empty;
                cmb_Khuvuc.Text = selectedRow.Cells["KhuVuc"].Value?.ToString() ?? string.Empty;
                txtThanhTien.Text = selectedRow.Cells["TongTien"].Value?.ToString() ?? "0 VND";

                // Clear all seat selections
                foreach (var button in seatButtons.Values)
                {
                    button.BackColor = Color.White;
                }

                // Recolor the seats that have been sold
                foreach (int seatNumber in gheDaBan)
                {
                    seatButtons[seatNumber].BackColor = Color.Yellow;
                }

                // Color the seats chosen by the selected customer
                string gheDaChon = selectedRow.Cells["GheDaChon"].Value?.ToString() ?? string.Empty;
                if (!string.IsNullOrEmpty(gheDaChon))
                {
                    string[] seatNumbers = gheDaChon.Split(new[] { ", " }, StringSplitOptions.None);
                    foreach (string seatNumber in seatNumbers)
                    {
                        if (int.TryParse(seatNumber, out int seat))
                        {
                            seatButtons[seat].BackColor = Color.Blue;
                        }
                    }
                }
            }
            UpdateGenderCounts(); // Add this line

        }
    }
    public partial class HoaDon
    {
        public int MaHoaDon { get; set; }
        public int? MaKhachHang { get; set; }
        public DateTime NgayMua { get; set; }
        public TimeSpan GioMua { get; set; } // Add this line
        public decimal TongTien { get; set; }
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDon { get; set; }
        public virtual KhachHang KhachHang { get; set; }
    }
}
