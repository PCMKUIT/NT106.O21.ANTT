using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace LAB2_NT106.O21.ANTT
{
   
    public partial class Form5 : Form
    {
        [Serializable]
        class Student
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public double Score1 { get; set; }
            public double Score2 { get; set; }
            public double DTB { get; set; }

            public Student(string id, string name, string phoneNumber, double score1, double score2)
            {
                ID = id;
                Name = name;
                PhoneNumber = phoneNumber;
                Score1 = score1;
                Score2 = score2;
            }
        }
        OpenFileDialog save;
        private bool fileRead = false;
        private List<Student> students = new List<Student>();
        public Form5()
        {
            InitializeComponent();
        }
        private void SerializeStudents(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, students);
                    fileRead = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        private List<Student> DeserializeStudents(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    return (List<Student>)bf.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                return null;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string content = textBox1.Text;
            if (string.IsNullOrWhiteSpace(content))
            {
                MessageBox.Show("Bạn chưa nhập dữ liệu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length % 5 != 0)
            {
                MessageBox.Show("Dữ liệu không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 2; i < lines.Length; i += 5)
            {
                if (!IsPhoneNumber(lines[i]))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!IsNumericInRange(lines[i + 1], 0, 10) || !IsNumericInRange(lines[i + 2], 0, 10))
                {
                    MessageBox.Show("Điểm toán hoặc điểm văn không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            students.Clear();

            for (int i = 0; i < lines.Length; i += 5)
            {
                students.Add(new Student(lines[i], lines[i + 1], lines[i + 2], double.Parse(lines[i + 3]), double.Parse(lines[i + 4])));
            }

            OpenFileDialog ofd = new OpenFileDialog();
            save = ofd;
            ofd.Filter = "Text files (*.txt)|*.txt";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                SerializeStudents(ofd.FileName);
                MessageBox.Show("Dữ liệu đã được ghi xuống file " + ofd.FileName, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private bool IsPhoneNumber(string phoneNumber)
        {
            return phoneNumber.Length == 10 && phoneNumber.All(char.IsDigit);
        }

        private bool IsNumericInRange(string input, double min, double max)
        {
            if (double.TryParse(input, out double value))
            {
                return value >= min && value <= max;
            }
            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!fileRead)
            {
                MessageBox.Show("Bạn chưa đọc dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            students = DeserializeStudents(save.FileName);
            if (students == null)
            {
                return;
            }

            richTextBox1.Clear();
            using (StreamWriter writer = new StreamWriter(openFileDialog.FileName))
            {
                foreach (var student in students)
                {
                    richTextBox1.AppendText(student.ID + "\n" + student.Name + "\n" + student.PhoneNumber + "\n" +
                                            student.Score1.ToString("0.0") + "\n" + student.Score2.ToString("0.0") + "\n");

                    double dtb = (student.Score1 + student.Score2) / 2.0;
                    student.DTB = dtb;
                    richTextBox1.AppendText(dtb.ToString("0.0")+"\n");

                    writer.WriteLine(student.ID);
                    writer.WriteLine(student.Name);
                    writer.WriteLine(student.PhoneNumber);
                    writer.WriteLine(student.Score1.ToString("0.0"));
                    writer.WriteLine(student.Score2.ToString("0.0"));
                    writer.WriteLine(dtb.ToString("0.0"));
                }
            }

            MessageBox.Show("Đã ghi dữ liệu vào file " + openFileDialog.FileName, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            richTextBox1.Clear();
            fileRead = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
