using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using Luxand;
using System.Diagnostics;
using System.Drawing.Imaging;
using CheatGameModel;
using System.Threading;
using System.IO;
using CheatGameModel.Network.Messages;
using CheatGameModel.Players;
using CheatGameApp.Video;
using CheatGameApp.Video.Multithreading;
using CheatGameModel.Network;

namespace CheatGameApp
{
    public partial class VideoForm : Form
    {
        public const int FrameRate = 24;

        //================================PRIVATE FIELDS================================
        #region Private Fields

        private TcpConnectionBase m_tcpConnection;
        private WebCamCapture m_webCam;
        private TaskManager m_taskManager;
        //private string m_cameraName;
        //private int m_cameraHandle;

        //private bool m_needClose = false;
        //private DateTime m_startTime = DateTime.Now;
        //private Stopwatch m_stopwatch = Stopwatch.StartNew();
        //private AsyncVideoProcessor m_asyncVideoProcessor;

        private int m_cameraIndex;
        private bool m_saveAndProcessImages;

        #endregion Private Fields

        //================================PUBLIC PROPERTIES================================
        #region Public Properties

        public int CaptureFrameRate { get; set; }
        //public TimeSpan ElapedTime { get { return m_startTime.TimeOfDay + m_stopwatch.Elapsed; } }
        //public PlayerVideo MyVideo { get; private set; }
        //public PlayerVideo OpponentVideo { get; private set; }
        public bool ShowPoints { get; set; }
        //public bool FaceDetect { get { return m_asyncVideoProcessor.FaceDetect; } set { m_asyncVideoProcessor.FaceDetect = value; } }

        public bool IsTasksCompleted { get { return m_taskManager == null || m_taskManager.Pending == 0; } }
        //{ get { return m_asyncVideoProcessor == null || m_asyncVideoProcessor.IsTasksCompleted(); } }

        /// <summary>
        /// Gets the number of pending images in the queues.
        /// </summary>
        public int PendingImageCount { get { return m_taskManager == null ? 0 : m_taskManager.Pending; } }
        //{ get { return m_asyncVideoProcessor == null ? 0 : m_asyncVideoProcessor.GetQueuedImageCount(); } }

        #endregion Public Properties

        //================================C'TORS & D'TORS================================
        #region C'tors & D'tors

        private int m_captureImageWidth;
        private int m_captureImageHeight;
        public VideoForm(int imageWidth, int imageHeight, TcpConnectionBase tcpConnection, int cameraIndex, bool saveAndProcessImages)
        {
            InitializeComponent();
            ShowPoints = false;
            m_tcpConnection = tcpConnection;
            m_cameraIndex = cameraIndex;
            m_saveAndProcessImages = saveAndProcessImages;

            m_taskManager = new TaskManager(2);

            this.DoubleBuffered = true;
            this.KeyPreview = true;

            m_captureImageWidth = imageWidth;
            m_captureImageHeight = imageHeight;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            //m_needClose = true;
            m_taskManager.Stop();
            //m_asyncVideoProcessor.Dispose();
        }

        #endregion C'tors & D'tors

        //================================DRAW METHODS================================
        #region Draw Methods

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.KeyCode == Keys.Escape)
            {
                string title = "Exit";
                string text = "Are you sure you want to exit the game?" + Environment.NewLine + "Any data that was unsaved will be lost.";
                if (DialogResult.Yes == MessageBox.Show(text, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    Application.Exit();
            }
        }
        private Rectangle GetMyVideoBounds(double ratio)
        {
            //calc size as 30% window size
            int width = (int)Math.Round(0.25d * this.Width);
            int height = (int)Math.Round(width / ratio);

            //locate in right bottom corner
            int top = this.Width - width;
            int left = this.Height - height;
            Rectangle myBounds = new Rectangle(top, left, width, height);

            return myBounds;
        }

        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            try
            {
                Graphics g = e.Graphics;
                g.ScaleTransform(-1f, 1);
                g.TranslateTransform(-this.Width, 0);
                //draw opponent
                lock (m_taskManager.SyncRoot)
                {
                    Rectangle opponentBounds = new Rectangle(Point.Empty, this.Size);

                    if (m_taskManager.OpponentImage != null)
                    {
                        g.DrawImage(m_taskManager.OpponentImage, opponentBounds);
                    }
                    else
                        g.FillRectangle(Brushes.DarkGray, opponentBounds);//draw dark blank background
                }

                //draw me
                //lock (m_taskManager.SyncRoot)
                {
                    Rectangle myBounds;

                    if (m_taskManager.MyImage != null)
                    {
                        //get image ratio
                        myBounds = GetMyVideoBounds(m_taskManager.MyImage.Width / (double)m_taskManager.MyImage.Height);

                        //draw my image
                        g.DrawImage(m_taskManager.MyImage, myBounds);
                    }
                    else
                    {
                        //get window ratio
                        myBounds = GetMyVideoBounds(this.Width / (double)this.Height);

                        //draw light blank image
                        g.FillRectangle(Brushes.DarkGray, myBounds);//draw dark blank background
                    }

                    //draw image border
                    g.DrawRectangle(new Pen(Brushes.DarkBlue, 2f), myBounds);
                }
                g.DrawString(m_taskManager.SavePending.ToString(), this.Font, Brushes.Green, new PointF(this.Width / 2 - 50, 10));
                g.DrawString(m_taskManager.FeaturesPending.ToString(), this.Font, Brushes.Red, new PointF(this.Width / 2 + 50, 10));
            }
            catch (Exception ex)
            {
                string errorMsg = "Failed in drawining." + Environment.NewLine + "Error: " + ex.Message;
                System.Diagnostics.Debug.WriteLine(errorMsg);
            }
        }

