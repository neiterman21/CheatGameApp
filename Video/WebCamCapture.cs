using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using CheatGameModel;
using System.Collections.Generic;

namespace CheatGameApp.Video
{
    /// <summary>
    /// Summary description for UserControl1.
    /// </summary>
    [System.Drawing.ToolboxBitmap(typeof(WebCamCapture), "CAMERA.ICO")] // toolbox bitmap
    [Designer("Sytem.Windows.Forms.Design.ParentControlDesigner,System.Design", typeof(System.ComponentModel.Design.IDesigner))] // make composite
    public sealed class WebCamCapture : System.Windows.Forms.UserControl
    {
        public event EventHandler<WebcamEventArgs> ImageCaptured;

        private IContainer m_components;
        private Timer m_captureTimer;

        // property variables
        private int m_captureHandlerWindow;

        // global variables to make the video capture go faster
        private IDataObject m_tempObj;
        private Image m_tempImg;
        private bool m_stopped = true;
        private Image m_blankImage;
        private Multimedia.Timer m_timer;

        #region Control Properties

        /// <summary>
        /// The time intervale between frames captures in milliseconds
        /// </summary>
        public int CaptureInterval { get; set; }

        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }

        /// <summary>
        /// The sequence number to start at for the frame number. OPTIONAL
        /// </summary>
        public ulong FrameNumber { get; set; }

        #endregion


        #region NOTES

        /*
		 * If you want to allow the user to change the display size and 
		 * color format of the video capture, call:
		 * SendMessage (mCapHwnd, WM_CAP_DLG_VIDEOFORMAT, 0, 0);
		 * You will need to requery the capture device to get the new settings
		*/

        #endregion


