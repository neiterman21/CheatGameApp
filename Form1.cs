﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CheatGameApp.Model;
using CheatGameModel.Network;
using CheatGameModel.Network.Messages;
using CheatGameModel.Players;
using System.IO;
using NAudio.Wave;
using System.Threading;
using System.Diagnostics;
using NAudio.Utils;
using CheatGameApp.Agents;
using System.Linq;

namespace CheatGameApp
{
    public partial class Form1 : Form
    {
        public bool is_agent = false;
        private int max_inslance_count = 20;
        private Card m_lastClaimCard; // = new Card(5, CardType.Heart)
        private Demographics _demographics;
        private Agent agent = null;
        private string record_dir = @"Resources\Records_Woman\";

        // Define the output wav file of the recorded audio
        public WaveIn waveSource = null;
        public WaveFileWriter waveFile = null;
        MemoryStream ws = null;
        private WaveStream recived_wave_stream = null;
        public WaveStream claim_record = null;
        System.Windows.Forms.Timer audioRecordTimer = new System.Windows.Forms.Timer();
        private static Mutex mut = new Mutex();

        ManualResetEvent isRecordingEvent = new ManualResetEvent(false);

        public static TcpConnectionBase[] _tcpConnection = new TcpConnectionBase[2];
        public static bool IsServer = false;

        public static string _paramsFileName = "Params.xml";
        public static string _rootDir;
        public static int _camIndex;
        public static int _camFrameRate;

        public const int NUM_PLAYERS = 2;
        public static int game_num = 1;
        public static int TrunTime = 50; //seconds  
        public static int GameTime = 8; //minuts
        private TimeSpan m_gameTime;
        private bool endGame = false;
        private bool game_ended_due_to_timeout = false;
        private TimeSpan total_game_time;
        private int num_of_unresponsive_turnes = 0;
        public static int connIndex;
        private static bool _needToClose = false;
        private BoardState Board = new BoardState();

        public Form1(bool _is_agent = false)
        {
            InitializeComponent();
          
            this.DoubleBuffered = true;
            this.is_agent = _is_agent;
            agent = new SmartAgent(record_dir , this);
                
        }

        public int getCountRunningInslances()
        {
            return System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count();
        }
        public DeckLabel getmyDeck()
        {
            return this.myDeck;
        }

        public DeckLabel getopponentDeck()
        {
            return this.oppDeckLabel;
        }
            public DeckLabel getLastClaimDeck()
        {
            return lastClaimDeckLabel;
        }

        public DeckLabel gethighClaimOptionDeck()
        {
            return highClaimOptionDeck;
        }
        public DeckLabel getlowClaimOptionDeck()
        {
            return lowClaimOptionDeck;
        }

        public DeckLabel getallClaimsDeckLabel()
        {
            return allClaimsDeckLabel;
        }

        public Button getStartGameButton()
        {
            return StartGameButton;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadParams();
            FormClosing += new FormClosingEventHandler(CheatGame_Closing);
            WaitWndFun loadingForm = new WaitWndFun();
            loadingForm.Show();
            //connect to server
            TCPConnect();
            addActivitydetection();
            loadingForm.Close();
            //show demographics dialog
            if (!is_agent)
            {
                try
                {
                    //if (!_needToClose) ShowsoundTestForm();
                    if (!_needToClose) _demographics = ShowDemographicsForm();
                }
                catch
                {
                    _needToClose = true;
                }              
            }else
            {
                _demographics = agent.fill_demographic_form();
            }
            if (_needToClose)  // raised on unsuccessful TCP connect attempt
            {
                Process.Start(Application.ExecutablePath);
                Close();
                
            }
            //configure audio capture
            audioRecordTimer.Interval = 4000;
            audioRecordTimer.Tick += new EventHandler(audioRecordTimer_Tick);
            //send demographics to opponent
            _tcpConnection[connIndex].Send(new DemographicsMessage(_demographics));
            agent_operation.Visible = false;
            if (is_agent)
            {
                agent.agentMove();
                PlayerName.Text = _demographics.FullName;
                PlayerName.Show();
                agent_operation.Text = "Agent On";
                agent_operation.Visible = true;
            }
            
        }

        private void addActivitydetection()
        {
          foreach (Control component in Controls)
          {
             if(component is Button)
             {
                Button b = (Button)component;
                b.Click += new System.EventHandler(this.ActivityDetection);
        }
          }
        }
        public void ActivityDetection(object sender, EventArgs e)
        {
          num_of_unresponsive_turnes = 0;

        }

        void CheatGame_Closing(object sender, FormClosingEventArgs e)
        {
            _tcpConnection[connIndex].Dispose();
         //   Application.Exit();
        }

