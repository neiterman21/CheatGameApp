using System;
using System.Drawing;

namespace CheatGameApp.Video
{
    /// <summary>
    /// EventArgs for the webcam control
    /// </summary>
    public class WebcamEventArgs : EventArgs
    {
        public WebcamEventArgs(Image image, ulong frameNumber)
        {
            this.WebCamImage = image;
            this.FrameNumber = frameNumber;
        }

        /// <summary>
        ///  Gets the image returned by the web camera capture
        /// </summary>
        public Image WebCamImage { get; private set; }

        /// <summary>
        /// Gets the sequence number of the frame capture
        /// </summary>
        public ulong FrameNumber { get; private set; }
    }
}
