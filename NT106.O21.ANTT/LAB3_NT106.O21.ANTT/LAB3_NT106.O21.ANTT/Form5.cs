﻿using System;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LAB3_NT106.O21.ANTT
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        Socket listenerSocket;
        Socket clientSocket;

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Columns[0].Text = "Telnet running on 127.0.0.1:8080\n";
            listView1.Refresh();
            button1.Enabled = false;
            CheckForIllegalCrossThreadCalls = false;
            Thread severthread = new Thread(new ThreadStart(StartUnsafeThread));
            severthread.Start();
        }
        private void StartUnsafeThread()
        {
            try
            {
                Task.Delay(1000).Wait();
                listView1.Items.Add(new ListViewItem("Waiting for connection..."));
                int bytesRecv = 0;
                byte[] recv = new byte[1];
                listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipepSV = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
                listenerSocket.Bind(ipepSV);
                listenerSocket.Listen(-1);
                clientSocket = listenerSocket.Accept();

                listView1.Items.Add(new ListViewItem("New client connected"));

                while (clientSocket.Connected)
                {
                    string text = "";
                    do
                    {
                        bytesRecv = clientSocket.Receive(recv);
                        text += Encoding.ASCII.GetString(recv);
                    }
                    while (text[text.Length - 1] != '\n');
                    listView1.Items.Add(new ListViewItem(text));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                listenerSocket.Close();
            }
        }
    }
}

