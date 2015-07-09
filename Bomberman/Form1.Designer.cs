namespace Bomberman
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.splitContainerMenu = new System.Windows.Forms.SplitContainer();
			this.buttonExit = new System.Windows.Forms.Button();
			this.buttonAbout = new System.Windows.Forms.Button();
			this.buttonControls = new System.Windows.Forms.Button();
			this.buttonMultiplayer = new System.Windows.Forms.Button();
			this.buttonSingleplayer = new System.Windows.Forms.Button();
			this.panelAbout = new System.Windows.Forms.Panel();
			this.labelAbout = new System.Windows.Forms.Label();
			this.panelControls = new System.Windows.Forms.Panel();
			this.panelMultiplayer = new System.Windows.Forms.Panel();
			this.panelGame = new System.Windows.Forms.Panel();
			this.splitContainerGame = new System.Windows.Forms.SplitContainer();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerMenu)).BeginInit();
			this.splitContainerMenu.Panel1.SuspendLayout();
			this.splitContainerMenu.Panel2.SuspendLayout();
			this.splitContainerMenu.SuspendLayout();
			this.panelAbout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerGame)).BeginInit();
			this.splitContainerGame.Panel1.SuspendLayout();
			this.splitContainerGame.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainerMenu
			// 
			this.splitContainerMenu.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerMenu.Location = new System.Drawing.Point(0, 0);
			this.splitContainerMenu.MinimumSize = new System.Drawing.Size(200, 300);
			this.splitContainerMenu.Name = "splitContainerMenu";
			// 
			// splitContainerMenu.Panel1
			// 
			this.splitContainerMenu.Panel1.AutoScroll = true;
			this.splitContainerMenu.Panel1.AutoScrollMinSize = new System.Drawing.Size(100, 0);
			this.splitContainerMenu.Panel1.Controls.Add(this.buttonExit);
			this.splitContainerMenu.Panel1.Controls.Add(this.buttonAbout);
			this.splitContainerMenu.Panel1.Controls.Add(this.buttonControls);
			this.splitContainerMenu.Panel1.Controls.Add(this.buttonMultiplayer);
			this.splitContainerMenu.Panel1.Controls.Add(this.buttonSingleplayer);
			// 
			// splitContainerMenu.Panel2
			// 
			this.splitContainerMenu.Panel2.Controls.Add(this.panelAbout);
			this.splitContainerMenu.Panel2.Controls.Add(this.panelControls);
			this.splitContainerMenu.Panel2.Controls.Add(this.panelMultiplayer);
			this.splitContainerMenu.Size = new System.Drawing.Size(726, 450);
			this.splitContainerMenu.SplitterDistance = 241;
			this.splitContainerMenu.TabIndex = 0;
			// 
			// buttonExit
			// 
			this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonExit.Location = new System.Drawing.Point(3, 180);
			this.buttonExit.Name = "buttonExit";
			this.buttonExit.Size = new System.Drawing.Size(235, 36);
			this.buttonExit.TabIndex = 4;
			this.buttonExit.Text = global::Bomberman.Properties.Resources.Menu_Exit;
			this.buttonExit.UseVisualStyleBackColor = true;
			this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
			// 
			// buttonAbout
			// 
			this.buttonAbout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAbout.Location = new System.Drawing.Point(3, 138);
			this.buttonAbout.Name = "buttonAbout";
			this.buttonAbout.Size = new System.Drawing.Size(235, 36);
			this.buttonAbout.TabIndex = 3;
			this.buttonAbout.Text = global::Bomberman.Properties.Resources.Menu_About;
			this.buttonAbout.UseVisualStyleBackColor = true;
			this.buttonAbout.Click += new System.EventHandler(this.buttonAbout_Click);
			// 
			// buttonControls
			// 
			this.buttonControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonControls.Location = new System.Drawing.Point(3, 96);
			this.buttonControls.Name = "buttonControls";
			this.buttonControls.Size = new System.Drawing.Size(235, 36);
			this.buttonControls.TabIndex = 2;
			this.buttonControls.Text = global::Bomberman.Properties.Resources.Menu_Controls;
			this.buttonControls.UseVisualStyleBackColor = true;
			this.buttonControls.Click += new System.EventHandler(this.buttonControls_Click);
			// 
			// buttonMultiplayer
			// 
			this.buttonMultiplayer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonMultiplayer.Location = new System.Drawing.Point(3, 54);
			this.buttonMultiplayer.Name = "buttonMultiplayer";
			this.buttonMultiplayer.Size = new System.Drawing.Size(235, 36);
			this.buttonMultiplayer.TabIndex = 1;
			this.buttonMultiplayer.Text = global::Bomberman.Properties.Resources.Menu_Multiplayer;
			this.buttonMultiplayer.UseVisualStyleBackColor = true;
			this.buttonMultiplayer.Click += new System.EventHandler(this.buttonMultiplayer_Click);
			// 
			// buttonSingleplayer
			// 
			this.buttonSingleplayer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSingleplayer.Location = new System.Drawing.Point(3, 12);
			this.buttonSingleplayer.Name = "buttonSingleplayer";
			this.buttonSingleplayer.Size = new System.Drawing.Size(235, 36);
			this.buttonSingleplayer.TabIndex = 0;
			this.buttonSingleplayer.Text = global::Bomberman.Properties.Resources.Menu_Singleplayer;
			this.buttonSingleplayer.UseVisualStyleBackColor = true;
			this.buttonSingleplayer.Click += new System.EventHandler(this.buttonSingleplayer_Click);
			// 
			// panelAbout
			// 
			this.panelAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelAbout.Controls.Add(this.labelAbout);
			this.panelAbout.Location = new System.Drawing.Point(3, 3);
			this.panelAbout.Name = "panelAbout";
			this.panelAbout.Size = new System.Drawing.Size(475, 444);
			this.panelAbout.TabIndex = 2;
			this.panelAbout.Visible = false;
			// 
			// labelAbout
			// 
			this.labelAbout.AutoSize = true;
			this.labelAbout.Location = new System.Drawing.Point(3, 9);
			this.labelAbout.Name = "labelAbout";
			this.labelAbout.Size = new System.Drawing.Size(422, 85);
			this.labelAbout.TabIndex = 0;
			this.labelAbout.Text = "Bomberman is a game for 1 - 4 player.\r\nMain goal is kill other three players with" +
    " bombs which you can put.\r\n\r\nAutor: Jan Kubový\r\nBuild: July 2015";
			// 
			// panelControls
			// 
			this.panelControls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelControls.Location = new System.Drawing.Point(3, 3);
			this.panelControls.Name = "panelControls";
			this.panelControls.Size = new System.Drawing.Size(475, 444);
			this.panelControls.TabIndex = 1;
			this.panelControls.Visible = false;
			// 
			// panelMultiplayer
			// 
			this.panelMultiplayer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelMultiplayer.Location = new System.Drawing.Point(3, 3);
			this.panelMultiplayer.Name = "panelMultiplayer";
			this.panelMultiplayer.Size = new System.Drawing.Size(475, 444);
			this.panelMultiplayer.TabIndex = 0;
			this.panelMultiplayer.Visible = false;
			// 
			// panelGame
			// 
			this.panelGame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelGame.Location = new System.Drawing.Point(3, 3);
			this.panelGame.Name = "panelGame";
			this.panelGame.Size = new System.Drawing.Size(473, 441);
			this.panelGame.TabIndex = 1;
			// 
			// splitContainerGame
			// 
			this.splitContainerGame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainerGame.Location = new System.Drawing.Point(0, 0);
			this.splitContainerGame.Name = "splitContainerGame";
			this.splitContainerGame.Visible = false;
			// 
			// splitContainerGame.Panel1
			// 
			this.splitContainerGame.Panel1.Controls.Add(this.panelGame);
			this.splitContainerGame.Size = new System.Drawing.Size(726, 450);
			this.splitContainerGame.SplitterDistance = 479;
			this.splitContainerGame.TabIndex = 5;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(200, 300);
			this.ClientSize = new System.Drawing.Size(726, 450);
			this.Controls.Add(this.splitContainerGame);
			this.Controls.Add(this.splitContainerMenu);
			this.Name = "Form1";
			this.Text = "Bomberman";
			this.splitContainerMenu.Panel1.ResumeLayout(false);
			this.splitContainerMenu.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerMenu)).EndInit();
			this.splitContainerMenu.ResumeLayout(false);
			this.panelAbout.ResumeLayout(false);
			this.panelAbout.PerformLayout();
			this.splitContainerGame.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerGame)).EndInit();
			this.splitContainerGame.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainerMenu;
		private System.Windows.Forms.Button buttonExit;
		private System.Windows.Forms.Button buttonAbout;
		private System.Windows.Forms.Button buttonControls;
		private System.Windows.Forms.Button buttonMultiplayer;
		private System.Windows.Forms.Button buttonSingleplayer;
		private System.Windows.Forms.Panel panelControls;
		private System.Windows.Forms.Panel panelAbout;
		private System.Windows.Forms.Panel panelMultiplayer;
		private System.Windows.Forms.Label labelAbout;
		private System.Windows.Forms.Panel panelGame;
		private System.Windows.Forms.SplitContainer splitContainerGame;


	}
}

