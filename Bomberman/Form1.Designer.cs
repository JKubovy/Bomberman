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
            this.panelMultiplayer = new System.Windows.Forms.Panel();
            this.panelSize = new System.Windows.Forms.Panel();
            this.labelSize = new System.Windows.Forms.Label();
            this.numericUpDownSize = new System.Windows.Forms.NumericUpDown();
            this.buttonStart = new System.Windows.Forms.Button();
            this.groupBoxPlayersCount = new System.Windows.Forms.GroupBox();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.labelPlayerCount = new System.Windows.Forms.Label();
            this.labelIPInfo = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.groupBoxServerClient = new System.Windows.Forms.GroupBox();
            this.radioButtonServer = new System.Windows.Forms.RadioButton();
            this.radioButtonClient = new System.Windows.Forms.RadioButton();
            this.panelControls = new System.Windows.Forms.Panel();
            this.labelControls = new System.Windows.Forms.Label();
            this.panelAbout = new System.Windows.Forms.Panel();
            this.labelAbout = new System.Windows.Forms.Label();
            this.panelGame = new System.Windows.Forms.Panel();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.labelInfoPlayer4 = new System.Windows.Forms.Label();
            this.labelInfoPlayer3 = new System.Windows.Forms.Label();
            this.labelInfoPlayer2 = new System.Windows.Forms.Label();
            this.labelInfoPlayer1 = new System.Windows.Forms.Label();
            this.buttonExit2 = new System.Windows.Forms.Button();
            this.pictureBoxAvatar = new System.Windows.Forms.PictureBox();
            this.labelAvatar = new System.Windows.Forms.Label();
            this.pictureNextMove1 = new System.Windows.Forms.PictureBox();
            this.pictureNextMove2 = new System.Windows.Forms.PictureBox();
            this.labelNextMoves = new System.Windows.Forms.Label();
            this.panelGameInfo = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMenu)).BeginInit();
            this.splitContainerMenu.Panel1.SuspendLayout();
            this.splitContainerMenu.Panel2.SuspendLayout();
            this.splitContainerMenu.SuspendLayout();
            this.panelMultiplayer.SuspendLayout();
            this.panelSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSize)).BeginInit();
            this.groupBoxPlayersCount.SuspendLayout();
            this.groupBoxServerClient.SuspendLayout();
            this.panelControls.SuspendLayout();
            this.panelAbout.SuspendLayout();
            this.panelInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAvatar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureNextMove1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureNextMove2)).BeginInit();
            this.panelGameInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMenu
            // 
            this.splitContainerMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMenu.IsSplitterFixed = true;
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
            this.splitContainerMenu.Panel2.Controls.Add(this.panelMultiplayer);
            this.splitContainerMenu.Panel2.Controls.Add(this.panelControls);
            this.splitContainerMenu.Panel2.Controls.Add(this.panelAbout);
            this.splitContainerMenu.Size = new System.Drawing.Size(725, 450);
            this.splitContainerMenu.SplitterDistance = 240;
            this.splitContainerMenu.TabIndex = 0;
            // 
            // buttonExit
            // 
            this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExit.Location = new System.Drawing.Point(3, 180);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(234, 36);
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
            this.buttonAbout.Size = new System.Drawing.Size(234, 36);
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
            this.buttonControls.Size = new System.Drawing.Size(234, 36);
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
            this.buttonMultiplayer.Size = new System.Drawing.Size(234, 36);
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
            this.buttonSingleplayer.Size = new System.Drawing.Size(234, 36);
            this.buttonSingleplayer.TabIndex = 0;
            this.buttonSingleplayer.Text = global::Bomberman.Properties.Resources.Menu_Singleplayer;
            this.buttonSingleplayer.UseVisualStyleBackColor = true;
            this.buttonSingleplayer.Click += new System.EventHandler(this.buttonSingleplayer_Click);
            // 
            // panelMultiplayer
            // 
            this.panelMultiplayer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMultiplayer.Controls.Add(this.panelSize);
            this.panelMultiplayer.Controls.Add(this.buttonStart);
            this.panelMultiplayer.Controls.Add(this.groupBoxPlayersCount);
            this.panelMultiplayer.Controls.Add(this.labelPlayerCount);
            this.panelMultiplayer.Controls.Add(this.labelIPInfo);
            this.panelMultiplayer.Controls.Add(this.textBoxIP);
            this.panelMultiplayer.Controls.Add(this.groupBoxServerClient);
            this.panelMultiplayer.Location = new System.Drawing.Point(3, 3);
            this.panelMultiplayer.Name = "panelMultiplayer";
            this.panelMultiplayer.Size = new System.Drawing.Size(475, 444);
            this.panelMultiplayer.TabIndex = 0;
            // 
            // panelSize
            // 
            this.panelSize.Controls.Add(this.labelSize);
            this.panelSize.Controls.Add(this.numericUpDownSize);
            this.panelSize.Location = new System.Drawing.Point(6, 174);
            this.panelSize.Name = "panelSize";
            this.panelSize.Size = new System.Drawing.Size(129, 53);
            this.panelSize.TabIndex = 10;
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Location = new System.Drawing.Point(6, 3);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(113, 17);
            this.labelSize.TabIndex = 8;
            this.labelSize.Text = "Playground size:";
            // 
            // numericUpDownSize
            // 
            this.numericUpDownSize.Location = new System.Drawing.Point(6, 24);
            this.numericUpDownSize.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownSize.Minimum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numericUpDownSize.Name = "numericUpDownSize";
            this.numericUpDownSize.Size = new System.Drawing.Size(113, 22);
            this.numericUpDownSize.TabIndex = 9;
            this.numericUpDownSize.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStart.Location = new System.Drawing.Point(232, 399);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(234, 36);
            this.buttonStart.TabIndex = 7;
            this.buttonStart.Text = global::Bomberman.Properties.Resources.Multiplayer_Start;
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // groupBoxPlayersCount
            // 
            this.groupBoxPlayersCount.Controls.Add(this.radioButton4);
            this.groupBoxPlayersCount.Controls.Add(this.radioButton3);
            this.groupBoxPlayersCount.Controls.Add(this.radioButton2);
            this.groupBoxPlayersCount.Controls.Add(this.radioButton1);
            this.groupBoxPlayersCount.Location = new System.Drawing.Point(6, 131);
            this.groupBoxPlayersCount.Name = "groupBoxPlayersCount";
            this.groupBoxPlayersCount.Size = new System.Drawing.Size(173, 37);
            this.groupBoxPlayersCount.TabIndex = 6;
            this.groupBoxPlayersCount.TabStop = false;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(135, 11);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(37, 21);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.Text = "4";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(92, 11);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(37, 21);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "3";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(49, 11);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(37, 21);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "2";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 11);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(37, 21);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // labelPlayerCount
            // 
            this.labelPlayerCount.AutoSize = true;
            this.labelPlayerCount.Location = new System.Drawing.Point(12, 111);
            this.labelPlayerCount.Name = "labelPlayerCount";
            this.labelPlayerCount.Size = new System.Drawing.Size(128, 17);
            this.labelPlayerCount.TabIndex = 5;
            this.labelPlayerCount.Text = "Number of players:";
            // 
            // labelIPInfo
            // 
            this.labelIPInfo.AutoSize = true;
            this.labelIPInfo.Location = new System.Drawing.Point(124, 36);
            this.labelIPInfo.Name = "labelIPInfo";
            this.labelIPInfo.Size = new System.Drawing.Size(70, 17);
            this.labelIPInfo.TabIndex = 4;
            this.labelIPInfo.Text = "IP Server:";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxIP.Enabled = false;
            this.textBoxIP.Location = new System.Drawing.Point(200, 33);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(266, 22);
            this.textBoxIP.TabIndex = 3;
            // 
            // groupBoxServerClient
            // 
            this.groupBoxServerClient.Controls.Add(this.radioButtonServer);
            this.groupBoxServerClient.Controls.Add(this.radioButtonClient);
            this.groupBoxServerClient.Location = new System.Drawing.Point(6, 13);
            this.groupBoxServerClient.Name = "groupBoxServerClient";
            this.groupBoxServerClient.Size = new System.Drawing.Size(112, 81);
            this.groupBoxServerClient.TabIndex = 2;
            this.groupBoxServerClient.TabStop = false;
            // 
            // radioButtonServer
            // 
            this.radioButtonServer.AutoSize = true;
            this.radioButtonServer.Checked = true;
            this.radioButtonServer.Location = new System.Drawing.Point(6, 21);
            this.radioButtonServer.Name = "radioButtonServer";
            this.radioButtonServer.Size = new System.Drawing.Size(71, 21);
            this.radioButtonServer.TabIndex = 0;
            this.radioButtonServer.TabStop = true;
            this.radioButtonServer.Text = "Server";
            this.radioButtonServer.UseVisualStyleBackColor = true;
            this.radioButtonServer.CheckedChanged += new System.EventHandler(this.radioButtonServer_CheckedChanged);
            // 
            // radioButtonClient
            // 
            this.radioButtonClient.AutoSize = true;
            this.radioButtonClient.Location = new System.Drawing.Point(6, 49);
            this.radioButtonClient.Name = "radioButtonClient";
            this.radioButtonClient.Size = new System.Drawing.Size(64, 21);
            this.radioButtonClient.TabIndex = 1;
            this.radioButtonClient.Text = "Client";
            this.radioButtonClient.UseVisualStyleBackColor = true;
            // 
            // panelControls
            // 
            this.panelControls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControls.Controls.Add(this.labelControls);
            this.panelControls.Location = new System.Drawing.Point(3, 3);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(475, 444);
            this.panelControls.TabIndex = 1;
            this.panelControls.Visible = false;
            // 
            // labelControls
            // 
            this.labelControls.AutoSize = true;
            this.labelControls.Location = new System.Drawing.Point(12, 19);
            this.labelControls.Name = "labelControls";
            this.labelControls.Size = new System.Drawing.Size(83, 17);
            this.labelControls.TabIndex = 0;
            this.labelControls.Text = "labelControl";
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
            this.labelAbout.Location = new System.Drawing.Point(12, 19);
            this.labelAbout.Name = "labelAbout";
            this.labelAbout.Size = new System.Drawing.Size(75, 17);
            this.labelAbout.TabIndex = 0;
            this.labelAbout.Text = "labelAbout";
            // 
            // panelGame
            // 
            this.panelGame.AutoSize = true;
            this.panelGame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGame.Location = new System.Drawing.Point(0, 0);
            this.panelGame.Name = "panelGame";
            this.panelGame.Size = new System.Drawing.Size(719, 444);
            this.panelGame.TabIndex = 1;
            // 
            // panelInfo
            // 
            this.panelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelInfo.Controls.Add(this.labelInfoPlayer4);
            this.panelInfo.Controls.Add(this.labelInfoPlayer3);
            this.panelInfo.Controls.Add(this.labelInfoPlayer2);
            this.panelInfo.Controls.Add(this.labelInfoPlayer1);
            this.panelInfo.Controls.Add(this.buttonExit2);
            this.panelInfo.Controls.Add(this.pictureBoxAvatar);
            this.panelInfo.Controls.Add(this.labelAvatar);
            this.panelInfo.Controls.Add(this.pictureNextMove1);
            this.panelInfo.Controls.Add(this.pictureNextMove2);
            this.panelInfo.Controls.Add(this.labelNextMoves);
            this.panelInfo.Location = new System.Drawing.Point(449, -3);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(267, 447);
            this.panelInfo.TabIndex = 5;
            // 
            // labelInfoPlayer4
            // 
            this.labelInfoPlayer4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelInfoPlayer4.AutoSize = true;
            this.labelInfoPlayer4.Location = new System.Drawing.Point(7, 425);
            this.labelInfoPlayer4.Name = "labelInfoPlayer4";
            this.labelInfoPlayer4.Size = new System.Drawing.Size(64, 17);
            this.labelInfoPlayer4.TabIndex = 9;
            this.labelInfoPlayer4.Text = "Player 4:";
            // 
            // labelInfoPlayer3
            // 
            this.labelInfoPlayer3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelInfoPlayer3.AutoSize = true;
            this.labelInfoPlayer3.Location = new System.Drawing.Point(7, 408);
            this.labelInfoPlayer3.Name = "labelInfoPlayer3";
            this.labelInfoPlayer3.Size = new System.Drawing.Size(64, 17);
            this.labelInfoPlayer3.TabIndex = 8;
            this.labelInfoPlayer3.Text = "Player 3:";
            // 
            // labelInfoPlayer2
            // 
            this.labelInfoPlayer2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelInfoPlayer2.AutoSize = true;
            this.labelInfoPlayer2.Location = new System.Drawing.Point(7, 391);
            this.labelInfoPlayer2.Name = "labelInfoPlayer2";
            this.labelInfoPlayer2.Size = new System.Drawing.Size(64, 17);
            this.labelInfoPlayer2.TabIndex = 7;
            this.labelInfoPlayer2.Text = "Player 2:";
            // 
            // labelInfoPlayer1
            // 
            this.labelInfoPlayer1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelInfoPlayer1.AutoSize = true;
            this.labelInfoPlayer1.Location = new System.Drawing.Point(7, 374);
            this.labelInfoPlayer1.Name = "labelInfoPlayer1";
            this.labelInfoPlayer1.Size = new System.Drawing.Size(64, 17);
            this.labelInfoPlayer1.TabIndex = 6;
            this.labelInfoPlayer1.Text = "Player 1:";
            // 
            // buttonExit2
            // 
            this.buttonExit2.Location = new System.Drawing.Point(10, 335);
            this.buttonExit2.Name = "buttonExit2";
            this.buttonExit2.Size = new System.Drawing.Size(251, 36);
            this.buttonExit2.TabIndex = 5;
            this.buttonExit2.TabStop = false;
            this.buttonExit2.Text = global::Bomberman.Properties.Resources.Menu_Exit;
            this.buttonExit2.UseVisualStyleBackColor = true;
            this.buttonExit2.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // pictureBoxAvatar
            // 
            this.pictureBoxAvatar.Location = new System.Drawing.Point(7, 107);
            this.pictureBoxAvatar.Name = "pictureBoxAvatar";
            this.pictureBoxAvatar.Size = new System.Drawing.Size(50, 50);
            this.pictureBoxAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxAvatar.TabIndex = 4;
            this.pictureBoxAvatar.TabStop = false;
            // 
            // labelAvatar
            // 
            this.labelAvatar.AutoSize = true;
            this.labelAvatar.Location = new System.Drawing.Point(7, 86);
            this.labelAvatar.Name = "labelAvatar";
            this.labelAvatar.Size = new System.Drawing.Size(86, 17);
            this.labelAvatar.TabIndex = 3;
            this.labelAvatar.Text = "Your avatar:";
            // 
            // pictureNextMove1
            // 
            this.pictureNextMove1.Location = new System.Drawing.Point(7, 29);
            this.pictureNextMove1.Name = "pictureNextMove1";
            this.pictureNextMove1.Size = new System.Drawing.Size(50, 50);
            this.pictureNextMove1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureNextMove1.TabIndex = 1;
            this.pictureNextMove1.TabStop = false;
            // 
            // pictureNextMove2
            // 
            this.pictureNextMove2.Location = new System.Drawing.Point(64, 29);
            this.pictureNextMove2.Name = "pictureNextMove2";
            this.pictureNextMove2.Size = new System.Drawing.Size(50, 50);
            this.pictureNextMove2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureNextMove2.TabIndex = 2;
            this.pictureNextMove2.TabStop = false;
            // 
            // labelNextMoves
            // 
            this.labelNextMoves.AutoSize = true;
            this.labelNextMoves.Location = new System.Drawing.Point(4, 9);
            this.labelNextMoves.Name = "labelNextMoves";
            this.labelNextMoves.Size = new System.Drawing.Size(85, 17);
            this.labelNextMoves.TabIndex = 0;
            this.labelNextMoves.Text = "Next moves:";
            // 
            // panelGameInfo
            // 
            this.panelGameInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGameInfo.Controls.Add(this.panelInfo);
            this.panelGameInfo.Controls.Add(this.panelGame);
            this.panelGameInfo.Location = new System.Drawing.Point(3, 3);
            this.panelGameInfo.Name = "panelGameInfo";
            this.panelGameInfo.Size = new System.Drawing.Size(719, 444);
            this.panelGameInfo.TabIndex = 3;
            this.panelGameInfo.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(200, 300);
            this.ClientSize = new System.Drawing.Size(725, 450);
            this.Controls.Add(this.panelGameInfo);
            this.Controls.Add(this.splitContainerMenu);
            this.Name = "Form1";
            this.Text = "Bomberman";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.splitContainerMenu.Panel1.ResumeLayout(false);
            this.splitContainerMenu.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMenu)).EndInit();
            this.splitContainerMenu.ResumeLayout(false);
            this.panelMultiplayer.ResumeLayout(false);
            this.panelMultiplayer.PerformLayout();
            this.panelSize.ResumeLayout(false);
            this.panelSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSize)).EndInit();
            this.groupBoxPlayersCount.ResumeLayout(false);
            this.groupBoxPlayersCount.PerformLayout();
            this.groupBoxServerClient.ResumeLayout(false);
            this.groupBoxServerClient.PerformLayout();
            this.panelControls.ResumeLayout(false);
            this.panelControls.PerformLayout();
            this.panelAbout.ResumeLayout(false);
            this.panelAbout.PerformLayout();
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAvatar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureNextMove1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureNextMove2)).EndInit();
            this.panelGameInfo.ResumeLayout(false);
            this.panelGameInfo.PerformLayout();
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
		private System.Windows.Forms.Label labelNextMoves;
		private System.Windows.Forms.PictureBox pictureNextMove2;
		private System.Windows.Forms.PictureBox pictureNextMove1;
		private System.Windows.Forms.Panel panelInfo;
		private System.Windows.Forms.Panel panelGameInfo;
		private System.Windows.Forms.PictureBox pictureBoxAvatar;
		private System.Windows.Forms.Label labelAvatar;
		private System.Windows.Forms.Button buttonExit2;
		private System.Windows.Forms.Label labelInfoPlayer4;
		private System.Windows.Forms.Label labelInfoPlayer3;
		private System.Windows.Forms.Label labelInfoPlayer2;
		private System.Windows.Forms.Label labelInfoPlayer1;
		private System.Windows.Forms.GroupBox groupBoxServerClient;
		private System.Windows.Forms.RadioButton radioButtonServer;
		private System.Windows.Forms.RadioButton radioButtonClient;
		private System.Windows.Forms.TextBox textBoxIP;
		private System.Windows.Forms.Label labelPlayerCount;
		private System.Windows.Forms.Label labelIPInfo;
		private System.Windows.Forms.Button buttonStart;
		private System.Windows.Forms.GroupBox groupBoxPlayersCount;
		private System.Windows.Forms.RadioButton radioButton4;
		private System.Windows.Forms.RadioButton radioButton3;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.Label labelControls;
		private System.Windows.Forms.NumericUpDown numericUpDownSize;
		private System.Windows.Forms.Label labelSize;
		private System.Windows.Forms.Panel panelSize;


	}
}

