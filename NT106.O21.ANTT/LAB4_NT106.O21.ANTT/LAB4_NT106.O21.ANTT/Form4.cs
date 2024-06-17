using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB4_NT106.O21.ANTT
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;
            string fileurl = textBox2.Text;

            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                response.Close();
            }
            catch (Exception ex)
            {
                richTextBox1.Text = "Error: " + ex.Message;
                return;
            }

            if (string.IsNullOrWhiteSpace(fileurl))
            {
                richTextBox1.Text = "Error: File path cannot be empty.";
                return;
            }

            fileurl = fileurl.Trim('"');
            if (!File.Exists(fileurl))
            {
                richTextBox1.Text = "Error: File does not exist.";
                return;
            }

            try
            {
                WebClient myClient = new WebClient();
                Stream response = myClient.OpenRead(url);
                myClient.DownloadFile(url, fileurl);

                string content = new StreamReader(response).ReadToEnd();
                richTextBox1.Text = content;
            }
            catch (Exception ex)
            {
                richTextBox1.Text = "Error: " + ex.Message;
            }
        }
    }
}
