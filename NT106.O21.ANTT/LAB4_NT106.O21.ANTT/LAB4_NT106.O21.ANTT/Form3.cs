using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB4_NT106.O21.ANTT
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        //testcase: Nhập URL: https://httpbin.org   Nhập postData: field1=value1&field2=value2
        private void button1_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;
            string postData = textBox2.Text;
            if (string.IsNullOrWhiteSpace(url))
            {
                richTextBox1.Text = "Error: Invalid URI: The URI is empty.";
                return;
            }
            if (!url.EndsWith("/post", StringComparison.OrdinalIgnoreCase))
            {
                url = url.TrimEnd('/') + "/post";
            }

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                request.KeepAlive = false; 
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3"; // Đặt User-Agent

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        if (dataStream != null)
                        {
                            using (StreamReader reader = new StreamReader(dataStream))
                            {
                                string responseFromServer = reader.ReadToEnd();
                                richTextBox1.Text = responseFromServer;
                            }
                        }
                        else
                        {
                            richTextBox1.Text = "No response received from the server.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                richTextBox1.Text = "Error: " + ex.Message;
            }
        }
    }
}
