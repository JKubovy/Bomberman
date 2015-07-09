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
		Connection[] clients;
		//static ManualResetEvent allDone;
		//static int clientsCount = 0;

		public Server()
		{
			clients = new Connection[4];
			TcpListener listener = TcpListener.Create(Program.port);
			listener.Start();
			for (int i = 0; i < 4; i++)
			{
				//TcpClient client = listener.AcceptTcpClient();
				//Task.Run(() => AcceptTcpClient(client, i));
				Task.Run(() =>
				{
					TcpClient client = listener.AcceptTcpClient();
					AcceptTcpClient(client, i);
				});
			}
			//allDone = new ManualResetEvent(false);
			//listener.Start();
			//while (true)
			//{
			//	allDone.Reset();
			//	listener.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpClient), listener);
			//	if (clientsCount == 4) break;
			//	allDone.WaitOne();
			//}
		}
		private void AcceptTcpClient(TcpClient client, int position)
		{
			Connection connection = new Connection(client);
			if (Handshake(connection))
				clients[position] = connection;
			else
			{
				// TODO doelat error
				return;
			}
		}
		//private static void AcceptTcpClient(IAsyncResult ar)
		//{
		//	Interlocked.Increment(ref clientsCount);
		//	allDone.Set();
		//	TcpListener listener = (TcpListener)ar.AsyncState;
		//	TcpClient client = listener.EndAcceptTcpClient(ar);
		//	Connection connection = new Connection(client);
		//	Handshake(connection);
		//}

		public void GetMovements()
		{

		}
		private static bool Handshake(Connection connection)
		{
			byte[] data = connection.reader.ReadBytes(3);
			if (data[0] == 0 && data[1] == byte.MaxValue && data[1] == Program.port)
			{
				connection.writer.Write(data);
				return true;
			}
			return false;
		}
	}
}
