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
using MimeKit;


namespace LAB5_NT106.O21.ANTT
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private async Task FetchEmailsAsync()
        {
            //22520708@gm.uit.edu.vn
            //yzhv obvn ilsn xstc
            string email = textBox1.Text;
            string password = textBox2.Text;

            try
            {
                using (var client = new ImapClient())
                {
                    await client.ConnectAsync("imap.gmail.com", 993, true);
                    await client.AuthenticateAsync(email, password);

                    var inbox = client.Inbox;
                    await inbox.OpenAsync(FolderAccess.ReadOnly);

                    listView1.Items.Clear();
                    listView1.Columns.Clear();
                    listView1.Columns.Add("Email", 200);
                    listView1.Columns.Add("From", 100);
                    listView1.Columns.Add("Thời gian", 120);
                    listView1.View = View.Details;

                    int totalEmails = inbox.Count;
                    int emailsThisMonth = 0;

                    var items = await inbox.FetchAsync(0, -1, MessageSummaryItems.Envelope | MessageSummaryItems.UniqueId);

                    foreach (var item in items)
                    {
                        ListViewItem listItem = new ListViewItem(item.Envelope.Subject);
                        listItem.SubItems.Add(item.Envelope.From.ToString());
                        listItem.SubItems.Add(item.Envelope.Date.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        listView1.Items.Insert(0, listItem);

                        if (item.Envelope.Date.Value.Month == DateTime.Now.Month && item.Envelope.Date.Value.Year == DateTime.Now.Year)
                        {
                            emailsThisMonth++;
                        }
                    }

                    textBox3.Text = totalEmails.ToString();
                    textBox4.Text = emailsThisMonth.ToString();

                    await client.DisconnectAsync(true);

                    MessageBox.Show("Emails fetched successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to fetch emails. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await FetchEmailsAsync();
        }
    }
}
