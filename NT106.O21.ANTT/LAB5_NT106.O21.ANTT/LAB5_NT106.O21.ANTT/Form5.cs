using MailKit.Net.Imap;
using MailKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB5_NT106.O21.ANTT
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            numericUpDown1.Maximum = 65535; 
            numericUpDown1.Minimum = 0;
            numericUpDown2.Maximum = 65535;
            numericUpDown2.Minimum = 0;
            textBox3.Text = "imap.gmail.com";
            textBox4.Text = "smtp.gmail.com";
            numericUpDown1.Value = 993;
            numericUpDown2.Value = 587;
        }
        bool toggle = true;
        private async Task FetchEmailsAsync()
        {
            //22520708@gm.uit.edu.vn
            //yzhv obvn ilsn xstc
            toggle = true;
            string email = textBox1.Text;
            string password = textBox2.Text;

            try
            {
                using (var client = new ImapClient())
                {
                    string imapServer = textBox3.Text;
                    int port = (int)numericUpDown1.Value;

                    await client.ConnectAsync(imapServer, port, true);
                    await client.AuthenticateAsync(email, password);

                    var inbox = client.Inbox;
                    await inbox.OpenAsync(FolderAccess.ReadOnly);

                    listView1.Items.Clear();
                    listView1.Columns.Clear();
                    listView1.Columns.Add("Number", 50);
                    listView1.Columns.Add("From", 280);
                    listView1.Columns.Add("Subject", 300);
                    listView1.Columns.Add("Datetime", 120);
                    listView1.View = View.Details;

                    var items = await inbox.FetchAsync(0, -1, MessageSummaryItems.Envelope | MessageSummaryItems.UniqueId);

                    int emailNumber = inbox.Count;

                    foreach (var item in items)
                    {
                        ListViewItem listItem = new ListViewItem(emailNumber.ToString());
                        listItem.SubItems.Add(item.Envelope.From.ToString());
                        listItem.SubItems.Add(item.Envelope.Subject);
                        listItem.SubItems.Add(item.Envelope.Date.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        listView1.Items.Insert(0, listItem);

                        emailNumber--;
                    }


                    await client.DisconnectAsync(true);

                    MessageBox.Show("Emails fetched successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to fetch emails. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toggle = false;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await FetchEmailsAsync();
            if (toggle) 
            { 
                button1.Visible = false;
                button2.Visible = true;
                button3.Visible = true;
                button5.Visible = true;
            }
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button4_Click(this, new EventArgs());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox2.UseSystemPasswordChar) textBox2.UseSystemPasswordChar = false;
            else textBox2.UseSystemPasswordChar = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            button1.Visible = true;
            button2.Visible = false;
            button3.Visible = false;
            button5.Visible = false;
            textBox1.Clear();
            textBox2.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6(textBox1.Text,textBox2.Text);
            form6.ShowDialog();
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            await ReFetchEmailsAsync();
        }
        private async Task ReFetchEmailsAsync()
        {
            //22520708@gm.uit.edu.vn
            //yzhv obvn ilsn xstc
            string email = textBox1.Text;
            string password = textBox2.Text;

            using (var client = new ImapClient())
            {
                string imapServer = textBox3.Text;
                int port = (int)numericUpDown1.Value;

                await client.ConnectAsync(imapServer, port, true);
                await client.AuthenticateAsync(email, password);

                var inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadOnly);

                listView1.Items.Clear();
                listView1.Columns.Clear();
                listView1.Columns.Add("Number", 50);
                listView1.Columns.Add("From", 280);
                listView1.Columns.Add("Subject", 280);
                listView1.Columns.Add("Datetime", 120);
                listView1.View = View.Details;

                var items = await inbox.FetchAsync(0, -1, MessageSummaryItems.Envelope | MessageSummaryItems.UniqueId);

                int emailNumber = inbox.Count;

                foreach (var item in items)
                {
                    ListViewItem listItem = new ListViewItem(emailNumber.ToString());
                    listItem.SubItems.Add(item.Envelope.From.ToString());
                    listItem.SubItems.Add(item.Envelope.Subject);
                    listItem.SubItems.Add(item.Envelope.Date.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    listView1.Items.Insert(0, listItem);
                    emailNumber--;
                }
                await client.DisconnectAsync(true);                  
            }
        }
    }
}