        public WebCamCapture(int imageWidth, int imageHeight)
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
        }

        /// <summary>
        /// Override the class's finalize method, so we can stop
        /// the video capture on exit
        /// </summary>
        ~WebCamCapture()
        {
            this.Stop();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

                if (m_components != null)
                    m_components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_components = new System.ComponentModel.Container();
            //this.m_captureTimer = new System.Windows.Forms.Timer(this.m_components);

            m_timer = new Multimedia.Timer(this.m_components);
            m_timer.SynchronizingObject = this;
            m_timer.Resolution = 1;
            // 
            // timer1
            // 
            //this.m_captureTimer.Tick += new System.EventHandler(this.OnCaptureTimer_Tick);
            m_timer.Tick += new EventHandler(this.OnCaptureTimer_Tick);
            // 
            // WebCamCapture
            // 
            this.Name = "WebCamCapture";
            this.Size = new System.Drawing.Size(1280, 720);

        }
        #endregion

        #region Start and Stop Capture Functions

        /// <summary>
        /// Starts the video capture
        /// </summary>
        /// <param name="FrameNumber">the frame number to start at. 
        /// Set to 0 to let the control allocate the frame number</param>
        public void Start(ulong frameNumber)
        {
            try
            {
                // for safety, call stop, just in case we are already running
                this.Stop();

                // setup a capture window
                m_captureHandlerWindow = WebCamAPI.capCreateCaptureWindowA("WebCap", 0, 0, 0, ImageWidth, ImageHeight, this.Handle.ToInt32(), 0);

                // connect to the capture device
                Application.DoEvents();
                WebCamAPI.SendMessage(m_captureHandlerWindow, WebCamAPI.WM_CAP_CONNECT, 0, 0);
                WebCamAPI.SendMessage(m_captureHandlerWindow, WebCamAPI.WM_CAP_SET_PREVIEW, 0, 0);

                // set the capture video format (width and height) to the value of the params given at c'tor
                WebCamAPI.BitMapInfo bInfo = new WebCamAPI.BitMapInfo();
                bInfo.bmiHeader = new WebCamAPI.BitMapInfoHeader();
                bInfo.bmiHeader.biSize = (uint)Marshal.SizeOf(bInfo.bmiHeader);
                bInfo.bmiHeader.biWidth = ImageWidth;
                bInfo.bmiHeader.biHeight = ImageHeight;
                bInfo.bmiHeader.biPlanes = 1;
                bInfo.bmiHeader.biBitCount = 24;

                IntPtr buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(bInfo));
                Marshal.StructureToPtr(bInfo, (IntPtr)buffer, true);
                WebCamAPI.SendMessage(m_captureHandlerWindow, WebCamAPI.WM_CAP_SET_VIDEOFORMAT, Marshal.SizeOf(bInfo), (int)buffer);

                // set the frame number
                this.FrameNumber = frameNumber;

                // set the timer information
                //this.m_captureTimer.Interval = this.CaptureInterval;
                m_timer.Period = this.CaptureInterval;
                m_stopped = false;
                m_timer.Start();
                //this.m_captureTimer.Start();
            }

            catch (Exception excep)
            {
                string errorMsg = "An error ocurred while starting the video capture. Check that your webcamera is connected properly and turned on." +
                    Environment.NewLine + "Error: " + excep.Message;
                MessageBox.Show(this, errorMsg, "Start Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Stop();
            }
        }

        /// <summary>
        /// Stops the video capture
        /// </summary>
        public void Stop()
        {
            try
            {
                // stop the timer
                m_stopped = true;
                //this.m_captureTimer.Stop();
                m_timer.Stop();

                // disconnect from the video source
                Application.DoEvents();
                WebCamAPI.SendMessage(m_captureHandlerWindow, WebCamAPI.WM_CAP_DISCONNECT, 0, 0);
            }
            catch (Exception ex)
            {
                string errorMsg = "Failed in Stop(). Error: " + ex.Message;
                System.Diagnostics.Debug.WriteLine(errorMsg);
            }

        }

        #endregion

        #region Video Capture Code

        /// <summary>
        /// Capture the next frame from the video feed
        /// </summary>
        private List<TimeSpan> timeTest = new List<TimeSpan>();
        private TimeSpan baseTime = TimeStamper.Time;
        private void OnCaptureTimer_Tick(object sender, EventArgs e)
        {
            var a = TimeStamper.Time;
            try
            {
                // pause the timer
                //this.m_captureTimer.Stop();

                // get the next frame;
                WebCamAPI.SendMessage(m_captureHandlerWindow, WebCamAPI.WM_CAP_GET_FRAME, 0, 0);

                // paste the frame into the event args image
                if (ImageCaptured != null)
                {
                    // copy the frame to the clipboard
                    WebCamAPI.SendMessage(m_captureHandlerWindow, WebCamAPI.WM_CAP_COPY, 0, 0);

                    // get from the clipboard
                    m_tempObj = Clipboard.GetDataObject();

                    //get bitmap data if there is any data in the clipboard
                    if (m_tempObj != null)
                        m_tempImg = m_tempObj.GetData(System.Windows.Forms.DataFormats.Bitmap) as Bitmap;

                    /*
                    * For some reason, the API is not resizing the video
                    * feed to the width and height provided when the video
                    * feed was started, so we must resize the image here
                    */

                    //swith to empty frame if no image was captured
                    if (m_tempImg != null)
                        //m_tempImg = global::CheatGameApp.Properties.Resources.blankTV;
//                    else

                        //fires the event
                        ImageCaptured(this, new WebcamEventArgs(m_tempImg, 0));
                }

                // restart the timer
                //Application.DoEvents();

                //if (!m_stopped)
                //    this.m_captureTimer.Start();

                timeTest.Add(TimeStamper.Time - baseTime);
                baseTime = TimeStamper.Time;
            }

            catch (Exception ex)
            {
                string errorMsg = "An error ocurred while capturing the video image. The video capture will now be terminated." +
                    Environment.NewLine + "Error: " + ex.Message;
                MessageBox.Show(this, errorMsg, "Failed Capture Timer Tick!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Stop(); // stop the process
            }
        }

        #endregion
    }
}
