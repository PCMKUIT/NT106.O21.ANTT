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
    public partial class Form10 : Form
    {
        public Form10()
        {
            InitializeComponent();
        }

        private byte[] recv;
        private int bytesReceived = 0;
        private List<Socket> ListClient;
        private Socket listenerSocket;
        private IPEndPoint ipepServer;

        private void button1_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Thread StartListenThread = new Thread(ListenThread);
            if (!StartListenThread.IsAlive)
            {
                StartListenThread.Start();
            }

            button1.Enabled = false;
            if (listView1.Items.Count != 0)
            {
                listView1.Items.Clear();
                listView1.Clear();
            }
        }

        private void ListenThread()
        {
            bytesReceived = 0;
            recv = new byte[1024];
            listenerSocket = new Socket(
                 AddressFamily.InterNetwork,
                 SocketType.Stream,
                 ProtocolType.Tcp
                 );
            try
            {
                ipepServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
                listenerSocket.Bind(ipepServer);
                listView1.Items.Add(new ListViewItem("Server running on: " + ipepServer.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            listenerSocket.Listen(-1);
            AcceptClient();
        }

        private void AcceptClient()
        {
            try
            {
                ListClient = new List<Socket>();
                while (true)
                {
                    Socket clientSocket = listenerSocket.Accept();
                    ListClient.Add(clientSocket);
                    listView1.Items.Add(new ListViewItem("New client connected from: " + clientSocket.RemoteEndPoint.ToString()));
                    Thread receiver = new Thread(() => ReceiveDataThread(clientSocket));
                    receiver.Start();
                }
            }
            catch (Exception)
            {
                CloseMe();
            }
        }

        private void ReceiveDataThread(Socket clientSocket)
        {
            try
            {
                while (true && clientSocket.Connected && listenerSocket.LocalEndPoint != null)
                {
                    string msg = "";
                    bytesReceived = clientSocket.Receive(recv);
                    msg = Encoding.UTF8.GetString(recv, 0, bytesReceived);
                    string listViewString = clientSocket.RemoteEndPoint.ToString() + ": " + msg;

                    listView1.Items.Add(new ListViewItem(listViewString));
                    broadcast(msg);
                    if (msg.Contains("quit"))
                    {
                        CloseClientConnection(clientSocket);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Close the connection!","Notification",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                this.Close();
            }
        }

        private void CloseClientConnection(Socket clientSocket)
        {
            clientSocket.Close();
            foreach (var item in ListClient.ToArray())
            {
                if (item == clientSocket)
                {
                    ListClient.RemoveAt(ListClient.IndexOf(item));
                }
            }
        }

        private void SendData(string msg, Socket client)
        {
            Byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
            client.Send(data);
        }

        private void broadcast(string msg)
        {
            foreach (var item in ListClient)
            {
                SendData(msg, item);
            }
        }

        private void CloseMe()
        {
            StopListening();

            foreach (var item in ListClient.ToArray())
            {
                CloseClientConnection(item);
            }

            ListClient.Clear();

            recv = null;
            bytesReceived = 0;
            ipepServer = null;
        }

        private void StopListening()
        {
            if (listenerSocket != null)
            {
                broadcast("server quit");

                if (listView1.SelectedItems.Count == 0 && listView1.Items.Count != 0)
                {
                    listView1.Items.Clear();
                }

                listenerSocket.Close();
            }
        }

        private void Form10_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseMe();
        }

        private void Form10_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseMe();
        }

        private void Form10_Load(object sender, EventArgs e)
        {

        }

    }
}
