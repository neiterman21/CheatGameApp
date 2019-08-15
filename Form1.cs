using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
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

namespace CheatGameApp
{
    public partial class Form1 : Form
    {
        private Card m_lastClaimCard; // = new Card(5, CardType.Heart);
        private VideoForm _videoCapture;
        private Demographics _demographics;

        // Define the output wav file of the recorded audio
        public WaveIn waveSource = null;
        public WaveFileWriter waveFile = null;
        MemoryStream ws = null;
        private WaveStream recived_wave_stream = null;
        private WaveStream claim_record = null;
        System.Windows.Forms.Timer audioRecordTimer = new System.Windows.Forms.Timer();

        ManualResetEvent isRecordingEvent = new ManualResetEvent(false);

        public static TcpConnectionBase[] _tcpConnection = new TcpConnectionBase[2];
        public static bool IsServer = false;

        public static string _paramsFileName = "Params.xml";
        public static string _rootDir;
        public static int _camIndex;
        public static int _camFrameRate;

        public const int NUM_PLAYERS = 2;
        public static int game_num = 1;
        public static int TrunTime = 50;
        private TimeSpan m_gameTime;
        private bool no_response_prev_round = false;
        public static int connIndex;
        private static bool _needToClose = false;
        private BoardState Board = new BoardState();

        public Form1()
        {
            InitializeComponent();
          
            this.DoubleBuffered = true;          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          LoadParams();

          //connect to server
          TCPConnect();

          //show demographics dialog
          //if (!_needToClose) ShowsoundTestForm();
          if (!_needToClose) _demographics = ShowDemographicsForm();

          if (_needToClose)  // raised on unsuccessful TCP connect attempt
          {
            Application.Exit();
            Close();
          }
          //configure audio capture
          audioRecordTimer.Interval = 4000;
          audioRecordTimer.Tick += new EventHandler(audioRecordTimer_Tick);
          //send demographics to opponent
          _tcpConnection[connIndex].Send(new DemographicsMessage(_demographics));
          FormClosing += new FormClosingEventHandler(CheatGame_Closing);
          Thread conectivity_check = new Thread(ConectivityCheck);
          addActivitydetection();

          // conectivity_check.Start();
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
          no_response_prev_round = false;
        }
        public void ConectivityCheck()
        {
          while (true)
          {
            _tcpConnection[connIndex].Send();
            Thread.Sleep(3000);
          }
        }
        void CheatGame_Closing(object sender, FormClosingEventArgs e)
        {
            _tcpConnection[connIndex].Dispose();
        }

