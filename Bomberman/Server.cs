using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Drawing;
using System.Threading;
using System.Timers;

namespace Bomberman
{
	class Server
	{
		static TcpListener listener;
		static List<Connection> clients;
		static List<Connection> clientUpdate;
		static List<Connection> clientAI;
		static List<Connection> clientPlayers;
		static Queue<FutureMove> futureMoves;
		static ManualResetEvent allDone;
		static System.Timers.Timer updateTimer = new System.Timers.Timer(1000);

		/// <summary>
		/// Start server and begin accepting clients
		/// </summary>
		public static void Start()
		{
			futureMoves = new Queue<FutureMove>();
			clients = new List<Connection>();
			clientUpdate = new List<Connection>();
			clientAI = new List<Connection>();
			clientPlayers = new List<Connection>();
			updateTimer.Elapsed += new ElapsedEventHandler(UpdateTick);
			updateTimer.AutoReset = false;
			if (listener != null) listener.Stop();
			listener = TcpListener.Create(Program.port);
			listener.Start();
			allDone = new ManualResetEvent(false);
			try
			{
				while (true)
				{
					allDone.Reset();
					listener.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpClient), listener);
					allDone.WaitOne();
				}
			}
			catch (ThreadAbortException)
			{
				listener.Stop();
			}
		}
		private static void AcceptTcpClient(IAsyncResult ar)
		{
			try
			{
				allDone.Set();
				TcpListener listener = (TcpListener)ar.AsyncState;
				TcpClient client = listener.EndAcceptTcpClient(ar);
				Connection connection = new Connection(client);
				Handshake(connection);
			}
			catch (ObjectDisposedException) // server has been stopped
			{
				return;
			}
		}
		private static async void Handshake(Connection connection)
		{
			string line = await connection.reader.ReadLineAsync();
			string[] tokens = line.Split(' ');
			if (tokens[0] == "Bomberman")
			{
				string response;
				lock(clients){
					response = "ACK " + clients.Count;
					connection.position = GameLogic.GetStartPosition(clients.Count.ToString());
					clients.Add(connection);
					connection.playerNumber = clients.Count;
				}
				Send(response, connection);
				if (tokens[1] == true.ToString()) // client need updates
				{
					lock (clientUpdate) clientUpdate.Add(connection);
					SendPlayground(connection);
				}
				if (tokens[2] == false.ToString()) // client is AI
				{
					lock (clientAI) clientAI.Add(connection);
					SendAll("Update " + connection.playerNumber + " AI");
				}
				else // client is user
				{
					lock (clientPlayers) clientPlayers.Add(connection);
					SendAll("Update " + connection.playerNumber + " Alive");
				}
				if (clients.Count == 4) SendInitUpdate();
				StartListening(connection);
			}
		}
		private static void SendInitUpdate()
		{
			for (int i = 0; i < clients.Count; i++)
			{
				if (clientAI.Contains(clients[i]))
				{
					SendUpdate("Update " + clients[i].playerNumber + " AI");
				}
				else
				{

					SendUpdate("Update " + clients[i].playerNumber + " Alive");
				}
			}
			SendUpdate("Start");
		}
		/// <summary>
		/// Send to coresponding client whole playground
		/// </summary>
		/// <param name="connection">Receiver connection</param>
		private static void SendPlayground(Connection connection)
		{
			StringBuilder sb = new StringBuilder("Playground " + Playground.playgroundSize + " ");
			for (int i = 0; i < Playground.playgroundSize; i++)
			{
				for (int j = 0; j < Playground.playgroundSize; j++)
				{
					sb.Append((int)Program.playground.board[i][j] + " ");
				}
			}
			string data = sb.ToString();
			Send(data, connection);
		}
		/// <summary>
		/// Send to coresponding client changes on playground
		/// </summary>
		/// <param name="connection">Receiver connection</param>
		private static void SendChanges(Connection connection)
		{
			StringBuilder sb = new StringBuilder("Change " + GameLogic.changes.Count + " ");
			for (int i = 0; i < GameLogic.changes.Count; i++)
			{
				Change change = GameLogic.changes[i];
				sb.Append(change.coordinate.X + " " + change.coordinate.Y + " " + (int)change.square + " ");
			}
			Send(sb.ToString(), connection);
		}
		private static async void StartListening(Connection connection)
		{
			string line;
			try
			{
				while (connection.client.Connected)
				{
					line = await connection.reader.ReadLineAsync();
					ProcessCommand(line, connection);
				}
			}
			catch (System.IO.IOException) // connection with client has been lost
			{
				clientUpdate.Remove(connection);
				Clean(connection);
				SendUpdate("Update " + connection.playerNumber + " Disconected");
				connection.client.Close();
				lock (futureMoves)
				{
					if (futureMoves.Count == clients.Count * 2) ProcessMoves();
				}
			}
			catch (NullReferenceException) // client quit game
			{
				clientUpdate.Remove(connection);
				Clean(connection);
				SendUpdate("Update " + connection.playerNumber + " Quit");
				connection.client.Close();
			}
		}
		private static void ProcessCommand(string command, Connection connection)
		{
			string[] tokens = command.Split(' ');
			switch (tokens[0])
			{
				case "Move":
					AddMove(tokens, connection);
					break;
				default:
					break;
			}
		}
		private static void AddMove(string[] moves, Connection connection)
		{
			if (!clients.Contains(connection)) return; // check if sender is regular client
			lock (futureMoves)
			{
				if (futureMoves.Count == 0)
				{
					for (int i = 0; i < clientAI.Count; i++)
					{
						Send("SendMoves", clientAI[i]);
					}
				}
				Movement movement = (Movement)int.Parse(moves[1]);
				futureMoves.Enqueue(new FutureMove(movement, connection));
				movement = (Movement)int.Parse(moves[2]);
				futureMoves.Enqueue(new FutureMove(movement, connection));
				lock (futureMoves)
				{
					if (futureMoves.Count == clients.Count * 2) ProcessMoves();
				}
			}
		}
		private static void ProcessMoves()
		{
			Program.playground.UpdateFire();
			int moveCount = futureMoves.Count / 2;
			for (int i = 0; i < moveCount; i++)
			{
				FutureMove futureMove = futureMoves.Dequeue();
				GameLogic.Process(futureMove.movement, futureMove.connection);
				futureMoves.Enqueue(futureMoves.Dequeue());
			}
			Program.playground.UpdateBombs();
			for (int i = 0; i < clientUpdate.Count; i++)
			{
				SendChanges(clientUpdate[i]);
			}
			GameLogic.changes.Clear();
			Form1.updatePictureBox();
			Program.playground.UpdateFire();
			moveCount = futureMoves.Count;
			for (int i = 0; i < moveCount; i++)
			{
				FutureMove futureMove = futureMoves.Dequeue();
				GameLogic.Process(futureMove.movement, futureMove.connection);
			}
			Program.playground.UpdateBombs();
			updateTimer.Enabled = true;
		}
		private static void UpdateTick(Object source, ElapsedEventArgs e)
		{
			for (int i = 0; i < clientUpdate.Count; i++)
			{
				SendChanges(clientUpdate[i]);
			}
			Form1.updatePictureBox();
			if (clients.Count == 1) EndOfGame();
			else Form1.waiting = false;
		}
		private static void EndOfGame()
		{
			SendUpdate("Update " + clients[0].playerNumber + " WINNER!");
		}
		private static void Clean(Connection connection)
		{
			try
			{
				clients.Remove(connection);
				clientAI.Remove(connection);
			}
			catch (ArgumentOutOfRangeException) // Called in middle of stopping server
			{
				return;
			} 
			for (int i = 0; i < futureMoves.Count; i++)
			{
				FutureMove tmp = futureMoves.Dequeue();
				if (tmp.connection != connection) futureMoves.Enqueue(tmp);
			}
			Program.playground.board[connection.position.X][connection.position.Y] = Square.Empty;
			GameLogic.changes.Add(new Change(new Point(connection.position.X, connection.position.Y), Square.Empty));
			lock (futureMoves)
			{
				if (futureMoves.Count == clients.Count * 2) ProcessMoves();
			}
		}
		/// <summary>
		/// Find out who died on given location and send update
		/// </summary>
		/// <param name="location">Coordinate of dead</param>
		internal static void Dead(Point location)
		{
			Connection connection = clients[0]; // just init value
			for (int i = 0; i < clients.Count; i++)
			{
				if (clients[i].position == location)
				{
					connection = clients[i];
					break;
				}
			}
			SendUpdate("Update " + connection.playerNumber + " Dead");
			if (clientPlayers.Contains(connection)) Send("Dead", connection);
			Clean(connection);
		}
		private static void SendAll(string msg)
		{
			for (int i = 0; i < clientUpdate.Count; i++)
			{
				Send(msg, clientUpdate[i]);
			}
		}
		private static void SendUpdate(string msg)
		{
			for (int i = 0; i < clientUpdate.Count; i++)
			{
				Send(msg, clientUpdate[i]);
			}
			if (clientPlayers.Count != 0) Send(msg, clientPlayers[0]);
		}
		private static void Send(string msg, Connection connection)
		{
			try
			{
				connection.writer.WriteLine(msg);
			}
			catch (System.IO.IOException) // client disconected
			{
				clientUpdate.Remove(connection);
				Clean(connection);
				SendUpdate("Update " + connection.playerNumber + " Disconected");
				connection.client.Close();
				lock (futureMoves)
				{
					if (futureMoves.Count == clients.Count * 2) ProcessMoves();
				}
				return;
			}
			catch (ObjectDisposedException) // Called in middle of stopping server
			{
				return;
			}
		}
		/// <summary>
		/// Send STOP command to all clients
		/// </summary>
		internal static void Stop()
		{
			for (int i = 0; i < clients.Count; i++)
			{
				Send("Stop", clients[i]);
			}
		}
	}
}
