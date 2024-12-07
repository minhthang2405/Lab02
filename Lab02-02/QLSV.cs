using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab02_02
{
    public partial class frm_QuanLySinhVien : Form
    {
        public frm_QuanLySinhVien()
        {
            InitializeComponent();
            dgvStudents.SelectionChanged += dgvStudents_SelectionChanged;
        }
        private int GetSelectedRow(string MSV)
        {
            for (int i = 0; i < dgvStudents.Rows.Count; i++)
            {
                if (dgvStudents.Rows[i].Cells[0].Value.ToString() == MSV)
                {
                    return i;
                }
            }
            return -1;
        }
        private void InsertUpdate(int selectedRow)
        {
            dgvStudents.Rows[selectedRow].Cells[0].Value = txtMSV.Text;
            dgvStudents.Rows[selectedRow].Cells[1].Value = txtHoTen.Text;
            dgvStudents.Rows[selectedRow].Cells[2].Value = optNu.Checked ? "Nữ" : "Nam";
            dgvStudents.Rows[selectedRow].Cells[3].Value =
                float.Parse(txtDTB.Text, System.Globalization.CultureInfo.InvariantCulture).ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);
            dgvStudents.Rows[selectedRow].Cells[4].Value = cbboxChuyenNganh.Text;
        }
        private void frm_QuanLySinhVien_Load(object sender, EventArgs e)
        {
            cbboxChuyenNganh.SelectedIndex = 0;
        }

    
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMSV.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text) || string.IsNullOrWhiteSpace(txtDTB.Text))
                    throw new Exception("Vui lòng nhập đầy đủ thông tin sinh viên");

                if (!float.TryParse(txtDTB.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float dtb) || dtb < 0.0f || dtb > 10.0f)
                    throw new Exception("Điểm trung bình phải là số từ 0.0 đến 10.0");

                if (txtHoTen.Text.Any(char.IsDigit))
                    throw new Exception("Họ tên không được chứa số");

                int selectedRow = GetSelectedRow(txtMSV.Text);
                if (selectedRow == -1)
                {
                    selectedRow = dgvStudents.Rows.Add();
                    InsertUpdate(selectedRow);
                    MessageBox.Show("Thêm mới dữ liệu thành công!", "Thông Báo", MessageBoxButtons.OK);
                }
                else
                {
                    InsertUpdate(selectedRow);
                    MessageBox.Show("Cập nhật dữ liệu thành công!", "Thông Báo", MessageBoxButtons.OK);
                }
                UpdateStudentCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedRow = GetSelectedRow(txtMSV.Text);
                if (selectedRow == -1)
                {
                    throw new Exception("Không tìm thấy sinh viên cần xóa");
                }
                else
                {
                    DialogResult dr = MessageBox.Show("Bạn có muốn xóa ?", "YES/NO", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {
                        dgvStudents.Rows.RemoveAt(selectedRow);
                        MessageBox.Show("Xóa sinh viên thành công!", "Thông Báo", MessageBoxButtons.OK);
                        UpdateStudentCount();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //2.4
        private void dgvStudents_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvStudents.SelectedRows[0];
                txtMSV.Text = selectedRow.Cells[0].Value.ToString();
                txtHoTen.Text = selectedRow.Cells[1].Value.ToString();
                if (selectedRow.Cells[2].Value.ToString() == "Nữ")
                {
                    optNu.Checked = true;
                }
                else
                {
                    optNam.Checked = true;
                }
                txtDTB.Text = float.Parse(selectedRow.Cells[3].Value.ToString(), System.Globalization.CultureInfo.InvariantCulture).ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);
                cbboxChuyenNganh.Text = selectedRow.Cells[4].Value.ToString();
            }
        }


        //2.5
        private void UpdateStudentCount()
        {
            int maleCount = 0;
            int femaleCount = 0;

            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                if (row.Cells[2].Value != null)
                {
                    if (row.Cells[2].Value.ToString() == "Nam")
                    {
                        maleCount++;
                    }
                    else if (row.Cells[2].Value.ToString() == "Nữ")
                    {
                        femaleCount++;
                    }
                }
            }

            lblTotalMale.Text = $"{maleCount}";
            lblTotalFemale.Text = $"{femaleCount}";
        }

     
        
    }
}
