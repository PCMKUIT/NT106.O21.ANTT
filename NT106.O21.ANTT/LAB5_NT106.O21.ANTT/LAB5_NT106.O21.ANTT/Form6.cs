using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MailKit.Net.Smtp;

namespace LAB5_NT106.O21.ANTT
{
    public partial class Form6 : Form
    {
        private string password;
        public Form6(string Email, string Password)
        {
            InitializeComponent();
            textBox1.Text = Email;
            this.password = Password;
        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox4.Text = openFileDialog.FileName;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //22520708@gm.uit.edu.vn
            //yzhv obvn ilsn xstc
            string from = textBox1.Text;
            string to = textBox2.Text;
            string subject = textBox3.Text;
            string attachmentPath = textBox4.Text;
            string body = richTextBox1.Text;

            bool isHtml = checkBox1.Checked;

            if (attachmentPath.StartsWith("\"") && attachmentPath.EndsWith("\""))
            {
                attachmentPath = attachmentPath.Substring(1, attachmentPath.Length - 2);
            }

            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(from, from));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;

                var builder = new BodyBuilder();

                if (isHtml)
                {
                    builder.HtmlBody = body;
                }
                else
                {
                    builder.TextBody = body;
                }

                if (File.Exists(attachmentPath))
                {
                    builder.Attachments.Add(attachmentPath);
                }

                message.Body = builder.ToMessageBody();

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
