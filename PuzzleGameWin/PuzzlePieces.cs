using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleGameWin
{
    class PuzzlePieces
    {
        private List<Piece> listOfPieces = new List<Piece>();
        private FlowLayoutPanel layout;
        private PuzzleBoard board;
        private Point startMouseDown;

        public PuzzlePieces(FlowLayoutPanel layout, PuzzleBoard board)
        {
            this.layout = layout;
            this.board = board;
        }


        //function to take all pictures from folder
        public void takePicture(string puzzleName)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\" + puzzleName + "\\Pieces\\";
            var images = Directory.GetFiles(path, "*.jpg");                                    //take all jpg pieces
            foreach (var image in images)
            {
                //making of new PictureBox to every image
                PictureBox pb = new PictureBox();
                pb.Image = new Bitmap(image);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;                                  // stretch the image
                pb.Height = 130;
                pb.Width = 130;
                pb.MouseDown += new MouseEventHandler(piece_MouseDown);
                pb.MouseMove += new MouseEventHandler(piece_MouseMove);
                pb.MouseDoubleClick += new MouseEventHandler(piece_DoubleCLick);

                string id = Regex.Match(Path.GetFileName(image), @"\d+").Value;                 //get id from name of file(only numeric)
                listOfPieces.Add(new Piece(Int32.Parse(id), pb));
            }
        }

        //getting images to the layout(and display)
        public void addImagesToLayout()
        {
            foreach (Piece piece in listOfPieces) {
                piece.randomRotatePicBox();
                layout.Controls.Add(piece.getPiecePicBox());
             }
        }


        public void makeListRandom()
        {
            Random random = new Random();
            int n = listOfPieces.Count;

            while(n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Piece value = listOfPieces[k];
                listOfPieces[k] = listOfPieces[n];
                listOfPieces[n] = value;
            }
        }

        //getting images to the layout 
        public void addImagesRandomToLayout()
        {
            makeListRandom();
            addImagesToLayout();
        }

        //return searching piece if it exist or null - if not
        public Piece getListOfPiecesElement(Piece searchingPiece)
        {
            foreach (Piece piece in listOfPieces)
                if (piece == searchingPiece)
                    return piece;

            return null;
        }

        //Event to rotate image by double clicking
        void piece_DoubleCLick(object sender, MouseEventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            p.Image = RotateImage(p.Image);
        }

        //Mouse down event
        void piece_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                startMouseDown = e.Location;
        }

        //Mouse Move event. This function help to have DragDrop effect only if we move Mouse
        void piece_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                //see if mouse was move for some pixels, if yes - make DragDrop
                if (Math.Abs(e.Location.X - startMouseDown.X) + Math.Abs(e.Location.Y - startMouseDown.Y) > 3)
                {
                    PictureBox p = (PictureBox)sender;
                    p.DoDragDrop(p.Image, DragDropEffects.Copy);

                    if (chekOnBoard(p))
                        removeFromLayout(p);
                }
        }

        //function to find piece in List by PictureBox
        public Piece findPieceByPicBox(PictureBox searchPicBox)
        {
            foreach(Piece piece in listOfPieces)
            {
                if (piece.getPiecePicBox() == searchPicBox) return piece;
            }

            return null;
        }

        //function to remove picture from layout(it will still be in listOfPieces)
        public void removeFromLayout(PictureBox boxToRemove)
        {
            PictureBox temp = null; ;
            foreach (Piece piece in listOfPieces)
            {
                if (piece.getPiecePicBox().Image == boxToRemove.Image)
                {
                    temp = boxToRemove;
                    break;
                }
            }
            layout.Controls.Remove(temp);           //deleting picture box from layout
        }

        //cheking if PictureBox exist on board
        public bool chekOnBoard(PictureBox chekingPicBox)
        {
            List<Piece> boardParts = board.getListOfParts();
            foreach (Piece boardPiece in boardParts)
            {
                if (chekingPicBox.Image == boardPiece.getPiecePicBox().Image)
                    return true;
            }
            return false;
        }

        public List<Piece> getListOfPiece()
        {
            return this.listOfPieces;
        }

        //find and return piece by ID
        public Piece findPieceById(int id)
        {
            foreach (Piece piece in listOfPieces)
                if (piece.getId() == id)
                    return piece;

            return null;
        }

        //function to rotate image
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


    }
}
