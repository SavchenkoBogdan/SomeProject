using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Label a = new Label();
        private int radius = 25;
        private string fileName;
        public Form1()
        {
            InitializeComponent();
            
            a.Size = new Size(100, 100);
            a.Location = new Point(50, 50);
            //a.Text = "LOL";
            Controls.Add(a);
            //DrawSomething(a);
        }

        private void DrawSomething()
        {
            if (string.IsNullOrEmpty(fileName))
                return;
            //Bitmap bmp = new Bitmap("test.jpg");
            Bitmap bmp = new Bitmap(fileName);
            bmp.MakeTransparent();
            var centerX = (int)(bmp.Width / 2);
            var centerY = (int)(bmp.Height / 2);

            /*
            var angle = 0f;
            for (int i = 0; i < 500; i++)
            {
                angle += 0.1f;
                //var x = 0 + i;// + (int)(Math.Cos(angle) * 50);
                var x = centerX + (int)(Math.Cos(angle) * 10);
                var y = centerY + (int)(Math.Sin(angle) * 10);
                x = (x + i / 4) % 500;
                bmp.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0));
            }
            */

            for (int i = 0; i < bmp.Width; i++)
                for (int j = 0; j < bmp.Height; j++)
                {
                    var dx = centerX - i;
                    var dy = centerY - j;
                    var distance = Math.Sqrt(dx * dx + dy * dy);
                    try
                    {
                        if (distance > radius)
                            bmp.SetPixel(i, j, Color.FromArgb(0, 0, 0, 0));
                    }
                    catch (Exception e)
                    {

                    }
                }

            var cropX = Clamp(centerX - radius, 0, bmp.Width);
            var cropY = Clamp(centerY - radius, 0, bmp.Height);
            var cropWidth = Clamp(radius * 2, 0, bmp.Width);
            var cropHeight = Clamp(radius * 2, 0, bmp.Height);
            var cropRectangle = new Rectangle(cropX, cropY, cropWidth, cropHeight);
            Bitmap newBitmap = bmp.Clone(cropRectangle, bmp.PixelFormat);
            a.Size = new Size(newBitmap.Width, newBitmap.Height);
            a.Image = newBitmap;
        }

        private T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            return value.CompareTo(min) < 0 ? min : value.CompareTo(max) > 0 ? max : value;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = true;
            var userClickedOK = openFileDialog1.ShowDialog();
            if (userClickedOK == DialogResult.OK)
            {
                fileName = openFileDialog1.FileName;
                DrawSomething();
                System.IO.Stream fileStream = openFileDialog1.OpenFile();

                using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                {
                    // Read the first line from the file and write it the textbox.
                    //tbResults.Text = reader.ReadLine();
                }
                fileStream.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            radius = int.Parse(textBox1.Text);
            DrawSomething();
        }
    }
}
