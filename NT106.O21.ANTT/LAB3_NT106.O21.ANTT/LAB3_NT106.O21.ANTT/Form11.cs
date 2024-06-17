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
using System.Runtime.Remoting.Messaging;

namespace LAB3_NT106.O21.ANTT
{
    public partial class Form11 : Form
    {
        private TcpClient tcpClient;
        private NetworkStream ns;
        private IPEndPoint ipEndPoint;
        private Thread UpdateUI;
        public Form11()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendData(textBox2.Text);
            textBox2.Clear();
        }


        private void Form11_Load(object sender, EventArgs e)
        {
            if (!NewClient()) this.Close();
        }
        private bool NewClient()
        {
            try
            {
                tcpClient = new TcpClient();
                ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
                tcpClient.Connect(ipEndPoint);
                ns = tcpClient.GetStream();
                Thread Receiver = new Thread(ReceiveFromSever);
                Receiver.Start();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                tcpClient = null;
                ipEndPoint = null;
                this.Close();
                return false;
            }
        }

        private void ReceiveFromSever()
        {
            try
            {
                byte[] recv = new byte[1024];
                while (true)
                {
                    string text = "";
                    int bytesReceived = tcpClient.Client.Receive(recv);
                    text = Encoding.UTF8.GetString(recv, 0, bytesReceived);

                    if (text == "server quit")
                    {
                        byte[] data = System.Text.Encoding.UTF8.GetBytes(textBox1.Text + ": quit");
                        SendData("quit because server stop listenning");
                        tcpClient.Close();
                        this.Dispose();
                        this.Close();
                        break;
                    }
                    UpdateUI = new Thread(() => UpdateUIThread(text));
                    UpdateUI.Start();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Close the connection!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }

        private void UpdateUIThread(string text)
        {
            listView1.Items.Add(new ListViewItem(text));
        }

        private void SendData(string msg)
        {
            ns = tcpClient.GetStream();
            Byte[] data = System.Text.Encoding.UTF8.GetBytes(textBox1.Text + ": " + msg);
            ns.Write(data, 0, data.Length);
        }

        private bool CloseClient()
        {
            try
            {
                if (tcpClient != null)
                {
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(textBox1.Text + ": quit");
                    ns = tcpClient.GetStream();
                    ns.Write(data, 0, data.Length);
                    ns.Close();
                    tcpClient.Close();
                    return true;
                }
                return true;
            }
            catch (Exception)
            {
                this.Close();
                return true;
            }
        }
        private void Form11_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CloseClient())
            {
                e.Cancel = true;
            }
        }

    }
}
