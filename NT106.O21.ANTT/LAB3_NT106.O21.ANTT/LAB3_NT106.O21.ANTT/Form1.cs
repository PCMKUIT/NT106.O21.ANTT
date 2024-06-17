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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
  
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.ShowDialog();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6();
            form6.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9();
            form9.ShowDialog();
        }
    }
}
