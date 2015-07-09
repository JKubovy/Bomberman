using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Bomberman
{
	class Client
	{
		private TcpClient server;
		private BinaryWriter writer;
		private BinaryReader reader;

		public Client(IPEndPoint ip)
		{
			server = new TcpClient(ip);
			writer = new BinaryWriter(server.GetStream());
			reader = new BinaryReader(server.GetStream());
		}
		private void Handshake()
		{

		}
	}
}
