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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        private string Convert (int num)
        {
            if (num == 0) return "không";
            string result = "";
            bool negative = false;
            if (num < 0) { negative = true; num = -num; }
            string[] large = { "", " ngàn", " triệu", " tỷ" };
            int i = 0;
            while (num > 0)
            {
                int tmp = num % 1000;
                if (tmp > 0)
                {
                    result = Solve(num,tmp) + large[i] + " " + result;
                }
                i++;
                num /= 1000;
            }
            if (negative) result = "âm " + result;
            return result.Trim();
        }
        private string Solve (int src, int num)
        {
            string[] small = { "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string result = "";
            int tram = (int)(num / 100);
            int chuc = (int)((num % 100) / 10);
            int donvi = (int)(num % 10);
            if (src > 1000 || tram > 0)
            {
                if (src > 1000 && tram == 0) result += "không";
                result += small[tram] + " trăm ";
                if (chuc == 0 && donvi > 0)
                    result += "lẻ ";
            }

            if (chuc > 1) result += small[chuc] + " mươi ";
            else if (chuc == 1)
            {
                result += "mười ";
                if (donvi == 5)
                    result += "lăm ";
                else result += small[donvi];
            }

            if (chuc != 1 && donvi > 0)
            {
                if (chuc != 0)
                {
                    if (donvi == 5) result += "lăm";
                    if (donvi == 1) result += "mốt";
                    if (donvi != 1 && donvi != 5) result += small[donvi];
                }
                else result += small[donvi];
            }

            return result.Trim();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int num;
            if (!int.TryParse(textBox1.Text, out num))
            {
                MessageBox.Show("Dữ liệu không hợp lệ, vui lòng nhập một số nguyên.", "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            textBox2.Text = Convert(num);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
