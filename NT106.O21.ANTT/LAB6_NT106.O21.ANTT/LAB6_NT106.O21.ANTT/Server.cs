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

namespace LAB6_NT106.O21.ANTT
{
    public partial class Server : Form
    {
        public event Action<int> ClientCountChanged;
        public event Action ServerClosed;
        public event Action Routing;
        private Socket serverSocket;
        private List<Socket> clientSockets = new List<Socket>();
        private byte[] buffer = new byte[2048];
        private int clientCount = 0;
        public Server()
        {
            InitializeComponent();
            button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8080)); 
            serverSocket.Listen(8080);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            button2.Enabled = true;
            Route();
        }



        private void AcceptCallback(IAsyncResult IAR)
        {
            Socket socket = serverSocket.EndAccept(IAR);
            clientSockets.Add(socket);
            clientCount++;
            UpdateClientCount();
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private void ReceiveCallback(IAsyncResult IAR)
        {
            Socket current = (Socket)IAR.AsyncState;
            int received;
            try
            {
                received = current.EndReceive(IAR);
            }
            catch (SocketException)
            {
                current.Close();
                clientSockets.Remove(current);
                clientCount--;
                UpdateClientCount();
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);

            foreach (Socket socket in clientSockets)
            {
                if (socket != current)
                {
                    socket.Send(recBuf);
                }
            }

            current.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), current);
        }

        private void UpdateClientCount()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateClientCount));
                return;
            }
            textBox1.Text = clientCount.ToString();
            if (clientCount == 5)
            {
                MessageBox.Show("The limit for the number of clients has been reached",
                                "Client Limit Reached",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
            OnClientCountChanged(clientCount);
        }

        protected virtual void OnClientCountChanged(int count)
        {
            ClientCountChanged?.Invoke(count);
        }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseServer();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CloseServer();
            this.Close();
        }

        private void CloseServer()
        {
            try
            {
                foreach (Socket socket in clientSockets)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                clientSockets.Clear();

                if (serverSocket != null)
                {
                    serverSocket.Close();
                    serverSocket = null; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error closing server: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                OnServerClosed();
            }
        }


        protected virtual void OnServerClosed()
        {
            ServerClosed?.Invoke();
        }

        protected virtual void Route()
        {
            Routing?.Invoke();
        }
    }
}
