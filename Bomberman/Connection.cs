using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Bomberman
{
	class Connection
	{
		public TcpClient connectionWith = null;
		public int protocolVersion;
		public DateTime lastTouch;
		// Reader and Writer (Stream or Binary)

		public Connection(TcpClient client, int protocolVersion)
		{
			this.connectionWith = client;
			this.protocolVersion = protocolVersion;
			this.lastTouch = DateTime.Now;
			// make Reader and Writer
		}
	}
}
