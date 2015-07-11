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
			this.KeyPreview = true;
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
			TEST();
		}
		private void TEST()
		{
			Program.playground = new Playground();
			initGraphicPlayground();
			splitContainerMenu.Visible = false;
			splitContainerGame.Visible = true;
			Program.playing = true;
			panelGame.Select();
			Task.Factory.StartNew(() =>
			{
				Server.Start();
			}, TaskCreationOptions.LongRunning);
			Task.Factory.StartNew(() =>
				{
					new Client(System.Net.IPAddress.IPv6Loopback, true, false);
				}, TaskCreationOptions.LongRunning);
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
			splitContainerGame.Visible = true;
		}

		private static PictureBox[][] screen = new PictureBox[Playground.playgroundSize][];
		internal static Client player;
		private void initGraphicPlayground()
		{
			for (int i = 0; i < Playground.playgroundSize; i++)
			{
				screen[i] = new PictureBox[Playground.playgroundSize];
				for (int j = 0; j < Playground.playgroundSize; j++)
				{
					PictureBox p = new PictureBox();
					p.Name = "pictureBox_" + i + "_"+j;
					//p.Size = new System.Drawing.Size(28, 28);
					p.Size = new System.Drawing.Size(35, 35);
					//p.Location = new System.Drawing.Point((28*j), (28*i));
					p.Location = new System.Drawing.Point((35 * j), (35 * i));
					p.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
					p.Image = getImage(i,j);
					screen[i][j] = p;
					panelGame.Controls.Add(p);
				}
			}
		}
		internal static void updatePictureBox()
		{
			for (int i = 0; i < Playground.playgroundSize; i++)
			{
				for (int j = 0; j < Playground.playgroundSize; j++)
				{
					screen[i][j].Image = getImage(i, j);
				}
			}
		}
		private static Image getImage(int x, int y)
		{
			Square square = Program.playground.board[x][y];
			switch (square)
			{
				case Square.Player_1:
					break;
				case Square.Player_2:
					break;
				case Square.Player_3:
					break;
				case Square.Player_4:
					break;
				case Square.Empty:
					break;
				case Square.Wall:
					break;
				case Square.Unbreakable_Wall:
					break;
				case Square.Fire:
					break;
				default:
					break;
			}
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
				case Square.Bomb_1:
					return Properties.Resources.Bomb_1;
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
				case Square.Bomb_2_1:
					return Properties.Resources.Bomb_2_1;
				case Square.Bomb_2_2:
					return Properties.Resources.Bomb_2_2;
				case Square.Bomb_2_3:
					return Properties.Resources.Bomb_2_3;
				case Square.Bomb_2_4:
					return Properties.Resources.Bomb_2_4;
				case Square.Bomb_3:
					return Properties.Resources.Bomb_3;
				case Square.Bomb_3_1:
					return Properties.Resources.Bomb_3_1;
				case Square.Bomb_3_2:
					return Properties.Resources.Bomb_3_2;
				case Square.Bomb_3_3:
					return Properties.Resources.Bomb_3_3;
				case Square.Bomb_3_4:
					return Properties.Resources.Bomb_3_4;
				case Square.Fire:
					return Properties.Resources.Fire;
				default:
					// error
					return null;
			}
		}
		private static Image getImage(Movement movement)
		{
			switch (movement)
			{
				case Movement.Nothing:
					return Properties.Resources.Nothing;
				case Movement.Up:
					return Properties.Resources.Arrow_Up;
				case Movement.Left:
					return Properties.Resources.Arrow_Left;
				case Movement.Down:
					return Properties.Resources.Arrow_Down;
				case Movement.Right:
					return Properties.Resources.Arrow_Right;
				case Movement.Plant_bomb:
					return Properties.Resources.Plant_Bomb;
				default:
					return null;
			}
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			if (Program.playing)
			{
				//player.ProcessKeyPress(e.KeyCode);
				Movement movement = GameLogic.ProcessKeyPress(e.KeyCode);
				UpdateFutureMoves(movement);
				player.ProcessMovement(movement);
			}
		}
		Movement[] futureMovements = new Movement[2];
		int indexFutureMovements = 0;
		private void UpdateFutureMoves(Movement movement)
		{
			switch (movement)
			{
				case Movement.Up:
				case Movement.Left:
				case Movement.Down:
				case Movement.Right:
				case Movement.Plant_bomb:
					if (indexFutureMovements < futureMovements.Length)
					{
						futureMovements[indexFutureMovements] = movement;
						indexFutureMovements++;
					}
					break;
				case Movement.Enter:
					if (indexFutureMovements == 2)
					{
						indexFutureMovements = 0;
						futureMovements[0] = Movement.Nothing;
						futureMovements[1] = Movement.Nothing;
					}
					break;
				case Movement.Backspace:
					if (indexFutureMovements == 0) break;
					indexFutureMovements--;
					futureMovements[indexFutureMovements] = Movement.Nothing;
					break;
				default:
					break;
			}
			UpdatePictureBoxMovements();
		}
		private void UpdatePictureBoxMovements()
		{
			pictureNextMove1.Image = getImage(futureMovements[0]);
			pictureNextMove2.Image = getImage(futureMovements[1]);
		}
	}
}
