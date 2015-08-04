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
		static List<Connection> clientsAll;
		static List<Connection> clientsPlaying;
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
			clientsPlaying = new List<Connection>();
			clientUpdate = new List<Connection>();
			clientAI = new List<Connection>();
			clientPlayers = new List<Connection>();
			clientsAll = new List<Connection>();
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
				lock(clientsPlaying){
					response = "ACK " + clientsPlaying.Count;
					connection.position = GameLogic.GetStartPosition(clientsPlaying.Count.ToString());
					clientsPlaying.Add(connection);
					clientsAll.Add(connection);
					connection.playerNumber = clientsPlaying.Count;
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
				if (clientsPlaying.Count == 4) SendInitUpdate();
				StartListening(connection);
			}
		}
		private static void SendInitUpdate()
		{
			for (int i = 0; i < clientsPlaying.Count; i++)
			{
				if (clientAI.Contains(clientsPlaying[i]))
				{
					SendUpdate("Update " + clientsPlaying[i].playerNumber + " AI");
				}
				else
				{

					SendUpdate("Update " + clientsPlaying[i].playerNumber + " Alive");
				}
			}
			SendUpdate("Start");
		}
		/// <summary>
		/// Send to coresponding client to connection whole playground
		/// </summary>
		/// <param name="connection">Receiver connection</param>
		private static void SendPlayground(Connection connection)
		{
			StringBuilder message = new StringBuilder("Playground " + Playground.playgroundSize + " ");
			for (int i = 0; i < Playground.playgroundSize; i++)
			{
				for (int j = 0; j < Playground.playgroundSize; j++)
				{
					message.Append((int)Program.playground.board[i][j] + " ");
				}
			}
			string data = message.ToString();
			Send(data, connection);
		}
		/// <summary>
		/// Send to coresponding client to connection changes on playground
		/// </summary>
		/// <param name="connection">Receiver connection</param>
		private static void SendChanges(Connection connection)
		{
			StringBuilder message = new StringBuilder("Change " + GameLogic.changes.Count + " ");
			for (int i = 0; i < GameLogic.changes.Count; i++)
			{
				Change change = GameLogic.changes[i];
				message.Append(change.coordinate.X + " " + change.coordinate.Y + " " + (int)change.square + " ");
			}
			Send(message.ToString(), connection);
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
					if (futureMoves.Count == clientsPlaying.Count * 2) ProcessMoves();
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
					AddMoves(tokens, connection);
					break;
				default:
					break;
			}
		}
		private static void AddMoves(string[] moves, Connection connection)
		{
			if (!clientsPlaying.Contains(connection)) return; // check if sender is regular client
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
					if (futureMoves.Count == clientsPlaying.Count * 2) ProcessMoves();
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
			if (clientsPlaying.Count == 1) EndOfGame();
			else Form1.waiting = false;
		}
		private static void EndOfGame()
		{
            lock (clientsPlaying) if (clientsPlaying.Count == 1) SendUpdate("Update " + clientsPlaying[0].playerNumber + " WINNER!");
        }
		private static void Clean(Connection connection)
		{
			try
			{
				clientsPlaying.Remove(connection);
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
				if (futureMoves.Count == clientsPlaying.Count * 2) ProcessMoves();
			}
		}
		/// <summary>
		/// Find out who died on given location and send update
		/// </summary>
		/// <param name="location">Coordinate of dead</param>
		internal static void Dead(Point location)
		{
			Connection connection = clientsPlaying[0]; // just init value
			for (int i = 0; i < clientsPlaying.Count; i++)
			{
				if (clientsPlaying[i].position == location)
				{
					connection = clientsPlaying[i];
					break;
				}
			}
			SendUpdate("Update " + connection.playerNumber + " Dead");
			if (clientPlayers.Contains(connection)) Send("Dead", connection);
			Clean(connection);
		}
		/// <summary>
		/// Send message to all clients in clientUpdate
		/// </summary>
		private static void SendAll(string msg) 
		{
			for (int i = 0; i < clientUpdate.Count; i++)
			{
				Send(msg, clientUpdate[i]);
			}
		}
		/// <summary>
		/// Send message to all clients in clientUpdate and player running server
		/// </summary>
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
					if (futureMoves.Count == clientsPlaying.Count * 2) ProcessMoves();
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
			for (int i = 0; i < clientsAll.Count; i++)
			{
				Send("Stop", clientsAll[i]);
			}
		}
	}
}
