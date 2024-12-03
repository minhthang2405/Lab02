using System.Globalization;
using System.Windows.Forms;
using System;

namespace Lab02
{
    public partial class frm_MayTinh : Form
    {
        public frm_MayTinh()
        {
            InitializeComponent();
        }

        private void btnCong_Click(object sender, EventArgs e)
        {
            try
            {
                if (float.TryParse(txtNum1.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float num1) &&
                    float.TryParse(txtNum2.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float num2))
                {
                    float result = num1 + num2;
                    txtResult.Text = result.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập số hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng nhập số hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTru_Click(object sender, EventArgs e)
        {
            try
            {
                if (float.TryParse(txtNum1.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float num1) &&
                    float.TryParse(txtNum2.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float num2))
                {
                    float result = num1 - num2;
                    txtResult.Text = result.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập số hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng nhập số hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNhan_Click(object sender, EventArgs e)
        {
            try
            {
                if (float.TryParse(txtNum1.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float num1) &&
                    float.TryParse(txtNum2.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float num2))
                {
                    float result = num1 * num2;
                    txtResult.Text = result.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập số hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng nhập số hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChia_Click(object sender, EventArgs e)
        {
            try
            {
                if (float.TryParse(txtNum1.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float num1) &&
                    float.TryParse(txtNum2.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float num2))
                {
                    float result = num1 / num2;
                    txtResult.Text = result.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập số hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng nhập số hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

   
    }
}