        private static void LoadParams()
        {
            //  XmlDocument doc = new XmlDocument();
            // doc.Load(_paramsFileName);e



            // _rootDir = doc.GetParamString("ROOT_DIR");
            _rootDir = "c:\\CheatGameLogs";
            if (_rootDir != "")
                Directory.CreateDirectory(_rootDir);


            string tcpConn = "Client";
            if (!string.IsNullOrEmpty(tcpConn))
            {
                //string SERVER_ENDPOINT = doc.GetParamString("SERVER_ENDPOINT");
                //string CLIENT_ENDPOINT = doc.GetParamString("CLIENT_ENDPOINT");
                //string SERVER_ENDPOINT = "18.191.4.43";  //fasr server

                //string SERVER_ENDPOINT = "18.223.29.163";  // slow server     
                string SERVER_ENDPOINT = "127.0.0.1";        // local host
   
                string CLIENT_ENDPOINT = SERVER_ENDPOINT; 
                for (var i = 0; i < NUM_PLAYERS; i++)
                    {
                        _tcpConnection[i] = new Client();
                        _tcpConnection[i].SetIPEndPoints(SERVER_ENDPOINT + ":5432" + (i + 1).ToString(),
                                                         CLIENT_ENDPOINT + ":5432" + (i + 1).ToString());
                    }
            }
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height;
        }

        protected void TCPConnect()
        {
            bool conn0Successful;
            try
            {
                _tcpConnection[0].MessageReceived += new EventHandler<MessageEventArg>(OnTcpConnection_MessageReceived);
                ((Client)_tcpConnection[0]).BeginStart1();
                conn0Successful = true;
                connIndex = 0;
            }
            catch
            {
                conn0Successful = false;
            }

            if (!conn0Successful)
            {
                try
                {
                    _tcpConnection[1].MessageReceived += new EventHandler<MessageEventArg>(OnTcpConnection_MessageReceived);
                    ((Client)_tcpConnection[1]).BeginStart1();
                }
                catch
                {
                    MessageBox.Show("Failed to connect to Server, please check Params.xml file for correct SERVER IP END POINT ");
                    _needToClose = true;
                }
                connIndex = 1;
            }
           
        }
        protected void playWelcomeMessagse()
        {
          WaveStream WelcomeRecording = new RawSourceWaveStream(CheatGameApp.Properties.Resources.Welcome1, new WaveFormat(48000, 2));
          Playrecording(WelcomeRecording);
        }

        protected void OnPlaybackStopped(object obj , StoppedEventArgs e) 
        {
        }
        protected void Playrecording(WaveStream record_strem)
        {
          using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
          {
            record_strem.Position = 0;
            waveOut.PlaybackStopped += new EventHandler<StoppedEventArgs>(OnPlaybackStopped);
            waveOut.Init(record_strem);
            waveOut.Play();
            while (waveOut.PlaybackState == PlaybackState.Playing)
            {
              Application.DoEvents();
            }
          }
        }

        protected void OnTcpConnection_MessageReceived(object sender, MessageEventArg e)
        {
            //thread safe
            if (InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<MessageEventArg>(OnTcpConnection_MessageReceived), sender, e);
                return;
            }
            //increased server disconection timeout because Smart agent wasn't calculating fasr enough. 
            //better solution is to make pythonAPI non blocking.
            if (e.Message is ControlMessage)
            {
                ControlMessage message = e.Message as ControlMessage;
                if (message.Commmand == ControlCommandType.EndMatch)
                    ShowEndGameMessage(message.msg);
                if (message.Commmand == ControlCommandType.OpponentDisconected)
                    ShowOpponentDisconectedMessage(message.msg);
                if (message.Commmand == ControlCommandType.Tick)
                {
                    _tcpConnection[connIndex].Send(new ControlMessage(ControlCommandType.Tick));
                }
                return;
            }

