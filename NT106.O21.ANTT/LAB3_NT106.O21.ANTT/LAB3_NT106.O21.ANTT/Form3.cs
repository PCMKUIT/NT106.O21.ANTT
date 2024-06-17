using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB3_NT106.O21.ANTT
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        public void serverThread()
        {
            int port = Convert.ToInt32(textBox1.Text);
            UdpClient udpClient = new UdpClient(port);
            while (true)
            {
                IPEndPoint IpEnd = new IPEndPoint(IPAddress.Any, 0);
                Byte[] recvBytes = udpClient.Receive(ref IpEnd);
                string Data = Encoding.UTF8.GetString(recvBytes);
                string mess = IpEnd.Address.ToString() + ": " + Data.ToString();
                InfoMessage(mess);
            }
        }
        public void InfoMessage(string mess)
        {
            richTextBox1.Text += mess + "\n";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập Port trước khi Listen!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            CheckForIllegalCrossThreadCalls = false;
            Thread UDPServer = new Thread(new ThreadStart(serverThread));
            UDPServer.Start();
            button1.Enabled = false;
        }
    }
}
