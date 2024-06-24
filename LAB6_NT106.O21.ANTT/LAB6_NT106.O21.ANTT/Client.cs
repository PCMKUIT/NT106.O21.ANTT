using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace LAB6_NT106.O21.ANTT
{
    public partial class Client : Form
    {
        private TcpClient clientSocket = new TcpClient();
        private NetworkStream serverStream;
        private Thread clientThread;
        private bool isDrawing = true;
        private bool isEraser = false;
        private bool execute = false;
        private Point previousPoint;
        private Pen currentPen;
        private Random random;
        private Bitmap drawingBitmap;
        private bool isDragging = false;
        private Point dragStartPoint;
        private Rectangle originalImageBounds;
        private Rectangle currentImageBounds;
        private bool isRunning = true;

        public Client()
        {
            InitializeComponent();
            currentPen = new Pen(Color.Black, 2);
            radioButton2.Checked = true;

            panel1.MouseDown += new MouseEventHandler(panel1_MouseDown);
            panel1.MouseMove += new MouseEventHandler(panel1_MouseMove);
            panel1.MouseUp += new MouseEventHandler(panel1_MouseUp);

            radioButton1.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            radioButton2.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            radioButton3.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            radioButton4.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            radioButton5.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

            drawingBitmap = new Bitmap(panel1.Width, panel1.Height);

            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                clientSocket.Connect("127.0.0.1", 8080); 
                serverStream = clientSocket.GetStream();
                clientThread = new Thread(GetServerResponse)
                {
                    IsBackground = true 
                };
                clientThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not connect to server: " + ex.Message);
            }
        }

        private void GetServerResponse()
        {
            while (isRunning)
            {
                try
                {
                    byte[] inStream = new byte[clientSocket.ReceiveBufferSize];
                    int bytesRead = serverStream.Read(inStream, 0, inStream.Length);
                    if (bytesRead > 0)
                    {
                        MemoryStream ms = new MemoryStream(inStream, 0, bytesRead);
                        BinaryReader br = new BinaryReader(ms);

                        int startX, startY, endX, endY;
                        Color color;
                        float width;

                        try
                        {
                            startX = br.ReadInt32();
                            startY = br.ReadInt32();
                            endX = br.ReadInt32();
                            endY = br.ReadInt32();
                            color = Color.FromArgb(br.ReadInt32());
                            width = br.ReadSingle();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error reading draw data: {ex.Message}");
                            continue; 
                        }

                        Pen pen = new Pen(color, width);

                        Invoke(new Action(() =>
                        {
                            try
                            {
                                using (Graphics g = Graphics.FromImage(drawingBitmap))
                                {
                                    g.DrawLine(pen, new Point(startX, startY), new Point(endX, endY));
                                }
                                panel1.Invalidate();
                            }
                            catch (OverflowException ex)
                            {
                                Console.WriteLine($"Overflow error in drawing line: {ex.Message}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error drawing line: {ex.Message}");
                            }
                        }));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GetServerResponse: {ex.Message}");
                    if (!isRunning) break; 
                }
            }
        }

        private void SendDrawData(Point start, Point end)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(start.X);
            bw.Write(start.Y);
            bw.Write(end.X);
            bw.Write(end.Y);
            bw.Write(currentPen.Color.ToArgb());
            bw.Write(currentPen.Width);
            byte[] data = ms.ToArray();
            serverStream.Write(data, 0, data.Length);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            execute = true;

            if (isDrawing)
            {
                previousPoint = e.Location;
            }
            else if (isEraser)
            {
                using (Graphics g = Graphics.FromImage(drawingBitmap))
                {
                    int eraserSize = GetEraserSize();
                    g.FillEllipse(Brushes.White, e.X - eraserSize / 2, e.Y - eraserSize / 2, eraserSize, eraserSize);
                }

                SendEraseData(e.Location);
            }

            if (e.Button == MouseButtons.Left)
            {
                if (currentImageBounds.Contains(e.Location))
                {
                    isDragging = true;
                    dragStartPoint = e.Location;
                    originalImageBounds = currentImageBounds;
                }
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (execute)
            {
                using (Graphics g = Graphics.FromImage(drawingBitmap))
                {
                    if (isDrawing)
                    {
                        g.DrawLine(currentPen, previousPoint, e.Location);
                        SendDrawData(previousPoint, e.Location);
                    }
                    else if (isEraser)
                    {
                        int eraserSize = GetEraserSize();
                        g.FillEllipse(Brushes.White, e.X - eraserSize / 2, e.Y - eraserSize / 2, eraserSize, eraserSize);
                        SendEraseData(e.Location);
                    }
                }
                previousPoint = e.Location;

                panel1.Invalidate();
            }

            if (isDragging)
            {
                int deltaX = e.X - dragStartPoint.X;
                int deltaY = e.Y - dragStartPoint.Y;

                currentImageBounds.X = originalImageBounds.X + deltaX;
                currentImageBounds.Y = originalImageBounds.Y + deltaY;

                panel1.Invalidate();
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            execute = false;
            isDragging = false;
        }

        private int GetEraserSize()
        {
            if (radioButton1.Checked)
            {
                return 5;
            }
            else if (radioButton2.Checked)
            {
                return 10;
            }
            else if (radioButton3.Checked)
            {
                return 15;
            }
            else if (radioButton4.Checked)
            {
                return 20;
            }
            else if (radioButton5.Checked)
            {
                return 25;
            }
            else
            {
                return 10; 
            }
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                currentPen.Width = 1;
            }
            else if (radioButton2.Checked)
            {
                currentPen.Width = 2;
            }
            else if (radioButton3.Checked)
            {
                currentPen.Width = 3;
            }
            else if (radioButton4.Checked)
            {
                currentPen.Width = 4;
            }
            else if (radioButton5.Checked)
            {
                currentPen.Width = 5;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            using (ColorDialog cd = new ColorDialog())
            {
                cd.AllowFullOpen = true;
                cd.AnyColor = true;
                cd.SolidColorOnly = false;
                cd.FullOpen = true;

                if (cd.ShowDialog() == DialogResult.OK)
                {
                    currentPen.Color = cd.Color;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            isDrawing = false;
            isEraser = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            isDrawing = true;
            isEraser = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete everything?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                drawingBitmap = new Bitmap(panel1.Width, panel1.Height);
                panel1.Invalidate();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            random = new Random();
            int shapeType = random.Next(1, 6);

            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);

            Color randomColor = Color.FromArgb(red, green, blue);
            switch (shapeType)
            {
                case 1:
                    DrawCircle(randomColor);
                    SendRandomShape(1, randomColor);
                    break;
                case 2:
                    DrawTriangle(randomColor);
                    SendRandomShape(2, randomColor);
                    break;
                case 3:
                    DrawSquare(randomColor);
                    SendRandomShape(3, randomColor);
                    break;
                case 4:
                    DrawDiamond(randomColor);
                    SendRandomShape(4, randomColor);
                    break;
                case 5:
                    DrawCrossLines(randomColor);
                    SendRandomShape(5, randomColor);
                    break;
                default:
                    break;
            }

            using (Graphics g = Graphics.FromImage(drawingBitmap))
            {
                switch (shapeType)
                {
                    case 1:
                        DrawCircle(randomColor, g);
                        break;
                    case 2:
                        DrawTriangle(randomColor, g);
                        break;
                    case 3:
                        DrawSquare(randomColor, g);
                        break;
                    case 4:
                        DrawDiamond(randomColor, g);
                        break;
                    case 5:
                        DrawCrossLines(randomColor, g);
                        break;
                    default:
                        break;
                }
            }

            panel1.Invalidate();
        }
        private void DrawCircle(Color color, Graphics g = null)
        {
            if (g == null)
            {
                g = panel1.CreateGraphics();
            }

            int centerX = panel1.Width / 2;
            int centerY = panel1.Height / 2;
            int radius = 50;

            using (Brush brush = new SolidBrush(color))
            {
                g.FillEllipse(brush, centerX - radius, centerY - radius, 2 * radius, 2 * radius);
            }
        }

        private void DrawTriangle(Color color, Graphics g = null)
        {
            if (g == null)
            {
                g = panel1.CreateGraphics();
            }

            int centerX = panel1.Width / 2;
            int centerY = panel1.Height / 2;

            Point[] points = new Point[3];
            points[0] = new Point(centerX, centerY - 50);
            points[1] = new Point(centerX - 50, centerY + 50);
            points[2] = new Point(centerX + 50, centerY + 50);

            using (Brush brush = new SolidBrush(color))
            {
                g.FillPolygon(brush, points);
            }
        }
        private void DrawCircle(Color color)
        {
            using (Graphics g = panel1.CreateGraphics())
            {
                int centerX = panel1.Width / 2;
                int centerY = panel1.Height / 2;
                int radius = 50;

                using (Brush brush = new SolidBrush(color))
                {
                    g.FillEllipse(brush, centerX - radius, centerY - radius, 2 * radius, 2 * radius);
                }
            }
        }
        private void DrawSquare(Color color, Graphics g = null)
        {
            if (g == null)
            {
                g = panel1.CreateGraphics();
            }

            int centerX = panel1.Width / 2;
            int centerY = panel1.Height / 2;
            int sideLength = 100;

            using (Brush brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, centerX - sideLength / 2, centerY - sideLength / 2, sideLength, sideLength);
            }
        }
        private void DrawDiamond(Color color, Graphics g = null)
        {
            if (g == null)
            {
                g = panel1.CreateGraphics();
            }

            int centerX = panel1.Width / 2;
            int centerY = panel1.Height / 2;
            int width = 100;
            int height = 100;

            Point[] points = new Point[4];
            points[0] = new Point(centerX, centerY - height / 2);
            points[1] = new Point(centerX - width / 2, centerY);
            points[2] = new Point(centerX, centerY + height / 2);
            points[3] = new Point(centerX + width / 2, centerY);

            using (Brush brush = new SolidBrush(color))
            {
                g.FillPolygon(brush, points);
            }
        }
        private void DrawCrossLines(Color color, Graphics g = null)
        {
            if (g == null)
            {
                g = panel1.CreateGraphics();
            }

            int centerX = panel1.Width / 2;
            int centerY = panel1.Height / 2;
            int lineLength = 50;

            using (Pen pen = new Pen(color))
            {
                g.DrawLine(pen, centerX - lineLength, centerY - lineLength, centerX + lineLength, centerY + lineLength);
                g.DrawLine(pen, centerX - lineLength, centerY + lineLength, centerX + lineLength, centerY - lineLength);
            }
        }

        private void DrawTriangle(Color color)
        {
            using (Graphics g = panel1.CreateGraphics())
            {
                int centerX = panel1.Width / 2;
                int centerY = panel1.Height / 2;

                Point[] points = new Point[3];
                points[0] = new Point(centerX, centerY - 50); 
                points[1] = new Point(centerX - 50, centerY + 50); 
                points[2] = new Point(centerX + 50, centerY + 50); 

                using (Brush brush = new SolidBrush(color))
                {
                    g.FillPolygon(brush, points);
                }
            }
        }

        private void DrawSquare(Color color)
        {
            using (Graphics g = panel1.CreateGraphics())
            {
                int centerX = panel1.Width / 2;
                int centerY = panel1.Height / 2;
                int sideLength = 100; 

                using (Brush brush = new SolidBrush(color))
                {
                    g.FillRectangle(brush, centerX - sideLength / 2, centerY - sideLength / 2, sideLength, sideLength);
                }
            }
        }

        private void DrawDiamond(Color color)
        {
            using (Graphics g = panel1.CreateGraphics())
            {
                int centerX = panel1.Width / 2;
                int centerY = panel1.Height / 2;
                int width = 100; 
                int height = 100; 

                Point[] points = new Point[4];
                points[0] = new Point(centerX, centerY - height / 2);
                points[1] = new Point(centerX - width / 2, centerY);
                points[2] = new Point(centerX, centerY + height / 2);
                points[3] = new Point(centerX + width / 2, centerY);

                using (Brush brush = new SolidBrush(color))
                {
                    g.FillPolygon(brush, points);
                }
            }
        }

        private void DrawCrossLines(Color color)
        {
            using (Graphics g = panel1.CreateGraphics())
            {
                int centerX = panel1.Width / 2;
                int centerY = panel1.Height / 2;
                int lineLength = 50; 

                using (Pen pen = new Pen(color))
                {
                    g.DrawLine(pen, centerX - lineLength, centerY, centerX + lineLength, centerY);
                    g.DrawLine(pen, centerX, centerY - lineLength, centerX, centerY + lineLength);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveDrawing();
            System.Windows.Forms.Application.Exit();
        }


        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.gif, *.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string imagePath = openFileDialog.FileName;
                InsertImage(imagePath);
            }
        }

        private void SaveDrawing()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PNG Image|*.png";
                saveFileDialog.Title = "Lưu bản vẽ";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // Đặt thư mục ban đầu

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Bitmap bmp = new Bitmap(panel1.Width, panel1.Height);
                        panel1.DrawToBitmap(bmp, new Rectangle(0, 0, panel1.Width, panel1.Height));
                        bmp.Save(saveFileDialog.FileName, ImageFormat.Png);

                        MessageBox.Show($"Ảnh đã được lưu thành công tại {saveFileDialog.FileName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            bmp.Save(ms, ImageFormat.Png); 
                            byte[] imageBytes = ms.ToArray();
                            SendImage(imageBytes);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lưu ảnh thất bại: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void InsertImage(string imagePath)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath);

            using (Graphics g = Graphics.FromImage(drawingBitmap))
            {
                g.DrawImage(image, new Rectangle(0, 0, panel1.Width, panel1.Height));
            }

            panel1.BackgroundImage = image;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;

            panel1.Invalidate();

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png); 
                byte[] imageBytes = ms.ToArray();
                SendImage(imageBytes);
            }
        }

        private void SendImage(byte[] imageBytes)
        {
            try
            {
                byte[] lengthBytes = BitConverter.GetBytes(imageBytes.Length);
                serverStream.Write(lengthBytes, 0, lengthBytes.Length);

                serverStream.Write(imageBytes, 0, imageBytes.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending image data to server: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SendEraseData(Point location)
        {
            try
            {
                byte[] xBytes = BitConverter.GetBytes(location.X);
                byte[] yBytes = BitConverter.GetBytes(location.Y);

                serverStream.Write(xBytes, 0, xBytes.Length);
                serverStream.Write(yBytes, 0, yBytes.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending erase data to server: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendRandomShape(int shapeType, Color color)
        {
            try
            {
                BinaryWriter bw = new BinaryWriter(serverStream);
                bw.Write(shapeType);
                bw.Write(color.ToArgb());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending random shape data to server: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (drawingBitmap != null)
            {
                e.Graphics.DrawImage(drawingBitmap, new Rectangle(0, 0, panel1.Width, panel1.Height));
            }
        }
 

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                isRunning = false; 
                if (serverStream != null)
                {
                    serverStream.Close();
                    serverStream.Dispose(); 
                }

                if (clientSocket != null)
                {
                    clientSocket.Close(); 
                    clientSocket.Dispose(); 
                }

                if (clientThread != null && clientThread.IsAlive)
                {
                    clientThread.Join(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error closing client: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                base.OnFormClosing(e);
            }
        }
    }
}
