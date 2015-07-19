using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Bomberman
{
	class AI
	{
		Point heading;
		Square character;
		bool enemyClose;
		Point[] players = new Point[4];
		int[][] boardWithObstacles;
		int[][] boardPossibleMovement;
		int[][] boardPossiblePath;
		Stack<Point> path = new Stack<Point>();
		Queue<Movement> saveMovement = new Queue<Movement>();

		public AI(Square character)
		{
			this.character = character;
		}

		/// <summary>
		/// Calculate next two movement
		/// </summary>
		/// <param name="position">Standing position</param>
		/// <param name="newPosition">New position after make movement</param>
		/// <returns>Array of TWO movement</returns>
		internal Movement[] GetNextMovement(Point position)
		{
			Point tmp;
			Movement[] movement = new Movement[2];
			PrepareBoardMovement(position);
			PrepareHeadingPoint(position);
			if (enemyClose) movement[0] = RandomStep(position);
			else movement[0] = ProcessStep(position);
			if (Check(position, movement[0], out tmp)) position = tmp;
			if (enemyClose) movement[1] = RandomStep(position);
			else movement[1] = ProcessStep(position);
			return movement;
		}

		private void PrepareBoardMovement(Point position)
		{
			boardWithObstacles = new int[Playground.playgroundSize][];
			boardPossibleMovement = new int[Playground.playgroundSize][];
			boardPossiblePath = new int[Playground.playgroundSize][];
			for (int i = 0; i < 4; i++)
			{
				players[i] = new Point();
			}
			for (int i = 0; i < Playground.playgroundSize; i++)
			{
				boardWithObstacles[i] = new int[Playground.playgroundSize];
				boardPossibleMovement[i] = new int[Playground.playgroundSize];
				boardPossiblePath[i] = new int[Playground.playgroundSize];
			}
			for (int i = 0; i < Playground.playgroundSize; i++)
			{
				for (int j = 0; j < Playground.playgroundSize; j++)
				{
					Square sqare = Program.playground.board[i][j];

					if (sqare == Square.Unbreakable_Wall) boardWithObstacles[i][j] = -3;
					else if (sqare == Square.Wall) boardWithObstacles[i][j] = -2;
					else boardWithObstacles[i][j] = -1;

					if (sqare == Square.Empty || sqare == Square.Fire || (sqare >= Square.Player_1 && sqare <= Square.Player_4)) boardPossibleMovement[i][j] = -1;
					else boardPossibleMovement[i][j] = -2;

					if (sqare == Square.Unbreakable_Wall) boardPossiblePath[i][j] = -2;
					else boardPossiblePath[i][j] = -1;

					RecognizePlayer(new Point(i, j));
				}
			}
			boardWithObstacles[position.X][position.Y] = 0;
			boardPossibleMovement[position.X][position.Y] = 0;
			boardPossiblePath[position.X][position.Y] = 0;
			Queue<Point> queue = new Queue<Point>();
			queue.Enqueue(new Point(position.X, position.Y));
			do
			{
				Point p = queue.Dequeue();
				#region Right
				if (boardWithObstacles[p.X][p.Y + 1] == -2)
				{
					boardWithObstacles[p.X][p.Y + 1] = boardWithObstacles[p.X][p.Y] + 1;
					queue.Enqueue(new Point(p.X, p.Y + 1));
				}
				else if (boardWithObstacles[p.X][p.Y + 1] == -1)
				{
					boardWithObstacles[p.X][p.Y + 1] = boardWithObstacles[p.X][p.Y];
					queue.Enqueue(new Point(p.X, p.Y + 1));
				}
				else if (boardWithObstacles[p.X][p.Y + 1] > boardWithObstacles[p.X][p.Y] + 1 && Program.playground.board[p.X][p.Y + 1] == Square.Wall)
				{
					boardWithObstacles[p.X][p.Y + 1] = boardWithObstacles[p.X][p.Y] + 1;
				}
				else if (boardWithObstacles[p.X][p.Y + 1] > boardWithObstacles[p.X][p.Y] && Program.playground.board[p.X][p.Y + 1] != Square.Wall)
				{
					boardWithObstacles[p.X][p.Y + 1] = boardWithObstacles[p.X][p.Y];
				}
				#endregion
				#region Down
				if (boardWithObstacles[p.X + 1][p.Y] == -2)
				{
					boardWithObstacles[p.X + 1][p.Y] = boardWithObstacles[p.X][p.Y] + 1;
					queue.Enqueue(new Point(p.X + 1, p.Y));
				}
				else if (boardWithObstacles[p.X + 1][p.Y] == -1)
				{
					boardWithObstacles[p.X + 1][p.Y] = boardWithObstacles[p.X][p.Y];
					queue.Enqueue(new Point(p.X + 1, p.Y));
				}
				else if (boardWithObstacles[p.X + 1][p.Y] > boardWithObstacles[p.X][p.Y] + 1 && Program.playground.board[p.X + 1][p.Y] == Square.Wall)
				{
					boardWithObstacles[p.X + 1][p.Y] = boardWithObstacles[p.X][p.Y] + 1;
				}
				else if (boardWithObstacles[p.X + 1][p.Y] > boardWithObstacles[p.X][p.Y] && Program.playground.board[p.X + 1][p.Y] != Square.Wall)
				{
					boardWithObstacles[p.X + 1][p.Y] = boardWithObstacles[p.X][p.Y];
				}
				#endregion
				#region Left
				if (boardWithObstacles[p.X][p.Y - 1] == -2)
				{
					boardWithObstacles[p.X][p.Y - 1] = boardWithObstacles[p.X][p.Y] + 1;
					queue.Enqueue(new Point(p.X, p.Y - 1));
				}
				else if (boardWithObstacles[p.X][p.Y - 1] == -1)
				{
					boardWithObstacles[p.X][p.Y - 1] = boardWithObstacles[p.X][p.Y];
					queue.Enqueue(new Point(p.X, p.Y - 1));
				}
				else if (boardWithObstacles[p.X][p.Y - 1] > boardWithObstacles[p.X][p.Y] + 1 && Program.playground.board[p.X][p.Y - 1] == Square.Wall)
				{
					boardWithObstacles[p.X][p.Y - 1] = boardWithObstacles[p.X][p.Y] + 1;
				}
				else if (boardWithObstacles[p.X][p.Y - 1] > boardWithObstacles[p.X][p.Y] && Program.playground.board[p.X][p.Y - 1] != Square.Wall)
				{
					boardWithObstacles[p.X][p.Y - 1] = boardWithObstacles[p.X][p.Y];
				}
				#endregion
				#region Up
				if (boardWithObstacles[p.X - 1][p.Y] == -2)
				{
					boardWithObstacles[p.X - 1][p.Y] = boardWithObstacles[p.X][p.Y] + 1;
					queue.Enqueue(new Point(p.X - 1, p.Y));
				}
				else if (boardWithObstacles[p.X - 1][p.Y] == -1)
				{
					boardWithObstacles[p.X - 1][p.Y] = boardWithObstacles[p.X][p.Y];
					queue.Enqueue(new Point(p.X - 1, p.Y));
				}
				else if (boardWithObstacles[p.X - 1][p.Y] > boardWithObstacles[p.X][p.Y] + 1 && Program.playground.board[p.X - 1][p.Y] == Square.Wall)
				{
					boardWithObstacles[p.X - 1][p.Y] = boardWithObstacles[p.X][p.Y] + 1;
				}
				else if (boardWithObstacles[p.X - 1][p.Y] > boardWithObstacles[p.X][p.Y] && Program.playground.board[p.X - 1][p.Y] != Square.Wall)
				{
					boardWithObstacles[p.X - 1][p.Y] = boardWithObstacles[p.X][p.Y];
				}
				#endregion
			} while (queue.Count != 0);
			queue.Enqueue(new Point(position.X, position.Y));
			do
			{
				Point p = queue.Dequeue();
				if (boardPossibleMovement[p.X][p.Y + 1] == -1)
				{
					boardPossibleMovement[p.X][p.Y + 1] = boardPossibleMovement[p.X][p.Y] + 1;
					queue.Enqueue(new Point(p.X, p.Y + 1));
				}
				if (boardPossibleMovement[p.X + 1][p.Y] == -1)
				{
					boardPossibleMovement[p.X + 1][p.Y] = boardPossibleMovement[p.X][p.Y] + 1;
					queue.Enqueue(new Point(p.X + 1, p.Y));
				}
				if (boardPossibleMovement[p.X][p.Y - 1] == -1)
				{
					boardPossibleMovement[p.X][p.Y - 1] = boardPossibleMovement[p.X][p.Y] + 1;
					queue.Enqueue(new Point(p.X, p.Y - 1));
				}
				if (boardPossibleMovement[p.X - 1][p.Y] == -1)
				{
					boardPossibleMovement[p.X - 1][p.Y] = boardPossibleMovement[p.X][p.Y] + 1;
					queue.Enqueue(new Point(p.X - 1, p.Y));
				}
			} while (queue.Count != 0);
			queue.Enqueue(new Point(position.X, position.Y));
			do
			{
				Point p = queue.Dequeue();
				if (boardPossiblePath[p.X][p.Y + 1] == -1)
				{
					boardPossiblePath[p.X][p.Y + 1] = boardPossiblePath[p.X][p.Y] + 1;
					queue.Enqueue(new Point(p.X, p.Y + 1));
				}
				if (boardPossiblePath[p.X + 1][p.Y] == -1)
				{
					boardPossiblePath[p.X + 1][p.Y] = boardPossiblePath[p.X][p.Y] + 1;
					queue.Enqueue(new Point(p.X + 1, p.Y));
				}
				if (boardPossiblePath[p.X][p.Y - 1] == -1)
				{
					boardPossiblePath[p.X][p.Y - 1] = boardPossiblePath[p.X][p.Y] + 1;
					queue.Enqueue(new Point(p.X, p.Y - 1));
				}
				if (boardPossiblePath[p.X - 1][p.Y] == -1)
				{
					boardPossiblePath[p.X - 1][p.Y] = boardPossiblePath[p.X][p.Y] + 1;
					queue.Enqueue(new Point(p.X - 1, p.Y));
				}
			} while (queue.Count != 0);
		}
		private void PrepareHeadingPoint(Point position)
		{
			int shortestPathObstacles = int.MaxValue;
			int shortestPathWithoutObstacles = int.MaxValue;
			for (int i = 0; i < 4; i++)
			{
				int tmp = boardWithObstacles[players[i].X][players[i].Y] * 5 + boardPossiblePath[players[i].X][players[i].Y];
				if (players[i] != new Point() && players[i] != position && tmp < shortestPathObstacles)
				{
					shortestPathObstacles = tmp;
					heading = players[i];
				}
				tmp = boardPossibleMovement[players[i].X][players[i].Y];
				if (players[i] != new Point() && players[i] != position && tmp > -1 && tmp < shortestPathWithoutObstacles)
				{
					shortestPathWithoutObstacles = tmp;
				}
			}
			FindPath(position, heading);
			if (shortestPathWithoutObstacles <= 3) enemyClose = true;
			else enemyClose = false;
		}
		private void FindPath(Point position, Point destination)
		{
			int size = boardPossiblePath[destination.X][destination.Y];
			Queue<Point> steps = new Queue<Point>();
			path.Clear();
			steps.Enqueue(destination);
			while (steps.Count != 0)
			{
				Point p = steps.Dequeue();
				if (p == position) break;
				size--;
				path.Push(p);
				if (boardPossiblePath[p.X][p.Y + 1] == boardPossiblePath[p.X][p.Y] - 1) steps.Enqueue(new Point(p.X, p.Y + 1));
				else if (boardPossiblePath[p.X + 1][p.Y] == boardPossiblePath[p.X][p.Y] - 1) steps.Enqueue(new Point(p.X + 1, p.Y));
				else if (boardPossiblePath[p.X][p.Y - 1] == boardPossiblePath[p.X][p.Y] - 1) steps.Enqueue(new Point(p.X, p.Y - 1));
				else if (boardPossiblePath[p.X - 1][p.Y] == boardPossiblePath[p.X][p.Y] - 1) steps.Enqueue(new Point(p.X - 1, p.Y));
			}
		}
		private Movement RandomStep(Point position)
		{
			if (saveMovement.Count != 0)
			{
				return saveMovement.Dequeue();
			}
			Movement movement = (Movement)(Program.random.Next(1, 6));
			if (movement == Movement.Plant_bomb)
			{
				RunAway(position, position);
			}
			return movement;
		}
		private Movement ProcessStep(Point position)
		{
			if (saveMovement.Count != 0)
			{
				return saveMovement.Dequeue();
			}
			if (path.Count == 0)
			{
				return RandomStep(position);
			}
			Point location = path.Pop();
			if (Program.playground.board[location.X][location.Y] == Square.Wall)
			{
				RunAway(position, position);
				return Movement.Plant_bomb;
			}
			else
			{
				return GetDirection(position, location);
			}
		}

		private Movement GetDirection(Point start, Point destination) // Have to be 1 step far from each other
		{
			if (start.X == destination.X && start.Y == destination.Y + 1) return Movement.Left;
			else if (start.X == destination.X + 1 && start.Y == destination.Y) return Movement.Up;
			else if (start.X == destination.X && start.Y == destination.Y - 1) return Movement.Right;
			else if (start.X == destination.X - 1 && start.Y == destination.Y) return Movement.Down;
			else return Movement.Nothing;
		}
		private void RunAway(Point bomb, Point location)
		{
			saveMovement.Clear();
			List<Point> dangerous = new List<Point>() { 
				bomb, 
				new Point(bomb.X, bomb.Y + 1),
				new Point(bomb.X + 1, bomb.Y),
				new Point(bomb.X, bomb.Y - 1),
				new Point(bomb.X - 1, bomb.Y)
				};
			Point newPosition, newPosition2;
			foreach (Movement move in new Movement[] { Movement.Up, Movement.Left, Movement.Down, Movement.Right })
			{
				if (Check(location, move, out newPosition) && !dangerous.Contains(newPosition))
				{
					saveMovement.Enqueue(move);
					return;
				}
			}
			foreach (Tuple<Movement, Movement> move in GameLogic.possibleDoubleMove)
			{
				if (Check(location, move.Item1, out newPosition) && Check(newPosition, move.Item2, out newPosition2) && !dangerous.Contains(newPosition2))
				{
					saveMovement.Enqueue(move.Item1);
					saveMovement.Enqueue(move.Item2);
					return;
				}
			}
		}
		/// <summary>
		/// Check if it is possible to change place specific direction
		/// </summary>
		/// <param name="position">Start position</param>
		/// <param name="movement">Wanted movement</param>
		/// <returns></returns>
		private bool Check(Point position, Movement movement, out Point newPosition)
		{
			Point oldPosition = position;
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
					newPosition = position;
					return false;
			}
			newPosition = position;
			if (position.X <= 0 || position.X >= Playground.playgroundSize - 1 || position.Y <= 0 || position.Y >= Playground.playgroundSize - 1) return false;
			Square square = Program.playground.board[position.X][position.Y];
			if (square == Square.Empty || square == Square.Fire || square == character) return true;
			else return false;
		}
		/// <summary>
		/// Save to players array location of player if he is on location
		/// </summary>
		/// <param name="location">coordinate on playground</param>
		private void RecognizePlayer(Point location)
		{
			Square square = Program.playground.board[location.X][location.Y];
			if (square == Square.Player_1 || square == Square.Bomb_1_1 || square == Square.Bomb_2_1 || square == Square.Bomb_3_1)
				players[0] = location;
			else if (square == Square.Player_2 || square == Square.Bomb_1_2 || square == Square.Bomb_2_2 || square == Square.Bomb_3_2)
				players[1] = location;
			else if (square == Square.Player_3 || square == Square.Bomb_1_3 || square == Square.Bomb_2_3 || square == Square.Bomb_3_3)
				players[2] = location;
			else if (square == Square.Player_4 || square == Square.Bomb_1_4 || square == Square.Bomb_2_4 || square == Square.Bomb_3_4)
				players[3] = location;
		}
	}
}
