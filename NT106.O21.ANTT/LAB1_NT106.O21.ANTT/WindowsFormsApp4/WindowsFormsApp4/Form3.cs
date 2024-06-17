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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (double.TryParse(textBox1.Text, out double number1) &&
               double.TryParse(textBox2.Text, out double number2) &&
               double.TryParse(textBox3.Text, out double number3))
            {
                double maxNumber = Math.Max(number1, Math.Max(number2, number3));
                double minNumber = Math.Min(number1, Math.Min(number2, number3));
                textBox4.Text = maxNumber.ToString();
                textBox5.Text = minNumber.ToString();
            }
            else
            {
                MessageBox.Show("Dữ liệu không hợp lệ, vui lòng nhập số vào các ô textbox!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
