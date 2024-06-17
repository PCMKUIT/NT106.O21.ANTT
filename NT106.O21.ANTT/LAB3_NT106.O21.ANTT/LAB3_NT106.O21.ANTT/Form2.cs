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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            button2.Enabled = false;
        }
    }
}
