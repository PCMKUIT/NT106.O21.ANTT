using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB2_NT106.O21.ANTT
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt";
            if (ofd.ShowDialog() != DialogResult.OK) return;
            FileStream fs = new FileStream(ofd.FileName, FileMode.OpenOrCreate);
            
                string url = fs.Name.ToString();
                string name = ofd.SafeFileName.ToString();
                int lineCount = 0;
                int wordCount = 0;
                int charCount = 0;
            try {                           
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string content = sr.ReadToEnd();
                        richTextBox1.Text = content;
                        charCount = content.Length;
                        content = content.Replace("\r\n","\r");
                        lineCount = richTextBox1.Lines.Count();
                        content = content.Replace('\r', ' ');
                        string[] words = content.Split(new char[] { ' ', '.', '!', '?',':',';',',' }, StringSplitOptions.RemoveEmptyEntries);
                        wordCount = words.Count();
                    }

     
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            textBox1.Text = name;
            textBox2.Text = url;
            textBox3.Text = lineCount.ToString();
            textBox4.Text = wordCount.ToString();
            textBox5.Text = charCount.ToString();
            fs.Close();
            
        }
    }
}
