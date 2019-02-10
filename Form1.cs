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
using CentipedeModel.Network.Messages;
using NAudio.Utils;

namespace CheatGameApp
{
    public partial class Form1 : Form
    {
        private Card m_lastClaimCard; // = new Card(5, CardType.Heart);
        private VideoForm _videoCapture;
        private Demographics _demographics;

        public WaveIn waveSource = null;
        WaveOutEvent waveOut = null;
        public WaveFileWriter waveFile = null;
        AudioFileReader Reader = null;
        MemoryStream ws = null;
        System.Windows.Forms.Timer audioRecordTimer = new System.Windows.Forms.Timer();
        // Define the output wav file of the recorded audio
        string outputFilePath;
        public static string recived_file_tmp_location;
        ManualResetEvent isRecordingEvent = new ManualResetEvent(false);

        public static TcpConnectionBase[] _tcpConnection = new TcpConnectionBase[2];
        public static bool IsServer = false;

        public static string _paramsFileName = "Params.xml";
        public static string _rootDir;
        public static int _camIndex;
        public static int _camFrameRate;

        public const int NUM_PLAYERS = 2;

        public Form1()
        {
            InitializeComponent();
          
            this.DoubleBuffered = true;

            LoadParams();

            //connect to server
            TCPConnect();

            if (_needToClose)  // raised on unsuccessful TCP connect attempt
            {
                Application.Exit();
            }

            // Start Video Capture
            // ShowVideoForm();

            //show demographics dialog
            _demographics = ShowDemographicsForm();

            //configure audio capture
            audioRecordTimer.Interval = 4000;
            audioRecordTimer.Tick += new EventHandler(audioRecordTimer_Tick);
            outputFilePath = @"C:\Users\neite\OneDrive\Desktop\recordings\system_recorded_audio" + _demographics.FullName + ".wav";
            recived_file_tmp_location = @"C: \Users\neite\OneDrive\Desktop\system_recorded_audio_recieved_player" + _demographics.FullName + ".wav";
        //send demographics to opponent
            _tcpConnection[connIndex].Send(new DemographicsMessage(_demographics));

            //InitGUI();
        }

        //private void InitGUI()
        //{
        //    UnrevealCards (allCardsInGroupBox: mainDeckGroupBox);
        //    UnrevealCards (allCardsInGroupBox: usedDeckGroupBox);
        //}

        //private void UnrevealCards(GroupBox allCardsInGroupBox)
        //{
        //    foreach (CardLabel cardLabel in allCardsInGroupBox.Controls)
        //    {
        //        cardLabel.Card = Card.EmptyDeck;
        //    }
        //}

        private static int _camImageWidth;
        private static int _camImageHeight;

        private static void LoadParams()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_paramsFileName);

            _camIndex = doc.GetParamInt32("CAM_INDEX");
            _camFrameRate = doc.GetParamInt32("CAM_FRAME_RATE");
            _camImageWidth = doc.GetParamInt32("CAM_IMAGE_WIDTH");
            _camImageHeight = doc.GetParamInt32("CAM_IMAGE_HEIGHT");

            _rootDir = doc.GetParamString("ROOT_DIR");
            if (_rootDir != "")
                Directory.CreateDirectory(_rootDir);


