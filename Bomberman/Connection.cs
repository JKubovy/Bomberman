using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Bomberman
{
	class Connection
	{
		public TcpClient connectionWith = null;
		//public bool playgroundUpdates;
		public DateTime lastTouch;
		public StreamWriter writer;
		public StreamReader reader;
		public Point position;

		public Connection(TcpClient client)
		{
			this.connectionWith = client;
			this.writer = new StreamWriter(connectionWith.GetStream());
			this.reader = new StreamReader(connectionWith.GetStream());
			this.writer.AutoFlush = true;
			//this.playgroundUpdates = false;
			this.lastTouch = DateTime.Now;
		}
	}
}
