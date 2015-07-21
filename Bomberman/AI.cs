using System;
using System.Collections.Generic;
using System.Drawing;

namespace Bomberman
{
	/// <summary>
	/// Class AI provides the possibility of calculating the next steps
	/// Use by computer-controlled characters
	/// </summary>
	class AI
	{
		Point target;
		Square character;
		bool enemyClose;
		Point[] players = new Point[4];
		int[][] boardWithObstacles;
		int[][] boardPossibleMovement;
		int[][] boardPossiblePath;
		Stack<Point> path = new Stack<Point>();
		Queue<Movement> savedMovement = new Queue<Movement>();

		public AI(Square character)
		{
			this.character = character;
		}
		/// <summary>
		/// Calculate next two movements
		/// </summary>
		/// <param name="position">Current position</param>
		/// <returns>Array of TWO movements</returns>
		internal Movement[] GetNextMovement(Point position)
		{
			Point temporaryPositions;
			Movement[] movements = new Movement[2];
			PrepareBoardMovement(position);
			SelectHeadingPoint(position);
			if (enemyClose) movements[0] = RandomStep(position);
			else movements[0] = MovingStep(position);
			if (Check(position, movements[0], out temporaryPositions)) position = temporaryPositions;
			if (enemyClose) movements[1] = RandomStep(position);
			else movements[1] = MovingStep(position);
			return movements;
		}
		/// <summary>
		/// Prepare boards with steps
		/// </summary>
		/// <param name="position">From position</param>
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
					Square square = Program.playground.board[i][j];

					if (square == Square.Unbreakable_Wall) boardWithObstacles[i][j] = -3;
					else if (square == Square.Wall) boardWithObstacles[i][j] = -2;
					else boardWithObstacles[i][j] = -1;

					if (square == Square.Empty || square == Square.Fire || (square >= Square.Player_1 && square <= Square.Player_4)) boardPossibleMovement[i][j] = -1;
					else boardPossibleMovement[i][j] = -2;

