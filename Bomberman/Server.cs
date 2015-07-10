using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Bomberman
{
	class Server
	{
		static List<Connection> clients;
		static ManualResetEvent allDone;

		public static void Start()
		{
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

		private void GetMovements()
		{
			
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
			// TODO dodelat Handshake + proceduru zpracovani
		}

		private static void SendPlayground(Connection connection)
		{
			connection.writer.WriteLine("Playground " + Program.playground.playgroundSize);
			for (int i = 0; i < Program.playground.playgroundSize; i++)
			{
				string line = "";
				for (int j = 0; j < Program.playground.playgroundSize; j++)
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
			// TODO dodelat
		}
	}
}