            string tcpConn = "Client";
            if (!string.IsNullOrEmpty(tcpConn))
            {
                string SERVER_ENDPOINT = doc.GetParamString("SERVER_ENDPOINT");
                string CLIENT_ENDPOINT = doc.GetParamString("CLIENT_ENDPOINT");
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
        public static int connIndex;
        private static bool _needToClose = false;
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
        protected void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            Reader.Position = 0;
        }
        protected void Playrecording()
        {
            if (!File.Exists(recived_file_tmp_location))
                return;
            FileInfo f = new FileInfo(recived_file_tmp_location);
            long s1 = f.Length;
            if (s1 < 10)
                return;
            waveOut = new WaveOutEvent();
            waveOut.PlaybackStopped += OnPlaybackStopped;   
            Reader = new AudioFileReader(recived_file_tmp_location);
            waveOut.Init(Reader);
            waveOut.Play();
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

                allClaimsDeckLabel.Deck = new Deck();
                allClaimsDeckLabel.FacingUp = false;

                msgLabel.Text = string.IsNullOrEmpty(myBoard.BoardMsg) ? string.Empty : myBoard.BoardMsg;

                // update the decks on the formas the board data in the message indicates
                UpdateMyDeck(myBoard.GetCards());
                myDeck.SelectNone();

                UpdateLastClaimGroupBox(myBoard.LastClaimNum, myBoard.LastClaimType, myBoard.PlayedCardsNum);
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
                    int takenCard = int.Parse(myBoard.PlayerMsg);
                    myDeck.SupressSelectionChanged();
                    myDeck.SelectNone();
                    foreach (CardLabel cardLabel in myDeck.Controls)
                    {
                        if (cardLabel.Card.Number != takenCard)
                            continue;
                        
                        cardLabel.Selected = true;
                        break;
                    }
                    myDeck.ResumeSelectionChanged();
                }
                Playrecording();
            }
        }

        protected Demographics ShowDemographicsForm()
        {
            DemographicsForm demographicsForm = new DemographicsForm();
            demographicsForm.ShowDialog();

            Demographics demographics = demographicsForm.GetDemographics();
            return demographics;
        }


        protected void ShowVideoForm()
        {
            _videoCapture = new VideoForm(_camImageWidth, _camImageHeight, _tcpConnection[connIndex], cameraIndex: 0,
                                          saveAndProcessImages: false);
            _videoCapture.CaptureFrameRate = 24;

            _videoCapture.StartPosition = FormStartPosition.Manual;
            _videoCapture.Location = System.Drawing.Point.Empty;
            _videoCapture.Shown += new EventHandler(OnVideoCapture_Shown);
            _videoCapture.Show();
        }

        protected void OnVideoCapture_Shown(object sender, EventArgs e)
        {
            _videoCapture.Location = System.Drawing.Point.Empty;
        }


        #region Board Message Processing 

        public void UpdateLastClaimGroupBox(string sLastClaimCount, string sLastClaimCardNumber, int playedCardsNum)
        {
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
                for (int i = 0; i < lastClaimCount; i++)
                    deck.Add(m_lastClaimCard);
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
            lowClaimOptionDeck.Visible =
                highClaimOptionDeck.Visible = (count != 0);
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
            lowClaimOptionDeck.ResumeSelectionChanged();
            highClaimOptionDeck.ResumeSelectionChanged();
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
                return;
            }

            bool isMyTurn = !myBoard.IsServerTurn;

            // other user has joined
            if (startButtonPressed && StatusLabel.Visible)
            {
                // hide waiting message
                StatusLabel.Visible = false;

                m_gameTime = TimeSpan.Zero;
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
                makeMoveButton.Visible = true;
                StartGameButton.Visible = false;
                ControlOfNormalGameState(isMyTurn);
                timeLabel.Visible = true;
                turnLabel.Visible = true;
                gamesCountLabel.Visible = true;
                recordingLable.Visible = false;
                timeLabel.Text = "00:00:00";
                return;
            }

            // end of game
            if (!StartGameButton.Visible && !startButtonPressed)
            {
                // hide board
                myDeck.Visible = false;
                TakeCardButton.Visible = false;
                oppCardsCountLabel.Visible = false;
                gameDeckLabel.Visible = false;
                allClaimsDeckLabel.Visible = false;
                lastClaimDeckLabel.Visible = false;
                oppDeckLabel.Visible = false;
                CallCheatButton.Visible = false;
                StartGameButton.Visible = true;
                makeMoveButton.Visible = false;
                allClaimsCountLabel.Visible = false;
                gameDeckCountLabel.Visible = false;
                turnOverCardsButton.Visible = false;
                recordingLable.Visible = false;
                allClaimsCountLabel.Text = "1 Cards";
                gameDeckCountLabel.Text = "35 Cards";
                StatusLabel.Visible = true;
                StatusLabel.Text = myBoard.BoardMsg;

                timeLabel.Visible = false;
                turnLabel.Visible = false;
                gamesCountLabel.Visible = false;
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
            WaveStream ws = CaptureAudio();
            isRecordingEvent.Reset();

            // prepare the move message's properties in case of a Play Move command
            var move = new Move();
            move.SetRealMoveCards(realMove);
            move.SetClaimMoveCards(claimMove);
            move.MoveTime = TimeStamper.Time; // NOTE: this is not used on the server side due to clock differences
            move.MoveType = MoveType.PlayMove;

            // send the move to the server
            _tcpConnection[connIndex].Send(new AudioMessage(ws));
            _tcpConnection[connIndex].Send(new MoveMessage(move));
            

        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            _tcpConnection[connIndex].Send(new MoveMessage(new Move { MoveType = MoveType.StartPressed,
                                                                      MoveTime = TimeStamper.Time
            }));
        } // NOTE: MoveTime may not be used on the server side due to clock differences

        #endregion Form Events

        private void TakeCardButton_Click(object sender, EventArgs e)
        {
            _tcpConnection[connIndex].Send(new MoveMessage(new Move { MoveType = MoveType.TakeCard,
                                                                      MoveTime = TimeStamper.Time
            }));
        } // NOTE: MoveTime may not be used on the server side due to clock differences

        private void CallCheatButton_Click(object sender, EventArgs e)
        {
            _tcpConnection[connIndex].Send(new MoveMessage(new Move { MoveType = MoveType.CallCheat,
                                                                      MoveTime = TimeStamper.Time
            }));
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

        private TimeSpan m_gameTime;
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            m_gameTime += TimeSpan.FromMilliseconds(gameTimer.Interval);
            timeLabel.Text = string.Format("{0:00}:{1:00}:{2:00}", m_gameTime.Hours, m_gameTime.Minutes, m_gameTime.Seconds);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
