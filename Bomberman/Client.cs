using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
	class Client
	{
		private Connection connection;

		public Client(Connection connection)
		{
			this.connection = connection;
		}
	}
}
