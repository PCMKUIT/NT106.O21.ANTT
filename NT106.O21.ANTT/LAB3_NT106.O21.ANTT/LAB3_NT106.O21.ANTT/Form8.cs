using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB3_NT106.O21.ANTT
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }

        TcpClient tcpClient;
        NetworkStream ns;

        private void button1_Click(object sender, EventArgs e)
        {
            Byte[] data = Encoding.UTF8.GetBytes("Hello Server\n");
            ns.Write(data, 0, data.Length);
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            tcpClient = new TcpClient();
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 8080);

            tcpClient.Connect(ipEndPoint);
            ns = tcpClient.GetStream();
        }

        private void Form8_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (tcpClient != null && tcpClient.Connected)
            {
                Byte[] data = System.Text.Encoding.ASCII.GetBytes("Quit\n");
                ns.Write(data, 0, data.Length);
                ns.Close();
                tcpClient.Close();
            }
            Form7.form8Closed = true;
        }
    }
}
