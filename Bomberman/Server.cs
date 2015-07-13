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
		static ManualResetEvent getMoves = new ManualResetEvent(false);
		static System.Timers.Timer updateTimer = new System.Timers.Timer(1000);

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
				}
				connection.writer.WriteLine(response);
				if (tokens[1] == true.ToString()) // if client need updates
				{
					//connection.playgroundUpdates = true;
					lock (clientUpdate) clientUpdate.Add(connection);
					SendPlayground2(connection);
				}
				if (tokens[2] == false.ToString()) // if client is AI
				{
					lock (clientAI) clientAI.Add(connection);
					// TODO user (cekat na pokyny)
				}
				else
				{
					lock (clientPlayers) clientPlayers.Add(connection);
				}
				StartListening(connection);
			}
		}

		private static void SendPlayground(Connection connection)
		{
			connection.writer.WriteLine("Playground " + Playground.playgroundSize);
			for (int i = 0; i < Playground.playgroundSize; i++)
			{
				string line = "";
				for (int j = 0; j < Playground.playgroundSize; j++)
				{
					line += (int)Program.playground.board[i][j] + " ";
				}
				connection.writer.WriteLine(line);
			}
			connection.writer.WriteLine("End");
		}
		private static void SendPlayground2(Connection connection)
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
			//connection.writer.WriteLineAsync(sb.ToString());
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
				connection.connectionWith.Close();
				// TODO error spadlo spojeni
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
		//static int playersPlayed = 0;
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
			if (futureMoves.Count == clientPlayers.Count * 2) ProcessMove();
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
				SendPlayground2(clientUpdate[i]);
			}
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
				SendPlayground2(clientUpdate[i]);
			}
			Form1.updatePictureBox();
		}
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
			clients.Remove(connection);
			clientAI.Remove(connection);
			clientPlayers.Remove(connection);
			clientUpdate.Remove(connection); // TODO nechat pro sledovani?
		}
		private static void Send(string msg, Connection connection)
		{
			//connection.writer.WriteLine(msg);
			System.IO.StreamWriter s = new System.IO.StreamWriter(connection.connectionWith.GetStream());
			s.WriteLine(msg);
		}
	}
}
