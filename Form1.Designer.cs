﻿namespace CheatGameApp
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
            this.components = new System.ComponentModel.Container();
            this.StartGameButton = new System.Windows.Forms.Button();
            this.TakeCardButton = new System.Windows.Forms.Button();
            this.CallCheatButton = new System.Windows.Forms.Button();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.gameDeckCountLabel = new System.Windows.Forms.Label();
            this.allClaimsCountLabel = new System.Windows.Forms.Label();
            this.oppCardsCountLabel = new System.Windows.Forms.Label();
            this.turnOverCardsButton = new System.Windows.Forms.Button();
            this.msgLabel = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.gamesCountLabel = new System.Windows.Forms.Label();
            this.turnLabel = new System.Windows.Forms.Label();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.recordingLable = new System.Windows.Forms.Label();
            this.replay = new System.Windows.Forms.Button();
            this.MakeClaim = new System.Windows.Forms.Button();
            this.FalseRecord = new System.Windows.Forms.Button();
            this.selfreplaybutton = new System.Windows.Forms.Button();
            this.hearednothing = new System.Windows.Forms.Button();
            this.TotaleTimeLable = new System.Windows.Forms.Label();
            this.PlayerName = new System.Windows.Forms.Label();
            this.agent_operation = new System.Windows.Forms.Button();
            this.highclaimhear = new CheatGameApp.DeckLabel();
            this.lowclaimhear = new CheatGameApp.DeckLabel();
            this.oppDeckLabel = new CheatGameApp.DeckLabel();
            this.lastClaimDeckLabel = new CheatGameApp.DeckLabel();
            this.allClaimsDeckLabel = new CheatGameApp.DeckLabel();
            this.gameDeckLabel = new CheatGameApp.DeckLabel();
            this.myDeck = new CheatGameApp.DeckLabel();
            this.highClaimOptionDeck = new CheatGameApp.DeckLabel();
            this.lowClaimOptionDeck = new CheatGameApp.DeckLabel();
            this.SuspendLayout();
            // 
            // StartGameButton
            // 
            this.StartGameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.StartGameButton.Location = new System.Drawing.Point(322, 246);
            this.StartGameButton.Name = "StartGameButton";
            this.StartGameButton.Size = new System.Drawing.Size(226, 59);
            this.StartGameButton.TabIndex = 5;
            this.StartGameButton.Text = "Start Game";
            this.StartGameButton.UseVisualStyleBackColor = true;
            this.StartGameButton.Click += new System.EventHandler(this.StartGameButton_Click);
            // 
            // TakeCardButton
            // 
            this.TakeCardButton.Location = new System.Drawing.Point(712, 324);
            this.TakeCardButton.Name = "TakeCardButton";
            this.TakeCardButton.Size = new System.Drawing.Size(96, 30);
            this.TakeCardButton.TabIndex = 7;
            this.TakeCardButton.Text = "Take 3 Cards";
            this.TakeCardButton.UseVisualStyleBackColor = true;
            this.TakeCardButton.Visible = false;
            this.TakeCardButton.Click += new System.EventHandler(this.TakeCardButton_Click);
            // 
            // CallCheatButton
            // 
            this.CallCheatButton.Location = new System.Drawing.Point(88, 324);
            this.CallCheatButton.Name = "CallCheatButton";
            this.CallCheatButton.Size = new System.Drawing.Size(96, 30);
            this.CallCheatButton.TabIndex = 9;
            this.CallCheatButton.Text = "Call A Cheat";
            this.CallCheatButton.UseVisualStyleBackColor = true;
            this.CallCheatButton.Visible = false;
            this.CallCheatButton.Click += new System.EventHandler(this.CallCheatButton_Click);
            // 
            // StatusLabel
            // 
            this.StatusLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.StatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.StatusLabel.Location = new System.Drawing.Point(56, 165);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(810, 33);
            this.StatusLabel.TabIndex = 9;
            this.StatusLabel.Text = "PLEASE WAIT FOR OTHER PLAYERS TO JOIN THE GAME";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.StatusLabel.Visible = false;
            // 
            // gameDeckCountLabel
            // 
            this.gameDeckCountLabel.AutoSize = true;
            this.gameDeckCountLabel.BackColor = System.Drawing.Color.Transparent;
            this.gameDeckCountLabel.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.gameDeckCountLabel.ForeColor = System.Drawing.Color.White;
            this.gameDeckCountLabel.Location = new System.Drawing.Point(717, 206);
            this.gameDeckCountLabel.Name = "gameDeckCountLabel";
            this.gameDeckCountLabel.Size = new System.Drawing.Size(78, 21);
            this.gameDeckCountLabel.TabIndex = 11;
            this.gameDeckCountLabel.Text = "10 Cards";
            // 
            // allClaimsCountLabel
            // 
            this.allClaimsCountLabel.AutoSize = true;
            this.allClaimsCountLabel.BackColor = System.Drawing.Color.Transparent;
            this.allClaimsCountLabel.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.allClaimsCountLabel.ForeColor = System.Drawing.Color.White;
            this.allClaimsCountLabel.Location = new System.Drawing.Point(390, 206);
            this.allClaimsCountLabel.Name = "allClaimsCountLabel";
            this.allClaimsCountLabel.Size = new System.Drawing.Size(78, 21);
            this.allClaimsCountLabel.TabIndex = 11;
            this.allClaimsCountLabel.Text = "10 Cards";
            // 
            // oppCardsCountLabel
            // 
            this.oppCardsCountLabel.AutoSize = true;
            this.oppCardsCountLabel.BackColor = System.Drawing.Color.Transparent;
            this.oppCardsCountLabel.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.oppCardsCountLabel.ForeColor = System.Drawing.Color.White;
            this.oppCardsCountLabel.Location = new System.Drawing.Point(285, 53);
            this.oppCardsCountLabel.Name = "oppCardsCountLabel";
            this.oppCardsCountLabel.Size = new System.Drawing.Size(78, 21);
            this.oppCardsCountLabel.TabIndex = 11;
            this.oppCardsCountLabel.Text = "10 Cards";
            // 
            // turnOverCardsButton
            // 
            this.turnOverCardsButton.Location = new System.Drawing.Point(394, 324);
            this.turnOverCardsButton.Name = "turnOverCardsButton";
            this.turnOverCardsButton.Size = new System.Drawing.Size(96, 30);
            this.turnOverCardsButton.TabIndex = 7;
            this.turnOverCardsButton.Text = "Turn Cards Over";
            this.turnOverCardsButton.UseVisualStyleBackColor = true;
            this.turnOverCardsButton.Visible = false;
            this.turnOverCardsButton.Click += new System.EventHandler(this.turnOverCardsButton_Click);
            // 
            // msgLabel
            // 
            this.msgLabel.BackColor = System.Drawing.Color.Transparent;
            this.msgLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.msgLabel.ForeColor = System.Drawing.Color.White;
            this.msgLabel.Location = new System.Drawing.Point(29, 372);
            this.msgLabel.Name = "msgLabel";
            this.msgLabel.Size = new System.Drawing.Size(610, 35);
            this.msgLabel.TabIndex = 12;
            this.msgLabel.Text = "messages to player";
            this.msgLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timeLabel
            // 
            this.timeLabel.BackColor = System.Drawing.Color.Transparent;
            this.timeLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.timeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.timeLabel.ForeColor = System.Drawing.Color.White;
            this.timeLabel.Location = new System.Drawing.Point(755, 87);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(100, 23);
            this.timeLabel.TabIndex = 13;
            this.timeLabel.Text = "00:00:00";
            this.timeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gamesCountLabel
            // 
            this.gamesCountLabel.BackColor = System.Drawing.Color.Transparent;
            this.gamesCountLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gamesCountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.gamesCountLabel.ForeColor = System.Drawing.Color.White;
            this.gamesCountLabel.Location = new System.Drawing.Point(755, 110);
            this.gamesCountLabel.Name = "gamesCountLabel";
            this.gamesCountLabel.Size = new System.Drawing.Size(100, 23);
            this.gamesCountLabel.TabIndex = 13;
            this.gamesCountLabel.Text = "1 of 3";
            this.gamesCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // turnLabel
            // 
            this.turnLabel.BackColor = System.Drawing.Color.Transparent;
            this.turnLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.turnLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.turnLabel.ForeColor = System.Drawing.Color.White;
            this.turnLabel.Location = new System.Drawing.Point(755, 133);
            this.turnLabel.Name = "turnLabel";
            this.turnLabel.Size = new System.Drawing.Size(100, 23);
            this.turnLabel.TabIndex = 13;
            this.turnLabel.Text = "Opponent";
            this.turnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gameTimer
            // 
            this.gameTimer.Interval = 1000;
            this.gameTimer.Tick += new System.EventHandler(this.gameTimer_Tick);
            // 
            // recordingLable
            // 
            this.recordingLable.AutoSize = true;
            this.recordingLable.BackColor = System.Drawing.SystemColors.HotTrack;
            this.recordingLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recordingLable.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.recordingLable.Location = new System.Drawing.Point(335, 260);
            this.recordingLable.Name = "recordingLable";
            this.recordingLable.Size = new System.Drawing.Size(213, 32);
            this.recordingLable.TabIndex = 14;
            this.recordingLable.Text = "state your claim";
            this.recordingLable.Visible = false;
            // 
            // replay
            // 
            this.replay.Location = new System.Drawing.Point(56, 206);
            this.replay.Name = "replay";
            this.replay.Size = new System.Drawing.Size(162, 31);
            this.replay.TabIndex = 15;
            this.replay.Text = "Replay Opponents Claim";
            this.replay.UseVisualStyleBackColor = true;
            this.replay.Visible = false;
            this.replay.Click += new System.EventHandler(this.replay_Click);
            // 
            // MakeClaim
            // 
            this.MakeClaim.Location = new System.Drawing.Point(50, 537);
            this.MakeClaim.Name = "MakeClaim";
            this.MakeClaim.Size = new System.Drawing.Size(96, 30);
            this.MakeClaim.TabIndex = 16;
            this.MakeClaim.Text = "Make Move";
            this.MakeClaim.UseVisualStyleBackColor = true;
            this.MakeClaim.Visible = false;
            this.MakeClaim.Click += new System.EventHandler(this.MakeClaim_Click);
            // 
            // FalseRecord
            // 
            this.FalseRecord.Location = new System.Drawing.Point(712, 525);
            this.FalseRecord.Name = "FalseRecord";
            this.FalseRecord.Size = new System.Drawing.Size(85, 42);
            this.FalseRecord.TabIndex = 17;
            this.FalseRecord.Text = "False Recording";
            this.FalseRecord.UseVisualStyleBackColor = true;
            this.FalseRecord.Visible = false;
            this.FalseRecord.Click += new System.EventHandler(this.FalseRecord_Click);
            // 
            // selfreplaybutton
            // 
            this.selfreplaybutton.Location = new System.Drawing.Point(50, 383);
            this.selfreplaybutton.Name = "selfreplaybutton";
            this.selfreplaybutton.Size = new System.Drawing.Size(85, 42);
            this.selfreplaybutton.TabIndex = 18;
            this.selfreplaybutton.Text = "Replay My Claim";
            this.selfreplaybutton.UseVisualStyleBackColor = true;
            this.selfreplaybutton.Visible = false;
            this.selfreplaybutton.Click += new System.EventHandler(this.selfReplayButton_Click);
            // 
            // hearednothing
            // 
            this.hearednothing.Location = new System.Drawing.Point(384, 295);
            this.hearednothing.Name = "hearednothing";
            this.hearednothing.Size = new System.Drawing.Size(113, 42);
            this.hearednothing.TabIndex = 23;
            this.hearednothing.Text = "heared nothing or didn\'t understand";
            this.hearednothing.UseVisualStyleBackColor = true;
            this.hearednothing.Visible = false;
            this.hearednothing.Click += new System.EventHandler(this.hearednothing_Click);
            // 
            // TotaleTimeLable
            // 
            this.TotaleTimeLable.BackColor = System.Drawing.Color.Transparent;
            this.TotaleTimeLable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.TotaleTimeLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.TotaleTimeLable.ForeColor = System.Drawing.Color.White;
            this.TotaleTimeLable.Location = new System.Drawing.Point(755, 64);
            this.TotaleTimeLable.Name = "TotaleTimeLable";
            this.TotaleTimeLable.Size = new System.Drawing.Size(100, 23);
            this.TotaleTimeLable.TabIndex = 24;
            this.TotaleTimeLable.Text = "00:00:00";
            this.TotaleTimeLable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PlayerName
            // 
            this.PlayerName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PlayerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.PlayerName.Location = new System.Drawing.Point(646, 9);
            this.PlayerName.Name = "PlayerName";
            this.PlayerName.Size = new System.Drawing.Size(220, 38);
            this.PlayerName.TabIndex = 25;
            this.PlayerName.Text = "PLEASE WAIT FOR OTHER PLAYERS TO JOIN THE GAME";
            this.PlayerName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.PlayerName.Visible = false;
            // 
            // agent_operation
            // 
            this.agent_operation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.agent_operation.Location = new System.Drawing.Point(24, 24);
            this.agent_operation.Name = "agent_operation";
            this.agent_operation.Size = new System.Drawing.Size(102, 46);
            this.agent_operation.TabIndex = 26;
            this.agent_operation.Text = "Agent Off";
            this.agent_operation.UseVisualStyleBackColor = true;
            this.agent_operation.Click += new System.EventHandler(this.agent_operation_Click);
            // 
            // highclaimhear
            // 
            this.highclaimhear.BackColor = System.Drawing.Color.Transparent;
            this.highclaimhear.CardSize = new System.Drawing.Size(55, 76);
            this.highclaimhear.DeckCards = "0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0";
            this.highclaimhear.Location = new System.Drawing.Point(533, 206);
            this.highclaimhear.Name = "highclaimhear";
            this.highclaimhear.SelectClick = true;
            this.highclaimhear.Size = new System.Drawing.Size(60, 122);
            this.highclaimhear.TabIndex = 20;
            this.highclaimhear.Visible = false;
            this.highclaimhear.SelectionChanged += new System.EventHandler<CheatGameApp.Model.DeckEventArgs>(this.highclaimhear_SelectionChanged);
            // 
            // lowclaimhear
            // 
            this.lowclaimhear.BackColor = System.Drawing.Color.Transparent;
            this.lowclaimhear.CardSize = new System.Drawing.Size(55, 76);
            this.lowclaimhear.DeckCards = "0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0";
            this.lowclaimhear.Location = new System.Drawing.Point(273, 206);
            this.lowclaimhear.Name = "lowclaimhear";
            this.lowclaimhear.SelectClick = true;
            this.lowclaimhear.Size = new System.Drawing.Size(56, 122);
            this.lowclaimhear.TabIndex = 19;
            this.lowclaimhear.Visible = false;
            this.lowclaimhear.SelectionChanged += new System.EventHandler<CheatGameApp.Model.DeckEventArgs>(this.lowclaimhear_SelectionChanged);
            // 
            // oppDeckLabel
            // 
            this.oppDeckLabel.BackColor = System.Drawing.Color.Transparent;
            this.oppDeckLabel.CardSize = new System.Drawing.Size(50, 70);
            this.oppDeckLabel.DeckCards = "0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0";
            this.oppDeckLabel.FacingUp = false;
            this.oppDeckLabel.Location = new System.Drawing.Point(55, 76);
            this.oppDeckLabel.Name = "oppDeckLabel";
            this.oppDeckLabel.Size = new System.Drawing.Size(538, 77);
            this.oppDeckLabel.Style = CheatGameApp.DeckLabel.LayoutStyle.Horizontal;
            this.oppDeckLabel.TabIndex = 10;
            this.oppDeckLabel.Visible = false;
            // 
            // lastClaimDeckLabel
            // 
            this.lastClaimDeckLabel.BackColor = System.Drawing.Color.Transparent;
            this.lastClaimDeckLabel.CardSize = new System.Drawing.Size(52, 76);
            this.lastClaimDeckLabel.DeckCards = "3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0";
            this.lastClaimDeckLabel.Location = new System.Drawing.Point(55, 236);
            this.lastClaimDeckLabel.Name = "lastClaimDeckLabel";
            this.lastClaimDeckLabel.Size = new System.Drawing.Size(163, 80);
            this.lastClaimDeckLabel.Style = CheatGameApp.DeckLabel.LayoutStyle.Horizontal;
            this.lastClaimDeckLabel.TabIndex = 10;
            this.lastClaimDeckLabel.Visible = false;
            // 
            // allClaimsDeckLabel
            // 
            this.allClaimsDeckLabel.BackColor = System.Drawing.Color.Transparent;
            this.allClaimsDeckLabel.CardSize = new System.Drawing.Size(52, 76);
            this.allClaimsDeckLabel.DeckCards = "1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0";
            this.allClaimsDeckLabel.FacingUp = false;
            this.allClaimsDeckLabel.Location = new System.Drawing.Point(276, 236);
            this.allClaimsDeckLabel.Name = "allClaimsDeckLabel";
            this.allClaimsDeckLabel.Size = new System.Drawing.Size(317, 80);
            this.allClaimsDeckLabel.Style = CheatGameApp.DeckLabel.LayoutStyle.Horizontal;
            this.allClaimsDeckLabel.TabIndex = 10;
            this.allClaimsDeckLabel.Visible = false;
            // 
            // gameDeckLabel
            // 
            this.gameDeckLabel.BackColor = System.Drawing.Color.Transparent;
            this.gameDeckLabel.CardSize = new System.Drawing.Size(52, 76);
            this.gameDeckLabel.DeckCards = "4, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0";
            this.gameDeckLabel.FacingUp = false;
            this.gameDeckLabel.Location = new System.Drawing.Point(678, 236);
            this.gameDeckLabel.Name = "gameDeckLabel";
            this.gameDeckLabel.Size = new System.Drawing.Size(163, 80);
            this.gameDeckLabel.Style = CheatGameApp.DeckLabel.LayoutStyle.Horizontal;
            this.gameDeckLabel.TabIndex = 10;
            this.gameDeckLabel.Visible = false;
            // 
            // myDeck
            // 
            this.myDeck.BackColor = System.Drawing.Color.Transparent;
            this.myDeck.CardSize = new System.Drawing.Size(52, 76);
            this.myDeck.DeckCards = "2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0";
            this.myDeck.Location = new System.Drawing.Point(50, 413);
            this.myDeck.Name = "myDeck";
            this.myDeck.SelectClick = true;
            this.myDeck.Size = new System.Drawing.Size(560, 154);
            this.myDeck.TabIndex = 0;
            this.myDeck.Visible = false;
            this.myDeck.SelectionChanged += new System.EventHandler<CheatGameApp.Model.DeckEventArgs>(this.deckLabel1_SelectionChanged);
            // 
            // highClaimOptionDeck
            // 
            this.highClaimOptionDeck.BackColor = System.Drawing.Color.Transparent;
            this.highClaimOptionDeck.CardSize = new System.Drawing.Size(60, 86);
            this.highClaimOptionDeck.DeckCards = "0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0";
            this.highClaimOptionDeck.Location = new System.Drawing.Point(786, 399);
            this.highClaimOptionDeck.Name = "highClaimOptionDeck";
            this.highClaimOptionDeck.SelectClick = true;
            this.highClaimOptionDeck.Size = new System.Drawing.Size(80, 134);
            this.highClaimOptionDeck.TabIndex = 4;
            this.highClaimOptionDeck.Visible = false;
            this.highClaimOptionDeck.SelectionChanged += new System.EventHandler<CheatGameApp.Model.DeckEventArgs>(this.OnClaimOptionDeck_SelectionChanged);
            // 
            // lowClaimOptionDeck
            // 
            this.lowClaimOptionDeck.BackColor = System.Drawing.Color.Transparent;
            this.lowClaimOptionDeck.CardSize = new System.Drawing.Size(60, 86);
            this.lowClaimOptionDeck.DeckCards = "0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0";
            this.lowClaimOptionDeck.Location = new System.Drawing.Point(645, 399);
            this.lowClaimOptionDeck.Name = "lowClaimOptionDeck";
            this.lowClaimOptionDeck.SelectClick = true;
            this.lowClaimOptionDeck.Size = new System.Drawing.Size(80, 134);
            this.lowClaimOptionDeck.TabIndex = 4;
            this.lowClaimOptionDeck.Visible = false;
            this.lowClaimOptionDeck.SelectionChanged += new System.EventHandler<CheatGameApp.Model.DeckEventArgs>(this.OnClaimOptionDeck_SelectionChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::CheatGameApp.Properties.Resources.cheatGameDesign;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(916, 625);
            this.Controls.Add(this.agent_operation);
            this.Controls.Add(this.PlayerName);
            this.Controls.Add(this.TotaleTimeLable);
            this.Controls.Add(this.hearednothing);
            this.Controls.Add(this.highclaimhear);
            this.Controls.Add(this.lowclaimhear);
            this.Controls.Add(this.selfreplaybutton);
            this.Controls.Add(this.FalseRecord);
            this.Controls.Add(this.MakeClaim);
            this.Controls.Add(this.replay);
            this.Controls.Add(this.recordingLable);
            this.Controls.Add(this.turnLabel);
            this.Controls.Add(this.gamesCountLabel);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.msgLabel);
            this.Controls.Add(this.oppCardsCountLabel);
            this.Controls.Add(this.allClaimsCountLabel);
            this.Controls.Add(this.gameDeckCountLabel);
            this.Controls.Add(this.StartGameButton);
            this.Controls.Add(this.CallCheatButton);
            this.Controls.Add(this.oppDeckLabel);
            this.Controls.Add(this.lastClaimDeckLabel);
            this.Controls.Add(this.allClaimsDeckLabel);
            this.Controls.Add(this.gameDeckLabel);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.turnOverCardsButton);
            this.Controls.Add(this.TakeCardButton);
            this.Controls.Add(this.myDeck);
            this.Controls.Add(this.highClaimOptionDeck);
            this.Controls.Add(this.lowClaimOptionDeck);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "Cheat Game";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DeckLabel myDeck;
        private DeckLabel highClaimOptionDeck;
        private DeckLabel lowClaimOptionDeck;
        private System.Windows.Forms.Button StartGameButton;
        private System.Windows.Forms.Button TakeCardButton;
        private System.Windows.Forms.Button CallCheatButton;
        private System.Windows.Forms.Label StatusLabel;
        private DeckLabel gameDeckLabel;
        private DeckLabel oppDeckLabel;
        private DeckLabel allClaimsDeckLabel;
        private DeckLabel lastClaimDeckLabel;
        private System.Windows.Forms.Label gameDeckCountLabel;
        private System.Windows.Forms.Label allClaimsCountLabel;
        private System.Windows.Forms.Label oppCardsCountLabel;
        private System.Windows.Forms.Button turnOverCardsButton;
        private System.Windows.Forms.Label msgLabel;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Label gamesCountLabel;
        private System.Windows.Forms.Label turnLabel;
        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Label recordingLable;
        private System.Windows.Forms.Button replay;
    private System.Windows.Forms.Button MakeClaim;
    private System.Windows.Forms.Button FalseRecord;
        private System.Windows.Forms.Button selfreplaybutton;
        private DeckLabel lowclaimhear;
        private DeckLabel highclaimhear;
        private System.Windows.Forms.Button hearednothing;
    private System.Windows.Forms.Label TotaleTimeLable;
        private System.Windows.Forms.Label PlayerName;
        private System.Windows.Forms.Button agent_operation;
    }
}