        #endregion Draw Methods

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            //if facial
            if (m_saveAndProcessImages)
            {
                //check license
                //CheckFSDKLicense();

                //init camera
                //InitializeFSDKCamera();
            }

            //listen to jpeg message
            m_tcpConnection.MessageReceived += new EventHandler<MessageEventArg>(OnTcpConnection_MessageReceived);

            //begin capture images and enqueue them
            BeginCaptureVideo();

            //start async facial features extractor and image saving
            //m_asyncVideoProcessor.Start();
            m_taskManager.Start();
        }
        private void OnTcpConnection_MessageReceived(object sender, MessageEventArg e)
        {
            //filter out all none jpg messages
            if (!(e.Message is JpgMessage))
                return;


            //enqueue image into opponent video queue
            JpgMessage msg = e.Message as JpgMessage;

            //extract images
            Image image = msg.GetImage();

            m_taskManager.EnqueueOpponent(image);
            msg.DisposeImage();

            //asyn invalidation
            this.Invalidate();

        }

        private void BeginCaptureVideo()
        {
            m_webCam = new WebCamCapture(m_captureImageWidth, m_captureImageHeight);
            m_webCam.FrameNumber = 0;
            m_webCam.CaptureInterval = 1000 / this.CaptureFrameRate;
            m_webCam.ImageCaptured += new EventHandler<WebcamEventArgs>(OnWebCam_ImageCaptured);
            m_webCam.Start(0);
        }
        private void OnWebCam_ImageCaptured(object source, WebcamEventArgs e)
        {
            try
            {
                //PlayerVideo myVideo;
                //lock (this)
                //{
                //    myVideo = MyVideo;
                //}

                //get image from camera
                Image frameImage = e.WebCamImage;

                m_taskManager.EnqueueMe(frameImage);

                //send image to server/client
                lock (m_taskManager.SyncRoot)
                {
                    m_tcpConnection.BeginSend(new JpgMessage(frameImage));
                }

                //add image to my video images detecion queue
                //PlayerVideo.Frame frame = myVideo.Enqueue(frameImage, ElapedTime);

                //async invalidation
                this.Invalidate();
            }
            catch (Exception ex)
            {
                string errorMsg = "Failed in OnWebCam_ImageCaptured(). Error: " + ex.Message;
                System.Diagnostics.Debug.WriteLine(errorMsg);
            }
        }

        private TaskResults m_myResults;
        private TaskResults m_opponentResults;
        public void SetFolders(string myFolder, string opponentFolder)
        {
            lock (this)
            {
                m_taskManager.SetFolders(myFolder, opponentFolder, out m_myResults, out m_opponentResults);
            }
            //lock (this)
            //{
            //    //if (MyVideo != null && OpponentVideo != null && myFolder == MyVideo.Folder && opponentFolder == OpponentVideo.Folder)
            //    //    return;

            //    ////create new video queues
            //    //PlayerVideo myVideo = new PlayerVideo(myFolder);
            //    //PlayerVideo opponentVideo = new PlayerVideo(opponentFolder);

            //    ////add to processor to process incoming frames
            //    //m_asyncVideoProcessor.Add(myVideo);
            //    //m_asyncVideoProcessor.Add(opponentVideo);

            //    //this.MyVideo = myVideo;
            //    //this.OpponentVideo = opponentVideo;
            //}
        }
        //public void BeginSave(Round round, PlayerBase myPlayer, PlayerBase opponentPlayer)
        //{
        //    //PlayerVideo myVideo, opponentVideo;
        //    //lock (this)
        //    //{
        //    //    myVideo = MyVideo;
        //    //    opponentVideo = OpponentVideo;
        //    //}

        //    //MoveType myMove = round.GetMove(myPlayer).Type;
        //    //MoveType opponentMove = round.GetMove(opponentPlayer).Type;
        //    ////myVideo.BeginSave(myMove, opponentMove);
        //    ////opponentVideo.BeginSave(opponentMove, myMove);


        //    TaskResults myResults, opponentResults;
        //    lock (this)
        //    {
        //        myResults = m_myResults;
        //        opponentResults = m_opponentResults;
        //    }

        //    MoveType myMove = round.GetMove(myPlayer).Type;
        //    MoveType opponentMove = round.GetMove(opponentPlayer).Type;
        //    myResults.BeginSave(myMove, opponentMove);
        //    opponentResults.BeginSave(opponentMove, myMove);

        //}

        public void BeginSave(string myFileName, string opponentFileName)
        {
            TaskResults myResults, opponentResults;
            lock (this)
            {
                myResults = m_myResults;
                opponentResults = m_opponentResults;
            }

            myResults.BeginSave(myFileName);
            opponentResults.BeginSave(opponentFileName);
        }
        public void StopRecording()
        {
            if (m_webCam != null)
                m_webCam.Stop();
        }
    }
}
