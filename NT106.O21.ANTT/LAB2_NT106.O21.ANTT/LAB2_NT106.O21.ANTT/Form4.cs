using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics;

namespace LAB2_NT106.O21.ANTT
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        private bool fileRead = false;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt";
            if (ofd.ShowDialog() != DialogResult.OK) return;
            FileStream fs = new FileStream(ofd.FileName, FileMode.OpenOrCreate);
            try
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string content = sr.ReadToEnd();
                    richTextBox1.Text = content;
                    fs.Close();
                    fileRead = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string RemoveSpaces(string input)
        {
            return Regex.Replace(input, @"\s+", "");
        }
        private string Process(string input)
        {
            string pattern = "-+";
            string output = Regex.Replace(input, pattern, match => match.Value.Length % 2 == 0 ? "+" : "-");
            return output;
        }
        private bool Check(string input)
        {
            input = Process(input);
            if (input.Length > 0 && input[0] == '+')
            {
                input = input.Substring(1);
            }
            return Regex.IsMatch(input, @"^[0-9\.+\-*\/]+$") && !Regex.IsMatch(input, @"\.\.+") && Regex.IsMatch(input, @"^-?\d+(\.\d+)?([+\-*\/]-?\d+(\.\d+)?)*$");
        }
        private double Solve(string expression)
        {
            bool Negative = false;
            expression = Process(expression);
            if (expression.Length > 0 && expression[0] == '+')
            {
                expression = expression.Substring(1);
            }
            if (expression.Length > 0 && expression[0] == '-')
            {
                expression = expression.Replace('-', 'x').Replace('+', '-').Replace('x', '+');
                Negative = true;
                expression = expression.Substring(1);
            }
            string[] elements = Regex.Split(expression, @"([+\-*/])");
            bool hasMultiplicationOrDivision = expression.Contains("*") || expression.Contains("/");
            if (!hasMultiplicationOrDivision)
            {
                double result = double.Parse(elements[0]);
                for (int i = 1; i < elements.Length; i += 2)
                {
                    string operation = elements[i];
                    double operand = double.Parse(elements[i + 1]);

                    switch (operation)
                    {
                        case "+":
                            result += operand;
                            break;
                        case "-":
                            result -= operand;
                            break;
                    }
                }
                if (!Negative)  return result;
                else
                {
                    result = - result;
                    return result;
                }
            }
            else
            {
                List<string> temp = new List<string>();
                double result = 0;
                double tmp;
                for (int i = 1; i < elements.Length-1; i+=2)
                {
                    string current = elements[i];
                    if (current == "*" || current == "/")
                    {
                        double operand1 = double.Parse(elements[i - 1]);
                        double operand2 = double.Parse(elements[i + 1]);
                        tmp = current == "*" ? operand1 * operand2 : operand1 / operand2;
                        if (i==1)
                            result += tmp;
                        else if (i !=1 && elements[i-2]=="+")
                            result += tmp;
                        else if (i != 1 && elements[i - 2] == "-")
                            result -= tmp;
                        else if (i != 1 && elements[i - 2] == "/")
                        { 
                            if (current == "/")
                                result = result/ double.Parse(elements[i - 1])/ double.Parse(elements[i + 1]);
                            if (current == "*")
                                result = result / double.Parse(elements[i - 1]) * double.Parse(elements[i + 1]);
                        }
                        else if (i != 1 && elements[i - 2] == "*")
                        {
                            if (current == "/")
                                result = result * double.Parse(elements[i - 1]) / double.Parse(elements[i + 1]);
                            if (current == "*")
                                result = result * double.Parse(elements[i - 1]) * double.Parse(elements[i + 1]);
                        }
                    }
                    else
                    {
                        if ((i==1)&&(elements[3] == "+" || elements[3] == "-"))
                        {
                                temp.Add(current);
                                temp.Add(elements[i + 1]);
                        }
                        if (i == 1) result += double.Parse(elements[0]);
                        else if ((i != elements.Length - 2 && elements[i + 2] == "*") || (i != elements.Length - 2 && elements[i + 2] == "/"))
                        {
                            continue;
                        }
                        else
                        {
                            temp.Add(current);
                            temp.Add(elements[i + 1]);
                        }
                    }
                }
                for (int i = 0; i < temp.Count; i += 2)
                {
                    string operation = temp[i];
                    double operand = double.Parse(temp[i + 1]);

                    switch (operation)
                    {
                        case "+":
                            result += operand;
                            break;
                        case "-":
                            result -= operand;
                            break;
                    }
                }

                if (!Negative) return result;
                else
                {
                    result = -result;
                    return result;
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
            string[] lines = richTextBox1.Lines;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt";
            if (ofd.ShowDialog() != DialogResult.OK) return;
            FileStream fs = new FileStream(ofd.FileName, FileMode.OpenOrCreate);
            try
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    foreach (string line in lines)
                    {
                        string formattedLine = RemoveSpaces(line);
                        if (Check(formattedLine))
                        {
                            double result = Solve(formattedLine);
                            writer.WriteLine($"{formattedLine}={result}");
                        }
                        else
                        {
                            writer.WriteLine("Syntax Error");
                        }
                    }
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
