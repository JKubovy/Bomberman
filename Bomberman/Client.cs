using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bomberman
{
	class Client
	{
		internal Point position;

		private TcpClient server;
		private StreamWriter writer;
		private StreamReader reader;
		private AI AI;

		/// <summary>
		/// Start new Client and connect it to server
		/// </summary>
		/// <param name="ip">IP address of server in local network</param>
		/// <param name="user">boolean represent if client control user</param>
		/// <param name="update">boolean represent if client want to recieve updates</param>
		public Client(IPAddress ip, bool user, bool update)
		{
			server = new TcpClient(AddressFamily.InterNetworkV6);
			server.Client.DualMode = true;
			server.Connect(ip,Program.port);
			writer = new StreamWriter(server.GetStream());
			reader = new StreamReader(server.GetStream());
			writer.AutoFlush = true;
			if (user)
			{
				Form1.player = this;
			}
			Handshake(update, user);
		}
		/// <summary>
		/// Start comunicate with server
		/// </summary>
		/// <param name="update">Boolean if client want to recive updates</param>
		/// <param name="user">Boolean if client is user</param>
		private void Handshake(bool update, bool user)
		{
			string request = "Bomberman " + update + " " + user;
			writer.WriteLine(request);
			string response = reader.ReadLine();
			string[] tokens = response.Split(' ');
			if (tokens[0] == "ACK")
			{
				position = GameLogic.GetStartPosition(tokens[1]);
				if (update)
				{
					response = reader.ReadLine();
					tokens = response.Split(' ');
					ProcessPlayground(tokens);
				}
				if (user)
				{
					Form form1 = Application.OpenForms[0];
					((Form1)form1).SetAvatar();
				}
				else
				{
					AI = new AI(Program.playground.board[position.X][position.Y]);
				}
				StartListening();
			}
		}

		private void ProcessPlayground(string[] data)
		{
			int size = int.Parse(data[1]);
			if (Program.playground == null || Program.playground.board.Length != size)
			{
				Program.playground = new Playground(size);
				Form form1 = Application.OpenForms[0];
				((Form1)form1).initGraphicPlayground();
			}
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					Program.playground.board[i][j] = (Square)int.Parse(data[2+size*i+j]);
				}
			}
			Form1.updatePictureBox();
			Form1.waiting = false;
		}
		private void StartListening()
		{
			while (server.Connected)
			{
				try
				{
					string command = reader.ReadLine();
					ProcessCommand(command);
				}
				catch (IOException) // Server is lost
				{
					ServerLost();
				}
				catch (TaskCanceledException) // Thread is canceled
				{
					break;
				}
			}
		}
		private void ProcessCommand(string command)
		{
			string[] tokens = command.Split(' ');
			switch (tokens[0])
			{
				case "SendMoves":
					futureMoves = AI.GetNextMovement(position, out position);
					indexFutureMoves = 2;
					SendMoves();
					break;
				case "Playground":
					ProcessPlayground(tokens);
					break;
				case "Change":
					ProcessChange(tokens);
					break;
				case "Update":
					ProcessUpdate(tokens);
					break;
				case "Start":
					Form1.waiting = false;
					break;
				case "Dead":
					Form1.alive = false;
					break;
				case "Stop":
					server.Close();
					throw new TaskCanceledException();
				default:
					break;
			}
		}

		Movement[] futureMoves = new Movement[2];
		int indexFutureMoves = 0;
		internal void ProcessMovement(Movement movement)
		{
			switch (movement)
			{
				case Movement.Up:
				case Movement.Left:
				case Movement.Down:
				case Movement.Right:
				case Movement.Plant_bomb:
					if (indexFutureMoves < futureMoves.Length)
					{
						futureMoves[indexFutureMoves] = movement;
						indexFutureMoves++;
						updatePosition(movement);
					}
					break;
				case Movement.Backspace:
					if (indexFutureMoves == 0) break;
					indexFutureMoves--;
					futureMoves[indexFutureMoves] = Movement.Nothing;
					break;
				case Movement.Enter:
					SendMoves();
					break;
				default:
					break;
			}
		}
		private void updatePosition(Movement movement)
		{
			switch (movement)
			{
				case Movement.Up:
					position.X--;
					break;
				case Movement.Left:
					position.Y--;
					break;
				case Movement.Down:
					position.X++;
					break;
				case Movement.Right:
					position.Y++;
					break;
				default:
					break;
			}
		}
		private void SendMoves()
		{
			if (indexFutureMoves != 2) return;
			Send("Move " + (int)futureMoves[0] + " " + (int)futureMoves[1]);
			futureMoves[0] = Movement.Nothing;
			futureMoves[1] = Movement.Nothing;
			indexFutureMoves = 0;
		}
		private void ProcessChange(string[] tokens)
		{
			for (int i = 0; i < int.Parse(tokens[1]); i++)
			{
				Program.playground.board[int.Parse(tokens[i*3+2])][int.Parse(tokens[i*3+3])] = (Square)int.Parse(tokens[i*3+4]);
			}
			Form1.updatePictureBox();
			Form1.waiting = false;
		}
		private void ProcessUpdate(string[] tokens)
		{
			Form form1 = Application.OpenForms[0];
			Control[] controls = form1.Controls.Find("labelInfoPlayer" + tokens[1], true);
			Label label = (Label)controls[0];
			string text = label.Text;
			string[] oldTextSplit = text.Split(':');
			text = oldTextSplit[0] + ": " + tokens[2];
			((Form1)form1).SetLabelText(label, text);
		}
		private void Send(string msg)
		{
			try
			{
				writer.WriteLine(msg);
			}
			catch (IOException)
			{
				ServerLost();
			}
		}
		private void ServerLost()
		{
			for (int i = 1; i <= 4; i++)
			{
				ProcessUpdate(new string[] { "", i.ToString(), "Disconected" });
			}
			Form1.waiting = true;
		}
	}
}
