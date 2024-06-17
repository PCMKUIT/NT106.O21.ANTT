using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            InitializeComboBoxes();
        }
        private void InitializeComboBoxes()
        {
            string[] option = { "Decimal", "Binary", "Hex" };
            comboBox1.Items.AddRange(option);
            comboBox2.Items.AddRange(option);
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 1;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form5_Load(object sender, EventArgs e)
        {
       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string input = textBox1.Text.Trim();
            int base1 = comboBox1.SelectedIndex;
            int base2 = comboBox2.SelectedIndex;

            if (input.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập số.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int num1= Convert.ToInt32(input, base1 == 1 ? 2 : base1 == 0 ? 10 : 16);
                string num2 = Convert.ToString(num1, base2 == 1 ? 2 : base2 == 0 ? 10 : 16);

                textBox2.Text = num2;
            }
            catch (FormatException)
            {
                MessageBox.Show("Dữ liệu không hợp lệ, vui lòng nhập lại số.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (OverflowException)
            {
                MessageBox.Show("Số quá lớn, vui lòng nhập lại một số nhỏ hơn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
    

