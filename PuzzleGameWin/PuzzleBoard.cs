using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleGameWin
{
    class PuzzleBoard
    {
        public List<Piece> listOfParts = new List<Piece>();
        private FlowLayoutPanel layout;
        private bool dropState = false;
        PictureBox pbControl = null;
        Image pbBufferSwap = null;
        private Point startMouseDown;

        public PuzzleBoard(FlowLayoutPanel layout)
        {
            this.layout = layout;
        }

        //making puzzle board 
        public void makeBoard(int count)
        {
            for (int i = 0; i < count; i++)
            {
                PictureBox pb = new PictureBox();
                pb.SizeMode = PictureBoxSizeMode.StretchImage;              // stretch the image
                pb.Height = 130;
                pb.Width = 130;
                pb.BackColor = Color.Red;
                pb.AllowDrop = true;
                //event to drag drop
                pb.DragDrop += boardPiece_DragDrop;
                pb.DragEnter += boardPiece_DragEnter;
                //Two events to see if object(PictureBox) is only cliked by mouse down, or if it dragged
                pb.MouseMove += new MouseEventHandler(boardPiece_MouseMove);
                pb.MouseDown += new MouseEventHandler(boardPiece_MouseDown);

                pb.MouseDoubleClick += new MouseEventHandler(boardPiece_DoubleCLick);

                //adding new element to layout
                layout.Controls.Add(pb);
                listOfParts.Add(new Piece(i, pb));
            }
        }

        //Function to event DragDrog
        private void boardPiece_DragDrop(object sender, DragEventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            Image temp = (Image)e.Data.GetData(DataFormats.Bitmap);
            pbControl = null;
            //cheking if image, that was dragged exist, if not exist(null) - cheking if we don't try to add this piece in the cell with image
            //if try - exit this function
            if ((findPieceByImage(temp) == null))
            {
                if (p.Image != null)
                    return;
            }
                pbControl = p;                          //variable to control on MouseDown if Drag drop was made, or we give element back
                //pbBuffer is variable to know, if we have to swap images of two PictureBoxes.
                //If image in dragdropped PB exist, we will give him in this variable, to swap in Mouse Down              
                pbBufferSwap = null;                    
                if (p.Image != null && (p.Image != (Image)e.Data.GetData(DataFormats.Bitmap)))
                    pbBufferSwap = p.Image;
                p.Image = (Image)e.Data.GetData(DataFormats.Bitmap);
                dropState = true; 
        }

        //DragEnter event
        private void boardPiece_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        public List<Piece> getListOfParts() { return listOfParts; }

        //Mouse down event
        void boardPiece_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                startMouseDown = e.Location;
        }

        //Mouse Move event. This function help two have DragDrop effect only if we move Mouse
        void boardPiece_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                //see if mouse was move for some pixels, if yes - make DragDrop
                if (Math.Abs(e.Location.X - startMouseDown.X) + Math.Abs(e.Location.Y - startMouseDown.Y) > 3)
                {
                    PictureBox p = (PictureBox)sender;
                    pbBufferSwap = null;
                    //we cant't to swap empty cell(with null Image)
                    if (p.Image != null)
                    {
                        pbControl = null;                               //make control variable empty(null)
                        p.DoDragDrop(p.Image, DragDropEffects.Copy);
                    }

                    //we have to make cell on board empty only if we take one the from board(with Image) to another(without it)
                    //and we can take it, but not drop and take back to begining cell
                    //so we chek,  if DragDrop was made(dropState)
                    //if we dont take back it(if take - we haven't dalete enything)
                    //and if we make swap - we haven't delete enythyng
                    if (dropState && (pbControl != p) && (pbControl != null) && (pbBufferSwap == null))
                        p.Image = null;
                    //if bBufferSwap != null it means, that we try to take Image from one cell to another. We change dropped cell's image
                    //so to swap, we have to change Image to this cell(cell that was taken) to make swap
                    else if (pbBufferSwap != null)
                        p.Image = pbBufferSwap;

                     dropState = false;
                }
         }

        void boardPiece_DoubleCLick(object sender, MouseEventArgs e)
        {
            PictureBox p = (PictureBox)sender;

            p.Image = RotateImage(p.Image);
        }

        //function to find piece only by image(return Piece)
        public Piece findPieceByImage(Image image)
        {
            foreach(Piece piece in listOfParts)
            {
                if (piece.getPiecePicBox().Image == image)
                    return piece;
            }

            return null;
        }

        //function to chek, if Image was stitched
        public bool chekFinish(PuzzlePieces piecesList)
        {
            //if there is one or more empty PictureBoxes(cells) - it's not finished
            foreach (Piece piece in listOfParts)
                if (piece.getPiecePicBox() == null)
                    return false;

            if (listOfParts == null) return false;
            for (int i = 0; i < listOfParts.Count; i++)
                if (!compareImages(listOfParts[i].getPiecePicBox().Image, piecesList.findPieceById(i).getRealImage()))
                    return false;

            return true;
        }

        //rotating for 90 degree
        public Image RotateImage(Image img)
        {
            var bmp = new Bitmap(img);

            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.Clear(Color.White);
                gfx.DrawImage(img, 0, 0, img.Width, img.Height);
            }

            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            return bmp;
        }

        //function to get hash from Image to compare two images
        public List<bool> getImageHash(Image bmpSource)
        {
            if (bmpSource == null) return null;
            List<bool> lResult = new List<bool>();
            //create new image with 16x16 pixel
            if (bmpSource == null) return null;

            //making a little picture
            Bitmap bmpMin = new Bitmap(bmpSource, new Size(16, 16));
            for (int j = 0; j < bmpMin.Height; j++)
            {
                for (int i = 0; i < bmpMin.Width; i++)
                {
                    //reduce colors to true / false                
                    lResult.Add(bmpMin.GetPixel(i, j).GetBrightness() < 0.5f);      //it helps to get only black and white(1 and 0) in list
                }
            }
            return lResult;
        }

        public bool compareImages(Image pic1, Image pic2)
        {
            List<bool> firsPicHash = getImageHash(pic1);
            List<bool> secondPicHash = getImageHash(pic2);

            if ((firsPicHash == null) || (secondPicHash == null)) return false;

            for(int i = 0; i < firsPicHash.Count; i++)
            {
                if (firsPicHash[i] != secondPicHash[i])
                    return false;
            }

            return true;
        }


    }
}