            mut.WaitOne(); 
            if (e.Message is BoardMessage)
            {
                // get the board data from the message
                Debug.WriteLine("got board message " + DateTime.Now);

                BoardMessage message = e.Message as BoardMessage;
                BoardState myBoard = message.GetBoardState();
                Board = myBoard;
                m_gameTime = TimeSpan.FromSeconds(TrunTime);
                allClaimsDeckLabel.Deck = new Deck();
                allClaimsDeckLabel.FacingUp = false;
                FalseRecord.Visible = false;
                selfreplaybutton.Visible = false;
                lowClaimOptionDeck.Visible = highClaimOptionDeck.Visible = false;

                msgLabel.Text = string.IsNullOrEmpty(myBoard.BoardMsg) ? string.Empty : myBoard.BoardMsg;
                // update the decks on the formas the board data in the message indicates
                UpdateMyDeck(myBoard.GetCards());
                myDeck.SelectNone();

                UpdateLastClaimGroupBox(myBoard);
                //mainDeckTopCard.Text = myBoard.BoardCardsNum.ToString();
                gameDeckCountLabel.Text = myBoard.BoardCardsNum + " Cards";
                if (gameDeckLabel.Deck.Count != myBoard.BoardCardsNum)
                {
                    gameDeckLabel.Deck = Deck.Parse(myBoard.BoardCardsNum.ToString());
                }
              //  HandleLastClaimGroupBox(myBoard.LastClaimType, myBoard.AgentStartPressed);
                //OpponentCardLabel.Text = myBoard.AgentCardsNum.ToString();
                oppCardsCountLabel.Text = myBoard.AgentCardsNum + " Cards";
                if (myBoard.AgentCardsNum != oppDeckLabel.Deck.Count)
                {
                    oppDeckLabel.Deck = Deck.Parse(myBoard.AgentCardsNum.ToString());
                }
                TakeCardButton.Enabled = myBoard.TakeCardEnable;
                CallCheatButton.Enabled = myBoard.CallCheatEnable;
                UpdateClaimDecks(myDeck.GetSelectedCards().Count);
                ControlOfGameStates(myBoard);

                //take card
                if (!string.IsNullOrEmpty(myBoard.PlayerMsg) && myBoard.IsServerTurn)
                {
                    //select the single card that we got from the main deck after pressing the take card button
                    string[] cardes_taken = myBoard.PlayerMsg.Split(',');
                    myDeck.SupressSelectionChanged();
                    myDeck.SelectNone();
                    foreach (string card_str in cardes_taken)
                    {
                        int takenCard = int.Parse(card_str);
                        foreach (CardLabel cardLabel in myDeck.Controls)
                        {
                          if (cardLabel.Card.Number != takenCard || cardLabel.Selected)
                            continue;
                    
                          cardLabel.Selected = true;
                          break;
                        }
                    }
                    myDeck.ResumeSelectionChanged();
                }
                if (agent != null) agent.updateBoardState(myBoard);
            }
            if (e.Message is AudioMessage)
            {
               Debug.WriteLine("got audio message " + DateTime.Now);
               AudioMessage message = e.Message as AudioMessage;
               recived_wave_stream = message.GetRecording();            
               if (lastClaimDeckLabel.Deck.Count != 0)
               {
                  replay.Visible = true;
                 // if (!is_agent) EnterVerifiyClaimState();
               }
                //   if (!is_agent).
                agent.get_claim(recived_wave_stream);
                if (!is_agent)
                Playrecording(recived_wave_stream);
                // else 
                    
            }
            
            if(is_agent)
                agent.agentMove();
            mut.ReleaseMutex();
         
        }

        public static void DelayAction(int millisecond, Action action)
        {
            var timer = new System.Windows.Forms.Timer();
            timer.Tick += delegate

            {
                action.Invoke();
                timer.Stop();
            };

            timer.Interval = millisecond;
            timer.Start();
        }
        public void ShowEndGameMessage(string code)
        {
            string massage = "Thank you for playing the game. It will help our reserch a lot. \n please save the code shown bellow. you will need to submit it in order to get paid. \n ";
            hideAllComponents();
            
            if (!is_agent && !endGame)
            { 
            EndgameForm eg = new EndgameForm(massage, code);       
            DialogResult a = eg.ShowDialog(this);
            }
            
            if(is_agent)
            {
                agent.gameEnd();
            }
            endGame = true;

            this.Close();
        }

        public void ShowOpponentDisconectedMessage(string code)
        {
            string massage;
            if (game_num - 1 == 0)
            {
              massage = "Unfortunatly your opponent disconected before compleating a single game.\n you may run the game again and find a different opponent. \n you will not be paid for an uncompleat game";
              code = "";
            }
            else
            {
              massage = "Unfortunatly your opponent disconected. \n you have compleated: " + (game_num - 1) + " games. you will be paid for the games to compleated. \n please save the code shown bellow. you will need to submit it in order to get paid. \n " ;
            }
            hideAllComponents();
            if (endGame) return;
            endGame = true;
            if (!is_agent)
            {
                EndgameForm eg = new EndgameForm(massage, code);             
                DialogResult a = eg.ShowDialog(this);
            }
            this.Close();

    }
        void DisputeFalseRecordClaim()
        {         
            Form2 disput_form = new Form2(claim_record, lastClaimDeckLabel);
            disput_form.ShowDialog(this);
           
            if (DialogResult.Yes == disput_form.DialogResult) OpenDispute();
            return;

        }

        private void OpenDispute()
        {
            _tcpConnection[connIndex].Send(new ControlMessage( ControlCommandType.Report));
        }

        private void EnableVerifiyClaimbuttons()
        {
            highclaimhear.Visible = true;
            lowclaimhear.Visible = true;
            hearednothing.Visible = true;
            replay.Visible = true;
        }

        private void DisableVerifiyClaimbuttons()
        {
            highclaimhear.Visible = false;
            lowclaimhear.Visible = false;
            hearednothing.Visible = false;
        }

