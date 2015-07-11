using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bomberman
{
	class Client
	{
		private TcpClient server;
		private StreamWriter writer;
		private StreamReader reader;
		internal Point position;
		internal bool alive;

		public Client(IPAddress ip, bool user, bool update)
		{
			alive = true;
			server = new TcpClient(AddressFamily.InterNetworkV6);
			server.Client.DualMode = true;
			server.Connect(ip,Program.port);
			writer = new StreamWriter(server.GetStream());
			reader = new StreamReader(server.GetStream());
			writer.AutoFlush = true;
			if (user) Form1.player = this;
			Handshake(update);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="update">Boolean if client want to recive updates of playground</param>
		private async void Handshake(bool update)
		{
			string request = "Bomberman " + update;
			writer.WriteLine(request);
			string response = await reader.ReadLineAsync();
			string[] tokens = response.Split(' ');
			if (tokens[0] == "ACK")
			{
				position = GameLogic.GetStartPosition(tokens[1]);
				if (update) RecivePlayground();
				StartListening();
			}
		}
		private async void RecivePlayground()
		{
			string data;
			data = await reader.ReadLineAsync();
			string[] tokens = data.Split(' ');
			if (tokens[0] == "Playground")
			{
				int size = Int32.Parse(tokens[1]);
				if (Program.playground == null) Program.playground = new Playground(size);
				for (int i = 0; i < size; i++)
				{
					data = await reader.ReadLineAsync();
					tokens = data.Split(' ');
					for (int j = 0; j < size; j++)
					{
						Program.playground.board[i][j] = (Square)int.Parse(tokens[j]);
					}
				}
				data = await reader.ReadLineAsync();
				if (data == "End") return;
				else
				{
					// TODO error
				}
			}
			else
			{
				// TODO error
			}
		}
		private async void StartListening()
		{
			while (server.Connected)
			{
				try
				{
					string command = await reader.ReadLineAsync();
					ProcessCommand(command);
				}
				catch (IOException)
				{
					// TODO error
				}
			}
		}
		private Point getPosition(string number)
		{
			switch (number)
			{
				case "0":
					return new Point(1, 1);
				case "1":
					return new Point(1, Playground.playgroundSize - 2);
				case "2":
					return new Point(Playground.playgroundSize - 2, 1);
				case "3":
					return new Point(Playground.playgroundSize - 2, Playground.playgroundSize - 2);
				default:
					// TODO error
					return new Point();
			}
		}
		private void ProcessCommand(string msg)
		{
			// TODO process command
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
		internal void Send(string msg)
		{
			try
			{
				writer.WriteLine(msg);
			}
			catch (IOException)
			{
				// TODO error
			}
		}
	}
}
