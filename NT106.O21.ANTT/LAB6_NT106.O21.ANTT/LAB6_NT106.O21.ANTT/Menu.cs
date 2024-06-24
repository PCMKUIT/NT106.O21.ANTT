using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB6_NT106.O21.ANTT
{
    public partial class Menu : Form
    {
        private Server server;
        public Menu()
        {
            InitializeComponent();
            button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (server == null || server.IsDisposed)
            {
                server = new Server();
                server.ClientCountChanged += Server_ClientCountChanged;
                server.ServerClosed += Server_ServerClosed;
                server.Routing += Server_Route;
                server.Show();
                button1.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Client client = new Client();
            client.Show();
        }

        private void Server_ClientCountChanged(int count)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int>(Server_ClientCountChanged), count);
                return;
            }

            button2.Enabled = count < 5;
        }

        private void Server_ServerClosed()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(Server_ServerClosed));
                return;
            }
            button1.Enabled = true;
            button2.Enabled = false;
            server = null;
        }
        private void Server_Route()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(Server_Route));
                return;
            }
            button2.Enabled = true;
        }
    }
}
