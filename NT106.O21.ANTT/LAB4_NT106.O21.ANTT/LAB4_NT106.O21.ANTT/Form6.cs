using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB4_NT106.O21.ANTT
{
    public partial class Form6 : Form
    {
        public Form6(string source)
        {
            InitializeComponent();
            richTextBox1.Text = source;
        }
    }
}
