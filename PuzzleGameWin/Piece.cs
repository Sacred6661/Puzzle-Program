using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleGameWin
{
    class Piece
    {
        private int id ;
        private PictureBox piecePicBox;
        private Bitmap realImage;
        private int rotatePosition;

        public Piece(int id, PictureBox piecePicBox)
        {
            this.id = id;
            this.piecePicBox = piecePicBox;
            rotatePosition = 0;
        }

        public int getId() { return this.id; }
        public void setId(int id) { this.id = id; }

        public PictureBox getPiecePicBox() { return this.piecePicBox; }
        public void setBieceOPicBox(PictureBox piecebm) { this.piecePicBox = piecebm; }

        public Image getRealImage() { return this.realImage; }

        public int getRotatePosition() { return this.rotatePosition; }

        //Functionn to rotate picture
        public void rotatePicBox(int rotatePos)
        {
            this.rotatePosition = rotatePos;
            if (rotatePosition == 1)
                piecePicBox.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            else if(rotatePosition == 2)
                piecePicBox.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
            else if (rotatePosition == 3)
                piecePicBox.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
        }

        //randomly rotate picture box
        public void randomRotatePicBox()
        {
            this.realImage = new Bitmap(piecePicBox.Image);
            int random = new Random().Next(4);
            rotatePicBox(random);
        }

        public void rotate90Degree()
        {
            piecePicBox.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            rotatePosition++;
            if (rotatePosition >= 4)
                rotatePosition = 0;
        }
    }
}
