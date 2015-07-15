using System;
using System.Net;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Bomberman
{
	enum Movement
	{
		Nothing,
		Up,
		Left,
		Down,
		Right,
		Plant_bomb,
		Enter,
		Backspace
	}
	class FutureMove
	{
		public Connection connection;
		public Movement movement;

		public FutureMove(Movement movement, Connection connection)
		{
			this.connection = connection;
			this.movement = movement;
		}
	}
	class Change
	{
		public Point coordinate;
		public Square square;
		public Change(Point coordinate, Square square)
		{
			this.coordinate = coordinate;
			this.square = square;
		}
	}
	class GameLogic
	{
		/// <summary>
		/// Determinate changes on playground base on player's movements
		/// </summary>
		/// <param name="playground">Instance of playground to update</param>
		/// <param name="movement">Array of moves by players</param>
		/// <param name="connection"></param>
		/// <returns>Updated playground</returns>
		internal static Playground Process(Movement movement, Connection connection)
		{
			Playground playground = Program.playground;
			if (playground.board[connection.position.X][connection.position.Y] == Square.Fire) return playground;
			switch (movement)
			{
				case Movement.Up:
					if ((playground.board[connection.position.X - 1][connection.position.Y] == Square.Empty) ||
						(playground.board[connection.position.X - 1][connection.position.Y] == Square.Fire))
					{
						Move(playground, new Point(connection.position.X, connection.position.Y), new Point(connection.position.X - 1, connection.position.Y));
						connection.position = new Point(connection.position.X - 1, connection.position.Y);
					}
					return playground;
				case Movement.Left:
					if ((playground.board[connection.position.X][connection.position.Y - 1] == Square.Empty) ||
						(playground.board[connection.position.X][connection.position.Y - 1] == Square.Fire))
					{
						Move(playground, new Point(connection.position.X, connection.position.Y), new Point(connection.position.X, connection.position.Y - 1));
						connection.position = new Point(connection.position.X,connection.position.Y - 1);
					}
					return playground;
				case Movement.Down:
					if ((playground.board[connection.position.X + 1][connection.position.Y] == Square.Empty) ||
						(playground.board[connection.position.X + 1][connection.position.Y] == Square.Fire))
					{
						Move(playground, new Point(connection.position.X, connection.position.Y), new Point(connection.position.X + 1, connection.position.Y));
						connection.position = new Point(connection.position.X + 1, connection.position.Y);
					}
					return playground;
				case Movement.Right:
					if ((playground.board[connection.position.X][connection.position.Y + 1] == Square.Empty) ||
						(playground.board[connection.position.X][connection.position.Y + 1] == Square.Fire))
					{
						Move(playground, new Point(connection.position.X, connection.position.Y), new Point(connection.position.X, connection.position.Y + 1));
						connection.position = new Point(connection.position.X, connection.position.Y + 1);
					}
					return playground;
				case Movement.Plant_bomb:
					playground.AddBomb(new Point(connection.position.X, connection.position.Y));
					return playground;
				default:
					return playground;
			}
		}
		private static Square GetFirstBombSquare(Square square)
		{
			switch (square)
			{
				case Square.Player_1:
					return Square.Bomb_1_1;
				case Square.Player_2:
					return Square.Bomb_1_2;
				case Square.Player_3:
					return Square.Bomb_1_3;
				case Square.Player_4:
					return Square.Bomb_1_4;
				default:
					return square;
			}
		}
		private static Square GetBombSquare(Square square)
		{
			switch (square)
			{
				case Square.Bomb_1_1:
				case Square.Bomb_1_2:
				case Square.Bomb_1_3:
				case Square.Bomb_1_4:
					return Square.Bomb_1;
				case Square.Bomb_2_1:
				case Square.Bomb_2_2:
				case Square.Bomb_2_3:
				case Square.Bomb_2_4:
					return Square.Bomb_2;
				case Square.Bomb_3_1:
				case Square.Bomb_3_2:
				case Square.Bomb_3_3:
				case Square.Bomb_3_4:
					return Square.Bomb_3;
				default:
					// TODO error
					return Square.Empty;
			}
		}
		private static Square GetPlayerSquare(Square square)
		{
			switch (square)
			{
				case Square.Bomb_1_1:
				case Square.Bomb_2_1:
				case Square.Bomb_3_1:
					return Square.Player_1;
				case Square.Bomb_1_2:
				case Square.Bomb_2_2:
				case Square.Bomb_3_2:
					return Square.Player_2;
				case Square.Bomb_1_3:
				case Square.Bomb_2_3:
				case Square.Bomb_3_3:
					return Square.Player_3;
				case Square.Bomb_1_4:
				case Square.Bomb_2_4:
				case Square.Bomb_3_4:
					return Square.Player_4;
				default:
					return square;
			}
		}

		internal static List<Change> changes = new List<Change>();
		private static void Move(Playground playground, Point oldLocation, Point newLocation)
		{
			Square oldSquare = playground.board[oldLocation.X][oldLocation.Y];
			if ((oldSquare >= Square.Bomb_1_1 && oldSquare <= Square.Bomb_1_4) ||
				(oldSquare >= Square.Bomb_2_1 && oldSquare <= Square.Bomb_2_4) ||
				(oldSquare >= Square.Bomb_3_1 && oldSquare <= Square.Bomb_3_4))
			{
				playground.board[newLocation.X][newLocation.Y] = GetPlayerSquare(oldSquare);
				changes.Add(new Change(new Point(newLocation.X, newLocation.Y), playground.board[newLocation.X][newLocation.Y]));
				playground.board[oldLocation.X][oldLocation.Y] = GetBombSquare(playground.board[oldLocation.X][oldLocation.Y]);
				changes.Add(new Change(new Point(oldLocation.X, oldLocation.Y), playground.board[oldLocation.X][oldLocation.Y]));
			}
			else
			{
				playground.board[newLocation.X][newLocation.Y] = playground.board[oldLocation.X][oldLocation.Y];
				changes.Add(new Change(new Point(newLocation.X, newLocation.Y), playground.board[newLocation.X][newLocation.Y]));
				playground.board[oldLocation.X][oldLocation.Y] = Square.Empty;
				changes.Add(new Change(new Point(oldLocation.X, oldLocation.Y), Square.Empty));
			}
		}
		/// <summary>
		/// Return coordinates on playground
		/// </summary>
		/// <param name="number">player's number</param>
		/// <returns>coordinates on playground</returns>
		internal static Point GetStartPosition(string number)
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
		internal static bool ValidMovement(Point position, Movement movement)
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
				case Movement.Plant_bomb:
					if (Program.playground.board[position.X][position.Y] >= Square.Player_1 &&
						Program.playground.board[position.X][position.Y] <= Square.Player_4) return true;
					else return false;
				default:
					return false;
			}
			if (Program.playground.board[position.X][position.Y] == Square.Empty) return true;
			else return false;
		}
		/// <summary>
		/// Get Movement which coresponding pressed key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		internal static Movement ProcessKeyPress(Keys key)
		{
			Point position = Form1.player.position;
			Movement movement;
			switch (key)
			{
				case Keys.Left:
				case Keys.A:
					movement = Movement.Left;
					break;
				case Keys.Up:
				case Keys.W:
					movement = Movement.Up;
					break;
				case Keys.Right:
				case Keys.D:
					movement = Movement.Right;
					break;
				case Keys.Down:
				case Keys.S:
					movement = Movement.Down;
					break;
				case Keys.Space:
					movement = Movement.Plant_bomb;
					break;
				case Keys.Enter:
					return Movement.Enter;
				case Keys.Back:
					return Movement.Backspace;
				default:
					movement = Movement.Nothing;
					break;
			}
			return movement;
		}
		/// <summary>
		/// Get local IP address of current computer
		/// </summary>
		/// <returns>IP address</returns>
		internal static string GetLanIP()
		{
			string strHostName = System.Net.Dns.GetHostName();
			IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
			foreach (IPAddress ipAddress in ipEntry.AddressList)
			{
				if (ipAddress.AddressFamily.ToString() == "InterNetwork")
				{
					return ipAddress.ToString();
				}
			}
			return "";
		}
	}
}
