using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PuzzleGameWin
{
    public partial class Form1 : Form
    {
        List<ToolStripMenuItem> stripMenuItems = new List<ToolStripMenuItem>();
        GameFunctional game;

        public Form1()
        {
            InitializeComponent();
            game = new GameFunctional(flowLayoutPanel1, flowLayoutPanel2);
            game.newChooseImageItemAdd(chooseImageToolStripMenuItem);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new PictureSlicer().slicePicture("South Park", 3);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
        }


        private void menuItemFlow_onClick(object sender, MouseEventArgs e)
        {

        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            game.checkFinish();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.reset();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.randomNewGame();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void showImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.showImage();
        }

        private void addPuzzleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormAddPicture().ShowDialog();
            Application.Restart();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by Livenets Bogdan \n 2018", "About");
        }
    }
}
