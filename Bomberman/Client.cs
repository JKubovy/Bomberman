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
		private TcpClient server;
		private StreamWriter writer;
		private StreamReader reader;
		internal Point position;

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
					PrepareHeadingPoint();
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
				catch (IOException)
				{
					// TODO error
				}
				// TESTING
				catch (TaskCanceledException) // Thread is canceled
				{
					break;
				}
				// TESTING
			}
		}
		private void ProcessCommand(string command)
		{
			string[] tokens = command.Split(' ');
			switch (tokens[0])
			{
				case "SendMoves":
					// TODO vypocitat a poslat tahy
					PrepareHeadingPoint();
					CalculateFutureMoves();
					updatePosition(futureMoves[0]);
					updatePosition(futureMoves[1]);
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
				case "Stop":
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
				// TODO error
			}
		}

		Point heading;
		bool enemyClose; 
		Point[] players = new Point[4];
		int[][] boardWithObstacles;
		int[][] boardPossibleMovement;
		int[][] boardPossiblePath;
		Stack<Point> path = new Stack<Point>();
		Queue<Movement> saveMovement = new Queue<Movement>();
		private void PrepareBoardMovement()
		{
			boardWithObstacles = new int[Playground.playgroundSize][];
			boardPossibleMovement = new int[Playground.playgroundSize][];
			boardPossiblePath = new int[Playground.playgroundSize][];
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

					if (sqare == Square.Empty || sqare == Square.Fire) boardPossibleMovement[i][j] = -1;
					else boardPossibleMovement[i][j] = -2;

					if (sqare == Square.Unbreakable_Wall) boardPossiblePath[i][j] = -2;
					else boardPossiblePath[i][j] = -1;

					FindPlayer(new Point(i, j));
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
					queue.Enqueue(new Point(p.X,p.Y + 1));
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
		private void PrepareHeadingPoint()
		{
			PrepareBoardMovement();
			int shortestPathObstacles = Playground.playgroundSize * Playground.playgroundSize;
			int shortestPathWithoutObstacles = Playground.playgroundSize * Playground.playgroundSize;
			for (int i = 0; i < 4; i++)
			{
				int tmp = boardWithObstacles[players[i].X][players[i].Y];
				if (tmp != 0 && tmp < shortestPathObstacles)
				{
					shortestPathObstacles = tmp;
					heading = players[i];
				}
				tmp = boardPossibleMovement[players[i].X][players[i].Y];
				if (tmp != -2 && tmp != 0 && tmp < shortestPathWithoutObstacles)
				{
					shortestPathWithoutObstacles = tmp;
				}
			}
			FindPath(heading);
			if (shortestPathWithoutObstacles <= 3) enemyClose = true;
			else enemyClose = false;
		}
		private void FindPath(Point destination)
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
		private void CalculateFutureMoves()
		{
			/*
			// TESTING
			Movement[] m = {Movement.Up, Movement.Right, Movement.Left, Movement.Down};
			Random r = new Random();
			futureMoves[0] = m[r.Next(4)];
			futureMoves[1] = m[r.Next(4)];
			indexFutureMoves = 2;
			// TESTING
			*/
			if (enemyClose)
			{

			}
			else
			{
				ProcessStep();
				ProcessStep();
			}
		}
		private void ProcessStep()
		{
			if (saveMovement.Count != 0)
			{
				futureMoves[indexFutureMoves] = saveMovement.Dequeue();
				indexFutureMoves++;
				return;
			}
			Point location = path.Pop();
			if (Program.playground.board[location.X][location.Y] == Square.Wall)
			{
				futureMoves[indexFutureMoves] = Movement.Plant_bomb;
				indexFutureMoves++;
				if (indexFutureMoves == 2)
				{
					Point tmp;
					Check(position, futureMoves[0], out tmp);
					RunAway(tmp, tmp);
				}
				else RunAway(position, position);
			}
			else
			{
				Movement movement;
				if (indexFutureMoves == 1)
				{
					Point tmp;
					Check(position, futureMoves[0], out tmp);
					movement = GetDirection(tmp, location);	
				}
				else movement = GetDirection(position, location);
				futureMoves[indexFutureMoves] = movement;
				indexFutureMoves++;
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
			Point newPosition,newPosition2;
			foreach (Movement move in new Movement[]{Movement.Up, Movement.Left, Movement.Down, Movement.Right})
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

			#region Trash
			/*
			int[][] board = new int[5][];
			for (int i = 0; i < 5; i++)
			{
				board[i] = new int[5];
			}
			for (int i = -2; i < 3; i++)
			{
				for (int j = -2; j < 3; j++)
				{
					Square square = Program.playground.board[location.X + i][location.Y + j];
					if (square == Square.Empty || square == Square.Fire) board[i + 2][j + 2] = -1;
					else board[i + 2][j + 2] = -2;
				}
			}
			board[2][2] = 0;
			Queue<Point> queue = new Queue<Point>();
			queue.Enqueue(new Point(2, 2));
			do
			{
				Point p = queue.Dequeue();
				if (board[p.X][p.Y + 1] == -1)
				{
					board[p.X][p.Y + 1] = board[p.X][p.Y] + 1;
					queue.Enqueue(new Point(p.X, p.Y + 1));
				}
				if (board[p.X + 1][p.Y] == -1)
				{
					board[p.X + 1][p.Y] = board[p.X][p.Y] + 1;
					queue.Enqueue(new Point(p.X + 1, p.Y));
				}
				if (board[p.X][p.Y - 1] == -1)
				{
					board[p.X][p.Y - 1] = board[p.X][p.Y] + 1;
					queue.Enqueue(new Point(p.X, p.Y - 1));
				}
				if (board[p.X - 1][p.Y] == -1)
				{
					board[p.X - 1][p.Y] = board[p.X][p.Y] + 1;
					queue.Enqueue(new Point(p.X - 1, p.Y));
				}
			} while (queue.Count != 0);
			*/
			#endregion
		}
		/// <summary>
		/// Check if it is possible to change place specific direction
		/// </summary>
		/// <param name="position">Start position</param>
		/// <param name="movement">Wanted movement</param>
		/// <returns></returns>
		private bool Check(Point position, Movement movement, out Point newPosition)
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
					newPosition = position;
					return false;
			}
			Square square = Program.playground.board[position.X][position.Y];
			newPosition = position;
			if (square == Square.Empty || square == Square.Fire || square == Program.playground.board[this.position.X][this.position.Y]) return true;
			else return false;
		}

		/// <summary>
		/// Save to players array location of player if he is on location
		/// </summary>
		/// <param name="location">coordinate on playground</param>
		private void FindPlayer(Point location)
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
