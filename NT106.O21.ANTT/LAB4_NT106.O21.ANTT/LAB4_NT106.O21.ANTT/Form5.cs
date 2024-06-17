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
using HtmlAgilityPack;

namespace LAB4_NT106.O21.ANTT
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            textBox1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                WebRequest request = WebRequest.Create(textBox1.Text);
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                response.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Valid URL.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            textBox1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("about:blank");
            textBox1.Enabled = true;
            textBox1.Clear();
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(textBox1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;
            string fileName = "index.html";
            try
            {
                using (WebClient client = new WebClient())
                {
                    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                    folderBrowserDialog.Description = "Select the folder to save the web page";

                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        string folderPath = folderBrowserDialog.SelectedPath;
                        string htmlFilePath = Path.Combine(folderPath, fileName);

                        string html = client.DownloadString(url);

                        string assetsFolder = Path.Combine(folderPath, "assets");
                        Directory.CreateDirectory(assetsFolder);

                        HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                        document.LoadHtml(html);

                        foreach (HtmlNode node in document.DocumentNode.SelectNodes("//img[@src] | //link[@href] | //script[@src]"))
                        {
                            string attribute = node.Name == "link" ? "href" : "src";
                            string fileUrl = node.GetAttributeValue(attribute, null);

                            if (!string.IsNullOrEmpty(fileUrl))
                            {
                                Uri uri = new Uri(fileUrl, UriKind.RelativeOrAbsolute);
                                if (!uri.IsAbsoluteUri)
                                {
                                    uri = new Uri(new Uri(url), uri);
                                }

                                fileName = Path.GetFileName(uri.LocalPath);
                                string localFilePath = Path.Combine(assetsFolder, fileName);

                                try
                                {
                                    client.DownloadFile(uri, localFilePath);
                                    node.SetAttributeValue(attribute, Path.Combine("assets", fileName));
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error downloading file: {fileUrl}\n{ex.Message}");
                                }
                            }
                        }

                        document.Save(htmlFilePath);
                        MessageBox.Show("Downloaded!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument content = new HtmlAgilityPack.HtmlDocument();
            content = web.Load(textBox1.Text);
            Form6 source = new Form6(content.Text);
            source.Show();
        }
    }
}
