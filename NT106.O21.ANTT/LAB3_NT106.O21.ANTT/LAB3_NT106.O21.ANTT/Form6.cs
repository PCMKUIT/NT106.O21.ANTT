using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB3_NT106.O21.ANTT
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7();
            form7.Show();
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!Form7.button1Clicked)
            {
                MessageBox.Show("Server chưa lắng nghe!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Form8 form8 = new Form8();
            form8.Show();
            button2.Enabled = false;
        }
    }
}
