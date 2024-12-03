using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab02_04
{
    public partial class frm_QuanLyTK : Form
    {
        public frm_QuanLyTK()
        {
            InitializeComponent();
            txtSotien.KeyPress += TxtSotien_KeyPress;
        }

        private void TxtSotien_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

    
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSTK.Text) ||
               string.IsNullOrWhiteSpace(txtTen.Text) ||
               string.IsNullOrWhiteSpace(txtDiachi.Text) ||
               string.IsNullOrWhiteSpace(txtSotien.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ListViewItem existingItem = null;
            foreach (ListViewItem item in listView_ThongTin.Items)
            {
                if (item.SubItems[1].Text == txtSTK.Text)
                {
                    existingItem = item;
                    break;
                }
            }

            if (existingItem == null)
            {
                var newItem = new ListViewItem(new[]
                {
                    (listView_ThongTin.Items.Count + 1).ToString(),
                    txtSTK.Text,
                    txtTen.Text,
                    txtDiachi.Text,
                    txtSotien.Text
                });
                listView_ThongTin.Items.Add(newItem);
                MessageBox.Show("Thêm mới dữ liệu thành công!", "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                existingItem.SubItems[2].Text = txtTen.Text;
                existingItem.SubItems[3].Text = txtDiachi.Text;
                existingItem.SubItems[4].Text = txtSotien.Text;
                MessageBox.Show("Cập nhật dữ liệu thành công!", "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            RecalculateTotalAmount();
            UpdateListViewIndices();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            var accountNumber = txtSTK.Text;
            ListViewItem itemToDelete = null;
            foreach (ListViewItem item in listView_ThongTin.Items)
            {
                if (item.SubItems[1].Text == accountNumber)
                {
                    itemToDelete = item;
                    break;
                }
            }

            if (itemToDelete == null)
            {
                MessageBox.Show("Không tìm thấy số tài khoản cần xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = MessageBox.Show("Bạn có muốn xóa số tài khoản này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                listView_ThongTin.Items.Remove(itemToDelete);
                MessageBox.Show("Xóa tài khoản thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RecalculateTotalAmount();
                UpdateListViewIndices();
            }
        }

        private void listView_ThongTin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_ThongTin.SelectedItems.Count > 0)
            {
                var selectedItem = listView_ThongTin.SelectedItems[0];
                txtSTK.Text = selectedItem.SubItems[1].Text;
                txtTen.Text = selectedItem.SubItems[2].Text;
                txtDiachi.Text = selectedItem.SubItems[3].Text;
                txtSotien.Text = selectedItem.SubItems[4].Text;
            }
        }

        private void RecalculateTotalAmount()
        {
            decimal totalAmount = 0;
            foreach (ListViewItem item in listView_ThongTin.Items)
            {
                if (decimal.TryParse(item.SubItems[4].Text, out var amount))
                {
                    totalAmount += amount;
                }
            }

            txtTongTien.Text = totalAmount.ToString("C");
        }

        private void UpdateListViewIndices()
        {
            for (int i = 0; i < listView_ThongTin.Items.Count; i++)
            {
                listView_ThongTin.Items[i].SubItems[0].Text = (i + 1).ToString();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
