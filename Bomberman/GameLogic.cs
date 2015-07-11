using System;
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
		Plant_bomb
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
	class GameLogic
	{
		/// <summary>
		/// Determinate changes on playground base on player's movements
		/// </summary>
		/// <param name="playground">Instance of playground to update</param>
		/// <param name="movement">Array of moves by players</param>
		/// <param name="connection"></param>
		/// <returns>Updated playground</returns>
		/// 
		internal static Playground Process(Playground playground, Movement movement, Connection connection)
		{
			Playground outPlayground = playground;
			switch (movement)
			{
				case Movement.Up:
					if (playground.board[connection.position.X - 1][connection.position.Y] == Square.Empty)
					{
						Move(playground, new Point(connection.position.X, connection.position.Y), new Point(connection.position.X - 1, connection.position.Y));
						connection.position = new Point(connection.position.X - 1, connection.position.Y);
					}
					return playground;
				case Movement.Left:
					if (playground.board[connection.position.X][connection.position.Y - 1] == Square.Empty)
					{
						Move(playground, new Point(connection.position.X, connection.position.Y), new Point(connection.position.X, connection.position.Y - 1));
						connection.position = new Point(connection.position.X,connection.position.Y - 1);
					}
					return playground;
				case Movement.Down:
					if (playground.board[connection.position.X + 1][connection.position.Y] == Square.Empty)
					{
						Move(playground, new Point(connection.position.X, connection.position.Y), new Point(connection.position.X + 1, connection.position.Y));
						connection.position = new Point(connection.position.X + 1, connection.position.Y);
					}
					return playground;
				case Movement.Right:
					if (playground.board[connection.position.X][connection.position.Y + 1] == Square.Empty)
					{
						Move(playground, new Point(connection.position.X, connection.position.Y), new Point(connection.position.X, connection.position.Y + 1));
						connection.position = new Point(connection.position.X, connection.position.Y + 1);
					}
					return playground;
				case Movement.Plant_bomb:
					playground.board[connection.position.X][connection.position.Y] = GetBombSquare(playground.board[connection.position.X][connection.position.Y]);
					return playground;
				default:
					return playground;
			}
		}
		private static Square GetBombSquare(Square square)
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
		private static Square GetPlayerSquare(Square square)
		{
			switch (square)
			{
				case Square.Bomb_1_1:
					return Square.Player_1;
				case Square.Bomb_1_2:
					return Square.Player_2;
				case Square.Bomb_1_3:
					return Square.Player_3;
				case Square.Bomb_1_4:
					return Square.Player_4;
				default:
					return square;
			}
		}
		private static void Move(Playground playground, Point oldLocation, Point newLocation)
		{
			Square oldSquare = playground.board[oldLocation.X][oldLocation.Y];
			if (oldSquare >= Square.Bomb_1_1 && oldSquare <= Square.Bomb_1_4)
			{
				//playground.board[newLocation.X][newLocation.Y] = (oldSquare - 7); // from bomb_1_x to Player_x
				playground.board[newLocation.X][newLocation.Y] = GetPlayerSquare(oldSquare);
				playground.board[oldLocation.X][oldLocation.Y] = Square.Bomb_2;
			}
			else
			{
				playground.board[newLocation.X][newLocation.Y] = playground.board[oldLocation.X][oldLocation.Y];
				playground.board[oldLocation.X][oldLocation.Y] = Square.Empty;
			}
		}
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
	}
}
