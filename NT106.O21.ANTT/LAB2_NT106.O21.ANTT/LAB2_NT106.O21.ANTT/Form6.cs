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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
            listView1.Columns.Add("Tên file");
            listView1.Columns.Add("Kích thước");
            listView1.Columns.Add("Đuôi mở rộng");
            listView1.Columns.Add("Ngày tạo");
            listView1.View = View.Details;
        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }
        private void LoadFiles(string folderPath)
        {
            listView1.Items.Clear();

            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            if (directoryInfo.Exists)
            {
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    ListViewItem item = new ListViewItem(file.Name);
                    item.SubItems.Add(file.Length.ToString());
                    item.SubItems.Add(file.Extension);
                    item.SubItems.Add(file.CreationTime.ToString());
                    listView1.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("Thư mục không tồn tại.");
            }
            foreach (ColumnHeader column in listView1.Columns)
            {
                if (column.Text == "Tên file" || column.Text == "Ngày tạo")
                {
                    column.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
                else
                {
                    column.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                string selectedFolder = fbd.SelectedPath;
                LoadFiles(selectedFolder);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