					if (square == Square.Unbreakable_Wall) boardPossiblePath[i][j] = -2;
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
				Point temporaryLocation = queue.Dequeue();
				#region Right
				if (boardWithObstacles[temporaryLocation.X][temporaryLocation.Y + 1] == -2)
				{
					boardWithObstacles[temporaryLocation.X][temporaryLocation.Y + 1] = boardWithObstacles[temporaryLocation.X][temporaryLocation.Y] + 1;
					queue.Enqueue(new Point(temporaryLocation.X, temporaryLocation.Y + 1));
				}
				else if (boardWithObstacles[temporaryLocation.X][temporaryLocation.Y + 1] == -1)
				{
					boardWithObstacles[temporaryLocation.X][temporaryLocation.Y + 1] = boardWithObstacles[temporaryLocation.X][temporaryLocation.Y];
					queue.Enqueue(new Point(temporaryLocation.X, temporaryLocation.Y + 1));
				}
				else if (boardWithObstacles[temporaryLocation.X][temporaryLocation.Y + 1] > boardWithObstacles[temporaryLocation.X][temporaryLocation.Y] + 1 && Program.playground.board[temporaryLocation.X][temporaryLocation.Y + 1] == Square.Wall)
				{
					boardWithObstacles[temporaryLocation.X][temporaryLocation.Y + 1] = boardWithObstacles[temporaryLocation.X][temporaryLocation.Y] + 1;
				}
				else if (boardWithObstacles[temporaryLocation.X][temporaryLocation.Y + 1] > boardWithObstacles[temporaryLocation.X][temporaryLocation.Y] && Program.playground.board[temporaryLocation.X][temporaryLocation.Y + 1] != Square.Wall)
				{
					boardWithObstacles[temporaryLocation.X][temporaryLocation.Y + 1] = boardWithObstacles[temporaryLocation.X][temporaryLocation.Y];
				}
				#endregion
				#region Down
				if (boardWithObstacles[temporaryLocation.X + 1][temporaryLocation.Y] == -2)
				{
					boardWithObstacles[temporaryLocation.X + 1][temporaryLocation.Y] = boardWithObstacles[temporaryLocation.X][temporaryLocation.Y] + 1;
					queue.Enqueue(new Point(temporaryLocation.X + 1, temporaryLocation.Y));
				}
				else if (boardWithObstacles[temporaryLocation.X + 1][temporaryLocation.Y] == -1)
				{
					boardWithObstacles[temporaryLocation.X + 1][temporaryLocation.Y] = boardWithObstacles[temporaryLocation.X][temporaryLocation.Y];
					queue.Enqueue(new Point(temporaryLocation.X + 1, temporaryLocation.Y));
				}
				else if (boardWithObstacles[temporaryLocation.X + 1][temporaryLocation.Y] > boardWithObstacles[temporaryLocation.X][temporaryLocation.Y] + 1 && Program.playground.board[temporaryLocation.X + 1][temporaryLocation.Y] == Square.Wall)
				{
					boardWithObstacles[temporaryLocation.X + 1][temporaryLocation.Y] = boardWithObstacles[temporaryLocation.X][temporaryLocation.Y] + 1;
				}
				else if (boardWithObstacles[temporaryLocation.X + 1][temporaryLocation.Y] > boardWithObstacles[temporaryLocation.X][temporaryLocation.Y] && Program.playground.board[temporaryLocation.X + 1][temporaryLocation.Y] != Square.Wall)
				{
					boardWithObstacles[temporaryLocation.X + 1][temporaryLocation.Y] = boardWithObstacles[temporaryLocation.X][temporaryLocation.Y];
				}
				#endregion
				#region Left
				if (boardWithObstacles[temporaryLocation.X][temporaryLocation.Y - 1] == -2)
				{
					boardWithObstacles[temporaryLocation.X][temporaryLocation.Y - 1] = boardWithObstacles[temporaryLocation.X][temporaryLocation.Y] + 1;
					queue.Enqueue(new Point(temporaryLocation.X, temporaryLocation.Y - 1));
				}
				else if (boardWithObstacles[temporaryLocation.X][temporaryLocation.Y - 1] == -1)
				{
					boardWithObstacles[temporaryLocation.X][temporaryLocation.Y - 1] = boardWithObstacles[temporaryLocation.X][temporaryLocation.Y];
					queue.Enqueue(new Point(temporaryLocation.X, temporaryLocation.Y - 1));
				}
				else if (boardWithObstacles[temporaryLocation.X][temporaryLocation.Y - 1] > boardWithObstacles[temporaryLocation.X][temporaryLocation.Y] + 1 && Program.playground.board[temporaryLocation.X][temporaryLocation.Y - 1] == Square.Wall)
				{
					boardWithObstacles[temporaryLocation.X][temporaryLocation.Y - 1] = boardWithObstacles[temporaryLocation.X][temporaryLocation.Y] + 1;
				}
				else if (boardWithObstacles[temporaryLocation.X][temporaryLocation.Y - 1] > boardWithObstacles[temporaryLocation.X][temporaryLocation.Y] && Program.playground.board[temporaryLocation.X][temporaryLocation.Y - 1] != Square.Wall)
				{
					boardWithObstacles[temporaryLocation.X][temporaryLocation.Y - 1] = boardWithObstacles[temporaryLocation.X][temporaryLocation.Y];
				}
				#endregion
				#region Up
				if (boardWithObstacles[temporaryLocation.X - 1][temporaryLocation.Y] == -2)
				{
					boardWithObstacles[temporaryLocation.X - 1][temporaryLocation.Y] = boardWithObstacles[temporaryLocation.X][temporaryLocation.Y] + 1;
					queue.Enqueue(new Point(temporaryLocation.X - 1, temporaryLocation.Y));
				}
				else if (boardWithObstacles[temporaryLocation.X - 1][temporaryLocation.Y] == -1)
				{
					boardWithObstacles[temporaryLocation.X - 1][temporaryLocation.Y] = boardWithObstacles[temporaryLocation.X][temporaryLocation.Y];
					queue.Enqueue(new Point(temporaryLocation.X - 1, temporaryLocation.Y));
				}
				else if (boardWithObstacles[temporaryLocation.X - 1][temporaryLocation.Y] > boardWithObstacles[temporaryLocation.X][temporaryLocation.Y] + 1 && Program.playground.board[temporaryLocation.X - 1][temporaryLocation.Y] == Square.Wall)
				{
					boardWithObstacles[temporaryLocation.X - 1][temporaryLocation.Y] = boardWithObstacles[temporaryLocation.X][temporaryLocation.Y] + 1;
				}
				else if (boardWithObstacles[temporaryLocation.X - 1][temporaryLocation.Y] > boardWithObstacles[temporaryLocation.X][temporaryLocation.Y] && Program.playground.board[temporaryLocation.X - 1][temporaryLocation.Y] != Square.Wall)
				{
					boardWithObstacles[temporaryLocation.X - 1][temporaryLocation.Y] = boardWithObstacles[temporaryLocation.X][temporaryLocation.Y];
				}
				#endregion
			} while (queue.Count != 0);
			#region boardPossibleMovement
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
			#endregion
			#region boardPossiblePath
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
			#endregion
		}
		private void SelectHeadingPoint(Point position)
		{
			int shortestPathObstacles = int.MaxValue;
			int shortestPathWithoutObstacles = int.MaxValue;
			for (int i = 0; i < 4; i++)
			{
				int tmp = boardWithObstacles[players[i].X][players[i].Y] * 5 + boardPossiblePath[players[i].X][players[i].Y];
				if (players[i] != new Point() && players[i] != position && tmp < shortestPathObstacles)
				{
					shortestPathObstacles = tmp;
					target = players[i];
				}
				tmp = boardPossibleMovement[players[i].X][players[i].Y];
				if (players[i] != new Point() && players[i] != position && tmp > -1 && tmp < shortestPathWithoutObstacles)
				{
					shortestPathWithoutObstacles = tmp;
				}
			}
			FindPath(position, target);
			if (shortestPathWithoutObstacles <= 3) enemyClose = true;
			else enemyClose = false;
		}
		private void FindPath(Point position, Point destination)
		{
			Queue<Point> steps = new Queue<Point>();
			path.Clear();
			steps.Enqueue(destination);
			while (steps.Count != 0)
			{
				Point temporaryPosition = steps.Dequeue();
				if (temporaryPosition == position) break;
				path.Push(temporaryPosition);
				if (boardPossiblePath[temporaryPosition.X][temporaryPosition.Y + 1] == boardPossiblePath[temporaryPosition.X][temporaryPosition.Y] - 1) steps.Enqueue(new Point(temporaryPosition.X, temporaryPosition.Y + 1));
				else if (boardPossiblePath[temporaryPosition.X + 1][temporaryPosition.Y] == boardPossiblePath[temporaryPosition.X][temporaryPosition.Y] - 1) steps.Enqueue(new Point(temporaryPosition.X + 1, temporaryPosition.Y));
				else if (boardPossiblePath[temporaryPosition.X][temporaryPosition.Y - 1] == boardPossiblePath[temporaryPosition.X][temporaryPosition.Y] - 1) steps.Enqueue(new Point(temporaryPosition.X, temporaryPosition.Y - 1));
				else if (boardPossiblePath[temporaryPosition.X - 1][temporaryPosition.Y] == boardPossiblePath[temporaryPosition.X][temporaryPosition.Y] - 1) steps.Enqueue(new Point(temporaryPosition.X - 1, temporaryPosition.Y));
			}
		}
		/// <summary>
		/// Randomly choose next movement
		/// using System.Random
		/// </summary>
		/// <param name="position">Current position</param>
		/// <returns>Random movement</returns>
		private Movement RandomStep(Point position)
		{
			if (savedMovement.Count != 0)
			{
				return savedMovement.Dequeue();
			}
			Movement movement = (Movement)(Program.random.Next(1, 6));
			if (movement == Movement.Plant_bomb)
			{
				RunAway(position, position);
			}
			return movement;
		}
		/// <summary>
		/// Get the next step following calculated path
		/// </summary>
		/// <param name="position">Current position</param>
		/// <returns>Next movement</returns>
		private Movement MovingStep(Point position)
		{
			if (savedMovement.Count != 0)
			{
				return savedMovement.Dequeue();
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
				return GetMovementDirection(position, location);
			}
		}
		private Movement GetMovementDirection(Point start, Point destination) // Have to be 1 step far from each other
		{
			if (start.X == destination.X && start.Y == destination.Y + 1) return Movement.Left;
			else if (start.X == destination.X + 1 && start.Y == destination.Y) return Movement.Up;
			else if (start.X == destination.X && start.Y == destination.Y - 1) return Movement.Right;
			else if (start.X == destination.X - 1 && start.Y == destination.Y) return Movement.Down;
			else return Movement.Nothing;
		}
		/// <summary>
		/// Find and save future steps to escape the explosion
		/// </summary>
		/// <param name="bomb">Location of bomb</param>
		/// <param name="location">Player location</param>
		private void RunAway(Point bomb, Point location)
		{
			savedMovement.Clear();
			List<Point> dangerous = new List<Point>() { 
				bomb, 
				new Point(bomb.X, bomb.Y + 1),
				new Point(bomb.X + 1, bomb.Y),
				new Point(bomb.X, bomb.Y - 1),
				new Point(bomb.X - 1, bomb.Y)
				};
			Point temporaryLocation, endingLocation;
			foreach (Movement move in new Movement[] { Movement.Up, Movement.Left, Movement.Down, Movement.Right })
			{
				if (Check(location, move, out temporaryLocation) && !dangerous.Contains(temporaryLocation))
				{
					savedMovement.Enqueue(move);
					return;
				}
			}
			foreach (Tuple<Movement, Movement> move in GameLogic.possibleDoubleMove)
			{
				if (Check(location, move.Item1, out temporaryLocation) && Check(temporaryLocation, move.Item2, out endingLocation) && !dangerous.Contains(endingLocation))
				{
					savedMovement.Enqueue(move.Item1);
					savedMovement.Enqueue(move.Item2);
					return;
				}
			}
		}
		/// <summary>
		/// Check if the next move is possible
		/// </summary>
		/// <param name="position">Current position</param>
		/// <param name="movement">Next movement</param>
		/// <param name="newPosition">If the movement is valid this out parametr holds new location</param>
		/// <returns>Possibility of movement</returns>
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
		/// Save players location
		/// </summary>
		/// <param name="location">Location on playground</param>
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
