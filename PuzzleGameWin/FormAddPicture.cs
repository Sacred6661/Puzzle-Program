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

namespace PuzzleGameWin
{
    public partial class FormAddPicture : Form
    {
        private string filePath;
        private string nameOfPuzzle;

        public FormAddPicture()
        {
            InitializeComponent();
            label3.Visible = false;
            label4.Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();

            // filter of type
            d.Filter = "JPG |*.jpg|PNG|*.png|BMP|*.bmp";

            // dialog to choose image
            if (d.ShowDialog() == DialogResult.OK)
            {
                filePath = d.FileName;
                Bitmap picture = new Bitmap(filePath);
                if((picture.Width < 1200) || (picture.Height < 1200))
                {
                    label3.Visible = true;
                    return;
                }
                label3.Visible = false;
                Bitmap pictureToGet = new Bitmap(filePath);
                pictureBox1.Image = new Bitmap(pictureToGet, new Size(1200, 1200));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                label4.Visible = true;
                return;
            }
            nameOfPuzzle = textBox1.Text;
            //Create folder for Puzzle
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\" + nameOfPuzzle + "\\Pieces");
            try
            {
                //Saving picture to created folder
                pictureBox1.Image.Save(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\" + nameOfPuzzle + "\\" + nameOfPuzzle + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                //making pieces
                new PictureSlicer().slicePicture(nameOfPuzzle, 3);
                Application.Exit();
            }
            catch
            {
                MessageBox.Show("Can't save picture!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