        private void DisableNonVerifiyClaimbuttons()
        {
            lastClaimDeckLabel.Visible = false;
            allClaimsDeckLabel.Visible = false;
            TakeCardButton.Visible = false;
            MakeClaim.Visible = false;
            CallCheatButton.Visible = false;
            myDeck.SelectClick = false; 
        }

        private void EnableNonVerifiyClaimbuttons()
        {
            lastClaimDeckLabel.Visible = true;
            allClaimsDeckLabel.Visible = true;
            TakeCardButton.Visible = true;
            MakeClaim.Visible = true;
            CallCheatButton.Visible = true;
            myDeck.SelectClick = true;
    }
        protected void EnterVerifiyClaimState()
        {
            EnableVerifiyClaimbuttons();
            DisableNonVerifiyClaimbuttons();
        }

        protected void ExitVerifiyClaimState()
        {
            DisableVerifiyClaimbuttons();
            EnableNonVerifiyClaimbuttons();
            m_gameTime = TimeSpan.FromSeconds(TrunTime);
        }

        protected void verifyClaim(Deck heared)
        {
            bool ishandeled = true;
            if (!heared.CompareTo(lastClaimDeckLabel.Deck)) {
                ishandeled = HandleCheatyOponet();
            }   
            if (ishandeled) {
                ExitVerifiyClaimState();
            }
           
        }

