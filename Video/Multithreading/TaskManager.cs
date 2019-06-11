using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using CheatGameModel;
using System.Diagnostics;
using System.IO;
using CheatGameApp;

namespace CheatGameApp.Video.Multithreading
{
    public sealed class TaskManager
    {
        private AutoResetEvent m_resetEvent;
        private readonly Queue<ITask> m_saveImageTasksQueue;
        private readonly Queue<ITask> m_featuresExtractTasksQueue;
        private bool m_cancel;
        private int m_threadCount;
        private object m_syncRoot = new object();

        private TaskResults m_myResults;
        private TaskResults m_opponentResults;

        private DateTime m_startTime = DateTime.Now;
        private Stopwatch m_stopwatch = Stopwatch.StartNew();

        public TimeSpan Time { get { return m_startTime.TimeOfDay + m_stopwatch.Elapsed; } }
        public int Pending { get { return m_saveImageTasksQueue.Count + m_featuresExtractTasksQueue.Count; } }

        public int SavePending { get { return m_saveImageTasksQueue.Count; } }
        public int FeaturesPending { get { return m_featuresExtractTasksQueue.Count; } }

        public Image MyImage { get; private set; }
        public Image OpponentImage { get; private set; }

        public Object SyncRoot { get { return m_syncRoot; } }

        public TaskManager(int threadCount)
        {
            m_resetEvent = new AutoResetEvent(false);
            m_saveImageTasksQueue = new Queue<ITask>();
            m_featuresExtractTasksQueue = new Queue<ITask>();
            m_cancel = false;
            m_threadCount = Math.Max(2, (int)(2 * Math.Ceiling(threadCount / 2.0)));
        }

        public void SetFolders(string myFolder, string opponentFolder, out TaskResults myResults, out TaskResults opponentResults)
        {
            lock (m_syncRoot)
            {
                //init to current results list
                myResults = m_myResults;
                opponentResults = m_opponentResults;

                //create new results list if folder has changed
                if (m_myResults == null || m_myResults.Folder != myFolder)
                    myResults = new TaskResults(myFolder);

                //create new results list if folder has changed
                if (m_opponentResults == null || m_opponentResults.Folder != opponentFolder)
                    opponentResults = new TaskResults(opponentFolder);

                m_myResults = myResults;
                m_opponentResults = opponentResults;
            }
        }


        private void EnqueuTo(TaskResults results, Image image)
        {
            if (!Form1.IsServer)
                return;
            lock (m_syncRoot)
            {
                SaveImageTask task = new SaveImageTask(this, this.Time, image, results.TotalTaskCount, results);
                Interlocked.Increment(ref results.TotalTaskCount);
                this.m_saveImageTasksQueue.Enqueue(task);
                m_resetEvent.Set();
            }
        }
        public FeatureExtractionTask Enqueue(TimeSpan time, string imagePath, TaskResults results)
        {
            FeatureExtractionTask task = null;
            lock (m_syncRoot)
            {
              //  task = new FeatureExtractionTask(time, imagePath, results);
              //  this.m_featuresExtractTasksQueue.Enqueue(task);
              //  m_resetEvent.Set();
            }
            return task;
        }
        public void EnqueueMe(Image image)
        {
            lock (m_syncRoot)
            {
                Image oldImage = MyImage;
                lock (image)
                {
                    MyImage = image.Clone() as Image;
                }

                if (oldImage != null)
                    oldImage.Dispose();

                //MyImage = new Bitmap(image);
                //EnqueuTo(m_myResults, image);
            }
        }
        public void EnqueueOpponent(Image image)
        {
            lock (m_syncRoot)
            {
                Image oldImage = OpponentImage;
                lock (image)
                {
                    OpponentImage = new Bitmap(image, image.Width, image.Height);
                   // OpponentImage = image.Clone() as Image;
                }
                if (oldImage != null)
                    oldImage.Dispose();
                //EnqueuTo(m_opponentResults, image);
            }
        }

        private List<Thread> m_backgroundThreads;
        private Thread CreateThread(Queue<ITask> queue, string prefix)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(ThreadLoop));
            thread.Name = prefix + "TaskThread" + m_backgroundThreads.Count;
            thread.Priority = ThreadPriority.Lowest;
            thread.IsBackground = true;
            thread.Start(queue);
            return thread;
        }
        public void Start()
        {
            m_backgroundThreads = new List<Thread>();
            Thread thread;
            for (int i = 0; i < m_threadCount / 2; i++)
            {
                //image thread
                thread = CreateThread(m_saveImageTasksQueue, "Image");
                m_backgroundThreads.Add(thread);

                //features thread
                //thread = CreateThread(m_featuresExtractTasksQueue, "Features");
                //m_backgroundThreads.Add(thread);
            }
        }
        public void Stop()
        {
            m_cancel = true;
            m_resetEvent.Close();
        }

        private void ThreadLoop(object queueObj)
        {
            Queue<ITask> queue = queueObj as Queue<ITask>;
            while (!m_cancel)
            {
                m_resetEvent.WaitOne();
                while (queue.Count > 0)
                {
                    if (m_cancel)
                        return;
                    ITask task = null;

                    lock (m_syncRoot)
                    {
                        if (queue.Count != 0)
                            task = queue.Dequeue();
                    }
                    if (task == null)
                        continue;

                    task.Process();
                    task = null;
                }

                // collect the garbage after the deletion
                GC.Collect();
            }
        }
    }

}
