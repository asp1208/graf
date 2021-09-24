using System;
using System.Drawing;
using System.Windows.Forms;

namespace lab2
{
    public partial class Form1 : Form
    {
        Bitmap picture;
        Bitmap newPicture;

        public Form1()
        {
            InitializeComponent();
        }

        (double H, double S, double V) RGB_to_HSV(double R, double G, double B)
        {
            double max = Math.Max(Math.Max(R, G), B) * 1.0;
            double min = Math.Min(Math.Min(R, G), B) * 1.0;

            double H = 0.0;
            double S = 0.0;
            double V = max;
            if (!max.Equals(0.0))
            {
                S = 1 - min / max;
            }
            if (max.Equals(R) && G >= B)
            {
                H = 60 * (G - B) / (max - min);
            }
            else if (max.Equals(R) && G < B)
            {
                H = 60 * (G - B) / (max - min) + 360;
            }
            else if (max.Equals(G))
            {
                H = 60 * (B - R) / (max - min) + 120;
            }
            else
            {
                H = 60 * (R - G) / (max - min) + 240;
            }
            return (H, S, V);
        }
        (double R, double G, double B) HSV_to_RGB(double H, double S, double V)
        {
            double hi = Math.Floor(H / 60.0) % 6;
            double a = H / 60.0 - Math.Floor(H / 60.0);
            double Vmin = V * (1 - S) * 1.0;
            double Vdec = V * (1 - a * S) * 1.0;
            double Vink = V * (1 - (1 - a) * S) * 1.0;

            switch (hi)
            {
                case 0:
                    return (V, Vink, Vmin);
                case 1:
                    return (Vdec, V, Vmin);
                case 2:
                    return (Vmin, V, Vink);
                case 3:
                    return (Vmin, Vdec, V);
                case 4:
                    return (Vink, Vmin, V);
                case 5:
                    return (V, Vmin, Vdec);
                default:
                    return (0, 0, 0);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();// создаем диалоговое окно
            if (openFile.ShowDialog() == DialogResult.OK) // открываем окно
            {
                string FileName = openFile.FileName;
                picture = new Bitmap(Image.FromFile(FileName));
                pictureBox1.Image = picture;
                newPicture = (Bitmap)picture.Clone();
                pictureBox1.Image = newPicture;

                //даем возможность взаимодейсвовать
                trackBar_Hue.Enabled = true;
                trackBar_Saturation.Enabled = true;
                trackBar_Value.Enabled = true;
            }
        }
        void Convert(double H, double S, double V)
        {
            for (int i = 0; i < newPicture.Width; ++i)
                for (int j = 0; j < newPicture.Height; ++j)
                {

                    var pixel = newPicture.GetPixel(i, j);
                    var result = RGB_to_HSV(pixel.R / 255.0, pixel.G / 255.0, pixel.B / 255.0);
                    var newH = result.H;
                    var newS = result.S;
                    var newV = result.V;


                    newH += H;
                    if (newH > 360)
                        newH -= 360;
                    if (newH < 0)
                        newH += 360;

                    newS += S;
                    newS = Math.Min(1, newS);
                    newS = Math.Max(0, newS);

                    newV += V;
                    newV = Math.Min(1, newV);
                    newV = Math.Max(0, newV);

                    var newRGB = HSV_to_RGB(newH, newS, newV);
                    newPicture.SetPixel(i, j, Color.FromArgb(pixel.A, (int)(newRGB.R * 255.0),
                        (int)(newRGB.G * 255.0), (int)(newRGB.B * 255.0)));
                }
            pictureBox1.Image = newPicture;
        }


        double current_H = 0;
        double current_S = 0;
        double current_V = 0;
        private void trackBar_Saturation_Scroll(object sender, EventArgs e)
        {

            current_S = trackBar_Saturation.Value / 100.0;
            Convert(current_H, current_S, current_V);
        }
        private void trackBar_Hue_Scroll(object sender, EventArgs e)
        {
            current_H = trackBar_Hue.Value * 1.8;
            Convert(current_H, current_S, current_V);
        }
        private void trackBar_Value_Scroll(object sender, EventArgs e)
        {
            current_V = trackBar_Value.Value / 100.0;
            Convert(current_H, current_S, current_V);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var FileName = new FolderBrowserDialog();
            var path = "";
            if (FileName.ShowDialog() == DialogResult.OK)
            {
                path = FileName.SelectedPath;
            }
            newPicture.Save(path + "\\_new.jpg");
        }
        private void button2_Click_1(object sender, EventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }

    }
}
