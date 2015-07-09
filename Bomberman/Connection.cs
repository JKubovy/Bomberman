using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Bomberman
{
	class Connection
	{
		public TcpClient connectionWith = null;
		public DateTime lastTouch;
		public BinaryWriter writer;
		public BinaryReader reader;

		public Connection(TcpClient client)
		{
			this.connectionWith = client;
			this.writer = new BinaryWriter(connectionWith.GetStream());
			this.reader = new BinaryReader(connectionWith.GetStream());
			this.lastTouch = DateTime.Now;
		}
	}
}
