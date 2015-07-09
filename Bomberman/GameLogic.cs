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
	class Move
	{
		public Point location;
		public Movement movement;

		public Move(Point location, Movement movement)
		{
			this.location = location;
			this.movement = movement;
		}
	}
	class Change
	{
		public Point location;
		public Square square;

		public Change(Point location, Square square)
		{
			this.location = location;
			this.square = square;
		}
	}
	class GameLogic
	{
		/// <summary>
		/// Determinate changes on playground base on player's movements
		/// </summary>
		/// <param name="playground">Instance of playground to update</param>
		/// <param name="moves">Array of moves by players</param>
		/// <returns>Updated playground</returns>
		public Playground Process(Playground playground, Move[] moves)
		{
			Playground outPlayground = playground;
			for (int i = 0; i < moves.Length; i++)
			{
				outPlayground = ProcessMove(outPlayground, moves[i]);
			}
			return outPlayground;
		}
		private Playground ProcessMove(Playground playground, Move move)
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

		public byte[] GetBytes(Move move)
		{
			byte[] data = new byte[3];
			data[0] = (byte)move.location.X;
			data[1] = (byte)move.location.Y;
			data[2] = (byte)move.movement;
			return data;
		}
		public Move GetMove(byte[] data)
		{
			Point location = new Point(data[0], data[1]);
			Movement movement = (Movement)data[2];
			return new Move(location, movement);
		}
		public Playground ProcessChanges(Playground playground, byte[] data)
		{
			// TODO data museji byt delitelna 3, jinak error
			byte[][] cutData = CutBytes(data);
			Change change;
			for (int i = 0; i < cutData.Length; i++)
			{
				change = GetChange(cutData[i]);
				playground.board[change.location.X][change.location.Y] = change.square;
			}
			return playground;
		}
		private Change GetChange(byte[] data)
		{
			Point location = new Point(data[0], data[1]);
			Square square = (Square)data[2];
			return new Change(location, square);
		}
		private byte[][] CutBytes(byte[] data)
		{
			int triplets = data.Length / 3;
			byte[][] outData = new byte[triplets][];
			for (int i = 0; i < triplets; i++)
			{
				outData[i] = new byte[3];
				outData[i][0] = data[i * 3];
				outData[i][1] = data[i * 3 + 1];
				outData[i][2] = data[i * 3 + 2];
			}
			return outData;
		}
	}
}
