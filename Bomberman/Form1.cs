using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bomberman
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void buttonAbout_Click(object sender, EventArgs e)
		{
			panelMultiplayer.Visible = false;
			panelControls.Visible = false;
			panelAbout.Visible = true;
		}

		private void buttonControls_Click(object sender, EventArgs e)
		{
			panelMultiplayer.Visible = false;
			panelControls.Visible = true;
			panelAbout.Visible = false;
		}

		private void buttonMultiplayer_Click(object sender, EventArgs e)
		{
			panelMultiplayer.Visible = true;
			panelControls.Visible = false;
			panelAbout.Visible = false;
		}

		private void buttonExit_Click(object sender, EventArgs e)
		{
			// add stop server, client, etc.
			this.Close();
		}

		private void buttonSingleplayer_Click(object sender, EventArgs e)
		{
			var p = new Playground();
		}
	}
}
