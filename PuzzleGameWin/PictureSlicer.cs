using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGameWin
{
    class PictureSlicer
    {
        public void slicePicture(String gameName, int size)
        {
            int x = 0, y = 0;
            int index = 0;
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\" + gameName + "\\" + gameName + ".jpg";
            string writtingPath = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\" + gameName + "\\Pieces\\";

            Bitmap orignal = new Bitmap(path);

            Bitmap newimage = new Bitmap((int)(orignal.Width / size), (int)(orignal.Height / size));
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Graphics newgraphics = Graphics.FromImage(newimage);
                    newgraphics.DrawImage(orignal, 0, 0, new System.Drawing.Rectangle(x, y, newimage.Height, newimage.Width), GraphicsUnit.Pixel);
                    newgraphics.Flush();

                    newimage.Save(new System.IO.FileInfo(writtingPath) + "out" + index + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    x += newimage.Height;
                    index++;
                }
                y += newimage.Width;
                x = 0;
            }
        }
    }
}
