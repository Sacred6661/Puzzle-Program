using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleGameWin
{
    public partial class FormFinishedGame : Form
    {
        public FormFinishedGame(string puzzleName)
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\" + puzzleName + "\\" + puzzleName + ".jpg");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