        protected DialogResult PopVerificationMessageBox(string message)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            return MessageBox.Show(message, "intent verification", buttons);
        }

        protected bool HandleCheatyOponet()
        {
            DialogResult result = PopVerificationMessageBox("What you have cliemed to hear is different than the Oponents claim. This insedent will be reported and handled manualy." +
            " the cheating player may not get paid. Are you sure your statment is true?");

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                _tcpConnection[connIndex].Send(new MoveMessage(new Move
                {
                    MoveType = MoveType.CallCheatyOpponent,
                    MoveTime = TimeStamper.Time
                }));
                return true;
            }
            else
            {
             //   EnterVerifiyClaimState();
                return false;
            }
        }

        protected void ShowsoundTestForm()
        {
          soundTestForm soundTestForm_ = new soundTestForm();
          soundTestForm_.ShowDialog();

          _needToClose = _needToClose || (!soundTestForm_.isTestPassed());
          return ;
        }

        protected Demographics ShowDemographicsForm()
        {
            DemographicsForm demographicsForm = new DemographicsForm();
            demographicsForm.ShowDialog();

            Demographics demographics = demographicsForm.GetDemographics();
            return demographics;
        }

        #region Board Message Processing 

        public void UpdateLastClaimGroupBox(BoardState myBoard)
        {

            string sLastClaimCount       = myBoard.LastClaimNum;
            string sLastClaimCardNumber  = myBoard.LastClaimType;
            int playedCardsNum           = myBoard.PlayedCardsNum;
            // update global var that holds the last claim card
            int lastClaimCardNumber;
            if (sLastClaimCardNumber != "")
            {
                m_lastClaimCard = Card.FromLiteral(sLastClaimCardNumber);
                lastClaimCardNumber = m_lastClaimCard.Number;
            }
            else
            {
                m_lastClaimCard = Card.EmptyDeck;
                lastClaimCardNumber = 0;
            }

            // update the claim decks with the low/high options coming from the last claim
            int lastClaimCount;
            if (sLastClaimCount != "")
            {
                lastClaimCount = Convert.ToInt16(sLastClaimCount);
                UpdateClaimDecks(lastClaimCount);
            }
            else
            {
                lastClaimCount = 0;
            }

            // update the last claim text label in the Used Deck Panel, to the last claim
            if (lastClaimCount != 0 && lastClaimCardNumber != 0)
            {
                //LastClaimLabel.Text = sLastClaimCount + " x ";
                Deck deck = new Deck();
                Deck unchoosenClainDeck = new Deck(); // the other leagal claim posebilety
                for (int i = 0; i < lastClaimCount; i++)
                {
                    deck.Add(m_lastClaimCard);
                }


                //updating the claim verification deck
                Deck low_claim_option_deck = new Deck();
                Deck high_claim_option_deck = new Deck();

                Card previusclaimcard = new Card(lastClaimDeckLabel.Deck.GetCounts());

                for (int i = 0; i < lastClaimCount ; i++) 
                {
                    low_claim_option_deck.Add(previusclaimcard.Decrease());
                    high_claim_option_deck.Add(previusclaimcard.Increase());
                }

                lowclaimhear.Deck  = low_claim_option_deck;
                highclaimhear.Deck = high_claim_option_deck;

                lastClaimDeckLabel.Deck = deck;
                //lastClaimDeckLabel.Deck =
            }

                // TODO: combine the following elseif with the logic above (its just the case during init that the count is zero 
                // ... but we still want the last claim to show the lastClaimCard (which is actually the "beggining card")
            else if (lastClaimCardNumber != 0)
            {
                Deck deck = new Deck();
                deck.Add(m_lastClaimCard);
                lastClaimDeckLabel.Deck = deck;
            }
                else
            {
                lastClaimDeckLabel.Deck = new Deck();
                //LastClaimLabel.Text = "";
            }

            // update the number of used cards as text on the image of the card that represents the deck
            //usedCardLabel1.Text = playedCardsNum.ToString();
            allClaimsCountLabel.Text = playedCardsNum + " Cards";
            allClaimsDeckLabel.Deck = Deck.Parse(playedCardsNum.ToString());

        }

        public void UpdateMyDeck(int[] deckCounts)
        {
            string sDeck = string.Join(",", deckCounts);
            myDeck.Deck = Deck.Parse(sDeck);
            //realMoveDeck.Deck = new Deck();
        }

        private void UpdateClaimDecks(int count)
        {            

            bool isLowSelected = lowClaimOptionDeck.GetSelectedCards().Count > 0;
            bool isHighSelected = highClaimOptionDeck.GetSelectedCards().Count > 0; 

            Deck claim1 = new Deck();
            Deck claim2 = new Deck();
            for (int i = 0; i < count; i++)
            {
                claim1.Add(m_lastClaimCard.Decrease());
                claim2.Add(m_lastClaimCard.Increase());
            }
            lowClaimOptionDeck.Deck = claim1;
            highClaimOptionDeck.Deck = claim2;


            lowClaimOptionDeck.SupressSelectionChanged();
            highClaimOptionDeck.SupressSelectionChanged();

            if (isLowSelected)
            {
                lowClaimOptionDeck.SelectAll();
                highClaimOptionDeck.SelectNone();
            }
            else if (isHighSelected)
            {
                highClaimOptionDeck.SelectAll();
                lowClaimOptionDeck.SelectNone();
            }
            else
            {
                //select nonw
                lowClaimOptionDeck.SelectNone(); 
                highClaimOptionDeck.SelectNone();
            }

            if ( count > 0 ) MakeClaim.Enabled = true;
            else MakeClaim.Enabled = false;
            lowClaimOptionDeck.ResumeSelectionChanged();
            highClaimOptionDeck.ResumeSelectionChanged();
        }

        public void forfeitGame()
        {
            _tcpConnection[connIndex].Send(new MoveMessage(new Move
            {
                MoveType = MoveType.ForfeitGame,
                MoveTime = TimeStamper.Time
            }));
            agent.gameEnd();
        }

        protected void hideAllComponents()
        {
          foreach (Control component in Controls)
          {
            component.Visible = false;
          }
        }

        private void ControlOfGameStates(BoardState myBoard)
        {
            bool startButtonPressed = myBoard.AgentStartPressed;
            // user pressed Start 
            if (StartGameButton.Visible && startButtonPressed)
            {
                // show please wait message
                StatusLabel.Visible = true;
                StatusLabel.Text = "Please wait for other players to join the game.";
                StartGameButton.Visible = false;
       // debugfunc();
                return;
            }
            Console.WriteLine("controll game");
            bool isMyTurn = !myBoard.IsServerTurn;

            // other user has joined
            if (startButtonPressed && StatusLabel.Visible)
            {
                playWelcomeMessagse();
                game_ended_due_to_timeout = false;
                // hide waiting message
                if (is_agent && game_num == 1 && getCountRunningInslances() < max_inslance_count) //start a new game for next player
                {
                   // Process.Start(Application.ExecutablePath);
                }
                StatusLabel.Visible = false;

                m_gameTime = TimeSpan.FromSeconds(TrunTime);
                total_game_time = TimeSpan.FromMinutes(GameTime);
                gameTimer.Start();

                // show board
                myDeck.Visible = true;
                TakeCardButton.Visible = true;
                msgLabel.Visible = true;
                oppCardsCountLabel.Visible = true;
                gameDeckLabel.Visible = true;
                allClaimsDeckLabel.Visible = true;
                lastClaimDeckLabel.Visible = true;
                oppDeckLabel.Visible = true;
                CallCheatButton.Visible = true;
                allClaimsCountLabel.Visible = true;
                gameDeckCountLabel.Visible = true;

                allClaimsCountLabel.Text = "1 Cards";
                gameDeckCountLabel.Text = "35 Cards";
                selfreplaybutton.Visible = false;
                FalseRecord.Visible = false;
                MakeClaim.Visible = true; MakeClaim.Enabled = false;
                StartGameButton.Visible = false;
                ControlOfNormalGameState(isMyTurn);
                timeLabel.Visible = true;
                TotaleTimeLable.Visible = true;
                turnLabel.Visible = true;
                gamesCountLabel.Visible = true;
                recordingLable.Visible = false;
                replay.Visible = false;
                timeLabel.Text = "00:00:40";
                if (agent != null) agent.setFirstCard(lastClaimDeckLabel.m_deck);
                return;
                }

            // end of game
            if (!StartGameButton.Visible && !startButtonPressed)
            {
        // hide board

                hideAllComponents();
                myDeck.Visible = false;
                TakeCardButton.Visible = false;
                oppCardsCountLabel.Visible = false;
                gameDeckLabel.Visible = false;
                allClaimsDeckLabel.Visible = false;
                lastClaimDeckLabel.Visible = false;
                oppDeckLabel.Visible = false;
                CallCheatButton.Visible = false;
                StartGameButton.Visible = true;
                StartGameButton.Enabled = true;
                selfreplaybutton.Visible = false;
                FalseRecord.Visible = false;
                MakeClaim.Visible = false;
                allClaimsCountLabel.Visible = false;
                gameDeckCountLabel.Visible = false;
                turnOverCardsButton.Visible = false;
                recordingLable.Visible = false;
                allClaimsCountLabel.Text = "1 Cards";
                gameDeckCountLabel.Text = "35 Cards";
                StatusLabel.Visible = true;
                StatusLabel.Text = myBoard.BoardMsg;
                game_num++;
                gamesCountLabel.Text = game_num.ToString() + " of 3";

                timeLabel.Visible = false;
                TotaleTimeLable.Visible = false;
                turnLabel.Visible = false;
                gamesCountLabel.Visible = false;
                replay.Visible = false;
                gameTimer.Stop();
                agent.gameEnd();
                return;
            }

            bool isRevealing = myBoard.IsRevealing;

            // reveal is requested
            if (isRevealing && !allClaimsDeckLabel.FacingUp)// this.usedCardLabel1.Card.Type == CardType.Deck)
            {
                // disable the option to select cards from this players deck
                myDeck.SelectClick = false;
                myDeck.SelectNone(); 

                string[] cardsToTurnOver = myBoard.UsedCardsNumbers.Split(',');

                Deck revealDeck = new Deck();
                
                foreach (var cardString in cardsToTurnOver)
                {
                    Card card = new Card(int.Parse(cardString), CardType.Heart);
                    revealDeck.Add(card);
                }

                allClaimsDeckLabel.Deck = revealDeck;
                allClaimsDeckLabel.FacingUp = true;

                for (int i = 0; i < lastClaimDeckLabel.Deck.Count; i++)
                {
                    (allClaimsDeckLabel.Controls[i] as CardLabel).Selected = true;
                }

                turnOverCardsButton.Visible = true;
                allClaimsCountLabel.Visible = false;
                if (is_agent)
                {
                    agent.onCardsRevel(new Deck(allClaimsDeckLabel.m_deck));
                    turnOverCardsButton_Click(this, null);
                }
                //SetCountDownTimer(time: 5);
                return;
            }

            ControlOfNormalGameState(isMyTurn);
            if (myBoard.CanDispute) DisputeFalseRecordClaim();
        }

        //private Timer WakeUpToCardsUnreveal = new Timer();
        //private void SetCountDownTimer(int time)
        //{
        //    WakeUpToCardsUnreveal.Interval = time * 1000;
        //    WakeUpToCardsUnreveal.Tick += new EventHandler(wakeUpToCardsDisclose_Tick);
        //    WakeUpToCardsUnreveal.Enabled = true;
        //    WakeUpToCardsUnreveal.Start();
        //}

        //private void wakeUpToCardsDisclose_Tick(object sender, EventArgs e)
        //{
        //    WakeUpToCardsUnreveal.Stop();
        //    WakeUpToCardsUnreveal.Enabled = false;

        //    //UnrevealCards(allCardsInGroupBox: usedDeckGroupBox);

        //    _tcpConnection[connIndex].Send(new ControlMessage(ControlCommandType.End));
        //}

        private void ControlOfNormalGameState(bool isMyTurn)
        {
            // a normal state during the game
            if (isMyTurn)
            {
                // prepare the form to play the turn
                myDeck.SelectClick = true;
                turnLabel.Text = "Yours";
                turnLabel.ForeColor = Color.Red;
                UpdateClaimDecks(count: 0);
            }
            else
            {
                // prepare the form controls to be disabled to avoid from playing
                myDeck.SelectClick = false;
                myDeck.SelectNone(); 
                turnLabel.Text = "Opponent";
                turnLabel.ForeColor = Color.White;
                //realMoveDeck.Deck = new Deck();
                //claimPanel.Visible = false;
      }
        }


