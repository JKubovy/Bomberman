using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bomberman
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void buttonAbout_Click(object sender, EventArgs e)
		{
			panelMultiplayer.Visible = false;
			panelControls.Visible = false;
			panelAbout.Visible = true;
		}

		private void buttonControls_Click(object sender, EventArgs e)
		{
			panelMultiplayer.Visible = false;
			panelControls.Visible = true;
			panelAbout.Visible = false;
		}

		private void buttonMultiplayer_Click(object sender, EventArgs e)
		{
			panelMultiplayer.Visible = true;
			panelControls.Visible = false;
			panelAbout.Visible = false;
		}

		private void buttonExit_Click(object sender, EventArgs e)
		{
			// add stop server, client, etc.
			this.Close();
		}

		private void buttonSingleplayer_Click(object sender, EventArgs e)
		{
			Program.playground = new Playground();
			initGraphicPlayground();
			splitContainerMenu.Visible = false;
			panelGame.Visible = true;
		}

		private static PictureBox[][] screen = new PictureBox[Playground.playgroundSize][];
		private void initGraphicPlayground()
		{
			for (int i = 0; i < Playground.playgroundSize; i++)
			{
				screen[i] = new PictureBox[Playground.playgroundSize];
				for (int j = 0; j < Playground.playgroundSize; j++)
				{
					PictureBox p = new PictureBox();
					p.Name = "pictureBox_" + i + "_"+j;
					p.Size = new System.Drawing.Size(28, 28);
					p.Location = new System.Drawing.Point((28*j), (28*i));
					p.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
					p.Image = getImage(i,j);
					screen[i][j] = p;
					panelGame.Controls.Add(p);
				}
			}
		}
		private Image getImage(int x, int y)
		{
			Square square = Program.playground.board[x][y];
			switch (square)
			{
				case Square.Player_1:
					return Properties.Resources.Player_1;
				case Square.Player_2:
					return Properties.Resources.Player_2;
				case Square.Player_3:
					return Properties.Resources.Player_3;
				case Square.Player_4:
					return Properties.Resources.Player_4;
				case Square.Empty:
					return Properties.Resources.Empty;
				case Square.Wall:
					return Properties.Resources.Wall;
				case Square.Unbreakable_Wall:
					return Properties.Resources.Unbreakable_Wall;
				case Square.Bomb_1_1:
					return Properties.Resources.Bomb_1_1;
				case Square.Bomb_1_2:
					return Properties.Resources.Bomb_1_2;
				case Square.Bomb_1_3:
					return Properties.Resources.Bomb_1_3;
				case Square.Bomb_1_4:
					return Properties.Resources.Bomb_1_4;
				case Square.Bomb_2:
					return Properties.Resources.Bomb_2;
				case Square.Bomb_3:
					return Properties.Resources.Bomb_3;
				case Square.Fire:
					return Properties.Resources.Fire;
				default:
					// error
					return null;
			}
		}
	}
}
