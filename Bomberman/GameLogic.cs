using System;
using System.Drawing;

namespace Bomberman
{
	enum Movement
	{
		Up,
		Left,
		Down,
		Right,
		Plant_bomb
	}
	struct Move
	{
		public Point location;
		public Movement movement;
	}
	class GameLogic
	{
		public Playground ProcessMove(Playground playground, Move move)
		{

			switch (move.movement)
			{
				case Movement.Up:
					if (playground.board[move.location.X - 1][move.location.Y] == Square.Empty)
					{
						playground.board[move.location.X - 1][move.location.Y] = playground.board[move.location.X][move.location.Y];
						playground.board[move.location.X][move.location.Y] = Square.Empty;
					}
					return playground;
				case Movement.Left:
					if (playground.board[move.location.X][move.location.Y - 1] == Square.Empty)
					{
						playground.board[move.location.X][move.location.Y] = playground.board[move.location.X][move.location.Y];
						playground.board[move.location.X][move.location.Y - 1] = Square.Empty;
					}
					return playground;
				case Movement.Down:
					if (playground.board[move.location.X + 1][move.location.Y] == Square.Empty)
					{
						playground.board[move.location.X + 1][move.location.Y] = playground.board[move.location.X][move.location.Y];
						playground.board[move.location.X][move.location.Y] = Square.Empty;
					}
					return playground;
				case Movement.Right:
					if (playground.board[move.location.X][move.location.Y + 1] == Square.Empty)
					{
						playground.board[move.location.X][move.location.Y + 1] = playground.board[move.location.X][move.location.Y];
						playground.board[move.location.X][move.location.Y] = Square.Empty;
					}
					return playground;
				case Movement.Plant_bomb:
					playground.board[move.location.X][move.location.Y] = GetBombSquare(playground.board[move.location.X][move.location.Y]);
					return playground;
				default:
					return playground;
			}
		}
		private Square GetBombSquare(Square square)
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
	}
}
