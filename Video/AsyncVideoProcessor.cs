using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CheatGameApp.Video
{/*
    public sealed class AsyncVideoProcessor : IDisposable
    {
        private Thread m_thread;
        private AutoResetEvent m_resetEvent;
        private bool m_cancel = true;
        private List<PlayerVideo> m_playerVideos = new List<PlayerVideo>();

        public bool IsRunning
        {
            get { return m_thread != null && m_thread.IsAlive; }
        }
        public bool FaceDetect { get; set; }

        public void Dispose()
        {
            Cancel();
            foreach (var player in m_playerVideos)
            {
                player.FrameAdded -= new EventHandler(OnPlayerVideo_FrameAdded);
            }
            m_playerVideos.Clear();
        }


        /// <summary>
        /// Returs the number of pending images in the queues.
        /// </summary>
        /// <returns></returns>
        public int GetQueuedImageCount()
        {
            PlayerVideo[] pv;
            lock (m_playerVideos)
                pv = m_playerVideos.ToArray();
            
            int count = pv.Sum(p => p.FrameQueue.Count);
            return count;
        }

        private void ProcessLoop()
        {
            while (!m_cancel)
            {
                m_resetEvent.WaitOne();
                PlayerVideo[] pv;
                lock (m_playerVideos)
                    pv = m_playerVideos.ToArray();
                foreach (var playerVideo in pv)
                {
                    if (m_cancel)
                        return;
                    playerVideo.ProcessDequeue(FaceDetect);
                }
            }
        }
        private void OnPlayerVideo_FrameAdded(object sender, EventArgs e)
        {
            m_resetEvent.Set();
        }

        public void Add(PlayerVideo playerVideo)
        {
            lock (m_playerVideos)
                m_playerVideos.Add(playerVideo);
            playerVideo.FrameAdded += new EventHandler(OnPlayerVideo_FrameAdded);
        }        
        public void Start()
        {
            if (m_thread != null)
                throw new Exception("Processser already started!");

            //init autoreset event
            m_resetEvent = new AutoResetEvent(false);

            //init thread
            m_thread = new Thread(new ThreadStart(ProcessLoop));
            m_thread.IsBackground = true;
            m_thread.Name = this.GetType().Name + "Thread";

            //enable loop
            m_cancel = false;

            //start thread
            m_thread.Start();
        }
        public void Cancel()
        {
            m_cancel = true;
            m_resetEvent.Set();            
        }
        public bool IsTasksCompleted()
        {
            return m_playerVideos.All(playerVideo => playerVideo.FrameQueue.Count == 0);

        }
    }*/
}