        private static void LoadParams()
        {
            //  XmlDocument doc = new XmlDocument();
            // doc.Load(_paramsFileName);



            // _rootDir = doc.GetParamString("ROOT_DIR");
            _rootDir = "c:\\CheatGameLogs";
            if (_rootDir != "")
                Directory.CreateDirectory(_rootDir);


            string tcpConn = "Client";
            if (!string.IsNullOrEmpty(tcpConn))
            {
                //string SERVER_ENDPOINT = doc.GetParamString("SERVER_ENDPOINT");
                //string CLIENT_ENDPOINT = doc.GetParamString("CLIENT_ENDPOINT");
                //string SERVER_ENDPOINT = "18.224.93.57";
                //string CLIENT_ENDPOINT = "18.224.93.57";
                string SERVER_ENDPOINT = "18.221.184.12";
                string CLIENT_ENDPOINT = "18.221.184.12";
                //string SERVER_ENDPOINT = "127.0.0.1";
                //string CLIENT_ENDPOINT = "127.0.0.1";
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
              System.Threading.Thread.Sleep(100);
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

            if (e.Message is BoardMessage)
            {
                // get the board data from the message
                BoardMessage message = e.Message as BoardMessage;
                BoardState myBoard = message.GetBoardState();
                Board = myBoard;
                m_gameTime = TimeSpan.FromSeconds(TrunTime);
                allClaimsDeckLabel.Deck = new Deck();
                allClaimsDeckLabel.FacingUp = false;
                makeMoveButton.Visible = false;
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
                HandleLastClaimGroupBox(myBoard.LastClaimType, myBoard.AgentStartPressed);
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
            }
            if (e.Message is AudioMessage)
            {
               AudioMessage message = e.Message as AudioMessage;
               recived_wave_stream = message.GetRecording();
               Playrecording(recived_wave_stream);
               replay.Visible = true;

               EnterVerifiyClaimState();
            }
            if (e.Message is ControlMessage)
            {
               ControlMessage message = e.Message as ControlMessage;
                if (message.Commmand == ControlCommandType.EndMatch)
                    ShowEndGameMessage(message.msg);
                if (message.Commmand == ControlCommandType.OpponentDisconected)
                    ShowOpponentDisconectedMessage(message.msg);
                if (message.Commmand == ControlCommandType.Tick)
                    _tcpConnection[connIndex].Send(new ControlMessage(ControlCommandType.Tick));
      }
        }
        public void ShowEndGameMessage(string code)
        {
            string massage = "Thank you for playing the game. It will help our reserch a lot. \n please save the code shown bellow. you will need to submit it in order to get paid. \n ";
            EndgameForm eg = new EndgameForm(massage , code);
            eg.Show();
            hideAllComponents();
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
            EndgameForm eg = new EndgameForm(massage,code);
            eg.Show();
            hideAllComponents();
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
        }

        private void EnableNonVerifiyClaimbuttons()
        {
            lastClaimDeckLabel.Visible = true;
            allClaimsDeckLabel.Visible = true;
            TakeCardButton.Visible = true;
            MakeClaim.Visible = true;
            CallCheatButton.Visible = true;
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
                EnterVerifiyClaimState();
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

        protected void OnVideoCapture_Shown(object sender, EventArgs e)
        {
            _videoCapture.Location = System.Drawing.Point.Empty;
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

        private void UpdateCurrentPlayingCard(string cardLiteral)
        {
            //TODO figer out what this does

            //if (cardLiteral != "" && cardLiteral != "None")
            //    currentCardCardLabel.Card = Card.FromLiteral(cardLiteral);
            //else
            //    currentCardCardLabel.Card = Card.EmptyDeck;
        }

        private void UpdateClaimDecks(int count)
        {            
            //lowClaimOptionDeck.Visible =
            //    highClaimOptionDeck.Visible = (count != 0);
            //if (count == 0)
            //    claimPanel.Visible = false;
            //else
            //    claimPanel.Visible = true;

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

            makeMoveButton.Enabled = lowClaimOptionDeck.GetSelectedCards().Count > 0 || highClaimOptionDeck.GetSelectedCards().Count > 0;
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

            bool isMyTurn = !myBoard.IsServerTurn;

            // other user has joined
            if (startButtonPressed && StatusLabel.Visible)
            {
                playWelcomeMessagse();
                // hide waiting message
                StatusLabel.Visible = false;

                m_gameTime = TimeSpan.FromSeconds(TrunTime);
                gameTimer.Start();

                // show board
                myDeck.Visible = true;
                TakeCardButton.Visible = true;
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
                makeMoveButton.Visible = false;
                selfreplaybutton.Visible = false;
                FalseRecord.Visible = false;
                MakeClaim.Visible = true; MakeClaim.Enabled = false;
                StartGameButton.Visible = false;
                ControlOfNormalGameState(isMyTurn);
                timeLabel.Visible = true;
                turnLabel.Visible = true;
                gamesCountLabel.Visible = true;
                recordingLable.Visible = false;
                replay.Visible = false;
                timeLabel.Text = "00:00:40";
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
                selfreplaybutton.Visible = false;
                makeMoveButton.Visible = false;
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
                turnLabel.Visible = false;
                gamesCountLabel.Visible = false;
                replay.Visible = false;
                gameTimer.Stop();
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
                //usedCardLabel1.Card = new Card(Convert.ToInt16(cardsToTurnOver[0]),
                //                                CardType.Heart);
                //if (cardsToTurnOver.Count() > 1)
                //    usedCardLabel2.Card = new Card(Convert.ToInt16(cardsToTurnOver[1]),
                //                                CardType.Heart);
                //if (cardsToTurnOver.Count() > 2)
                //    usedCardLabel3.Card = new Card(Convert.ToInt16(cardsToTurnOver[2]),
                //                                CardType.Heart);
                //if (cardsToTurnOver.Count() > 3)
                //    usedCardLabel4.Card = new Card(Convert.ToInt16(cardsToTurnOver[3]),
                //                                CardType.Heart);
                //usedCardLabel1.Text = "";

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
                turnLabel.Text = "Your";
                UpdateClaimDecks(count: 0);
            }
            else
            {
                // prepare the form controls to be disabled to avoid from playing
                myDeck.SelectClick = false;
                myDeck.SelectNone(); 
                turnLabel.Text = "Opponent";
                //realMoveDeck.Deck = new Deck();
                //claimPanel.Visible = false;
            }
        }

        private void HandleLastClaimGroupBox(string cardLiteral, bool startButtonPressed)
        {
            //TODO Ask amir what this does
            //if (cardLiteral != "" && cardLiteral != "None")
            //    currentCardCardLabel.Card = Card.FromLiteral(cardLiteral);
            //else
            //    currentCardCardLabel.Card = Card.EmptyDeck;

            //currentCardCardLabel.Visible = startButtonPressed;
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
        private void OnClaimOptionDeck_SelectionChanged(object sender, DeckEventArgs e)
        {
            DeckLabel deck = sender as DeckLabel;
            DeckLabel otherDeck = deck == lowClaimOptionDeck ? highClaimOptionDeck : lowClaimOptionDeck;

            //unhook events to prevent recursive call
            deck.SupressSelectionChanged();
            otherDeck.SupressSelectionChanged();

            //change selection
           // if (deck.GetSelectedCards().Count == 0)
            {
                deck.SelectAll();
                otherDeck.SelectNone();
            }
            makeMoveButton.Enabled = deck.GetSelectedCards().Count > 0 || otherDeck.GetSelectedCards().Count > 0;

            //rehook events
            deck.ResumeSelectionChanged();
            otherDeck.ResumeSelectionChanged();
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

            recordingLable.Visible = true;
            Thread recordingThread = new Thread(waveSource.StartRecording);
            recordingThread.Start();

            audioRecordTimer.Start();
            audioRecordTimer.Enabled = true;
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

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            _tcpConnection[connIndex].Send(new MoveMessage(new Move { MoveType = MoveType.StartPressed,
                                                                      MoveTime = TimeStamper.Time
            }));
            //remove replay button if was visable
            replay.Visible = false;
    } // NOTE: MoveTime may not be used on the server side due to clock differences

        #endregion Form Events

        private void TakeCardButton_Click(object sender, EventArgs e)
        {
            _tcpConnection[connIndex].Send(new MoveMessage(new Move { MoveType = MoveType.TakeCard,
                                                                      MoveTime = TimeStamper.Time
            }));
            //remove replay button if was visable
            replay.Visible = false;
        } // NOTE: MoveTime may not be used on the server side due to clock differences

        private void CallCheatButton_Click(object sender, EventArgs e)
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
            base.OnClosed(e);
        }

        private void turnOverCardsButton_Click(object sender, EventArgs e)
        {
            _tcpConnection[connIndex].Send(new ControlMessage(ControlCommandType.End));
            turnOverCardsButton.Visible = false;
            msgLabel.Text = "Please wait for other players to press the Turn Cards Over button";
           // StatusLabel.Visible = true;
            //allClaimsDeckLabel.Deck = new Deck();
            //allClaimsDeckLabel.FacingUp = false;
            //UnrevealCards(allCardsInGroupBox: usedDeckGroupBox);
            //_tcpConnection[connIndex].Send(new ControlMessage(ControlCommandType.End));
            //CallCheatButton.Text = WAIT_END_REVEAL_STATE_TEXT;
            //CallCheatButton.Enabled = false;
        }

        
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            m_gameTime -= TimeSpan.FromMilliseconds(gameTimer.Interval);
            timeLabel.Text = string.Format("{0:00}:{1:00}:{2:00}", m_gameTime.Hours, m_gameTime.Minutes, m_gameTime.Seconds);
            if (m_gameTime <= TimeSpan.Zero && ! Board.IsServerTurn ) time_up();
        }

        private void time_up()
        {
          if (no_response_prev_round)
          {
            forfeitGame();
            MessageBox.Show("You where unactive for too long. Please reopen the aplication to play again.");
            Thread.Sleep(2000);        
            Application.Exit();
          }
          no_response_prev_round = true;
          TakeCardButton_Click(this, null);
        }

    private void replay_Click(object sender, EventArgs e)
    {
      Playrecording(recived_wave_stream);
    }

    private void myDeck_Load(object sender, EventArgs e)
    {

    }

    private void MakeClaim_Click(object sender, EventArgs e)
    {
      m_gameTime = TimeSpan.FromSeconds(TrunTime / 2);
      claim_record  = CaptureAudio();
      isRecordingEvent.Reset();
      lowClaimOptionDeck.Visible =
               highClaimOptionDeck.Visible = (myDeck.Deck.Count != 0);

      myDeck.SelectClick = false;
      makeMoveButton.Visible = FalseRecord.Visible = selfreplaybutton.Visible = true;
      makeMoveButton.Enabled = MakeClaim.Enabled = false;
      FalseRecord.Enabled = true;
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


  }
}
