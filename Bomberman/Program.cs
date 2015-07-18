using System;
using System.Windows.Forms;

namespace Bomberman
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}

		internal static Playground playground;
		internal static bool playing = false;
		internal static int port = 4684;
		internal static Random random = new Random();
	}
}
