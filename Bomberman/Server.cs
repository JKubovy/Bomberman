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
		List<Connection> clients;
		public Server()
		{
			clients = new List<Connection>();
			//TcpListener listener = TcpListener.Create(4684);
			//ManualResetEvent allDone = new ManualResetEvent(false);
			//listener.Start();
		}

		public void GetMovements()
		{

		}
		private void Handshake()
		{

		}
	}
}
