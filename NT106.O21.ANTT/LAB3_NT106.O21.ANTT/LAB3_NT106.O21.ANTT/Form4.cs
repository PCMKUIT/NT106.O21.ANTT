using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB3_NT106.O21.ANTT
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty || textBox2.Text == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Port và IP Remote Host trước khi Listen!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            UdpClient udpClient = new UdpClient();
            IPAddress ipadd = IPAddress.Parse(textBox1.Text);
            int port = Convert.ToInt32(textBox2.Text);
            IPEndPoint ipend = new IPEndPoint(ipadd, port);
            Byte[] sendBytes = Encoding.UTF8.GetBytes(richTextBox1.Text);
            udpClient.Send(sendBytes, sendBytes.Length, ipend);
            richTextBox1.Clear();
        }
    }
}
