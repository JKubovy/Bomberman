using System;
using System.Threading;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bomberman
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
            numericUpDownSize.Value = 14;
			SetText();
			this.KeyPreview = true;
		}

		private void SetText()
		{
			this.textBoxIP.Text = GameLogic.GetLanIP();

			this.labelAbout.Text = Properties.Resources.About_Text;
			this.labelControls.Text = Properties.Resources.Controls_Text;
			this.labelInfoPlayer1.Text = Properties.Resources.Info_Player1;
			this.labelInfoPlayer2.Text = Properties.Resources.Info_Player2;
			this.labelInfoPlayer3.Text = Properties.Resources.Info_Player3;
			this.labelInfoPlayer4.Text = Properties.Resources.Info_Player4;
			this.labelPlayerCount.Text = Properties.Resources.Multiplayer_PlayersCount;
			this.labelSize.Text = Properties.Resources.Multiplayer_Size;
			this.labelNextMoves.Text = Properties.Resources.Info_NextMoves;
			this.labelAvatar.Text = Properties.Resources.Info_Avatar;

			this.buttonAbout.Text = Properties.Resources.Menu_About;
			this.buttonControls.Text = Properties.Resources.Menu_Controls;
			this.buttonExit.Text = Properties.Resources.Menu_Exit;
			this.buttonExit2.Text = Properties.Resources.Menu_Exit;
			this.buttonMultiplayer.Text = Properties.Resources.Menu_Multiplayer;
			this.buttonSingleplayer.Text = Properties.Resources.Menu_Singleplayer;
			this.buttonStart.Text = Properties.Resources.Multiplayer_Start;
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
			radioButton4.Checked = true;
		}
		private void buttonExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void buttonSingleplayer_Click(object sender, EventArgs e)
		{
			panelMultiplayer.Visible = true;
			panelControls.Visible = false;
			panelAbout.Visible = false;
			radioButton1.Checked = true;
			radioButtonServer.Checked = true;
		}

		private static PictureBox[][] screen;
		internal static Client player;
		static object screenLock = new object();
		/// <summary>
		/// Create PictureBoxes grid coresponding to size of array
		/// </summary>
		internal void initGraphicPlayground()
		{
			lock (screenLock)
			{
				screen = new PictureBox[Playground.playgroundSize][];
				for (int i = 0; i < Playground.playgroundSize; i++)
				{
					screen[i] = new PictureBox[Playground.playgroundSize];
					for (int j = 0; j < Playground.playgroundSize; j++)
					{
						PictureBox pictureBox = new PictureBox();
						pictureBox.Name = "pictureBox_" + i + "_" + j;
						pictureBox.Size = new System.Drawing.Size(32, 32);
						pictureBox.Location = new System.Drawing.Point((32 * j), (32 * i));
						pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
						pictureBox.Image = getImage(new Point(i, j));
						screen[i][j] = pictureBox;
						AddPictureBox(pictureBox);
					}
				}
			}
		}
		/// <summary>
		/// Remove PictureBoxes
		/// </summary>
		private void CleanScreen()
		{
			lock (screenLock)
			{
				if (screen != null)
				{
					for (int i = 0; i < screen.Length; i++)
					{
						for (int j = 0; j < screen[i].Length; j++)
						{
							screen[i][j].Dispose();
						}
					}
					screen = null;
				}
			}
		}

		delegate void AddPictureBoxCallback(PictureBox pictureBox);
		private void AddPictureBox(PictureBox pictureBox)
		{
			if (panelGame.InvokeRequired)
			{
				AddPictureBoxCallback d = new AddPictureBoxCallback(AddPictureBox);
				this.Invoke(d, new object[] { pictureBox });
			}
			else
			{
				panelGame.Controls.Add(pictureBox);
			}
		}
		delegate void SetLabelTextCallback(Label label, String text);
		internal void SetLabelText(Label label, String text)
		{
			if (label.InvokeRequired)
			{
				SetLabelTextCallback d = new SetLabelTextCallback(SetLabelText);
				this.Invoke(d, new object[] { label, text });
			}
			else
			{
				label.Text = text;
				this.Invalidate();
			}
		}

		internal static void updatePictureBox()
		{
			lock (screenLock)
			{
				if (screen == null) return;
				for (int i = 0; i < Playground.playgroundSize; i++)
				{
					for (int j = 0; j < Playground.playgroundSize; j++)
					{
						screen[i][j].Image = getImage(new Point(i, j));
					}
				}
			}
		}
		private static Image getImage(Point position)
		{
			Square square = Program.playground.board[position.X][position.Y];
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
                if (e.KeyCode == Keys.Escape)
                {
                    Stop();
                    return;
                }
                Movement movement = GameLogic.ProcessKeyPress(e.KeyCode);
                if (!waiting & alive)
                {
                    UpdateFutureMoves(movement);
                    player.ProcessMovement(movement);
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
		}
		Movement[] futureMovements = new Movement[2];
		int indexFutureMovements = 0;
		internal static bool waiting = true;
		internal static bool alive = true;
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
						DeleteFutureMovement();
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
		private void DeleteFutureMovement()
		{
			indexFutureMovements = 0;
			futureMovements[0] = Movement.Nothing;
			futureMovements[1] = Movement.Nothing;
			waiting = true;
		}
		internal void SetAvatar()
		{
			Point position = player.Position;
			Image image;
			if (position == new Point(1, 1)) image = Properties.Resources.Player_1;
			else if (position == new Point(1, Playground.playgroundSize-2)) image = Properties.Resources.Player_2;
			else if (position == new Point(Playground.playgroundSize - 2,1)) image = Properties.Resources.Player_3;
			else image = Properties.Resources.Player_4;
			this.pictureBoxAvatar.Image = image;
		}

		private void radioButtonServer_CheckedChanged(object sender, EventArgs e)
		{
			textBoxIP.Enabled = !radioButtonServer.Checked;
			panelSize.Enabled = radioButtonServer.Checked;
			groupBoxPlayersCount.Enabled = radioButtonServer.Checked;
		}
		private void buttonStart_Click(object sender, EventArgs e)
		{
			if (radioButtonServer.Checked)
			{
				int playersCount;
				Playground.playgroundSize = (int)numericUpDownSize.Value;
				if (radioButton1.Checked) playersCount = 1;
				else if (radioButton2.Checked) playersCount = 2;
				else if (radioButton3.Checked) playersCount = 3;
				else playersCount = 4;
				StartGame(playersCount);
			}
			else
			{
				try
				{
					System.Net.IPAddress ip = System.Net.IPAddress.Parse(textBoxIP.Text);
					Task.Factory.StartNew(() =>
					{
						new Client(ip, true, true);
					}, TaskCreationOptions.LongRunning);
				}
				catch(FormatException)
				{
					textBoxIP.Text = Properties.Resources.Error_IP;
					return;
				}
			}
			splitContainerMenu.Visible = false;
			panelGameInfo.Visible = true;
			Program.playing = true;
			UpdatePictureBoxMovements();
			panelGame.Select();
		}
		Thread server;
		private void StartGame(int playersCount)
		{
			Program.playground = new Playground();
			initGraphicPlayground();

			server = new Thread(() => Server.Start());
			server.IsBackground = true;
			server.Start();

			Task.Factory.StartNew(() =>
			{
				new Client(System.Net.IPAddress.IPv6Loopback, true, false);
			}, TaskCreationOptions.LongRunning);
			for (int i = 0; i < (4 - playersCount); i++)
			{
				Task.Factory.StartNew(() =>
				{
					new Client(System.Net.IPAddress.IPv6Loopback, false, false);
				}, TaskCreationOptions.LongRunning);
			}
		}
		private void Stop()
		{
			if (server == null)
			{
				if (player != null) player.Stop();
			}
			else
			{
				Server.Stop();
				server.Abort();
				server = null;
			}
			Program.playing = false;
			waiting = true;
			alive = true;
            DeleteFutureMovement();
            UpdatePictureBoxMovements();
            panelGameInfo.Visible = false;
			splitContainerMenu.Visible = true;
			CleanScreen();
		}
	}
}
