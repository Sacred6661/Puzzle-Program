using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleGameWin
{
    class GameFunctional
    {
        List<ToolStripMenuItem> stripMenuItems = new List<ToolStripMenuItem>();
        PuzzlePieces pieces;
        PuzzleBoard puzzleBoard;
        FlowLayoutPanel flowLayoutPanel1 = null;
        FlowLayoutPanel flowLayoutPanel2 = null;
        private string puzzleName;
        private string[] directtories;
        ToolStripMenuItem stripMenuItem;

        public GameFunctional(FlowLayoutPanel flowLayoutPanel1, FlowLayoutPanel flowLayoutPanel2)
        {
            this.flowLayoutPanel1 = flowLayoutPanel1;
            this.flowLayoutPanel2 = flowLayoutPanel2;
        }

        //adding all games from Resource folder to menu
        public void newChooseImageItemAdd(ToolStripMenuItem smItem)
        {
            this.stripMenuItem = smItem;
            directtories = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\");

            foreach (var dir in directtories)
            {
                //getting only names of folders, 'cause they are games names
                string str = dir;
                string substr = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\";
                str = str.Replace(substr, "");

                var menuItem = new System.Windows.Forms.ToolStripMenuItem();
                menuItem.Name = str + "Item";
                menuItem.Size = new System.Drawing.Size(152, 22);
                menuItem.Text = str;
                menuItem.Click += new EventHandler(chooseImageItem_Click);
                stripMenuItems.Add(menuItem);
            }

            foreach (ToolStripMenuItem menuItem in stripMenuItems)
                smItem.DropDownItems.Add(menuItem);
        }


        //strarting choosen game
        private void chooseImageItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            puzzleName = item.Text;
            startGame(item.Text);
        }

        public void startGame(string gameName)
        {
            cleanPieces();
            cleanBoard();
            puzzleBoard = null;
            pieces = null;
            puzzleBoard = new PuzzleBoard(flowLayoutPanel2);
            puzzleBoard.makeBoard(9);
            pieces = new PuzzlePieces(flowLayoutPanel1, puzzleBoard);
            pieces.takePicture(gameName);
            pieces.addImagesRandomToLayout();
        }

        
        public void cleanPieces()
        {
            if (pieces == null) return;
            if (pieces.getListOfPiece() == null) return;

            List<Piece> piece = pieces.getListOfPiece();
            foreach(Piece p in piece)
            {
                flowLayoutPanel1.Controls.Remove(p.getPiecePicBox());
            }
        }

        public void cleanBoard()
        {
            if (puzzleBoard == null) return;
            if (puzzleBoard.getListOfParts() == null) return;

            List<Piece> parts = puzzleBoard.getListOfParts();
            foreach (Piece part in parts)
            {
                flowLayoutPanel2.Controls.Remove(part.getPiecePicBox());
            }
        }

        public void reset()
        {
            cleanPieces();
            cleanBoard();
            puzzleBoard = null;
            puzzleBoard = new PuzzleBoard(flowLayoutPanel2);
            puzzleBoard.makeBoard(9);

            if (pieces == null) return;
            if (puzzleBoard == null) return;

            List<Piece> piece = pieces.getListOfPiece();
            foreach (Piece p in piece)
            {
                flowLayoutPanel1.Controls.Add(p.getPiecePicBox());
            }

            cleanBoard();
            puzzleBoard.makeBoard(9);

        }

        //function to check puzzle, if game could be finished
        public void checkFinish()
        {
            bool result = puzzleBoard.chekFinish(pieces);
            if (!result)
                MessageBox.Show("You make puzzles bad, try another combination!", "Information message", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            if (result)
                new FormFinishedGame(puzzleName).ShowDialog();
        }

        //make random new game from existed("New game" in menu)
        public void randomNewGame()
        {
            Random r = new Random();
            puzzleName = stripMenuItems[r.Next(stripMenuItems.Count)].Text;
            startGame(puzzleName);
        }

        //show image to help make puzzle
        public void showImage()
        {
            if(puzzleName != null)
                new FormImageHelp(puzzleName).ShowDialog();
            else
                MessageBox.Show("You don't choose any puzzle!","Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

    }
}
