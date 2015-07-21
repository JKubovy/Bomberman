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
		/// <param name="movement">Array of moves by players</param>
		/// <param name="connection">Connection coresponding to player who want to make a move</param>
		internal static void Process(Movement movement, Connection connection)
		{
			Playground playground = Program.playground;
			if (playground.board[connection.position.X][connection.position.Y] == Square.Fire) return;
			switch (movement)
			{
				case Movement.Up:
					if ((playground.board[connection.position.X - 1][connection.position.Y] == Square.Empty) ||
						(playground.board[connection.position.X - 1][connection.position.Y] == Square.Fire))
					{
						Move(playground, new Point(connection.position.X, connection.position.Y), new Point(connection.position.X - 1, connection.position.Y));
						connection.position = new Point(connection.position.X - 1, connection.position.Y);
					}
					return;
				case Movement.Left:
					if ((playground.board[connection.position.X][connection.position.Y - 1] == Square.Empty) ||
						(playground.board[connection.position.X][connection.position.Y - 1] == Square.Fire))
					{
						Move(playground, new Point(connection.position.X, connection.position.Y), new Point(connection.position.X, connection.position.Y - 1));
						connection.position = new Point(connection.position.X,connection.position.Y - 1);
					}
					return;
				case Movement.Down:
					if ((playground.board[connection.position.X + 1][connection.position.Y] == Square.Empty) ||
						(playground.board[connection.position.X + 1][connection.position.Y] == Square.Fire))
					{
						Move(playground, new Point(connection.position.X, connection.position.Y), new Point(connection.position.X + 1, connection.position.Y));
						connection.position = new Point(connection.position.X + 1, connection.position.Y);
					}
					return;
				case Movement.Right:
					if ((playground.board[connection.position.X][connection.position.Y + 1] == Square.Empty) ||
						(playground.board[connection.position.X][connection.position.Y + 1] == Square.Fire))
					{
						Move(playground, new Point(connection.position.X, connection.position.Y), new Point(connection.position.X, connection.position.Y + 1));
						connection.position = new Point(connection.position.X, connection.position.Y + 1);
					}
					return;
				case Movement.Plant_bomb:
					playground.AddBomb(new Point(connection.position.X, connection.position.Y));
					return;
				default:
					return;
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
		/// Get a starting position based on the start number
		/// </summary>
		/// <param name="startNumber">Player's start number</param>
		/// <returns>Coordinates on playground</returns>
		internal static Point GetStartPosition(string startNumber)
		{
			switch (startNumber)
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
					return new Point();  //unsupported start number
			}
		}
		/// <summary>
		/// Get Movement which coresponding to pressed key
		/// </summary>
		/// <param name="key">Pressed key</param>
		/// <returns>Coresponding Movement</returns>
		internal static Movement ProcessKeyPress(Keys key)
		{
			Point position = Form1.player.Position;
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
				case Keys.E:
					movement = Movement.Plant_bomb;
					break;
				case Keys.Enter:
					return Movement.Enter;
				case Keys.Back:
				case Keys.Q:
					return Movement.Backspace;
				default:
					movement = Movement.Nothing;
					break;
			}
			return movement;
		}
		/// <summary>
		/// Get the local IP address of current computer
		/// </summary>
		/// <returns>IP address</returns>
		internal static string GetLanIP()
		{
			try
			{
				string hostName = System.Net.Dns.GetHostName();
				IPHostEntry ipAddresses = System.Net.Dns.GetHostEntry(hostName);
				foreach (IPAddress ipAddress in ipAddresses.AddressList)
				{
					if (ipAddress.AddressFamily.ToString() == "InterNetwork")
					{
						return ipAddress.ToString();
					}
				}
				return ""; // can't find any local IP address
			}
			catch (System.Net.Sockets.SocketException)
			{
				return ""; // unable to get valid data
			}
		}
		/// <summary>
		/// Determinate if the square represent some player
		/// </summary>
		/// <param name="square"></param>
		/// <param name="startPosition">Player's start number</param>
		/// <returns></returns>
		internal static bool IsPlayerSquare(Square square, int startPosition)
		{
			switch (square)
			{
				case Square.Player_1:
				case Square.Bomb_1_1:
				case Square.Bomb_2_1:
				case Square.Bomb_3_1:
					if (startPosition == 0) return true;
					else return false;
				case Square.Player_2:
				case Square.Bomb_1_2:
				case Square.Bomb_2_2:
				case Square.Bomb_3_2:
					if (startPosition == 1) return true;
					else return false;
				case Square.Player_3:
				case Square.Bomb_1_3:
				case Square.Bomb_2_3:
				case Square.Bomb_3_3:
					if (startPosition == 2) return true;
					else return false;
				case Square.Player_4:
				case Square.Bomb_1_4:
				case Square.Bomb_2_4:
				case Square.Bomb_3_4:
					if (startPosition == 3) return true;
					else return false;
				default:
					return false;
			}
		}
		/// <summary>
		/// Every combination of two movement ends in different position
		/// </summary>
		public readonly static Tuple<Movement, Movement>[] possibleDoubleMove = { new Tuple<Movement, Movement>(Movement.Up, Movement.Left),
																				new Tuple<Movement, Movement>(Movement.Up, Movement.Up),
																				new Tuple<Movement, Movement>(Movement.Up, Movement.Right),
																				new Tuple<Movement, Movement>(Movement.Left, Movement.Up),
																				new Tuple<Movement, Movement>(Movement.Left, Movement.Left),
																				new Tuple<Movement, Movement>(Movement.Left, Movement.Down),
																				new Tuple<Movement, Movement>(Movement.Down, Movement.Right),
																				new Tuple<Movement, Movement>(Movement.Down, Movement.Down),
																				new Tuple<Movement, Movement>(Movement.Down, Movement.Left),
																				new Tuple<Movement, Movement>(Movement.Right, Movement.Up),
																				new Tuple<Movement, Movement>(Movement.Right, Movement.Right),
																				new Tuple<Movement, Movement>(Movement.Right, Movement.Down)
																				};
	}
}
