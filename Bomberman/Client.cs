using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Bomberman
{
	class Client
	{
		private TcpClient server;
		private StreamWriter writer;
		private StreamReader reader;
		private Point position;

		public Client(IPAddress ip, bool user)
		{
			server = new TcpClient(AddressFamily.InterNetworkV6);
			server.Client.DualMode = true;
			server.Connect(ip,Program.port);
			writer = new StreamWriter(server.GetStream());
			reader = new StreamReader(server.GetStream());
			writer.AutoFlush = true;
			if (user) Form1.player = this;
			Handshake(user);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="update">Boolean if client want to recive updates of playground</param>
		private async void Handshake(bool update)
		{
			string request = "Bomberman " + update;
			writer.WriteLine(request);
			string response = await reader.ReadLineAsync();
			string[] tokens = response.Split(' ');
			if (tokens[0] == "ACK")
			{
				position = getPosition(tokens[1]);
				StartListening();
			}
		}
		private async void StartListening()
		{
			while (server.Connected)
			{
				try
				{
					string command = await reader.ReadLineAsync();
					ProcessCommand(command);
				}
				catch (IOException)
				{
					// TODO error
				}
			}
		}
		private Point getPosition(string number)
		{
			switch (number)
			{
				case "0":
					return new Point(1, 1);
				case "1":
					return new Point(1, Playground.playgroundSize - 2);
				case "2":
					return new Point(Playground.playgroundSize - 2, 1);
				case "3":
					return new Point(Playground.playgroundSize - 2, Playground.playgroundSize - 2);
				default:
					// TODO error
					return new Point();
			}
		}
		private void ProcessCommand(string msg)
		{
			// TODO process command
		}
		internal void Send(string msg)
		{
			try
			{
				writer.WriteLine(msg);
			}
			catch (IOException)
			{
				// TODO error
			}
		}
	}
}
