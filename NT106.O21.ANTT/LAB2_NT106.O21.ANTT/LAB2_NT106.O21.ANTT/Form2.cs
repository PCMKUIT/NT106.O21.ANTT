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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private bool fileRead = false;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(ofd.FileName).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    FileStream fs = new FileStream(ofd.FileName, FileMode.OpenOrCreate);
                    StreamReader sr = new StreamReader(fs);
                    string content = sr.ReadToEnd();
                    richTextBox1.Text = content;
                    fs.Close();
                    fileRead = true;
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!fileRead) 
            {
                MessageBox.Show("Bạn chưa đọc file", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
            string data = richTextBox1.Text;
            string uppercaseData = data.ToUpper();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt";
            if (ofd.ShowDialog() != DialogResult.OK) return;
            FileStream fs = new FileStream(ofd.FileName, FileMode.OpenOrCreate);
            try
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(uppercaseData);
                }
                MessageBox.Show("Dữ liệu đã được ghi xuống file " + ofd.FileName, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
        
    

    

