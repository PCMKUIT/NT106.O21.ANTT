using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp4
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string input = textBoxInput.Text;
            string[] score = input.Split(' ');
            if (score.Length != 12)
            {
                MessageBox.Show("Số lượng điểm không hợp lệ. Vui lòng nhập đúng 12 số điểm cho 12 môn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int[] arr = new int[12];
            for (int i = 0; i < 12; i++)
            {
                if (!(int.TryParse(score[i], out arr[i]) && arr[i] >= 0 && arr[i] <= 10))
                {
                    MessageBox.Show("Dữ liệu không hợp lệ. Vui lòng nhập một số nguyên từ 0 đến 10.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            float avg = (float)arr.Average();
            int max = arr.Max();
            int min = arr.Min();
            int pass = arr.Count(browse => browse >= 5);
            int fail = 12 - pass;
            string grade;
            if (avg >= 8 && arr.All(browse => browse >= 6.5))
            {
                grade = "Giỏi";
            }

            else if (avg >= 6.5 && arr.All(browse => browse >= 5))
            {
                grade = "Khá";
            }
            else if (avg >= 5 && arr.All(browse => browse >= 3.5))
            {
                grade = "Trung bình";
            }
            else if (avg >= 3.5 && arr.All(browse => browse >= 2))
            {
                grade = "Yếu";
            }
            else
            {
                grade = "Kém";
            }
            for (int i = 0; i < score.Length; i++)
            {
                System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)this.Controls.Find("textBox" + (i + 1), true)[0];
                textBox.Text = score[i] + "đ";
            }
            textBox13.Text = avg.ToString();
            textBox14.Text = grade;
            textBox15.Text = $"{max} đ";
            textBox16.Text = $"{min} đ";
            textBox17.Text = pass.ToString();
            textBox18.Text = fail.ToString();
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
    

