using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using System.Timers;

namespace Bomberman
{
	class Server
	{
		static List<Connection> clients;
		static ManualResetEvent allDone;
		static System.Timers.Timer updateTimer = new System.Timers.Timer(1000);

		public static void Start()
		{
			updateTimer.Elapsed += new ElapsedEventHandler(UpdateTick);
			updateTimer.AutoReset = false;
			clients = new List<Connection>();
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
				if (tokens[1] == true.ToString())
				{
					connection.playgroundUpdates = true;
					SendPlayground(connection);
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
					ProcessMove(tokens, connection);
					break;
				default:
					break;
			}
		}
		//static int playersPlayed = 0;
		static Queue<FutureMove> futureMoves = new Queue<FutureMove>();
		private static void ProcessMove(string[] moves, Connection connection)
		{
			Movement movement = (Movement)int.Parse(moves[1]);
			/*
			//futureMoves.Enqueue((Movement)int.Parse(moves[2]));
			GameLogic.Process(Program.playground, movement, connection);
			Form1.updatePictureBox();
			playersPlayed++;
			if (playersPlayed == 4)
			{
				playersPlayed = 0;
				// TODO dodelat
			}
			*/
			futureMoves.Enqueue(new FutureMove((Movement)int.Parse(moves[2]),connection));
			GameLogic.Process(Program.playground, movement, connection);
			Form1.updatePictureBox();
			FutureMove futureMove = futureMoves.Dequeue();
			GameLogic.Process(Program.playground, futureMove.movement, futureMove.connection);
			updateTimer.Enabled = true;
		}
		private static void UpdateTick(Object source, ElapsedEventArgs e)
		{
			Form1.updatePictureBox();
		}
	}
}
