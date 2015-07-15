using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

using System.Timers;

namespace Bomberman
{
	class Server
	{
		static List<Connection> clients = new List<Connection>();
		static List<Connection> clientUpdate = new List<Connection>();
		static List<Connection> clientAI = new List<Connection>();
		static List<Connection> clientPlayers = new List<Connection>();
		static ManualResetEvent allDone;
		static System.Timers.Timer updateTimer = new System.Timers.Timer(1000);

		/// <summary>
		/// Start server and begin accepting clients
		/// </summary>
		public static void Start()
		{
			updateTimer.Elapsed += new ElapsedEventHandler(UpdateTick);
			updateTimer.AutoReset = false;
			TcpListener listener = TcpListener.Create(Program.port);
			listener.Start();
			allDone = new ManualResetEvent(false);
			listener.Start();
			while (true)
			{
				allDone.Reset();
				listener.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpClient), listener);
				allDone.WaitOne();
			}
		}
		private static void AcceptTcpClient(IAsyncResult ar)
		{
			allDone.Set();
			TcpListener listener = (TcpListener)ar.AsyncState;
			TcpClient client = listener.EndAcceptTcpClient(ar);
			Connection connection = new Connection(client);
			Handshake(connection);
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
				connection.writer.WriteLine(response);
				if (tokens[1] == true.ToString()) // if client need updates
				{
					lock (clientUpdate) clientUpdate.Add(connection);
					SendPlayground(connection);
				}
				if (tokens[2] == false.ToString()) // if client is AI
				{
					lock (clientAI) clientAI.Add(connection);
					SendAll("Update " + connection.playerNumber + " AI");
				}
				else // client is Player
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
		}
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
		private static void SendChanges(Connection connection)
		{
			StringBuilder sb = new StringBuilder("Change " + GameLogic.changes.Count + " ");
			for (int i = 0; i < GameLogic.changes.Count; i++)
			{
				Change change = GameLogic.changes[i];
				sb.Append(change.coordinate.X + " " + change.coordinate.Y + " " + (int)change.square + " ");
			}
			connection.writer.WriteLine(sb.ToString());
		}
		private static async void StartListening(Connection connection)
		{
			string line;
			try
			{
				while (connection.connectionWith.Connected)
				{
					line = await connection.reader.ReadLineAsync();
					ProcessCommand(line, connection);
				}
			}
			catch (System.IO.IOException)
			{
				//TESTING
				//Send("Stop", clientAI[0]);
				//TESTING
				clientUpdate.Remove(connection);
				Clean(connection);
				SendUpdate("Update " + connection.playerNumber + " Disconected");
				connection.connectionWith.Close();
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
		static Queue<FutureMove> futureMoves = new Queue<FutureMove>();
		private static void AddMove(string[] moves, Connection connection)
		{
			if (!clients.Contains(connection)) return; // check if moves send regular client
			if (futureMoves.Count == 0)
			{
				for (int i = 0; i < clientAI.Count; i++)
				{
					clientAI[i].writer.WriteLine("SendMoves");
				}
			}
			Movement movement = (Movement)int.Parse(moves[1]);
			futureMoves.Enqueue(new FutureMove(movement, connection));
			movement = (Movement)int.Parse(moves[2]);
			futureMoves.Enqueue(new FutureMove(movement, connection));
			//if (futureMoves.Count == clients.Count * 2) ProcessMove();
			if (futureMoves.Count == clientPlayers.Count * 2) ProcessMove();  // for testing
		}
		private static void ProcessMove()
		{
			int moveCount = futureMoves.Count / 2;
			for (int i = 0; i < moveCount; i++)
			{
				FutureMove futureMove = futureMoves.Dequeue();
				GameLogic.Process(Program.playground, futureMove.movement, futureMove.connection);
				futureMoves.Enqueue(futureMoves.Dequeue());
			}
			Program.playground.UpdateBombsFire();
			for (int i = 0; i < clientUpdate.Count; i++)
			{
				//SendPlayground(clientUpdate[i]);
				SendChanges(clientUpdate[i]);
			}
			GameLogic.changes.Clear();
			Form1.updatePictureBox();
			for (int i = 0; i < moveCount; i++)
			{
				FutureMove futureMove = futureMoves.Dequeue();
				GameLogic.Process(Program.playground, futureMove.movement, futureMove.connection);
			}
			Program.playground.UpdateBombsFire();
			updateTimer.Enabled = true;
		}
		private static void UpdateTick(Object source, ElapsedEventArgs e)
		{
			for (int i = 0; i < clientUpdate.Count; i++)
			{
				SendPlayground(clientUpdate[i]);
			}
			Form1.updatePictureBox();
			Form1.waiting = false;
		}
		private static void Clean(Connection connection)
		{
			clients.Remove(connection);
			clientAI.Remove(connection);
			clientPlayers.Remove(connection);
			Program.playground.board[connection.position.X][connection.position.Y] = Square.Empty;
			GameLogic.changes.Add(new Change(new Point(connection.position.X, connection.position.Y), Square.Empty));
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
			Send(msg, clientPlayers[0]);
		}
		private static void Send(string msg, Connection connection)
		{
			connection.writer.WriteLine(msg);
		}
	}
}
