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
    public partial class FormImageHelp : Form
    {
        public FormImageHelp(string puzzleName)
        {
            InitializeComponent();
            if(puzzleName != null)
                pictureBox1.Image = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\" + puzzleName + "\\" + puzzleName + ".jpg");
        }
    }
}
