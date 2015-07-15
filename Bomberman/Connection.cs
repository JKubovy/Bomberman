using System;
using System.IO;
using System.Drawing;
using System.Net.Sockets;

namespace Bomberman
{
	class Connection
	{
		public TcpClient connectionWith = null;
		public DateTime lastTouch;
		public StreamWriter writer;
		public StreamReader reader;
		public Point position;
		public int playerNumber;

		public Connection(TcpClient client)
		{
			this.connectionWith = client;
			this.writer = new StreamWriter(connectionWith.GetStream());
			this.reader = new StreamReader(connectionWith.GetStream());
			this.writer.AutoFlush = true;
			this.lastTouch = DateTime.Now;
		}
	}
}
