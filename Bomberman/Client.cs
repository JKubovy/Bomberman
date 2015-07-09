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

		public Client(IPAddress ip)
		{
			server = new TcpClient(AddressFamily.InterNetworkV6);
			server.Client.DualMode = true;
			server.Connect(ip,Program.port);
			writer = new BinaryWriter(server.GetStream());
			reader = new BinaryReader(server.GetStream());
			if (!Handshake())
			{
				// TODO dodelat error
				return;
			}
		}
		private bool Handshake()
		{
			byte[] data = new byte[3];
			data[0] = 0;
			data[1] = byte.MaxValue;
			data[2] = (byte)Program.port;
			writer.Write(data);
			byte[] dataRecive = reader.ReadBytes(3);
			if (data[0] == dataRecive[0] && data[1] == dataRecive[1] && data[2] == dataRecive[2])
				return true;
			return false;
		}
	}
}
