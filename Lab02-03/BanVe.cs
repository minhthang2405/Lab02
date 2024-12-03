using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Lab02_3
{
    public partial class frm_banvechieuphim : Form
    {
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

        public frm_banvechieuphim()
        {
            InitializeComponent();
            InitializeSeatButtons();
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

        private void btnChooseASeat(object sender, EventArgs e)
        {
            Button btn = sender as Button;
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

        private void btnChon_Click(object sender, EventArgs e)
        {
            int totalAmount = 0;

            foreach (int seatNumber in gheDangChon)
            {
                seatButtons[seatNumber].BackColor = Color.Yellow;
                gheDaBan.Add(seatNumber);
                totalAmount += giaGhe[seatNumber];
            }

            gheDangChon.Clear();
            txtThanhTien.Text = $"{totalAmount} VND";
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

       
    }
}