#endregion Board Message Processing

        #region Form Events
        public void GetMove(out int[] real, out int[] claim)
        {
            claim = new int[Card.MaxNumber];

            Deck realMove = myDeck.GetSelectedCards();
            real = realMove.GetCounts();  //realMoveDeck.Deck.GetCounts();


            if (realMove.Count > 0)
            {
                if (lowClaimOptionDeck.GetSelectedCards().Count > 0)
                    claim[m_lastClaimCard.Decrease().Number - 1] = realMove.Count;
                else
                    claim[m_lastClaimCard.Increase().Number - 1] = realMove.Count;
            }

        }

        private void deckLabel1_SelectionChanged(object sender, Model.DeckEventArgs e)
        {
           // realMoveDeck.Deck = e.SelectedDeck;
            UpdateClaimDecks(e.SelectedDeck.Count);
        }
        public void OnClaimOptionDeck_SelectionChanged(object sender, DeckEventArgs e)
        {
            DeckLabel deck = sender as DeckLabel;
            DeckLabel otherDeck = deck == lowClaimOptionDeck ? highClaimOptionDeck : lowClaimOptionDeck;
            FalseRecord.Visible = false;
            //unhook events to prevent recursive call
            deck.SupressSelectionChanged();
            otherDeck.SupressSelectionChanged();

            //change selection
           // if (deck.GetSelectedCards().Count == 0)
            {
                deck.SelectAll();
                otherDeck.SelectNone();
            }

            //rehook events
            deck.ResumeSelectionChanged();
            otherDeck.ResumeSelectionChanged();

            OnMakeMoveButton_Click(sender, null);
        }

        public WaveStream CaptureAudio()
        {
            //Console.WriteLine("Now recording...");
            waveSource = new WaveIn();
            waveSource.WaveFormat = new WaveFormat(16000, 1);

            waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

            ws = new MemoryStream() ;
            waveFile = new WaveFileWriter(new IgnoreDisposeStream(ws), waveSource.WaveFormat);

            
            Thread recordingThread = new Thread(waveSource.StartRecording);
            recordingThread.Start();

            audioRecordTimer.Start();
            audioRecordTimer.Enabled = true;
            recordingLable.Visible = true;
            while (!isRecordingEvent.WaitOne(200))
            {
                // NAudio requires the windows message pump to be operational
                // this works but you better raise an event
                Application.DoEvents();
            }
            return new RawSourceWaveStream(ws, new WaveFormat(16000, 1));
        }

        void audioRecordTimer_Tick(object sender, EventArgs e)
        {
            waveSource.StopRecording();
            waveSource.Dispose();
            waveSource = null;
            //waveFile.Close();
            //waveFile = null;

            recordingLable.Visible = false;
            //disable the timer here so it won't fire again...
            audioRecordTimer.Enabled = false;
            isRecordingEvent.Set();
        }

        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFile != null)
            {
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();
            }
        }

        void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }

            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }
        }

        private void OnMakeMoveButton_Click(object sender, EventArgs e)
        {
            // get the turn's move
            int[] realMove;
            int[] claimMove;
            GetMove(out realMove, out claimMove);


            // prepare the move message's properties in case of a Play Move command
            var move = new Move();
            move.SetRealMoveCards(realMove);
            move.SetClaimMoveCards(claimMove);
            move.MoveTime = TimeStamper.Time; // NOTE: this is not used on the server side due to clock differences
            move.MoveType = MoveType.PlayMove;

            // send the move to the server
            _tcpConnection[connIndex].Send(new AudioMessage(claim_record));
            _tcpConnection[connIndex].Send(new MoveMessage(move));
            //remove replay button if was visable
            replay.Visible = false;
        }

        public void SleepWithEvents(int milisecs)
        {
          Stopwatch stopwatch = new Stopwatch();
          stopwatch.Start();
          while (TimeSpan.FromMilliseconds(milisecs) > stopwatch.Elapsed)
          {
            Application.DoEvents();
          }
          stopwatch.Stop();
        }

        public void StartGameButton_Click(object sender, EventArgs e)
        {
            _tcpConnection[connIndex].Send(new MoveMessage(new Move { MoveType = MoveType.StartPressed,
                                                                      MoveTime = TimeStamper.Time
            }));
            //remove replay button if was visable
            replay.Visible = false;
            StartGameButton.Enabled = false;
        } // NOTE: MoveTime may not be used on the server side due to clock differences

        #endregion Form Events

        public void TakeCardButton_Click(object sender, EventArgs e)
        {
            _tcpConnection[connIndex].Send(new MoveMessage(new Move { MoveType = MoveType.TakeCard,
                                                                      MoveTime = TimeStamper.Time
            }));
            //remove replay button if was visable
            ExitVerifiyClaimState();
            replay.Visible = false;
        } // NOTE: MoveTime may not be used on the server side due to clock differences

        public void CallCheatButton_Click(object sender, EventArgs e)
        {
            _tcpConnection[connIndex].Send(new MoveMessage(new Move { MoveType = MoveType.CallCheat,
                                                                      MoveTime = TimeStamper.Time
            }));
            //remove replay button if was visable
            replay.Visible = false;
        } // NOTE: MoveTime may not be used on the server side due to clock differences

        protected override void OnClosed(EventArgs e)
        {
            DurationMeasureLog.Save();
       //     if(is_agent && StatusLabel.Visible && game_num == 1)
        //    {
        //        Process.Start(Application.ExecutablePath);
        //    }
            base.OnClosed(e);
        }

        public void turnOverCardsButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("clicking turn cards over");
            _tcpConnection[connIndex].Send(new ControlMessage(ControlCommandType.End));
            Console.WriteLine("clicked turn cards over");
            turnOverCardsButton.Visible = false;
            allClaimsCountLabel.Visible = true;
            msgLabel.Text = "Please wait for other players to press the Turn Cards Over button";
        }

        
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            m_gameTime -= TimeSpan.FromMilliseconds(gameTimer.Interval);
            total_game_time -= TimeSpan.FromMilliseconds(gameTimer.Interval);
            timeLabel.Text = string.Format("{0:00}:{1:00}:{2:00}", m_gameTime.Hours, m_gameTime.Minutes, m_gameTime.Seconds);
            TotaleTimeLable.Text = string.Format("{0:00}:{1:00}:{2:00}", total_game_time.Hours, total_game_time.Minutes, total_game_time.Seconds);
            if (m_gameTime <= TimeSpan.Zero && ! Board.IsServerTurn ) time_up();
            if (m_gameTime <= TimeSpan.Zero) m_gameTime = TimeSpan.Zero;
            if (total_game_time <= TimeSpan.Zero) EndGameDueToTimeOut();
        }

        private void EndGameDueToTimeOut()
        {
          if (game_ended_due_to_timeout) return;
          game_ended_due_to_timeout = true;
          if ((oppDeckLabel.Deck.Count < myDeck.Deck.Count) ||
             (oppDeckLabel.Deck.Count == myDeck.Deck.Count && Board.IsServerTurn))
          {
            forfeitGame();
            if(!is_agent)
                MessageBox.Show("Time for a single game has ended. You have lost because your opponent has less cards. ");
          }
          else
          {
            if (!is_agent)
               MessageBox.Show("Time for a single game has ended. You have won because you have less cards. ");

          }
        }

        private void time_up()
        {
          num_of_unresponsive_turnes++;
          if (num_of_unresponsive_turnes == 3)
          {
            forfeitGame();
            MessageBox.Show("You where unactive for too long. Please reopen the aplication to play again.");
            Thread.Sleep(60000);        
            Application.Exit();
          }
          _tcpConnection[connIndex].Send(new MoveMessage(new Move
          {
            MoveType = MoveType.TimeUp,
            MoveTime = TimeStamper.Time
          }));
          //remove replay button if was visable
          ExitVerifiyClaimState();
          replay.Visible = false;
        }

    private void replay_Click(object sender, EventArgs e)
    {
      Playrecording(recived_wave_stream);
    }

    public void MakeClaim_Click(object sender, EventArgs e)
    {
      m_gameTime = TimeSpan.FromSeconds(TrunTime / 2);
      claim_record  = CaptureAudio();
      isRecordingEvent.Reset();
      lowClaimOptionDeck.Visible =
               highClaimOptionDeck.Visible = (myDeck.Deck.Count != 0);

      myDeck.SelectClick = false;
      FalseRecord.Enabled = true;
      FalseRecord.Visible = true;
      selfreplaybutton.Enabled = true;
     }

    private void FalseRecord_Click(object sender, EventArgs e)
    {
      if (TakeCardButton.Enabled) TakeCardButton_Click(sender, e);
      else forfeitGame();
      
    }
    
    private void   selfReplayButton_Click(object sender, EventArgs e)
    {
           Playrecording(claim_record);
    }

    private void hearednothing_Click(object sender, EventArgs e)
    {
        verifyClaim(new Deck());
    }

    private void highclaimhear_SelectionChanged(object sender, DeckEventArgs e)
    {
      highclaimhear.SupressSelectionChanged();
      highclaimhear.SelectNone();
      lowclaimhear.SelectNone();
      verifyClaim(highclaimhear.Deck);
      highclaimhear.ResumeSelectionChanged();
    }

    private void lowclaimhear_SelectionChanged(object sender, DeckEventArgs e)
    {
      lowclaimhear.SupressSelectionChanged();
      lowclaimhear.SelectNone();
      highclaimhear.SelectNone();
      verifyClaim(lowclaimhear.Deck);
      lowclaimhear.ResumeSelectionChanged();
    }

        private void agent_operation_Click(object sender, EventArgs e)
        {
            is_agent = !is_agent;
            if (is_agent) agent_operation.Text = "Agent On";
            else agent_operation.Text = "Agent Off";
        }
    }
}
