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
using MailKit.Net.Smtp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LAB5_NT106.O21.ANTT
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //user is 22520708@gm.uit.edu.vn
            //bctp ztgr mjnt ytcs
            //fyhe jfiw ofbg olsh
            string from = textBox1.Text;
            string to = textBox2.Text;
            string subject = textBox3.Text;
            string password = textBox4.Text;
            string body = richTextBox1.Text;

            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(from, from));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;
                message.Body = new TextPart("plain")
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate(from, password);
                    client.Send(message);
                    client.Disconnect(true);
                }

                MessageBox.Show("Email sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send email. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
